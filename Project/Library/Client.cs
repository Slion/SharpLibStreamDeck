﻿using HidLibrary;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Special thanks to Lange (Alex Van Camp) - https://github.com/Lange/node-elgato-stream-deck
//The node-js implementation was the basis of this .NET C# implementation

namespace SharpLib.StreamDeck
{

    /// <summary>
    /// A (very simple) .NET Wrapper for the StreamDeck HID
    /// </summary>
    public class Client : IDisposable
    {
        // Assuming the following constants
        public const int KKeyCount = 15; 
        public const int KKeyWidthInPixels = 72;
        public const int KKeyHeightInPixels = 72;
        public const int KRowCount = 3;
        public const int KColumnCount = 5;


        /// <summary>
        /// Open the first Stream Deck device we find.
        /// </summary>
        /// <exception cref="SharpLib.StreamDeck.NotFoundException">Thrown when no Stream Deck is found</exception>
        public void Open()
        {
            var dev = HidDevices.Enumerate(vendorId, productId).FirstOrDefault();

            if (dev == null)
            {
                throw new NotFoundException();
            }
                
            Open(dev);
        }

        /// <summary>
        /// Open the Stream Deck device at the given location.
        /// </summary>
        /// <exception cref="SharpLib.StreamDeck.NotFoundException">Thrown when no Stream Deck is found</exception>
        public void Open(string devicePath)
        {
            var dev = HidDevices.GetDevice(devicePath);

            if (dev == null)
                throw new NotFoundException();

            Open(dev);
        }


        /// <summary>
        /// The number of keys present on the Stream Deck
        /// </summary>
        /// 
        /// TODO: Fetch this property from the device instead of having it hardcoded?
        public int KeyCount { get { return KKeyCount; } }
        public int RowCount { get { return KRowCount; } }
        public int ColumnCount { get { return KColumnCount; } }
        public int KeyWidthInpixels { get { return KKeyWidthInPixels; } }
        public int KeyHeightInpixels { get { return KKeyHeightInPixels; } }

        /// <summary>
        /// Is raised when a key is pressed
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyPressed;

        private HidDevice iDevice;
        private byte[] keyStates = new byte[KKeyCount];
        private volatile bool disposed = false;

        internal const int rawBitmapDataLength = KKeyWidthInPixels * KKeyWidthInPixels * 3;
        internal const int pagePacketSize = 8191;
        internal const int numFirstPagePixels = 2583;
        internal const int numSecondPagePixels = 2601;

        internal const int vendorId = 0x0fd9;    //Elgato Systems GmbH
        internal const int productId = 0x0060;   //Stream Deck

        private readonly Task[] iTasks;
        private readonly CancellationTokenSource threadCancelSource = new CancellationTokenSource();
        private readonly KeyRepaintQueue qqq = new KeyRepaintQueue();
        private readonly object disposeLock = new object();

        private static readonly byte[] headerTemplatePage1 = new byte[] {
            0x02, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x42, 0x4d, 0xf6, 0x3c, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00,
            0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x48, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xc0, 0x3c, 0x00, 0x00, 0xc4, 0x0e,
            0x00, 0x00, 0xc4, 0x0e, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] headerTemplatePage2 = new byte[] {
            0x02, 0x01, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] KHidFeatureShowLogo = new byte[] { 0x0B, 0x63 };

        /// <summary>
        /// Size of the icon in pixels
        /// </summary>
        public int IconSize { get => KKeyWidthInPixels; }


