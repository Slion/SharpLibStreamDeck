using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using StreamDeck = SharpLib.StreamDeck;

namespace StreamDeckDemo
{
    public partial class FormMain : Form
    {
        StreamDeck.Client iClient;
        protected bool validData;
        string path;
        protected Image image;
        protected Thread getImageThread;

        public FormMain()
        {
            InitializeComponent();

            ((Control)pictureBox1).AllowDrop = true;

            iClient = new StreamDeck.Client();
            iClient.Open();

        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            iClient.Dispose();
            iClient = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                path = filename;
                getImageThread = new Thread(new ThreadStart(LoadImage));
                getImageThread.Start();
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                pictureBox1.Image = image;
                Bitmap bitmap = new Bitmap(pictureBox1.Width,pictureBox1.Height);
                pictureBox1.DrawToBitmap(bitmap,pictureBox1.ClientRectangle);
                //iClient.SetKeyBitmap(1, StreamDeck.KeyBitmap.FromFile(path).CloneBitmapData());
                iClient.SetKeyBitmap(0, StreamDeck.KeyBitmap.FromDrawingBitmap(bitmap).CloneBitmapData());
            }
        }

        protected void LoadImage()
        {
            image = new Bitmap(path);
        }

        private void pictureBox1_DragLeave(object sender, EventArgs e)
        {

        }

        private bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;
            filename = String.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

    }
}
