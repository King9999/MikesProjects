//Weights should not be eaten.  If it is, it will slow down the player temporarily.

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
    class Weight : Food
    {
        public Weight(ContentManager content, Vector2 location)
            :base(content, "Food/weight", "Weight", location, 0)
        {
            mPosition = location;
        }

        public override void ActivateSpecial(Player player)
        {
            //slow down the player for some time
            player.SetMod(-3);
            player.SetTimer(10);
            Debug.WriteLine("Slowed down!");
        }

        public override string EffectMessage()
        {
            return "Slowed Down!";
        }
    }
}
