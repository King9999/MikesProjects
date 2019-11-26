using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Vest : Armor
    {
        public Vest()
        {
            ItemSubtype = "Vest";
            ArmEvade = 0.4f;
            ArmDefMod = 2;
        }
    }
}
