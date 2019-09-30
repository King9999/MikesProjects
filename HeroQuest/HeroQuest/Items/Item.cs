/* Base class for all item types in the game */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace HeroQuest.Items
{
    abstract class Item
    {
        protected int mPrice;     //cost of item. Sale price is 50% of this. Items with a price of 0 can't be sold.
        protected string mName;   //name
        protected string mDesc;   //item info
        protected Texture2D mItemImage;   //graphics for item
       // protected abstract void ConsumeEffect();      //this gets called whenever an item is used. The effect varies by item. The item disappears after use.
        //protected abstract void PassiveEffect();     //effects that are always active. Applies mainly to equipment.
    }
}
