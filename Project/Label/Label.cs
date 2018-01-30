using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SharpLib.StreamDeck
{
    /// <summary>
    /// Adds outline support to Label control.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Label: System.Windows.Forms.Label
    {
        // Data members
        private float iOutlineThickness;
        private Color iOutlineColor;
        private Pen iOutlinePen;
        private GraphicsPath iPath;
        private SolidBrush iForeBrush;
        private StringFormat iStringFormat;

        // Constructor
        public Label()
        {
            InitializeComponent();

            iStringFormat = StringFormat.GenericDefault.Clone() as StringFormat;
            SyncAlignments();
            iPath = new GraphicsPath();
            iOutlinePen = new Pen(new SolidBrush(iOutlineColor), iOutlineThickness);
            iForeBrush = new SolidBrush(ForeColor);
            iOutlineThickness = 1f;
            iOutlineColor = Color.Black;

            Invalidate();
        }


        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Font outline thickness")]
        [DefaultValue(1f)]
        public float OutlineThickness
        {
            get { return iOutlineThickness; }
            set
            {
                iOutlineThickness = value;
                iOutlinePen.Width = value;

                Invalidate();
            }
        }


        /// <summary>
        ///
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Font outline color")]
        public Color OutlineColor
        {
            get { return iOutlineColor; }
            set
            {
                iOutlineColor = value;
                iOutlinePen.Color = value;
                    
                Invalidate();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Invalidate();
        }

        protected override void OnTextAlignChanged(EventArgs e)
        {
            base.OnTextAlignChanged(e);
            SyncAlignments();
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            iForeBrush.Color = base.ForeColor;
            base.OnForeColorChanged(e);
            Invalidate();
        }
 

        protected override void OnPaint(PaintEventArgs e)
        {
            // Check if we need to render anything
            if (Text.Length == 0)
            {
                return;
            }

            if (iOutlineThickness==0)
            {
                // No outline, default to base class rendering
                // Conveniently that let's us compare our rendering with base class rendering.
                base.OnPaint(e);
                return;
            }

            // Use high quality anti-alias for now.
            // Consider making this configurable.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
   
            // Work out the size of our text in em unit, whatever that is.
            // See: https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/c6a4ff68-5997-460a-9360-24c99f532b16/the-font-size-of-graphicspathaddstring?forum=windowsgeneraldevelopmentissues
            float emsize = Font.Height * Font.FontFamily.GetCellAscent(Font.Style) / Font.FontFamily.GetEmHeight(Font.Style);

            iPath.Reset();
            iPath.AddString(Text, Font.FontFamily, (int)Font.Style, emsize, ClientRectangle, iStringFormat);

            // Draw Fill
            e.Graphics.FillPath(iForeBrush, iPath);
            // Draw Outline
            e.Graphics.DrawPath(iOutlinePen, iPath);

        }


        /// <summary>
        /// Interpret TextAlign to setup our StringFormat 
        /// </summary>
        private void SyncAlignments()
        {
            // Vertical alignment
            if (IsTop(TextAlign))
            {
                iStringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (IsMiddle(TextAlign))
            {
                iStringFormat.LineAlignment = StringAlignment.Center;
            }
            else if (IsBottom(TextAlign)) // Must be
            {
                iStringFormat.LineAlignment = StringAlignment.Far;
            }

            // Horizontal alignment
            if (IsLeft(TextAlign))
            {
                iStringFormat.Alignment = StringAlignment.Near;
            }
            else if (IsCenter(TextAlign))
            {
                iStringFormat.Alignment = StringAlignment.Center;
            }
            else if (IsRight(TextAlign)) // Must be
            {
                iStringFormat.Alignment = StringAlignment.Far;
            }

        }


        static bool IsTop(ContentAlignment aAlignment)
        {
            if (aAlignment == ContentAlignment.TopLeft ||
                aAlignment == ContentAlignment.TopCenter ||
                aAlignment == ContentAlignment.TopRight)
            {
                return true;
            }

            return false;
        }

        static bool IsMiddle(ContentAlignment aAlignment)
        {
            if (aAlignment == ContentAlignment.MiddleLeft ||
                aAlignment == ContentAlignment.MiddleCenter ||
                aAlignment == ContentAlignment.MiddleRight)
            {
                return true;
            }

            return false;
        }

        static bool IsBottom(ContentAlignment aAlignment)
        {
            if (aAlignment == ContentAlignment.BottomLeft ||
                aAlignment == ContentAlignment.BottomCenter ||
                aAlignment == ContentAlignment.BottomRight)
            {
                return true;
            }

            return false;
        }

        static bool IsLeft(ContentAlignment aAlignment)
        {
            if (aAlignment == ContentAlignment.TopLeft ||
                aAlignment == ContentAlignment.MiddleLeft ||
                aAlignment == ContentAlignment.BottomLeft)
            {
                return true;
            }

            return false;
        }

        static bool IsCenter(ContentAlignment aAlignment)
        {
            if (aAlignment == ContentAlignment.TopCenter ||
                aAlignment == ContentAlignment.MiddleCenter ||
                aAlignment == ContentAlignment.BottomCenter)
            {
                return true;
            }

            return false;
        }

        static bool IsRight(ContentAlignment aAlignment)
        {
            if (aAlignment == ContentAlignment.TopRight ||
                aAlignment == ContentAlignment.MiddleRight ||
                aAlignment == ContentAlignment.BottomRight)
            {
                return true;
            }

            return false;
        }
    }
}
