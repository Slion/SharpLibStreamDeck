using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.StreamDeck
{
    [DataContract]
    public class Key
    {
        [DataMember]
        public string EventName;

        [DataMember]
        public string Text;

        [DataMember]
        public ContentAlignment TextAlign;

        [DataMember]
        public string FontName { get; set; }

        [DataMember]
        public Color FontColor { get; set; }

        [DataMember]
        public Color OutlineColor { get; set; }

        [DataMember]
        public float OutlineThickness { get; set; }

        [DataMember]
        public Bitmap Bitmap;

        public void Construct()
        {
            if (Bitmap == null)
            {
                Bitmap = new Bitmap(Client.KKeyWidthInPixels, Client.KKeyWidthInPixels);
                FillBitmap(Brushes.Black);
                FontColor = Color.White;
                TextAlign = ContentAlignment.MiddleCenter;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aBrush"></param>
        public void FillBitmap(Brush aBrush)
        {
            using (Graphics g = Graphics.FromImage(Bitmap))
            {
                g.FillRectangle(aBrush, 0, 0, Bitmap.Width, Bitmap.Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aColor"></param>
        public void FillBitmap(Color aColor)
        {
            using (Graphics gfx = Graphics.FromImage(Bitmap))
            using (SolidBrush brush = new SolidBrush(aColor))
            {
                gfx.FillRectangle(brush, 0, 0, Bitmap.Width, Bitmap.Height);
            }
        }


        /// <summary>
        /// The Font object itself is not serialisable.
        /// Therefore we persist our font through the FontName string.
        /// </summary>
        public Font Font
        {
            get
            {
                if (string.IsNullOrEmpty(FontName))
                {
                    return null;
                }
                FontConverter cvt = new FontConverter();
                Font font = cvt.ConvertFromInvariantString(FontName) as Font;
                return font;
            }

            set
            {
                FontConverter cvt = new FontConverter();
                FontName = cvt.ConvertToInvariantString(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aKey"></param>
        public void CopyStyle(Key aKey)
        {
            TextAlign = aKey.TextAlign;
            FontName = aKey.FontName;
            FontColor = aKey.FontColor;
            OutlineColor = aKey.OutlineColor;
            OutlineThickness = aKey.OutlineThickness;
            Bitmap = (Bitmap)aKey.Bitmap.Clone();
            Font = (Font)aKey.Font.Clone();            
        }
    }
}
