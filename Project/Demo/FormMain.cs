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

        //Panel[] iKeyPanels;

        public FormMain()
        {
            InitializeComponent();

            //((Control)pictureBox1).AllowDrop = true;
            //((Control)panel1).AllowDrop = true;

            iClient = new StreamDeck.Client();
            iClient.Open();

            CreateKeyControls();
        }

        private void CreateKeyControls()
        {
            //iKeyPanels = new Panel[iClient.KeyCount];

            SuspendLayout();

            const int KKeyPaddingInPixels = 2;
            const int KKeyBordersInPixels = KKeyPaddingInPixels * 2;
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
            for (int i=0;i<iClient.ColumnCount;i++)
            {
                iTableLayoutPanelStreamDeck.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, iClient.KeyWidthInpixels+KKeyBordersInPixels));
            }
            
            iTableLayoutPanelStreamDeck.RowCount = iClient.RowCount;
            for (int i = 0; i < iClient.ColumnCount; i++)
            {
                iTableLayoutPanelStreamDeck.RowStyles.Add(new RowStyle(SizeType.Absolute, iClient.KeyHeightInpixels+KKeyBordersInPixels));
            }

            // Add table to our form
            Controls.Add(iTableLayoutPanelStreamDeck);

            //iTableLayoutPanelStreamDeck.TabIndex = 1;
            //iTableLayoutPanelStreamDeck.Name = "iTableLayoutPanelStreamDeck";

            //For each row
            for (int j=0; j < iTableLayoutPanelStreamDeck.RowCount; j++)
            {
                //For each column
                for (int i = 0; i < iTableLayoutPanelStreamDeck.ColumnCount; i++)
                {
                    int panelIndex = i + iTableLayoutPanelStreamDeck.ColumnCount * j;
                    //Panel panel = iKeyPanels[panelIndex];

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
                    label.DragDrop += new DragEventHandler(label1_DragDrop);
                    label.DragEnter += new DragEventHandler(label1_DragEnter);
                    // 
                    // Create picture box
                    // 
                    PictureBox pictureBox= new PictureBox();
                    pictureBox.Controls.Add(label);
                    pictureBox.BackColor = System.Drawing.SystemColors.Control;
                    pictureBox.Location = new System.Drawing.Point(0, 0);
                    pictureBox.Margin = new Padding(2);
                    //pictureBox1.Name = "pictureBox1";
                    pictureBox.Size = new System.Drawing.Size(72, 72);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    //pictureBox1.TabIndex = 16;
                    pictureBox.TabStop = false;
                    //
                    // Setup panel
                    //
                    //panel.Controls.Add(pictureBox);
                    //panel.Location = new System.Drawing.Point(2, 154);
                    //panel.Margin = new Padding(2);
                    //panel.Size = new System.Drawing.Size(72, 72);
                    //panel.TabIndex = panelIndex;

                    //
                    iTableLayoutPanelStreamDeck.Controls.Add(pictureBox, iTableLayoutPanelStreamDeck.ColumnCount-i-1, j);
                }
            }

            ResumeLayout(false);
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            iClient.Dispose();
            iClient = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_DragEnter(object sender, DragEventArgs e)
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

        private void label1_DragDrop(object sender, DragEventArgs e)
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
