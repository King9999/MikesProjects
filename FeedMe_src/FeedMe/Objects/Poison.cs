//Poison reduces the food meter. Players must avoid this! On the other hand, it could help...

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FeedMe.Objects
{
    class Poison : Food
    {
        public Poison(ContentManager content, Vector2 location)
            :base(content, "Food/poison", "Poison", location, -40)
        {
            mPosition = location;
        }

        public override void ActivateSpecial(Player player)
        {
          
        }

        public override string EffectMessage()
        {
            return "Poison!";
        }
    }
}
