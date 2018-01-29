using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using StreamDeck = SharpLib.StreamDeck;
using System.Configuration;
using System.Runtime.Serialization;
using CodeProject.Dialog;

namespace StreamDeckDemo
{
    public partial class FormMain : Form
    {
        StreamDeck.Client iClient;
        StreamDeck.Model iStreamDeckModel;

        int iCurrentProfileIndex = 0;
        int iCurrentKeyIndex = 0;

        const int KKeyPaddingInPixels = 8;
        const int KKeyBordersInPixels = KKeyPaddingInPixels * 2;


        private TableLayoutPanel iTableLayoutPanelStreamDeck;

        public FormMain()
        {
            InitializeComponent();

            iClient = new StreamDeck.Client();
            try
            {
                iClient.Open();
            }
            catch
            {
                Trace.WriteLine("ERROR: Could not open Stream Deck!");
            }

            LoadModel();
            if (iStreamDeckModel == null)
            {
                // Create a default model
                iStreamDeckModel = new StreamDeck.Model();
                iStreamDeckModel.Construct();
            }

            CreateStreamDeckControls();
        }

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
            iTableLayoutPanelStreamDeck.Location = new Point(50, 50);
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
            //label.TabIndex = panelIndex;
            //label.TabStop = true;
            // Fetch our text from our model
            label.Text = iStreamDeckModel.Profiles[0].Keys[panelIndex].Text;
            label.Font = iStreamDeckModel.Profiles[0].Keys[panelIndex].Font;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            label.DragDrop += new DragEventHandler(KeyDragDrop);
            label.DragEnter += new DragEventHandler(KeyDragEnter);
            label.Click += new EventHandler(KeyClick);
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
            pictureBox.Image = iStreamDeckModel.Profiles[0].Keys[panelIndex].Bitmap;

            //
            iTableLayoutPanelStreamDeck.Controls.Add(pictureBox, iTableLayoutPanelStreamDeck.ColumnCount - aColumn - 1, aRow);
        }


        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            iClient.Dispose();
            iClient = null;
        }

        /// <summary>
        /// Event triggered when the user drag some stuff over our control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDragEnter(object sender, DragEventArgs e)
        {
            // Just check if our payload is supported and give visual feeadback accordingly
            string filename;
            bool supported = GetDragPayloadFilename(out filename, e);
            if (supported)
            {
                // Supported payload set pointer cursor to "copy" type.
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                // Payload not supported set pointer cursor to "none" type.
                e.Effect = DragDropEffects.None;
            }
        }


        /// <summary>
        /// Event triggered when the user drops some stuff over our control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void KeyDragDrop(object sender, DragEventArgs e)
        {
            string filename;
            bool supported = GetDragPayloadFilename(out filename, e);
            if (supported)
            {
                // Load our bitmap asynchronously
                Bitmap bmp = await Task.Run(() =>
                {
                    return new Bitmap(filename);
                });

                // Render our key
                PictureBox pictureBox=(PictureBox)(((Label)sender).Parent);
                pictureBox.Image = bmp;
                Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                pictureBox.DrawToBitmap(bitmap, pictureBox.ClientRectangle);

                // Upload our render
                int keyIndex = iTableLayoutPanelStreamDeck.Controls.IndexOf(pictureBox);
                iClient.SetKeyBitmap(keyIndex, StreamDeck.KeyBitmap.FromDrawingBitmap(bitmap).CloneBitmapData());
                
                // Store the in our model bitmap in our model
                iStreamDeckModel.Profiles[0].Keys[keyIndex].Bitmap = bmp;

                // Persist our modified model
                SaveModel();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyClick(object sender, EventArgs e)
        {
            // Workout the index of the key that was just clicked
            PictureBox pictureBox = (PictureBox)(((Label)sender).Parent);            
            iCurrentKeyIndex = iTableLayoutPanelStreamDeck.Controls.IndexOf(pictureBox);
            // Load that key into our editor
            EditCurrentKey();
        }


        /// <summary>
        /// Get drag event payload filename if any.
        /// </summary>
        /// <param name="aFilename">Will contain the filename of the drag operation payload.</param>
        /// <param name="aEvent">Our drag event argument.</param>
        /// <returns>True if we support the filename extension, false otherwise.</returns>
        private static bool GetDragPayloadFilename(out string aFilename, DragEventArgs aEvent)
        {
            aFilename = String.Empty;
            if ((aEvent.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)aEvent.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        aFilename = ((string[])data)[0];
                        string ext = Path.GetExtension(aFilename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveModel()
        {
            //string destFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            //destFile += ".streamdeck.xml";
            //
            DataContractSerializer s = new DataContractSerializer(typeof(StreamDeck.Model));
            using (FileStream fs = File.Open("stream-deck.xml", FileMode.Create))
            //using (FileStream fs = File.Open(destFile, FileMode.Create))
            {
                Console.WriteLine("Testing for type: {0}", typeof(StreamDeck.Model));
                s.WriteObject(fs, iStreamDeckModel);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadModel()
        {
            try
            {
                DataContractSerializer s = new DataContractSerializer(typeof(StreamDeck.Model));
                using (FileStream fs = File.Open("stream-deck.xml", FileMode.Open))
                //using (FileStream fs = File.Open(destFile, FileMode.Create))
                {
                    Console.WriteLine("Testing for type: {0}", typeof(StreamDeck.Model));
                    iStreamDeckModel = (StreamDeck.Model)s.ReadObject(fs);
                }
            }
            catch
            {
                Trace.WriteLine("WARNING: Could not load model");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        void EditCurrentKey()
        {
            LoadKeyInEditor(iStreamDeckModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex]);
        }

        StreamDeck.Key CurrentKey { get { return iStreamDeckModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex]; } }
        Label CurrentKeyLabel { get { return (Label)iTableLayoutPanelStreamDeck.Controls[iCurrentKeyIndex].Controls[0]; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aKey"></param>
        void LoadKeyInEditor(StreamDeck.Key aKey)
        {
            iTextBoxKeyEditor.Text = aKey.Text;
            iTextBoxKeyEditor.Font = aKey.Font;
            //iTextBoxKeyEditor.TextAlign = aKey.TextAlign;
        }

        private void iTextBoxKeyEditor_TextChanged(object sender, EventArgs e)
        {
            CurrentKey.Text = iTextBoxKeyEditor.Text;
            CurrentKeyLabel.Text = iTextBoxKeyEditor.Text;
        }

        private void iButtonSave_Click(object sender, EventArgs e)
        {
            SaveModel();
        }

        private void iButtonFont_Click(object sender, EventArgs e)
        {
            //fontDialog.ShowColor = true;
            //fontDialog.ShowApply = true;
            iFontDialog.ShowEffects = true;
            iFontDialog.Font = CurrentKeyLabel.Font;

            //fontDialog.ShowHelp = true;

            //fontDlg.MaxSize = 40;
            //fontDlg.MinSize = 22;

            //fontDialog.Parent = this;
            //fontDialog.StartPosition = FormStartPosition.CenterParent;

            //DlgBox.ShowDialog(fontDialog);

            //if (fontDialog.ShowDialog(this) != DialogResult.Cancel)
            if (DlgBox.ShowDialog(iFontDialog) != DialogResult.Cancel)
            {
                //Save font settings
                CurrentKeyLabel.Font = iFontDialog.Font;
                CurrentKey.Font = iFontDialog.Font;
                iTextBoxKeyEditor.Font = iFontDialog.Font;
                SaveModel();
            }
        }
    }
}
