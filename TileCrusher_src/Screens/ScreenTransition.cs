/* This class is used to fade out any screen.  The class should be modular enough to be usable in any game.  For now,
 * I will do a simple fade to black, but in the future I could add different transitions. */


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileCrusher.Screens
{
    class ScreenTransition : Sprite
    {
        Texture2D blackScreen;      //texture that's used to fade screen.
        Vector2 screenPos = new Vector2(0,0);       //should always equal the screen dimensions
        Color fadeColor;
       // SpriteBatch batch;
        bool transitionDone;        //if true, transition won't happen again until set by user

        public ScreenTransition(ContentManager content, Color screenColor):base(content, "Images/BlackScreen")
        {
            //fadeScreen = new Rectangle(0, 0, GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight);
            fadeColor = screenColor;
            fadeColor.A = 0;    //no alpha by default
            transitionDone = false;
        }

        public void DrawScreen(SpriteBatch batch)
        {
       
             batch.Draw(texture, screenPos, fadeColor);
           
        }

        /***should always go into the update loop 
         * rate: how fast the screen's alpha changes 
         * delayTime: the time in frames that must pass before screen is restored. ***/
        public void FadeOut()
        {
            /* Fade to black */
        
                if (fadeColor.A != 255)
                    fadeColor.A += 1;
      
         
        }

        public void FadeIn()
        {
            if (fadeColor.A != 0)
                fadeColor.A -= 1;
        }

        public bool ScreenIsDark()
        {
            //if true, then screen is completely black
            return fadeColor.A == 255;
        }

    }
}