        /// <summary>
        /// Set Stream Deck display brightness.
        /// </summary>
        /// <param name="percent">Brightness in percent (0 - 100)</param>
        /// <remarks>
        /// The brightness on the device is controlled with PWM (https://en.wikipedia.org/wiki/Pulse-width_modulation).
        /// This results in a non-linear correlation between set percentage and perceived brightness.
        /// 
        /// In a nutshell: changing from 10 - 30 results in a bigger change than 80 - 100 (barely visible change)
        /// This effect should be compensated outside this library
        /// </remarks>
        public void SetBrightness(byte percent)
        {
            if (iDevice==null)
            {
                // Just ignore failure for now as it makes development without device handy
                return;
            }

            VerifyNotDisposed();
            if (percent > 100) throw new ArgumentOutOfRangeException(nameof(percent));
            var buffer = new byte[] { 0x05, 0x55, 0xaa, 0xd1, 0x01, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            buffer[5] = percent;
            iDevice.WriteFeatureData(buffer);
        }


        /// <summary>
        /// Sets a background image for a given key
        /// </summary>
        /// <param name="keyId">Specifies which key the image will be applied on</param>
        /// <param name="bitmapData">The raw bitmap pixel data. Details see remarks section. The key will be painted black if this value is null.</param>
        /// <remarks>
        /// The raw pixel format is a byte array of length 15552. This number is based on the image
        /// dimensions used by StreamDeck 72x72 pixels and 3 channels (RGB) for each pixel. 72 x 72 x 3 = 15552.
        /// 
        /// The channels are in the order BGR and the pixel rows (stride) are in reverse order.
        /// If you need some help try <see cref="KeyBitmap"/>
        /// </remarks>
        public void SetKeyBitmap(int keyId, byte[] bitmapData)
        {
            VerifyNotDisposed();
            if (bitmapData != null && bitmapData.Length != (KKeyWidthInPixels * KKeyWidthInPixels * 3)) throw new NotSupportedException();
            qqq.Enqueue(keyId, bitmapData);
        }

        public void Dispose()
        {
            lock (disposeLock)
            {
                if (disposed) return;
                disposed = true;
            }

            if (iDevice == null) return;

            threadCancelSource.Cancel();
            Task.WaitAll(iTasks);

            ShowLogo();

            iDevice.CloseDevice();
            iDevice.Dispose();
            iDevice = null;
        }

        public Client()
        {
            iTasks = new Task[KeyCount];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDevice"></param>
        private void Open(HidDevice aDevice)
        {
            if (aDevice == null) throw new ArgumentNullException();
            if (aDevice.IsOpen) throw new NotSupportedException();
            aDevice.MonitorDeviceEvents = true;
            aDevice.ReadReport(ReadCallback);
            aDevice.OpenDevice(DeviceMode.Overlapped, DeviceMode.Overlapped, ShareMode.ShareRead | ShareMode.ShareWrite);
            if (!aDevice.IsOpen) throw new Exception("Device could not be opened");
            iDevice = aDevice;

            for (int i = 0; i < KKeyCount; i++)
            {
                keyLocks[i] = new object();
            }

            for (int i = 0; i < iTasks.Count(); i++)
            {
                iTasks[i] = Task.Factory.StartNew(() =>
                {
                    var cancelToken = threadCancelSource.Token;

                    while (true)
                    {
                        var res = qqq.Dequeue(out Tuple<int, byte[]> nextBm, cancelToken);
                        if (!res) break;

                        var id = nextBm.Item1;
                        lock (keyLocks[id])
                        {
                            aDevice.Write(GeneratePage1(id, nextBm.Item2));
                            aDevice.Write(GeneratePage2(id, nextBm.Item2));
                        }
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }

        private void VerifyNotDisposed()
        {
            if (disposed) throw new ObjectDisposedException(nameof(Client));
        }

        private void ReadCallback(HidReport report)
        {
            var _d = iDevice;
            if (_d == null || disposed) return;
            ProcessNewStates(report.Data);
            _d.ReadReport(ReadCallback);
        }

        private readonly object[] keyLocks = new object[KKeyCount];


        private void ProcessNewStates(byte[] newStates)
        {
            for (int i = 0; i < KKeyCount; i++)
            {
                if (keyStates[i] != newStates[i])
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(i, newStates[i] != 0));
                    keyStates[i] = newStates[i];
                }
            }
        }

        private static byte[] GeneratePage1(int keyId, byte[] imgData)
        {
            var p1 = new byte[pagePacketSize];
            Array.Copy(headerTemplatePage1, p1, headerTemplatePage1.Length);

            if (imgData != null)
                Array.Copy(imgData, 0, p1, headerTemplatePage1.Length, numFirstPagePixels * 3);

            p1[5] = (byte)(keyId + 1);
            return p1;
        }

        private static byte[] GeneratePage2(int keyId, byte[] imgData)
        {
            var p2 = new byte[pagePacketSize];
            Array.Copy(headerTemplatePage2, p2, headerTemplatePage2.Length);

            if (imgData != null)
                Array.Copy(imgData, numFirstPagePixels * 3, p2, headerTemplatePage2.Length, numSecondPagePixels * 3);

            p2[5] = (byte)(keyId + 1);
            return p2;
        }

        /// <summary>
        /// Display the Elgato Gaming logo on the connected Stream Deck.
        /// </summary>
        public void ShowLogo()
        {
            iDevice.WriteFeatureData(KHidFeatureShowLogo);
        }
    }

}
