using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using TileCrusher.Sprites;
using TileCrusher.Screens;

namespace TileCrusher.Sprites
{
    class ScrollBackground : Sprite
    {
        public ScrollBackground(ContentManager content)
            : base(content, "Images/ScrollBackground")
        {
        }

        
    }
}
