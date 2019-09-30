
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
    class Chicken : Food
    {
        public Chicken(ContentManager content, Vector2 location)
            :base(content, "Food/chicken", "Chicken", location, 40)
        {
            mPosition = location;
        }

        public override void ActivateSpecial(Player player)
        {
          
        }

        public override string EffectMessage()
        {
            return base.EffectMessage();
        }
    }
}
