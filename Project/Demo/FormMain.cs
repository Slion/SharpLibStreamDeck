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

        private TableLayoutPanel iTableLayoutPanelStreamDeck;

        public FormMain()
        {
            InitializeComponent();

            iClient = new StreamDeck.Client();
            iClient.Open();

            CreateStreamDeckControls();
        }


        const int KKeyPaddingInPixels = 2;
        const int KKeyBordersInPixels = KKeyPaddingInPixels * 2;

        /// <summary>
        /// 
        /// </summary>
        private void CreateStreamDeckControls()
        {

            SuspendLayout();
            //
            CreateStreamDeckTableLayoutPanel();

            //For each row
            for (int j=0; j < iTableLayoutPanelStreamDeck.RowCount; j++)
            {
                //For each column
                for (int i = 0; i < iTableLayoutPanelStreamDeck.ColumnCount; i++)
                {
                    CreateStreamDeckKeyControls(i,j);
                }
            }

            ResumeLayout(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateStreamDeckTableLayoutPanel()
        {
            //
            // Create table panel
            //
            iTableLayoutPanelStreamDeck = new TableLayoutPanel();
            iTableLayoutPanelStreamDeck.Location = new Point(89, 58);
            iTableLayoutPanelStreamDeck.Margin = new Padding(0);
            int widthInPixels = iClient.ColumnCount * (iClient.KeyWidthInpixels + KKeyBordersInPixels);
            int heightInPixels = iClient.RowCount * (iClient.KeyHeightInpixels + KKeyBordersInPixels);
            iTableLayoutPanelStreamDeck.Size = new Size(widthInPixels, heightInPixels);

            iTableLayoutPanelStreamDeck.BackColor = SystemColors.ControlDarkDark;
            iTableLayoutPanelStreamDeck.ColumnCount = iClient.ColumnCount;
            // Create columns
            for (int i = 0; i < iClient.ColumnCount; i++)
            {
                iTableLayoutPanelStreamDeck.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, iClient.KeyWidthInpixels + KKeyBordersInPixels));
            }

            // Create rows
            iTableLayoutPanelStreamDeck.RowCount = iClient.RowCount;
            for (int i = 0; i < iClient.ColumnCount; i++)
            {
                iTableLayoutPanelStreamDeck.RowStyles.Add(new RowStyle(SizeType.Absolute, iClient.KeyHeightInpixels + KKeyBordersInPixels));
            }

            // Add table to our form
            Controls.Add(iTableLayoutPanelStreamDeck);

            //iTableLayoutPanelStreamDeck.TabIndex = 1;
            //iTableLayoutPanelStreamDeck.Name = "iTableLayoutPanelStreamDeck";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aColumn"></param>
        /// <param name="aRow"></param>
        private void CreateStreamDeckKeyControls(int aColumn, int aRow)
        {
            int panelIndex = aColumn + iTableLayoutPanelStreamDeck.ColumnCount * aRow;

            // 
            // Create label
            // 
            Label label = new Label();
            label.AllowDrop = true;
            label.BackColor = System.Drawing.Color.Transparent;
            label.Location = new System.Drawing.Point(0, 0);
            label.Margin = new Padding(0);
            //label.Name = "label1";
            label.Size = new System.Drawing.Size(72, 72);
            //label.TabIndex = 19;
            label.Text = panelIndex.ToString();
            label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            label.DragDrop += new DragEventHandler(label_DragDrop);
            label.DragEnter += new DragEventHandler(label_DragEnter);
            // 
            // Create picture box
            // 
            PictureBox pictureBox = new PictureBox();
            pictureBox.Controls.Add(label);
            pictureBox.BackColor = System.Drawing.SystemColors.Control;
            pictureBox.Location = new System.Drawing.Point(0, 0);
            pictureBox.Margin = new Padding(KKeyPaddingInPixels);
            //pictureBox1.Name = "pictureBox1";
            pictureBox.Size = new System.Drawing.Size(72, 72);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox1.TabIndex = 16;
            pictureBox.TabStop = false;

            //
            iTableLayoutPanelStreamDeck.Controls.Add(pictureBox, iTableLayoutPanelStreamDeck.ColumnCount - aColumn - 1, aRow);
        }


        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            iClient.Dispose();
            iClient = null;
        }


        private void label_DragEnter(object sender, DragEventArgs e)
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

        private void label_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }

                PictureBox pictureBox=(PictureBox)(((Label)sender).Parent);
                pictureBox.Image = image;
                Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                pictureBox.DrawToBitmap(bitmap, pictureBox.ClientRectangle);

                int keyIndex = iTableLayoutPanelStreamDeck.Controls.IndexOf(pictureBox);
                //iClient.SetKeyBitmap(1, StreamDeck.KeyBitmap.FromFile(path).CloneBitmapData());
                iClient.SetKeyBitmap(keyIndex, StreamDeck.KeyBitmap.FromDrawingBitmap(bitmap).CloneBitmapData());
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
