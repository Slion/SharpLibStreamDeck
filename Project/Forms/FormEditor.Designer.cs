namespace SharpLib.StreamDeck
{
    partial class FormEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            DoDispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.iTextBoxKeyEditor = new System.Windows.Forms.TextBox();
            this.iButtonSave = new System.Windows.Forms.Button();
            this.iButtonFont = new System.Windows.Forms.Button();
            this.iFontDialog = new System.Windows.Forms.FontDialog();
            this.iComboBoxProfiles = new System.Windows.Forms.ComboBox();
            this.iButtonNewProfile = new System.Windows.Forms.Button();
            this.iButtonDeleteProfile = new System.Windows.Forms.Button();
            this.iColorDialog = new System.Windows.Forms.ColorDialog();
            this.iButtonFontColor = new System.Windows.Forms.Button();
            this.iGroupBoxTextAlign = new System.Windows.Forms.GroupBox();
            this.iButtonTextAlignBottomLeft = new System.Windows.Forms.Button();
            this.iButtonTextAlignBottomCenter = new System.Windows.Forms.Button();
            this.iButtonTextAlignMiddleLeft = new System.Windows.Forms.Button();
            this.iButtonTextAlignMiddleCenter = new System.Windows.Forms.Button();
            this.iButtonTextAlignTopLeft = new System.Windows.Forms.Button();
            this.iButtonTextAlignTopCenter = new System.Windows.Forms.Button();
            this.iButtonTextAlignBottomRight = new System.Windows.Forms.Button();
            this.iButtonTextAlignMiddleRight = new System.Windows.Forms.Button();
            this.iButtonTextAlignTopRight = new System.Windows.Forms.Button();
            this.iButtonBitmapColor = new System.Windows.Forms.Button();
            this.iButtonOutlineColor = new System.Windows.Forms.Button();
            this.iNumericOutlineThickness = new System.Windows.Forms.NumericUpDown();
            this.iTrackBarBrightness = new System.Windows.Forms.TrackBar();
            this.iButtonApplyToAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.iComboBoxEvents = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.iGroupBoxTextAlign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iNumericOutlineThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTrackBarBrightness)).BeginInit();
            this.SuspendLayout();
            // 
            // iTextBoxKeyEditor
            // 
            this.iTextBoxKeyEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iTextBoxKeyEditor.Location = new System.Drawing.Point(546, 312);
            this.iTextBoxKeyEditor.Multiline = true;
            this.iTextBoxKeyEditor.Name = "iTextBoxKeyEditor";
            this.iTextBoxKeyEditor.Size = new System.Drawing.Size(156, 76);
            this.iTextBoxKeyEditor.TabIndex = 0;
            this.iTextBoxKeyEditor.TextChanged += new System.EventHandler(this.iTextBoxKeyEditor_TextChanged);
            // 
            // iButtonSave
            // 
            this.iButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iButtonSave.Location = new System.Drawing.Point(12, 423);
            this.iButtonSave.Name = "iButtonSave";
            this.iButtonSave.Size = new System.Drawing.Size(75, 23);
            this.iButtonSave.TabIndex = 1;
            this.iButtonSave.Text = "Save";
            this.iButtonSave.UseVisualStyleBackColor = true;
            this.iButtonSave.Click += new System.EventHandler(this.iButtonSave_Click);
            // 
            // iButtonFont
            // 
            this.iButtonFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonFont.Location = new System.Drawing.Point(627, 394);
            this.iButtonFont.Name = "iButtonFont";
            this.iButtonFont.Size = new System.Drawing.Size(75, 23);
            this.iButtonFont.TabIndex = 2;
            this.iButtonFont.Text = "Font";
            this.iButtonFont.UseVisualStyleBackColor = true;
            this.iButtonFont.Click += new System.EventHandler(this.iButtonFont_Click);
            // 
            // iComboBoxProfiles
            // 
            this.iComboBoxProfiles.FormattingEnabled = true;
            this.iComboBoxProfiles.Location = new System.Drawing.Point(12, 12);
            this.iComboBoxProfiles.Name = "iComboBoxProfiles";
            this.iComboBoxProfiles.Size = new System.Drawing.Size(197, 21);
            this.iComboBoxProfiles.TabIndex = 3;
            this.iComboBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.iComboBoxProfiles_SelectedIndexChanged);
            this.iComboBoxProfiles.TextUpdate += new System.EventHandler(this.iComboBoxProfiles_TextUpdate);
            // 
            // iButtonNewProfile
            // 
            this.iButtonNewProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonNewProfile.Location = new System.Drawing.Point(575, 10);
            this.iButtonNewProfile.Name = "iButtonNewProfile";
            this.iButtonNewProfile.Size = new System.Drawing.Size(127, 23);
            this.iButtonNewProfile.TabIndex = 5;
            this.iButtonNewProfile.Text = "New Profile";
            this.iButtonNewProfile.UseVisualStyleBackColor = true;
            this.iButtonNewProfile.Click += new System.EventHandler(this.iButtonNewProfile_Click);
            // 
            // iButtonDeleteProfile
            // 
            this.iButtonDeleteProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonDeleteProfile.Location = new System.Drawing.Point(575, 39);
            this.iButtonDeleteProfile.Name = "iButtonDeleteProfile";
            this.iButtonDeleteProfile.Size = new System.Drawing.Size(127, 23);
            this.iButtonDeleteProfile.TabIndex = 6;
            this.iButtonDeleteProfile.Text = "Delete Profile";
            this.iButtonDeleteProfile.UseVisualStyleBackColor = true;
            this.iButtonDeleteProfile.Click += new System.EventHandler(this.iButtonDeleteProfile_Click);
            // 
            // iButtonFontColor
            // 
            this.iButtonFontColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonFontColor.Location = new System.Drawing.Point(627, 423);
            this.iButtonFontColor.Name = "iButtonFontColor";
            this.iButtonFontColor.Size = new System.Drawing.Size(75, 23);
            this.iButtonFontColor.TabIndex = 7;
            this.iButtonFontColor.Text = "Font Color";
            this.iButtonFontColor.UseVisualStyleBackColor = true;
            this.iButtonFontColor.Click += new System.EventHandler(this.iButtonFontColor_Click);
            // 
            // iGroupBoxTextAlign
            // 
            this.iGroupBoxTextAlign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignBottomLeft);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignBottomCenter);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignMiddleLeft);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignMiddleCenter);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignTopLeft);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignTopCenter);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignBottomRight);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignMiddleRight);
            this.iGroupBoxTextAlign.Controls.Add(this.iButtonTextAlignTopRight);
            this.iGroupBoxTextAlign.Location = new System.Drawing.Point(572, 167);
            this.iGroupBoxTextAlign.Name = "iGroupBoxTextAlign";
            this.iGroupBoxTextAlign.Size = new System.Drawing.Size(126, 139);
            this.iGroupBoxTextAlign.TabIndex = 8;
            this.iGroupBoxTextAlign.TabStop = false;
            this.iGroupBoxTextAlign.Text = "Text alignment";
            // 
            // iButtonTextAlignBottomLeft
            // 
            this.iButtonTextAlignBottomLeft.Location = new System.Drawing.Point(6, 97);
            this.iButtonTextAlignBottomLeft.Name = "iButtonTextAlignBottomLeft";
            this.iButtonTextAlignBottomLeft.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignBottomLeft.TabIndex = 11;
            this.iButtonTextAlignBottomLeft.TabStop = false;
            this.iButtonTextAlignBottomLeft.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.iButtonTextAlignBottomLeft.UseVisualStyleBackColor = true;
            this.iButtonTextAlignBottomLeft.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignBottomCenter
            // 
            this.iButtonTextAlignBottomCenter.Location = new System.Drawing.Point(45, 97);
            this.iButtonTextAlignBottomCenter.Name = "iButtonTextAlignBottomCenter";
            this.iButtonTextAlignBottomCenter.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignBottomCenter.TabIndex = 5;
            this.iButtonTextAlignBottomCenter.TabStop = false;
            this.iButtonTextAlignBottomCenter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iButtonTextAlignBottomCenter.UseVisualStyleBackColor = true;
            this.iButtonTextAlignBottomCenter.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignMiddleLeft
            // 
            this.iButtonTextAlignMiddleLeft.Location = new System.Drawing.Point(6, 58);
            this.iButtonTextAlignMiddleLeft.Name = "iButtonTextAlignMiddleLeft";
            this.iButtonTextAlignMiddleLeft.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignMiddleLeft.TabIndex = 10;
            this.iButtonTextAlignMiddleLeft.TabStop = false;
            this.iButtonTextAlignMiddleLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iButtonTextAlignMiddleLeft.UseVisualStyleBackColor = true;
            this.iButtonTextAlignMiddleLeft.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignMiddleCenter
            // 
            this.iButtonTextAlignMiddleCenter.Location = new System.Drawing.Point(45, 58);
            this.iButtonTextAlignMiddleCenter.Name = "iButtonTextAlignMiddleCenter";
            this.iButtonTextAlignMiddleCenter.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignMiddleCenter.TabIndex = 4;
            this.iButtonTextAlignMiddleCenter.TabStop = false;
            this.iButtonTextAlignMiddleCenter.UseVisualStyleBackColor = true;
            this.iButtonTextAlignMiddleCenter.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignTopLeft
            // 
            this.iButtonTextAlignTopLeft.Location = new System.Drawing.Point(6, 19);
            this.iButtonTextAlignTopLeft.Name = "iButtonTextAlignTopLeft";
            this.iButtonTextAlignTopLeft.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignTopLeft.TabIndex = 9;
            this.iButtonTextAlignTopLeft.TabStop = false;
            this.iButtonTextAlignTopLeft.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.iButtonTextAlignTopLeft.UseVisualStyleBackColor = true;
            this.iButtonTextAlignTopLeft.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignTopCenter
            // 
            this.iButtonTextAlignTopCenter.Location = new System.Drawing.Point(45, 19);
            this.iButtonTextAlignTopCenter.Name = "iButtonTextAlignTopCenter";
            this.iButtonTextAlignTopCenter.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignTopCenter.TabIndex = 3;
            this.iButtonTextAlignTopCenter.TabStop = false;
            this.iButtonTextAlignTopCenter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iButtonTextAlignTopCenter.UseVisualStyleBackColor = true;
            this.iButtonTextAlignTopCenter.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignBottomRight
            // 
            this.iButtonTextAlignBottomRight.Location = new System.Drawing.Point(84, 97);
            this.iButtonTextAlignBottomRight.Name = "iButtonTextAlignBottomRight";
            this.iButtonTextAlignBottomRight.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignBottomRight.TabIndex = 2;
            this.iButtonTextAlignBottomRight.TabStop = false;
            this.iButtonTextAlignBottomRight.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.iButtonTextAlignBottomRight.UseVisualStyleBackColor = true;
            this.iButtonTextAlignBottomRight.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignMiddleRight
            // 
            this.iButtonTextAlignMiddleRight.Location = new System.Drawing.Point(84, 58);
            this.iButtonTextAlignMiddleRight.Name = "iButtonTextAlignMiddleRight";
            this.iButtonTextAlignMiddleRight.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignMiddleRight.TabIndex = 1;
            this.iButtonTextAlignMiddleRight.TabStop = false;
            this.iButtonTextAlignMiddleRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iButtonTextAlignMiddleRight.UseVisualStyleBackColor = true;
            this.iButtonTextAlignMiddleRight.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonTextAlignTopRight
            // 
            this.iButtonTextAlignTopRight.Location = new System.Drawing.Point(84, 19);
            this.iButtonTextAlignTopRight.Name = "iButtonTextAlignTopRight";
            this.iButtonTextAlignTopRight.Size = new System.Drawing.Size(33, 33);
            this.iButtonTextAlignTopRight.TabIndex = 0;
            this.iButtonTextAlignTopRight.TabStop = false;
            this.iButtonTextAlignTopRight.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.iButtonTextAlignTopRight.UseVisualStyleBackColor = true;
            this.iButtonTextAlignTopRight.Click += new System.EventHandler(this.ButtonTextAlignClick);
            // 
            // iButtonBitmapColor
            // 
            this.iButtonBitmapColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonBitmapColor.Location = new System.Drawing.Point(430, 423);
            this.iButtonBitmapColor.Name = "iButtonBitmapColor";
            this.iButtonBitmapColor.Size = new System.Drawing.Size(110, 23);
            this.iButtonBitmapColor.TabIndex = 9;
            this.iButtonBitmapColor.Text = "Background Color";
            this.iButtonBitmapColor.UseVisualStyleBackColor = true;
            this.iButtonBitmapColor.Click += new System.EventHandler(this.iButtonBitmapColor_Click);
            // 
            // iButtonOutlineColor
            // 
            this.iButtonOutlineColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonOutlineColor.Location = new System.Drawing.Point(546, 423);
            this.iButtonOutlineColor.Name = "iButtonOutlineColor";
            this.iButtonOutlineColor.Size = new System.Drawing.Size(75, 23);
            this.iButtonOutlineColor.TabIndex = 10;
            this.iButtonOutlineColor.Text = "Outline Color";
            this.iButtonOutlineColor.UseVisualStyleBackColor = true;
            this.iButtonOutlineColor.Click += new System.EventHandler(this.iButtonOutlineColor_Click);
            // 
            // iNumericOutlineThickness
            // 
            this.iNumericOutlineThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iNumericOutlineThickness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iNumericOutlineThickness.DecimalPlaces = 1;
            this.iNumericOutlineThickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.iNumericOutlineThickness.Location = new System.Drawing.Point(547, 396);
            this.iNumericOutlineThickness.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.iNumericOutlineThickness.Name = "iNumericOutlineThickness";
            this.iNumericOutlineThickness.Size = new System.Drawing.Size(74, 20);
            this.iNumericOutlineThickness.TabIndex = 11;
            this.iNumericOutlineThickness.ValueChanged += new System.EventHandler(this.iNumericOutlineThickness_ValueChanged);
            // 
            // iTrackBarBrightness
            // 
            this.iTrackBarBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.iTrackBarBrightness.Cursor = System.Windows.Forms.Cursors.Default;
            this.iTrackBarBrightness.Location = new System.Drawing.Point(12, 39);
            this.iTrackBarBrightness.Maximum = 100;
            this.iTrackBarBrightness.Name = "iTrackBarBrightness";
            this.iTrackBarBrightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.iTrackBarBrightness.Size = new System.Drawing.Size(45, 378);
            this.iTrackBarBrightness.TabIndex = 12;
            this.iTrackBarBrightness.TickFrequency = 5;
            this.iTrackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.iTrackBarBrightness.Scroll += new System.EventHandler(this.iTrackBarBrightness_Scroll);
            // 
            // iButtonApplyToAll
            // 
            this.iButtonApplyToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iButtonApplyToAll.Location = new System.Drawing.Point(349, 423);
            this.iButtonApplyToAll.Name = "iButtonApplyToAll";
            this.iButtonApplyToAll.Size = new System.Drawing.Size(75, 23);
            this.iButtonApplyToAll.TabIndex = 13;
            this.iButtonApplyToAll.Text = "Apply To All";
            this.iButtonApplyToAll.UseVisualStyleBackColor = true;
            this.iButtonApplyToAll.Click += new System.EventHandler(this.iButtonApplyToAll_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(446, 398);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Outline Thickness:";
            // 
            // iComboBoxEvents
            // 
            this.iComboBoxEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iComboBoxEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iComboBoxEvents.FormattingEnabled = true;
            this.iComboBoxEvents.Location = new System.Drawing.Point(546, 140);
            this.iComboBoxEvents.Name = "iComboBoxEvents";
            this.iComboBoxEvents.Size = new System.Drawing.Size(156, 21);
            this.iComboBoxEvents.TabIndex = 15;
            this.iComboBoxEvents.SelectionChangeCommitted += new System.EventHandler(this.iComboBoxEvents_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(543, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Event:";
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 458);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iComboBoxEvents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.iButtonApplyToAll);
            this.Controls.Add(this.iTrackBarBrightness);
            this.Controls.Add(this.iNumericOutlineThickness);
            this.Controls.Add(this.iButtonOutlineColor);
            this.Controls.Add(this.iButtonBitmapColor);
            this.Controls.Add(this.iGroupBoxTextAlign);
            this.Controls.Add(this.iButtonFontColor);
            this.Controls.Add(this.iButtonDeleteProfile);
            this.Controls.Add(this.iButtonNewProfile);
            this.Controls.Add(this.iComboBoxProfiles);
            this.Controls.Add(this.iButtonFont);
            this.Controls.Add(this.iButtonSave);
            this.Controls.Add(this.iTextBoxKeyEditor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEditor";
            this.Text = "Stream Deck Demo";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.iGroupBoxTextAlign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iNumericOutlineThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTrackBarBrightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox iTextBoxKeyEditor;
        private System.Windows.Forms.Button iButtonSave;
        private System.Windows.Forms.Button iButtonFont;
        private System.Windows.Forms.FontDialog iFontDialog;
        private System.Windows.Forms.ComboBox iComboBoxProfiles;
        private System.Windows.Forms.Button iButtonNewProfile;
        private System.Windows.Forms.Button iButtonDeleteProfile;
        private System.Windows.Forms.ColorDialog iColorDialog;
        private System.Windows.Forms.Button iButtonFontColor;
        private System.Windows.Forms.GroupBox iGroupBoxTextAlign;
        private System.Windows.Forms.Button iButtonTextAlignBottomLeft;
        private System.Windows.Forms.Button iButtonTextAlignBottomCenter;
        private System.Windows.Forms.Button iButtonTextAlignMiddleLeft;
        private System.Windows.Forms.Button iButtonTextAlignMiddleCenter;
        private System.Windows.Forms.Button iButtonTextAlignTopLeft;
        private System.Windows.Forms.Button iButtonTextAlignTopCenter;
        private System.Windows.Forms.Button iButtonTextAlignBottomRight;
        private System.Windows.Forms.Button iButtonTextAlignMiddleRight;
        private System.Windows.Forms.Button iButtonTextAlignTopRight;
        private System.Windows.Forms.Button iButtonBitmapColor;
        private System.Windows.Forms.Button iButtonOutlineColor;
        private System.Windows.Forms.NumericUpDown iNumericOutlineThickness;
        private System.Windows.Forms.TrackBar iTrackBarBrightness;
        private System.Windows.Forms.Button iButtonApplyToAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox iComboBoxEvents;
        private System.Windows.Forms.Label label2;
    }
}

