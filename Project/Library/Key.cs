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
        //[DataMember]
        //public int Id;

        [DataMember]
        public Bitmap Bitmap;

        [DataMember]
        public string Text;

        [DataMember]
        public ContentAlignment TextAlign;

        [DataMember]
        public string FontName { get; set; }

        [DataMember]
        public string EventName;

        public void Construct()
        {
            if (Bitmap == null)
            {
                Bitmap = new Bitmap(Client.iconSize, Client.iconSize);
            }

            TextAlign = ContentAlignment.MiddleCenter;
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
    }
}
