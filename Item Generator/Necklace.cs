using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Necklace : Accessory
    {
        public Necklace()
        {
            //Necklaces always give magic bonuses
            ItemSubtype = "Necklace";
        }
    }
}
