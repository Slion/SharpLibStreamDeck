using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.StreamDeck
{
    [DataContract]
    public class Profile
    {
        [DataMember]
        public string Name;

        [DataMember]
        public List<Key> Keys;

        public void Construct()
        {
            if (Keys == null)
            {
                Keys = new List<Key>();
                
                // Create our Keys
                for (int i=0; i<Client.numOfKeys; i++)
                {
                    Key key = new Key();
                    key.Construct();
                    key.Text = i.ToString();
                    Keys.Add(key);
                }
            }
        }
    }
}
