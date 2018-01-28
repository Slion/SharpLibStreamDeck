using SharpLib.StreamDeck;

namespace SharpLib.StreamDeck.Examples.Austria
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create some color we use later to draw the flag of austria
            var red = StreamDeckKeyBitmap.FromRGBColor(237, 41, 57);
            var white = StreamDeckKeyBitmap.FromRGBColor(255, 255, 255);
            var rowColors = new StreamDeckKeyBitmap[] { red, white, red };

            //Open the Stream Deck device

            using (Client deck = new Client())
            {
                deck.Open();
                deck.SetBrightness(100);

                //Send the bitmap informaton to the device
                for (int i = 0; i < deck.NumberOfKeys; i++)
                    deck.SetKeyBitmap(i, rowColors[i / 5]);
            }
        }
    }
}
