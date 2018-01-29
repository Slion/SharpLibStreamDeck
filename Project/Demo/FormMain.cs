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

            PopulateProfiles();            
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateStreamDeckControls()
        {
            SuspendLayout();

            // Trash existing control first 
            if (iTableLayoutPanelStreamDeck != null)
            {
                Controls.Remove(iTableLayoutPanelStreamDeck);
                iTableLayoutPanelStreamDeck.Dispose();
                iTableLayoutPanelStreamDeck = null;
            }

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
            label.Margin = new Padding(KKeyPaddingInPixels);
            //label.Name = "label1";
            label.Size = new System.Drawing.Size(72, 72);
            //label.TabIndex = panelIndex;
            //label.TabStop = true;
            // Fetch our text from our model
            label.Text = CurrentProfile.Keys[panelIndex].Text;
            label.Font = CurrentProfile.Keys[panelIndex].Font;
            label.TextAlign = CurrentProfile.Keys[panelIndex].TextAlign;
            label.ForeColor = CurrentProfile.Keys[panelIndex].FontColor;
            label.BackgroundImage = CurrentProfile.Keys[panelIndex].Bitmap;
            label.BackgroundImageLayout = ImageLayout.Stretch;

            // Hook in event handlers
            label.DragDrop += new DragEventHandler(KeyDragDrop);
            label.DragEnter += new DragEventHandler(KeyDragEnter);
            label.Click += new EventHandler(KeyClick);

            //
            iTableLayoutPanelStreamDeck.Controls.Add(label, iTableLayoutPanelStreamDeck.ColumnCount - aColumn - 1, aRow);
        }


        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveModel();
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
                Label ctrl = sender as Label;
                ctrl.BackgroundImage = bmp;
                Bitmap bitmap = new Bitmap(ctrl.Width, ctrl.Height);
                ctrl.DrawToBitmap(bitmap, ctrl.ClientRectangle);

                // Upload our render
                int keyIndex = iTableLayoutPanelStreamDeck.Controls.IndexOf(ctrl);
                iClient.SetKeyBitmap(keyIndex, StreamDeck.KeyBitmap.FromDrawingBitmap(bitmap).CloneBitmapData());
                
                // Store the bitmap in our model bitmap in our model
                iStreamDeckModel.Profiles[iCurrentProfileIndex].Keys[keyIndex].Bitmap = bmp;

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
            iCurrentKeyIndex = iTableLayoutPanelStreamDeck.Controls.IndexOf(sender as Label);
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
        Label CurrentKeyLabel { get { return iTableLayoutPanelStreamDeck.Controls[iCurrentKeyIndex] as Label; } }
        StreamDeck.Profile CurrentProfile { get { return iStreamDeckModel.Profiles[iCurrentProfileIndex]; } }

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
            SaveModelAndReload();
        }

        void SaveModelAndReload()
        {
            SaveModel();

            // Make sure we update our profile list in case profile name was edited 
            PopulateProfiles();
            iComboBoxProfiles.SelectedItem = CurrentProfile.Name;
        }

        private void iButtonFont_Click(object sender, EventArgs e)
        {
            iFontDialog.ShowColor = true;
            //fontDialog.ShowApply = true;
            iFontDialog.ShowEffects = true;
            iFontDialog.Font = CurrentKeyLabel.Font;
            iFontDialog.Color = CurrentKeyLabel.ForeColor;

            if (DlgBox.ShowDialog(iFontDialog) != DialogResult.Cancel)
            {
                //Save font settings
                CurrentKeyLabel.Font = iFontDialog.Font;
                CurrentKey.Font = iFontDialog.Font;
                CurrentKeyLabel.ForeColor = iFontDialog.Color;
                CurrentKey.FontColor = iFontDialog.Color;

                iTextBoxKeyEditor.Font = iFontDialog.Font;
                
                SaveModelAndReload();
            }
        }

        private void iButtonFontColor_Click(object sender, EventArgs e)
        {

        }

        private void iButtonNewProfile_Click(object sender, EventArgs e)
        {
            CreateNewProfile();
        }

        private void iButtonDeleteProfile_Click(object sender, EventArgs e)
        {
            DeleteCurrentProfile();
        }

        void DeleteCurrentProfile()
        {
            iStreamDeckModel.Profiles.Remove(CurrentProfile);
            // Create default profile if none present
            if (iStreamDeckModel.Profiles.Count==0)
            {
                iStreamDeckModel.CreateDefaultProfile();
            }
            SaveModelAndReload();
        }

        void CreateNewProfile()
        {
            StreamDeck.Profile profile = new StreamDeck.Profile();
            profile.Construct();            
            iStreamDeckModel.Profiles.Add(profile);
            profile.Name = "Profile " + iStreamDeckModel.Profiles.Count.ToString();
            PopulateProfiles();
            iComboBoxProfiles.SelectedItem = profile.Name;
        }

        void PopulateProfiles()
        {
            iComboBoxProfiles.Items.Clear();
            foreach (StreamDeck.Profile profile in iStreamDeckModel.Profiles)
            {
                iComboBoxProfiles.Items.Add(profile.Name);
            }
        }

        private void iComboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCurrentProfileIndex = iComboBoxProfiles.SelectedIndex;
            LoadCurrentProfile();
        }

        void LoadCurrentProfile()
        {
            CreateStreamDeckControls();
            UploadAllKeys(); 
        }

        private void iComboBoxProfiles_TextUpdate(object sender, EventArgs e)
        {
            CurrentProfile.Name = iComboBoxProfiles.Text;
        }

        void UploadAllKeys()
        {
            for (int i = 0; i < StreamDeck.Client.numOfKeys; i++)
            {
                UploadKey(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aIndex"></param>
        void UploadKey(int aIndex)
        {
            // Render our key
            Label ctrl = iTableLayoutPanelStreamDeck.Controls[aIndex] as Label;
            Bitmap bitmap = new Bitmap(ctrl.Width, ctrl.Height);
            ctrl.DrawToBitmap(bitmap, ctrl.ClientRectangle);

            // Upload our render
            iClient.SetKeyBitmap(aIndex, StreamDeck.KeyBitmap.FromDrawingBitmap(bitmap).CloneBitmapData());
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // Will trigger our first profile load and render
            // We had to delay this otherwise labels would not be rendered
            iComboBoxProfiles.SelectedIndex = 0;
        }
    }
}
