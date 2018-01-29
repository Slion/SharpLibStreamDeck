using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SharpLib.StreamDeck
{
    [DataContract(Name ="StreamDeck")]
    public class Model
    {
        [DataMember]
        public List<Profile> Profiles;


        public void Construct()
        {
            if (Profiles==null)
            {
                Profiles = new List<Profile>();
                CreateDefaultProfile();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateDefaultProfile()
        {
            Profile defaultProfile = new Profile();
            defaultProfile.Name = "Default";
            defaultProfile.Construct();
            Profiles.Add(defaultProfile);
        }
    }
}
