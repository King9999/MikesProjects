using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Staff : Weapon
    {
        public Staff()
        {
            WpnAcc = 0.60f;
            ItemSubtype = "Staff";
            WpnAtkMod = 1;
            WpnMagMod = 3;
        }
    }
}
