/* Unlike how I used to play this game, I will use the rules as given on Wikipedia. However, I will still retain some of the
 * house rules I used to use. These are the following card effects that I will use:
 * 
 * 10 = -10 to score only
 * 9 = score is set to 99
 * K = +0
 * 4 = reverse turn order
 * Ace = +1 to score only
 * Queen & Jack = +10 to score
 * All other cards add their face value to the score.
 * 
 * Also, each player gets 3 cards instead of 4.
 * 
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _99CardGame.Classes
{
    /* Card object for a deck of playing cards.  Consists of a suit and a value. */
    class Card
    {
        private int cardSuit;      //0=clubs, 1=spades, 2=hearts, 3=diamonds.
        private int cardValue;     //a number ranging from 1 to 13. 11=Jack, 12=Queen, 13=King, 1=Ace. 0=Joker?
        private string fileName;   //card image

        public Card(int suit, int value)
        {
            cardValue = value;
            cardSuit = suit;
        }

        public int GetValue() { return cardValue; }

        public string GetSuit(int suit)
        {
            string result = "";  //returns suit as a string

            switch (suit)
            {
                case 0:
                    result = "Clubs";
                    break;
                case 1:
                    result = "Diamonds";
                    break;
                case 2:
                    result = "Hearts";
                    break;
                case 3:
                    result = "Spades";
                    break;
                default:
                    result = "";
                    break;

            }
            return result;
        }
    }
}
