//pepper adds little to the meter, but it has a side effect: it causes the player to speed up temporarily, for better or worse.

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
    class Pepper : Food
    {
        public Pepper(ContentManager content, Vector2 location)
            :base(content, "Food/pepper", "Pepper", location, 2)
        {
            mPosition = location;
        }

        public override void ActivateSpecial(Player player)
        {
            //speed up the player for some time
            player.SetMod(10);
            player.SetTimer(12);
           
        }

        public override string EffectMessage()
        {
            return "Speed Up!";
        }
    }
}
