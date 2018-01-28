using SharpLib.StreamDeck;

namespace SharpLib.StreamDeck.Examples.Austria
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create some color we use later to draw the flag of austria
            var red = KeyBitmap.FromRGBColor(237, 41, 57);
            var white = KeyBitmap.FromRGBColor(255, 255, 255);
            var rowColors = new KeyBitmap[] { red, white, red };

            //Open the Stream Deck device

            using (Client deck = new Client())
            {
                deck.Open();
                deck.SetBrightness(100);

                //Send the bitmap informaton to the device
                for (int i = 0; i < deck.KeyCount; i++)
                    deck.SetKeyBitmap(i, rowColors[i / 5]);
            }
        }
    }
}
