# Momir-Viginator
This is an android app that will allow Magic players to play a game of Momir Basic(and similar formats) in real life.
You can set the desired CMC and then press "Generate" to randomly generate a creature card and print it with a connected bluetooth receipt printer.

## Usage
To use the app, you will need a usb-connected receipt printer. The app has been tested with the PT-230 printer, other PT-2XX printers might work.
Once the printer is paired to the phone, it needs to be selected in the settings page of the app. The settings page has a "print test page" button to see if it works.

Once configured correctly, you can just generate a card and press print.
Internet connection is required, the app uses Scryfall to download the card data.

## Known issues
- Generating with CMC 0 can result in unplayable cards(e.g. augment cards from unstable)
  - Similarly CMC 15 tends to result in one of the halves of B.F.M., they are somehow considered separate cards
- Printers that aren't announced as PT-2XX over bluetooth won't be recognized