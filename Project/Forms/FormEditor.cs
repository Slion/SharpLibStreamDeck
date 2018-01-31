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


namespace SharpLib.StreamDeck
{
    public partial class FormEditor : Form
    {
        StreamDeck.Client iClient;
        StreamDeck.Model iModel;

        int iCurrentProfileIndex = 0;
        int iCurrentKeyIndex = 0;

        const int KKeyPaddingInPixels = 8;
        const int KKeyBordersInPixels = KKeyPaddingInPixels * 2;

        private TableLayoutPanel iTableLayoutPanelStreamDeck;

        public FormEditor()
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
            if (iModel == null)
            {
                // Create a default model
                iModel = new StreamDeck.Model();
                iModel.Construct();
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
            iTableLayoutPanelStreamDeck.Location = new Point(62, 50);
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
            StreamDeck.Label label = new StreamDeck.Label();
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
            label.OutlineColor = CurrentProfile.Keys[panelIndex].OutlineColor;
            label.OutlineThickness = CurrentProfile.Keys[panelIndex].OutlineThickness;
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

                // Work out drop target key index and make it current
                StreamDeck.Label ctrl = sender as StreamDeck.Label;
                iCurrentKeyIndex = iTableLayoutPanelStreamDeck.Controls.IndexOf(ctrl);
                // Store the bitmap in our model bitmap in our model
                iModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex].Bitmap = bmp;
                UploadKey(iCurrentKeyIndex);
                EditCurrentKey();
                                              
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
                s.WriteObject(fs, iModel);
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
                    iModel = (StreamDeck.Model)s.ReadObject(fs);
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
            LoadKeyInEditor(iModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex]);
        }

        StreamDeck.Key CurrentKey { get { return iModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex]; } }
        StreamDeck.Label CurrentKeyLabel { get { return iTableLayoutPanelStreamDeck.Controls[iCurrentKeyIndex] as StreamDeck.Label; } }
        StreamDeck.Profile CurrentProfile { get { return iModel.Profiles[iCurrentProfileIndex]; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aKey"></param>
        void LoadKeyInEditor(StreamDeck.Key aKey)
        {
            iTextBoxKeyEditor.Text = aKey.Text;
            iTextBoxKeyEditor.Font = aKey.Font;
            SetupTextAlignButtons(aKey.TextAlign);
            iNumericOutlineThickness.Value = Convert.ToDecimal(aKey.OutlineThickness);
        }

        void SetupTextAlignButtons(ContentAlignment aContentAlignment)
        {
            iButtonTextAlignTopLeft.BackColor = (aContentAlignment == ContentAlignment.TopLeft ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignTopCenter.BackColor = (aContentAlignment == ContentAlignment.TopCenter ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignTopRight.BackColor = (aContentAlignment == ContentAlignment.TopRight ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignMiddleLeft.BackColor = (aContentAlignment == ContentAlignment.MiddleLeft ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignMiddleCenter.BackColor = (aContentAlignment == ContentAlignment.MiddleCenter ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignMiddleRight.BackColor = (aContentAlignment == ContentAlignment.MiddleRight ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignBottomLeft.BackColor = (aContentAlignment == ContentAlignment.BottomLeft ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignBottomCenter.BackColor = (aContentAlignment == ContentAlignment.BottomCenter ? Color.LightBlue : Color.LightGray);
            iButtonTextAlignBottomRight.BackColor = (aContentAlignment == ContentAlignment.BottomRight ? Color.LightBlue : Color.LightGray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTextAlignClick(object sender, EventArgs e)
        {
            iButtonSave.Focus(); // As we don't want focus on the button
            ContentAlignment alignment = (sender as Button).TextAlign;
            CurrentKey.TextAlign = alignment;
            CurrentKeyLabel.TextAlign = alignment;
            SetupTextAlignButtons(alignment);
        }

        /// <summary>
        /// User is changing text of the current key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            iFontDialog.ShowEffects = true;
            iFontDialog.Font = CurrentKeyLabel.Font;

            if (Forms.DlgBox.ShowDialog(iFontDialog) != DialogResult.Cancel)
            {
                //Apply new font to key label
                CurrentKeyLabel.Font = iFontDialog.Font;
                //Apply new font to model
                CurrentKey.Font = iFontDialog.Font;
                //Apply new font to label editor
                iTextBoxKeyEditor.Font = iFontDialog.Font;
                
                SaveModelAndReload();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iButtonFontColor_Click(object sender, EventArgs e)
        {
            iColorDialog.Color = CurrentKeyLabel.ForeColor;

            if (Forms.DlgBox.ShowDialog(iColorDialog) != DialogResult.Cancel)
            {
                //Save font settings
                CurrentKeyLabel.ForeColor = iColorDialog.Color;
                CurrentKey.FontColor = iColorDialog.Color;
                //
                SaveModelAndReload();
            }
        }

        private void iButtonOutlineColor_Click(object sender, EventArgs e)
        {
            iColorDialog.Color = CurrentKeyLabel.OutlineColor;

            if (Forms.DlgBox.ShowDialog(iColorDialog) != DialogResult.Cancel)
            {
                //Save font settings
                CurrentKeyLabel.OutlineColor = iColorDialog.Color;
                CurrentKey.OutlineColor = iColorDialog.Color;
                //
                SaveModelAndReload();
            }

        }

        private void iButtonBitmapColor_Click(object sender, EventArgs e)
        {

            if (Forms.DlgBox.ShowDialog(iColorDialog) != DialogResult.Cancel)
            {
                //Save font settings                
                CurrentKey.FillBitmap(iColorDialog.Color);
                //
                SaveModelAndReload();
            }
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
            iModel.Profiles.Remove(CurrentProfile);
            // Create default profile if none present
            if (iModel.Profiles.Count==0)
            {
                iModel.CreateDefaultProfile();
            }

            iCurrentProfileIndex = 0;
            SaveModelAndReload();
        }

        void CreateNewProfile()
        {
            StreamDeck.Profile profile = new StreamDeck.Profile();
            profile.Construct();            
            iModel.Profiles.Add(profile);
            profile.Name = "Profile " + iModel.Profiles.Count.ToString();
            PopulateProfiles();
            iComboBoxProfiles.SelectedItem = profile.Name;
        }

        void PopulateProfiles()
        {
            iComboBoxProfiles.Items.Clear();
            foreach (StreamDeck.Profile profile in iModel.Profiles)
            {
                iComboBoxProfiles.Items.Add(profile.Name);
            }
        }

        private void iComboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            iCurrentProfileIndex = iComboBoxProfiles.SelectedIndex;
            LoadCurrentProfile();
            EditCurrentKey();
        }

        /// <summary>
        /// Load current profile into UI and Stream Deck
        /// </summary>
        void LoadCurrentProfile()
        {
            // Deal with brightness
            iTrackBarBrightness.Value = CurrentProfile.Brightness;
            iClient.SetBrightness(CurrentProfile.Brightness);
            // Update controls
            CreateStreamDeckControls();
            // Upload keys to Stream Deck
            UploadAllKeys();
        }

        /// <summary>
        /// User is editing current profile name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iComboBoxProfiles_TextUpdate(object sender, EventArgs e)
        {
            CurrentProfile.Name = iComboBoxProfiles.Text;
        }


        /// <summary>
        /// Render and upload all keys from the current profile.
        /// </summary>
        void UploadAllKeys()
        {
            for (int i = 0; i < StreamDeck.Client.numOfKeys; i++)
            {
                UploadKey(i);
            }
        }

        /// <summary>
        /// Render and upload a key bitmap to our Stream Deck device.
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

        /// <summary>
        /// User adjusting font outline thickness.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iNumericOutlineThickness_ValueChanged(object sender, EventArgs e)
        {
            CurrentKey.OutlineThickness = (float)iNumericOutlineThickness.Value;
            CurrentKeyLabel.OutlineThickness = (float)iNumericOutlineThickness.Value;
        }

        private void iTrackBarBrightness_Scroll(object sender, EventArgs e)
        {
            // Update our Stream Deck brightness
            iClient.SetBrightness((byte)iTrackBarBrightness.Value);
            // Save it into our profile
            CurrentProfile.Brightness = (byte)iTrackBarBrightness.Value;
        }
    }
}
