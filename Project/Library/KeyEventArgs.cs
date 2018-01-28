using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.StreamDeck
{
    public class KeyEventArgs : EventArgs
    {
        public int Key { get; }
        public bool IsDown { get; }

        public KeyEventArgs(int key, bool isDown)
        {
            Key = key;
            IsDown = isDown;
        }
    }
}
