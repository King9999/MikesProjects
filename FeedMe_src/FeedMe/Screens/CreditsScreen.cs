using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;


namespace FeedMe.Screens
{
    class CreditsScreen : Screen
    {
        private List<string> role;    //contains roles
        private List<string> name;    //contains names
        private string finalCredit;     //end of credits
        private Texture2D backButtonImg;      //goes back to previous screen
        Rectangle backButtonArea;
        private SpriteFont creditFont;  //using "Moire" font
        const string ActionExit = "Back";
        private float rolePos;     //the Y position of the credits
        private float namePos;
        private float finalPos;     //Y position of last credit
        private Texture2D screen;   //background
        private bool musicPlaying = false;
        private Song creditsMusic;

        public CreditsScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Other)
        {

        }

        protected override void LoadScreenContent(ContentManager content)
        {
            creditFont = content.Load<SpriteFont>(@"Fonts/screenFont");
            
            screen = content.Load<Texture2D>(@"Images/BlackScreen");
            backButtonImg = content.Load<Texture2D>(@"Images/backbutton");
            creditsMusic = content.Load<Song>(@"Sounds/nintendo");
            rolePos = 480;
            namePos = 520;

            backButtonArea = new Rectangle(10, 440, backButtonImg.Width, backButtonImg.Height);

            /* Credit setup. The order of the roles corresponds to the names associated with them */
            role = new List<string>();
            role.Add("Game Design");
            role.Add("Graphics");
            role.Add("Programming");
            role.Add("Music");
            role.Add("Sound FX");
            role.Add("Special Thanks");

            name = new List<string>();
            name.Add("Mike Murray");
            name.Add("Mike Murray");
            name.Add("Mike Murray");
            name.Add("Matt McFarland - mattmcfarland.com");
            name.Add("Freesound.org");
            name.Add("TriOS College - trios.com"
                + "\nXMG Studio - xmgstudio.com");

            finalCredit = "Copyright 2012 Mike Murray"
                + "\nAll rights reserved"
                + "\nmmking9999.com"
                + "\nThank you for playing!  This is only" 
                + "\nthe beginning. Watch me...";
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, backButtonArea);
        }

        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation screenOrientation)
        {
            //draw background
            batch.Draw(screen, new Vector2(0, 0), Color.White);

            //back button
            batch.Draw(backButtonImg, backButtonArea, Color.White);

            //display credits
            for (int i = 0; i < role.Count; i++)
            {
                batch.DrawString(creditFont, role[i], new Vector2(225, rolePos + (i * 80)), Color.Aquamarine);
                batch.DrawString(creditFont, name[i], new Vector2(225, namePos + (i * 80)), Color.White);
                finalPos = namePos + (i * 80);
               // batch.DrawString(creditFont, "Name Pos: " + namePos, new Vector2(0, 400), Color.Red);
            }

            batch.DrawString(creditFont, finalCredit, new Vector2(225, finalPos + 300), Color.White);
        }

        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation screenOrientation)
        {
            //play music only if game has control of media player
            if (MediaPlayer.GameHasControl && !musicPlaying)
            {
                MediaPlayer.Play(creditsMusic);
                musicPlaying = true;
            }
           //the credits scroll from the bottom to the top of the screen.  It should stop scrolling when it gets to the
            //thank you message.
          
            rolePos--;
            namePos--; 
            
            //the final credit should remain on the screen until the player exits.
            if (namePos < -630)
            {
                namePos = -630; //stop counting, otherwise something bad could happen
                rolePos = -590;
            }

            if (input.IsPressed(ActionExit) || BackButtonPressed())
            {
                //reset credits
                rolePos = 480;
                namePos = 520;

                if (MediaPlayer.GameHasControl)
                {
                    MediaPlayer.Stop();
                }
                musicPlaying = false;
                changeScreenDelegate(ScreenState.Title);
            }
        }
    }
}
