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
        public string EventName;

        public void Construct()
        {
            if (Bitmap == null)
            {
                Bitmap = new Bitmap(Client.iconSize, Client.iconSize);
            }

            TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}
