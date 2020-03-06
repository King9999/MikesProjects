/* This class describes the effects of every skill in the game.  XML files will contain tags
 * whose values are used to determine the effects a spell or item has, as well as who gets targeted. */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleRPG.Skills
{
    class Skills
    {
        enum Effect
        {
            Critical,   //Hit points is less than 25%
            Poison,     //HP decreases by 1% every 3 seconds
            Stun,        //prevents swapping or raising blocks for 2 seconds.
            Stop,       //blocks will not auto-rise for a given amount of time. Can be manually raised.
            RemoveAttack,
            RemoveShield,
            RemoveRed,      //removes blocks of a given type
            RemoveGreen,
            RemoveBlue,
            Heal         //restores HP by a given amount
        }
        string mName;   //identifier of skill

    }
}
