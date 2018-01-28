using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLib.StreamDeck
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// This extension class adds some commonly used functions to make things simpler.
    /// </remarks>
    public static class Extensions
    {
        /// <summary>
        /// Sets a background image for a given key
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="keyId"></param>
        /// <param name="bitmap"></param>
        public static void SetKeyBitmap(this Client deck, int keyId, KeyBitmap bitmap)
        {
            deck.SetKeyBitmap(keyId, bitmap.rawBitmapData);
        }

        /// <summary>
        /// Sets a background image for all keys
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="bitmap"></param>
        public static void SetKeyBitmap(this Client deck, KeyBitmap bitmap)
        {
            for (int i = 0; i < deck.KeyCount; i++)
                deck.SetKeyBitmap(i, bitmap.rawBitmapData);
        }

        /// <summary>
        /// Sets background to black for a given key
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="keyId"></param>
        public static void ClearKey(this Client deck, int keyId)
        {
            deck.SetKeyBitmap(keyId, KeyBitmap.Black);
        }

        /// <summary>
        /// Sets background to black for all given keys
        /// </summary>
        /// <param name="deck"></param>
        public static void ClearKeys(this Client deck)
        {
            deck.SetKeyBitmap(KeyBitmap.Black);
        }
    }
}
