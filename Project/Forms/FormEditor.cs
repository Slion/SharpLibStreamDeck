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
        protected Client iClient;
        protected Model iModel;

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
                // Register handler
                iClient.KeyPressed += StreamDeckKeyPressed;
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
        /// Provide access to events' combo box so that it can be filled by clients.
        /// </summary>
        public ComboBox ComboBoxEvents { get { return  iComboBoxEvents; } }
            

        /// <summary>
        /// Derived class should use that.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void StreamDeckKeyPressed(object sender, KeyEventArgs e)
        {

        }


        /// <summary>
        /// Load current profile into our UI
        /// </summary>
        private void UpdateStreamDeckControls()
        {
            if (iTableLayoutPanelStreamDeck==null)
            {
                // First time around 
                CreateStreamDeckControls();
                return;
            }

            int i = 0;
            foreach (Key key in CurrentProfile.Keys)
            {
                LoadKeyInLabel(key, iTableLayoutPanelStreamDeck.Controls[i]);
                i++;
            }
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
            iTableLayoutPanelStreamDeck.CellPaint += TableCellPaint;
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
        /// Used to draw a focus effect arround the Stream Deck key which is currently being edited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableCellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            // Check if current key index matches this cell
            if (iCurrentKeyIndex==KeyIndexForObject(iTableLayoutPanelStreamDeck.GetControlFromPosition(e.Column, e.Row)))
            {
                e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
            }
        }


        private void LoadKeyInLabel(Key aKey, object aLabel)
        {
            Label label = aLabel as Label;

            label.Text = aKey.Text;
            label.Font = aKey.Font;
            label.TextAlign = aKey.TextAlign;
            label.ForeColor = aKey.FontColor;
            label.OutlineColor = aKey.OutlineColor;
            label.OutlineThickness = aKey.OutlineThickness;
            label.BackgroundImage = aKey.Bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aColumn"></param>
        /// <param name="aRow"></param>
        private void CreateStreamDeckKeyControls(int aColumn, int aRow)
        {
            // Compute our key index, it matches hardware index and control index in table control collection
            int keyIndex = aColumn + iTableLayoutPanelStreamDeck.ColumnCount * aRow;

            // 
            // Create label
            // 
            StreamDeck.Label label = new StreamDeck.Label();
            label.AllowDrop = true;
            label.BackColor = Color.Transparent;
            label.Location = new Point(0, 0);
            label.Margin = new Padding(KKeyPaddingInPixels);
            label.BackgroundImageLayout = ImageLayout.Stretch;
            //label.Name = "label1";
            label.Size = new Size(Client.KKeyWidthInPixels, Client.KKeyHeightInPixels);
            //label.TabIndex = panelIndex;
            //label.TabStop = true;

            // Hook in event handlers
            label.DragDrop += new DragEventHandler(KeyDragDrop);
            label.DragEnter += new DragEventHandler(KeyDragEnter);
            label.Click += new EventHandler(KeyClick);
            label.MouseDown += new MouseEventHandler(KeyMouseDown);

            // Push key model data into label control
            LoadKeyInLabel(CurrentProfile.Keys[keyIndex], label);            

            //
            iTableLayoutPanelStreamDeck.Controls.Add(label, iTableLayoutPanelStreamDeck.ColumnCount - aColumn - 1, aRow);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoDispose()
        {
            SaveModel();
            iClient.Dispose();
            iClient = null;
        }

        /// <summary>
        /// Fetch the key index corresponding to the give object.
        /// </summary>
        /// <param name="aObject"></param>
        /// <returns></returns>
        int KeyIndexForObject(object aObject)
        {
            return iTableLayoutPanelStreamDeck.Controls.IndexOf(aObject as Label);
        }

        /// <summary>
        /// Fetch the key corresponding to the give object.
        /// </summary>
        /// <param name="aObject"></param>
        /// <returns></returns>
        Key KeyForObject(object aObject)
        {
            return CurrentProfile.Keys[KeyIndexForObject(aObject)];
        }

        /// <summary>
        /// Provide access to a key at the given index.
        /// </summary>
        /// <param name="aIndex"></param>
        /// <returns></returns>
        protected Key KeyForIndex(int aIndex)
        {
            return CurrentProfile.Keys[aIndex];
        }

        /// <summary>
        /// Fetch index for given key.
        /// </summary>
        /// <param name="aKey"></param>
        /// <returns></returns>
        int KexIndex(Key aKey)
        {
            return CurrentProfile.Keys.IndexOf(aKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyMouseDown(object sender, MouseEventArgs e)
        {
            // Workout the index of the key that was just clicked
            iCurrentKeyIndex = KeyIndexForObject(sender);
            // Load that key into our editor
            EditCurrentKey();

            ((Label)sender).DoDragDrop(CurrentKey, DragDropEffects.Move);
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
            else if (e.Data.GetDataPresent(typeof(Key)) 
                // Make sure we are not trying to drop it on itself
                && KeyForObject(sender) != e.Data.GetData(typeof(Key)) as Key)
            {
                e.Effect = DragDropEffects.Move;
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
                // Set target key as current
                iCurrentKeyIndex = KeyIndexForObject(ctrl);
                // Update our UI
                ctrl.BackgroundImage = bmp;
                // Store the bitmap in our model
                CurrentKey.Bitmap = bmp;
                // Upload our key to Stream Deck
                UploadKey(iCurrentKeyIndex);
                // Load target key is editor
                EditCurrentKey();
                                              
                // Persist our modified model
                SaveModel();
            }
            else if (e.Data.GetDataPresent(typeof(Key)))
            {
                // We are dropping a key on another key
                // Just swap them then
                Key source = e.Data.GetData(typeof(Key)) as Key;
                int sourceIndex = CurrentProfile.Keys.IndexOf(source);
                int targetIndex = KeyIndexForObject(sender);
                Key target = CurrentProfile.Keys[targetIndex];
                // Swap target and source
                CurrentProfile.Keys[targetIndex] = source;
                CurrentProfile.Keys[sourceIndex] = target;

                iCurrentKeyIndex = targetIndex;
                SaveModelAndReload();
                EditCurrentKey();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyClick(object sender, EventArgs e)
        {
            // That's now being down on mouse down event
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
        public virtual void SaveModel()
        {
            DoSaveModel("stream-deck.xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFilename"></param>
        public void DoSaveModel(string aFilename)
        {
            //string destFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            //destFile += ".streamdeck.xml";
            //
            DataContractSerializer s = new DataContractSerializer(typeof(Model));
            using (FileStream fs = File.Open(aFilename, FileMode.Create))
            {
                s.WriteObject(fs, iModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadModel()
        {
            DoLoadModel("stream-deck.xml");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFilename"></param>
        public void DoLoadModel(string aFilename)
        {
            try
            {
                DataContractSerializer s = new DataContractSerializer(typeof(StreamDeck.Model));
                using (FileStream fs = File.Open(aFilename, FileMode.Open))
                //using (FileStream fs = File.Open(destFile, FileMode.Create))
                {
                    iModel = (Model)s.ReadObject(fs);
                }
            }
            catch
            {
                Trace.WriteLine("WARNING: Could not load Stream Deck Model");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void EditCurrentKey()
        {
            LoadKeyInEditor(iModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex]);
            // Make sure we show our current key highlight properly
            // We will need to redraw our table then
            iTableLayoutPanelStreamDeck.Invalidate();
        }

        Key CurrentKey { get { return iModel.Profiles[iCurrentProfileIndex].Keys[iCurrentKeyIndex]; } }
        Label CurrentKeyLabel { get { return iTableLayoutPanelStreamDeck.Controls[iCurrentKeyIndex] as StreamDeck.Label; } }
        protected Profile CurrentProfile { get { return iModel.Profiles[iCurrentProfileIndex]; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aKey"></param>
        void LoadKeyInEditor(StreamDeck.Key aKey)
        {
            iTextBoxKeyEditor.Text = aKey.Text;
            iTextBoxKeyEditor.Font = aKey.Font;
            iComboBoxEvents.SelectedItem = aKey.EventName;
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
            UpdateStreamDeckControls();
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
            for (int i = 0; i < StreamDeck.Client.KKeyCount; i++)
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

        private void iButtonApplyToAll_Click(object sender, EventArgs e)
        {
            //Copy current key style to all keys
            foreach (Key key in CurrentProfile.Keys)
            {
                if (key != CurrentKey)
                {
                    key.CopyStyle(CurrentKey);
                }
            }

            SaveModelAndReload();
        }


        private void iComboBoxEvents_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CurrentKey.EventName = iComboBoxEvents.Text;
            SaveModel();
        }
    }
}
