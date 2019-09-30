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
using FeedMe.UI;
using FeedMe.Screens;


namespace FeedMe.UI
{
    class Background : Sprite
    {
        public Background(ContentManager content)
            : base(content, "Images/Background")
        {
        }
    }
}
