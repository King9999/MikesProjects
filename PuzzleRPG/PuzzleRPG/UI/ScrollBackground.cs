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
using PuzzleRPG.UI;
using PuzzleRPG.Screens;

namespace PuzzleRPG.UI
{
    class ScrollBackground : Sprite
    {
        public ScrollBackground(ContentManager content)
            : base(content, "Images/ScrollBackground")
        {
        }

        
    }
}
