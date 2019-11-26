using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Bow : Weapon
    {
        public Bow()
        {
            WpnAcc = 0.95f;
            ItemSubtype = "Bow";
            WpnAtkMod = 1;
        }
    }
}
