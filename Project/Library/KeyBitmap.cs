using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.StreamDeck
{
    /// <summary>
    /// Represents a bitmap that can be used as key images
    /// </summary>
    public class KeyBitmap
    {
        /// <summary>
        /// Solid black bitmap
        /// </summary>
        /// <remarks>
        /// If you need a black bitmap (for example to clear keys) use this property for better performance (in theory ^^)
        /// </remarks>
        public static KeyBitmap Black { get { return black; } }
        private static readonly KeyBitmap black = new KeyBitmap(null);

        internal readonly byte[] rawBitmapData;

        /// <summary>
        /// Returns a copy of the internal bitmap.
        /// </summary>
        /// <returns></returns>
        public byte[] CloneBitmapData()
        {
            return (byte[])rawBitmapData.Clone();
        }

        internal KeyBitmap(byte[] bitmapData)
        {
            if (bitmapData != null)
            {
                if (bitmapData.Length != Client.rawBitmapDataLength) throw new NotSupportedException("Unsupported bitmap array length");
                this.rawBitmapData = bitmapData;
            }
        }

        public static KeyBitmap FromRawBitmap(byte[] bitmapData)
        {
            return new KeyBitmap(bitmapData);
        }

        /// <summary>
        /// Creates a solid color bitmap
        /// </summary>
        /// <param name="R">Red channel</param>
        /// <param name="G">Green channel</param>
        /// <param name="B">Blue channel</param>
        /// <returns></returns>
        public static KeyBitmap FromRGBColor(byte R, byte G, byte B)
        {
            //If everything is 0 (black) take a shortcut ;-)
            if (R == 0 && G == 0 && B == 0) return Black;

            var buffer = new byte[Client.rawBitmapDataLength];
            for (int i = 0; i < buffer.Length; i += 3)
            {
                buffer[i + 0] = B;
                buffer[i + 1] = G;
                buffer[i + 2] = R;
            }

            return new KeyBitmap(buffer);
        }

        public static KeyBitmap FromStream(Stream bitmapStream)
        {
            using (Bitmap bitmap = (Bitmap)Image.FromStream(bitmapStream))
            {
                return FromDrawingBitmap(bitmap);
            }
        }

        public static KeyBitmap FromFile(string bitmapFile)
        {
            using (Bitmap bitmap = (Bitmap)Image.FromFile(bitmapFile))
            {
                return FromDrawingBitmap(bitmap);
            }
        }

        public static KeyBitmap FromDrawingBitmap(Bitmap bitmap)
        {
            if (bitmap.Width != Client.iconSize || bitmap.Height != Client.iconSize) throw new NotSupportedException("Unsupported bitmap dimensions");
           
            BitmapData data = null;
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
                var managedRGB = new byte[Client.rawBitmapDataLength];

                unsafe
                {
                    byte* bdata = (byte*)data.Scan0;

                    //TODO: This should be cleaned up
                    //I'm locking for a different approach to parse different PixelFormats without
                    //copying 90% of the code ;-)
                    if (data.PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        for (int y = 0; y < Client.iconSize; y++)
                        {
                            for (int x = 0; x < Client.iconSize; x++)
                            {
                                var ps = data.Stride * y + x * 3;
                                var pt = Client.iconSize * 3 * (y + 1) - (x + 1) * 3;
                                managedRGB[pt + 0] = bdata[ps + 0];
                                managedRGB[pt + 1] = bdata[ps + 1];
                                managedRGB[pt + 2] = bdata[ps + 2];
                            }
                        }
                    }
                    else if (data.PixelFormat == PixelFormat.Format32bppArgb)
                    {
                        for (int y = 0; y < Client.iconSize; y++)
                        {
                            for (int x = 0; x < Client.iconSize; x++)
                            {
                                var ps = data.Stride * y + x * 4;
                                var pt = Client.iconSize * 3 * (y + 1) - (x + 1) * 3;
                                double alpha = (double)bdata[ps + 3] / 255f;
                                managedRGB[pt + 0] = (byte)Math.Round(bdata[ps + 0] * alpha);
                                managedRGB[pt + 1] = (byte)Math.Round(bdata[ps + 1] * alpha);
                                managedRGB[pt + 2] = (byte)Math.Round(bdata[ps + 2] * alpha);
                            }
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Unsupported pixel format");
                    }
                }

                return new KeyBitmap(managedRGB);
            }
            finally
            {
                if (data != null)
                    bitmap.UnlockBits(data);
            }
        }
    }
}
