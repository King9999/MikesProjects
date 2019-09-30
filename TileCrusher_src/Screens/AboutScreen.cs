/* About screen.  Provides contact information and version number. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.IO.IsolatedStorage;
using TileCrusher.Sprites;

namespace TileCrusher.Screens
{
    class AboutScreen : Screen
    {

        ScrollBackground background, backgroundCopy;
        Button exitButton;
        SpriteFont screenFont;

        int windowWidth;

        const string ActionExit = "Exit";

        //version number
        const string VERSION = "1.1";

        //contact info. should be able to tap these and pull up the appropriate page
        const string twitter= "@MikeADMurray";
        const string email = "mmking9999@gmail.com";
        const string website = "mmking9999.com";

        public AboutScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Other)
        {
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.TouchArea);
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            background = new ScrollBackground(content);
            backgroundCopy = new ScrollBackground(content);
            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backgroundCopy.Position.X = -windowWidth;

            exitButton = new Button(content, "Back", new Vector2(300, 400), Color.White);
            screenFont = content.Load<SpriteFont>(@"Fonts/screenFont");
            
        }

        public override void Activate()
        {
        }

        //Vector2 touchPos;
        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {

            if (input.IsPressed(ActionExit))
            {
                changeScreenDelegate(ScreenState.Options);
            }


            //update background
            background.Position.X += 2;
            backgroundCopy.Position.X += 2;

            if (background.Position.X > windowWidth)
                background.Position.X = -windowWidth + 2;   //2 is added to eliminate gap between images.

            if (backgroundCopy.Position.X > windowWidth)
                backgroundCopy.Position.X = -windowWidth + 2;
        }

        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
        {
            background.Draw(batch);
            backgroundCopy.Draw(batch);

            //contact
            batch.DrawString(screenFont, "Follow me on Twitter: " + twitter, new Vector2(300, 100), Color.White);
            batch.DrawString(screenFont, "Email: " + email, new Vector2(300, 150), Color.White);
            batch.DrawString(screenFont, "Web site: " + website, new Vector2(300, 200), Color.White);
          
            //draw version information
            batch.DrawString(screenFont, "Version: " + VERSION, new Vector2(300, 300), Color.Salmon);

            //buttons
            exitButton.Draw(batch);
        }



    }
}

