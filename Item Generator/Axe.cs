using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Axe : Weapon
    {
        public Axe()
        {
            WpnAcc = 0.65f;
            ItemSubtype = "Axe";
            WpnAtkMod = 3;
        }
    }
}
