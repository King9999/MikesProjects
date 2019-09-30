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

using TileCrusher.Texts;
//using TileCrusher.Sprites;

namespace TileCrusher.Sprites
{
    class Button : Sprite
    {
        Text buttonText;
        SpriteFont buttonFont;

        private Rectangle touchArea;
        public Rectangle TouchArea
        {
            get { return touchArea; }
        }

        string displayText;
        public string DisplayText
        {
            get { return displayText; }
        }

        public Button(ContentManager content,string displayText,Vector2 displayPosition,Color color)
            : base(content, "Images/Button")
        {
            Position = displayPosition;
            this.displayText = displayText;
            this.color = color;

            touchArea = new Rectangle((int)displayPosition.X, (int)displayPosition.Y, texture.Width,texture.Height);

            buttonFont = content.Load<SpriteFont>("Fonts/buttonFont");
            buttonText = new Text(buttonFont,displayText,Vector2.Zero,Color.White,Text.Alignment.Both,TouchArea);
        }

        public void ChangePosition(Vector2 adjustment)
        {
            Position += adjustment;
            touchArea = new Rectangle((int)Position.X,(int)Position.Y,texture.Width,texture.Height);
            buttonText.Position += adjustment;
        }

        protected override void DrawSprite(SpriteBatch batch)
        {
            buttonText.Draw(batch);
        }
    }
}
