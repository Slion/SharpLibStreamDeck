[![NuGet Badge](https://buildstats.info/nuget/SharpLibStreamDeck)](https://www.nuget.org/packages/SharpLibStreamDeck/)
<img align="right" src="https://slions.visualstudio.com/_apis/public/build/definitions/ad16bbd0-a884-4787-8e3a-85daf30cca16/7/badge" />

# SharpLibStreamDeck

SharpLibStreamDeck is a .NET library to interface with [Elgato Gaming Stream Deck].

## Quickstart

I want to...              | Code (C#)
------------------------- | ---------------------------------------------------------
create a device reference | `var deck = new SharpLib.StreamDeck.Client(); deck.Open();`  
set the brightness        | `deck.SetBrightness(50);`
create bitmap for key     | `var bitmap = KeyBitmap.FromFile("icon.png")`
set key image             | `deck.SetKeyBitmap(keyId,bitmap)`
clear key image           | `deck.ClearKey(keyId)`
process key events        | `deck.KeyPressed += KeyHandler;`

**Make sure to dispose the device reference correctly** _(use `using` whenever possible)_

## Features

- Set Stream Deck brightness.
- Display Elgato Gaming logo on Stream Deck.
- Upload bitmaps to Stream Deck keys.
- Receive key press up and down notifications from Stream Deck.
- Provide Stream Deck serializable multi-profile Model. 
- Provide Windows Classic Forms Stream Deck Model Editor:
  - Drag & Drop image files onto keys.
  - Drag & Drop keys to swap them.
  - Apply current key style to every keys in current profile.
  - Keys editor featuring:
    - Text editor.
    - Background color selection.
    - Font selection.
    - Font color selection.
    - Font outline color selection.
    - Font outline thickness selection.
    - Event selection for consumer application to associate actions with keys.

## Rainbow Example

![Rainbow example photo](doc/images/rainbow_example.png?raw=true "Rainbow demo after pressing some keys")



[Elgato Gaming Stream Deck]: https://www.elgato.com/de/gaming/stream-deck