/* This is the base class for all heroes in the game. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeroQuest.Items;

namespace HeroQuest.Heroes
{
    abstract class Hero
    {
        protected int mBodyPoints;    //health. Barbarian has the most, Wizard has the least.
        protected int mMaxBodyPoints;
        protected int mMindPoints;    //spell resistance. Wizard has the most, Barbarian has the least.
        protected int mMaxMindPoints;
        protected int mAttack;        //attack dice. Depends on equipped weapon
        protected int mDefend;        //defend dice. Depends on armor, but base is always 2 dice
        protected int mGold;          //amount of gold coins
        protected int mVision;         //how far they can see in the fog of war. Dwarf has the highest vision
        protected string mName;       //hero name
        protected string mClass;      //hero class
        protected int mMove;          //how far a hero can move. Elf is the fastest, Dwarf is the slowest

        //constants
        const int MAXDICE = 6;              //6 is the maximum

        protected Item[] mInventory;  //holds all items.  Max load is 10
        protected const int MAX_ITEMS = 10;

        public int BodyPoints()
        {
            return mBodyPoints;
        }

        public void ModifyBodyPoints(int amount)
        {        
             mBodyPoints += amount;
             if (mBodyPoints > mMaxBodyPoints)
                 mBodyPoints = mMaxBodyPoints;
             if (mBodyPoints < 0)
                 mBodyPoints = 0;
            
        }
    }
}
