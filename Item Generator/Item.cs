/* This class is the parent of all other items. Every piece of gear is derived from an item. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    abstract class Item
    {
        protected byte ItemLevel;                   //item's level. Will be used to determine an item's stats during generation. Minimum level is 1.
        protected const byte LEVEL_MAX = 200;
        protected string ItemName;                  //Names are procedurally generated and can determine what effects/stats an item has.

        protected byte ItemRank;                    //Item quality from 0 to 3, with 3 being the best. Rank is a multiplier that increases stats on an item. 
                                                    //Rank is indicated by a plus sign next to an item's name.

        protected const byte RANK_MAX = 3;

        protected short ItemHealthPointBonus;        //increases stat points by a certain amount while item is equipped.
        protected short ItemMagicPointBonus;        //Generally, these bonuses would be granted by accessories, but other 
        protected short ItemSpeedBonus;               //gear can have these bonuses.

        protected const short HEALTHBONUS_MAX = 1000;
        protected const short MAGICBONUS_MAX = 1000;
        protected const short SPEEDBONUS_MAX = 1000;


        protected string ItemType;                  //type of item, e.g. weapon, armor, etc.
        protected string ItemSubtype;               //eg. sword, shield, ring, etc.

        /* This function is used to execute any special effects an item might have. I won't actually use this for this tool, but it's my interpretation
        * of how a game might check for any special effects. */
        protected virtual void ExecuteSpecialAbility(short effectID)
        {

        }

        #region Getters/Setters

        public string GetItemType()
        {
            return ItemType;
        }

        public void SetItemType(string name)
        {
            ItemType = name;
        }


        public string GetItemSubtype()
        {
            return ItemSubtype;
        }

        public void SetItemSubtype(string name)
        {
            ItemSubtype = name;
        }

        public byte GetItemLevel()
        {
            return ItemLevel;
        }

        public void SetItemLevel(byte value)
        {
            if (value > LEVEL_MAX)
                value = LEVEL_MAX;

            if (value < 1)
                value = 1;

            ItemLevel = value;
        }

        public string GetItemName()
        {
            return ItemName;
        }

        public byte GetRank()
        {
            return ItemRank;
        }

        public void SetRank(byte rank)
        {
            if (rank > RANK_MAX)    //do not need to check if rank goes below 0 because byte is unsigned
                rank = RANK_MAX;

            ItemRank = rank;
        }

        public void SetItemName(string name)
        {
            ItemName = name;
        }

        public short GetHealthPointBonus()
        {
            return ItemHealthPointBonus;
        }

        public void SetHealthPointBonus(short bonus)
        {
            if (bonus > HEALTHBONUS_MAX)
                bonus = HEALTHBONUS_MAX;

            if (bonus < 0)
                bonus = 0;

            ItemHealthPointBonus = bonus;
        }

        public short GetMagicPointBonus()
        {
            return ItemMagicPointBonus;
        }

        public void SetMagicPointBonus(short bonus)
        {
            if (bonus > MAGICBONUS_MAX)
                bonus = MAGICBONUS_MAX;

            if (bonus < 0)
                bonus = 0;

            ItemMagicPointBonus = bonus;
        }

        public short GetSpeedBonus()
        {
            return ItemSpeedBonus;
        }

        public void SetSpeedBonus(short bonus)
        {
            if (bonus > SPEEDBONUS_MAX)
                bonus = SPEEDBONUS_MAX;

            if (bonus < 0)
                bonus = 0;

            ItemSpeedBonus = bonus;
        }

        public byte GetMaxLevel()
        {
            return LEVEL_MAX;
        }

        public byte GetMaxRank()
        {
            return RANK_MAX;
        }
#endregion
    }
}
