using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Robe : Armor
    {
        //Robes are more likely to have elemental resistance
        public Robe()
        {
            ItemSubtype = "Robe";
            ArmDefMod = 1;
            ArmResMod = 3;
            ArmEvade = 0.15f;
        }
    }
}
