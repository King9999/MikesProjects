﻿/* base class for weapons.  Subclass of Item. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace HeroQuest.Items.Weapons
{
    class Weapon : Item
    {
        protected int mAttack;      //attack dice
    }
}
