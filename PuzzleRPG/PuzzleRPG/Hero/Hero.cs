/* This is the parent class for all hero classes in the game. */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleRPG.Hero
{
    class Hero
    {
        protected string name;        //player's name
        protected short level;          //player's level
        protected short hitPoints;      //health
        protected short atk;            //attack power
        protected short def;            //base shield points
        protected short redMana;
        protected short greenMana;        //amount of mana
        protected short blueMana;

        //class gear
        //Weapon mWeapon;
        //Armor mArmor;
        //Accessory mAccessory;
        //List<Consumable> mItems;

    }
}
