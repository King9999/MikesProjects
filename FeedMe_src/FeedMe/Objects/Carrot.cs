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
    class Carrot : Food
    {
        //with derived classes, I can assign values in the classes themselves without having to define them in GameScreen
        public Carrot(ContentManager content, Vector2 location)
            :base(content, "Food/carrot", "Carrot", location, 6)
        {
            mPosition = location;
        }

        public override void ActivateSpecial(Player player)
        {
            Debug.WriteLine("Carrot eaten");
        }

        public override string EffectMessage()
        {
            return base.EffectMessage();
        }
    }
}
