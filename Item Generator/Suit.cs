using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Suit : Armor
    {
        public Suit()
        {
            ItemSubtype = "Suit";
            ArmDefMod = 3;
            ArmEvade = 0.1f;
        }
    }
}
