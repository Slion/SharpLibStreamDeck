using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.StreamDeck
{
    /// <summary>
    /// Base class for all StreamDeckSharp Exceptions
    /// </summary>
    public abstract class BaseException : Exception
    {
        public BaseException(string Message) : base(Message) { }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException() : base("Stream Deck not found.") { }
    }
}
