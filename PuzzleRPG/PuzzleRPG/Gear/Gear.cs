/* Base class for all item types in the game.
 * 
 * Gear types
 * ---------------
 * Weapon: modifies ATK. Can provide additional offensive bonuses
 * Armor: modifies DEF. Can provide additional defensive bonuses
 * Accessory: provides different effects
 * Consumable: one-time use items.
 * 
 * All gear will be composed of XML files to allow for rapid creation of items.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleRPG.Gear
{
    class Gear
    {
        protected short mPrice;          //cost of item. Sale price is 50% of this. Items with a price of 0 can't be sold.
        protected string mName;          //name
        protected string mDesc;          //item info
        protected Texture2D mImage;       //graphics for item
    }
}
