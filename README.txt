This is done by Suhaib Asaad Taha. 

The app contains the following features:

-You can enter the numbers of rows and columns you want to generate in the top left settings, and hit generate

-You can click on the cards to reveal them, hitting two of the same card will increase your score

-every time you increase score consecutively, you gain more combo which adds on top of the next score

-getting a wrong set of two cards, will cost you 1 life, and reset your combo to x1

-if you run out of life, the game will generate a new layout , and reset your score, since its game over

-you can save your current game state using the save button on the top right, this will create a json file in the assets folder that contains the game state

-you can load your last save using the load button on the top right, this will load all the data of the last game state, however it will re-shuffle the cards to avoid cheating

-The Cards Layout will adapt to its container width and height so the cards are never outside the container box and try to be big enough to be usable on the screen, you can test this by changing the size of the "CardsWindow" Rect Transform and then hitting Generate Layout Button. 

-The game has sounds for getting a match, getting a mistmatch, selecting a card, and game over

