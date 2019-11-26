using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Ring : Accessory
    {
        public Ring()
        {
            //Rings always give health bonuses
            ItemSubtype = "Ring";
           
        }
    }
}
