/* Swords are weapons that generally have high accuracy, but the other stats are average. Swords have the following modifiers;
 * base weapon accuracy = 85% */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Sword : Weapon
    {
        public Sword()
        {
            WpnAcc = 0.85f;
            ItemSubtype = "Sword";
            WpnAtkMod = 2;
        }
    }
}
