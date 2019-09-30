/* This class will use a 2D array to point to specific cards in a deck */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _99CardGame.Classes
{
    /* Creates and manipulates a deck of Card objects.  Can shuffle and deal cards. */
    class Deck
    {
       
        const int MAX_SUIT = 4;         //serves as number of rows in card sprite sheet
        const int MAX_VALUE = 13;       //serves as number of columns in card sprite sheet
        const int MAX_CARDS_IN_HAND = 3;         //total number of cards in a hand
        bool[,] cards = new bool[MAX_SUIT, MAX_VALUE];           //tracks which cards are taken and which are remaining. true = available, false = taken from the deck.

        //the following variables are used to access cards from a sprite sheet.  It uses a 2D array to navigate the sheet and
        //access any given card.  A copy of the card is then given to the player.
        Texture2D cardIterator;         //an iterator
        Texture2D cardSheet;
        Texture2D cursor;
        Rectangle[] cursorRect = new Rectangle[MAX_CARDS_IN_HAND];             //checks current card.
        Rectangle[] iterRect = new Rectangle[MAX_CARDS_IN_HAND];
        int cardWidth;          //width of image frame. Found by dividing the sprite sheet's width by the number of sprites in a row.
        int cardHeight;         //height of image frame. Found by dividing the sprite sheet's height by the number of sprites in a column.
        int currentFrameCol;      //points to the current frame in a sheet's column.
        int currentFrame;        //points to current column in a sheet

        public Deck(Game game)
        {
            cardWidth = 72;
            cardHeight = 98;
            cardSheet = game.Content.Load<Texture2D>(@"Cards/cards");
            cursor = game.Content.Load<Texture2D>(@"Cards/red");
           // cursorRect = new Rectangle((cardWidth+1) * 6, cardHeight * 2, cardWidth, cardHeight);
            //iterRect = new Rectangle(500, 450, cardWidth * 3/2, cardHeight * 3/2);    //displays current card highlighted by cursor
            cardIterator = cardSheet;
         
            //set up cards
            for (int i = 0; i < MAX_SUIT; i++)
            {
                for (int j = 0; j < MAX_VALUE; j++)
                {
                    cards[i, j] = true;
                }
            }

            //get random cards from deck
            Random randNum = new Random();
            int k = 0;
            while (k < MAX_CARDS_IN_HAND)
            {
                int randSuit = randNum.Next(MAX_SUIT);
                int randValue = randNum.Next(MAX_VALUE);
                cursorRect[k] = new Rectangle((cardWidth + 1) * randValue, cardHeight * randSuit, cardWidth, cardHeight);

                //TODO: Add code so that the card information is collected.
                //if the card is not taken, then take it
                if (cards[randSuit, randValue] == true)
                {

                    cards[randSuit, randValue] = false;     //card taken
                }
               

                iterRect[k] = new Rectangle(300 + (k * 100), 450, cardWidth * 3 / 2, cardHeight * 3 / 2);    //displays current card highlighted by cursor
                k++;
            }
            //for (int i = 0; i < MAX_SUIT; i++)
            //{
            //    for (int j = 0; j < MAX_VALUE; j++)
            //    {
            //        cards[i, j] = new Card(i, j);
                    
            //    }
            //}
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            batch.Draw(cardSheet, new Vector2(0, 0), Color.White);

            for (int i = 0; i < MAX_CARDS_IN_HAND; i++)
            {
                batch.Draw(cursor, cursorRect[i], Color.White);
                batch.Draw(cardIterator, iterRect[i], cursorRect[i], Color.White);
            }
            batch.End();
        }
    }
}
