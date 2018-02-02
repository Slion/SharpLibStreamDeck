[![NuGet Badge](https://buildstats.info/nuget/SharpLibStreamDeck)](https://www.nuget.org/packages/SharpLibStreamDeck/)
<img align="right" src="https://slions.visualstudio.com/_apis/public/build/definitions/ad16bbd0-a884-4787-8e3a-85daf30cca16/7/badge" />

# SharpLibStreamDeck

SharpLibStreamDeck is a .NET library to interface with [Elgato Gaming Stream Deck].
This project is not related to _Elgato Systems GmbH_ in any way.

## Quickstart
***At the moment only Windows is supported (tested with 10, should also work with 7 and 8)***
1. Add StreamDeckSharp reference (via nuget or download latest release)
2. Add a using directive for StreamDeckSharp: `using StreamDeckSharp;`

I want to...              | Code (C#)
------------------------- | ---------------------------------------------------------
create a device reference | `var deck = new SharpLib.StreamDeck.Client(); deck.Open();`  
set the brightness        | `deck.SetBrightness(50);`
create bitmap for key     | `var bitmap = KeyBitmap.FromFile("icon.png")`
set key image             | `deck.SetKeyBitmap(keyId,bitmap)`
clear key image           | `deck.ClearKey(keyId)`
process key events        | `deck.KeyPressed += KeyHandler;`

**Make sure to dispose the device reference correctly** _(use `using` whenever possible)_

## Examples
If you want to see some examples take a look at the example projects in the repo.  
Here is a short example called "Austria". Copy the code and start hacking ;-)

```C#
using SharpLib.StreamDeck;

namespace StreamDeckSharp.Examples.Austria
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
            using (var deck = SharpLib.StreamDeck.Client())
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
```

Here is what the "Rainbow" example looks like after pressing some keys

![Rainbow example photo](doc/images/rainbow_example.png?raw=true "Rainbow demo after pressing some keys")


[Elgato Gaming Stream Deck]: https://www.elgato.com/de/gaming/stream-deck