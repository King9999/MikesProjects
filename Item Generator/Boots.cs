using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Boots : Accessory
    {
        public Boots()
        {
            //Boots always give speed bonuses
            ItemSubtype = "Boots";
        }
    }
}
