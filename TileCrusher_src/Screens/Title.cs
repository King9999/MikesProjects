/* The title screen will have the words "TILE" and "CRUSHER" appear from the top and bottom of the screen, respectively.
 * The letters appear individually, one after the other, and move into their designated place. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using TileCrusher.Sprites;
using TileCrusher.Texts;

namespace TileCrusher.Screens
{
    class Title : Screen
    {
        Background background;
        Button startButton;
        Button exitButton;
        Button creditsButton;
       // Button editorButton;    //will not be in the release version

        //title music
        Song titleTheme;

        //destination y position for T, I, L, E
        const float LETTERPOS1 = 150;
        float[] currentPos1 = {-10, -60, -110, -160};   //letters begin off screen

        //destination y positions for CRUSHER
        const float LETTERPOS2 = 250;
         float[] currentPos2 = {490, 540, 590, 640, 690, 740, 790};   //letters begin off screen on opposite end

        bool titleFinishedAnimating = false;    //allow player to tap buttons when title screen is finished
        bool musicPlaying = false;              //checks for title music

        //legal info
        string legal = "Copyright 2011-2012 Mike Murray, all rights reserved.";
        SpriteFont legalFont;

        Texture2D[] letterSetOne = new Texture2D[4];
        Texture2D[] letterSetTwo = new Texture2D[7];
      

        const string ActionStart = "Start";
        const string ActionExit = "Exit";
        const string ActionEdit = "Edit";
        const string ActionCredits = "Credits";

        public Title(Game game,SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.First)
        {
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionStart, GestureType.Tap, startButton.CollisionRectangle);
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.CollisionRectangle);
            input.AddTouchGestureInput(ActionCredits, GestureType.Tap, creditsButton.CollisionRectangle);
            //input.AddTouchGestureInput(ActionEdit, GestureType.Tap, editorButton.CollisionRectangle);
        }

        public override void Activate()
        {

        }

        protected override void LoadScreenContent(ContentManager content)
        {
            background = new Background(content);
          

           startButton = new Button(content, "Start", new Vector2(60, 400), Color.White);

            exitButton = new Button(content, "Exit", new Vector2(300, 400), Color.White);
            creditsButton = new Button(content, "Credits", new Vector2(540, 400), Color.White);
            //editorButton = new Button(content, "Editor", new Vector2(300, 300), Color.White);
            legalFont = content.Load<SpriteFont>(@"Fonts/legalFont");

            titleTheme = content.Load<Song>(@"Music/Time Flies By");

            //letters
            letterSetOne[0] = content.Load<Texture2D>(@"Images/T");
            letterSetOne[1] = content.Load<Texture2D>(@"Images/I");
            letterSetOne[2] = content.Load<Texture2D>(@"Images/L");
            letterSetOne[3] = content.Load<Texture2D>(@"Images/E");

            letterSetTwo[0] = content.Load<Texture2D>(@"Images/C");
            letterSetTwo[1] = content.Load<Texture2D>(@"Images/R");
            letterSetTwo[2] = content.Load<Texture2D>(@"Images/U");
            letterSetTwo[3] = content.Load<Texture2D>(@"Images/S");
            letterSetTwo[4] = content.Load<Texture2D>(@"Images/H");
            letterSetTwo[5] = content.Load<Texture2D>(@"Images/E");
            letterSetTwo[6] = content.Load<Texture2D>(@"Images/R");
        }

        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {
            //increment letter positions
           
            for (int i = 0; i < letterSetOne.Length; i++)
            {
                currentPos1[i] += 8;
                if (currentPos1[i] > LETTERPOS1)
                    currentPos1[i] = LETTERPOS1;

            }

            for (int j = 0; j < letterSetTwo.Length; j++)
            {
                currentPos2[j] -= 8;
                if (currentPos2[j] < LETTERPOS2)
                    currentPos2[j] = LETTERPOS2;

                //if last letter is finished, show buttons.
                if (currentPos2[6] <= LETTERPOS2)
                    titleFinishedAnimating = true;
            }

            //play music
            if (MediaPlayer.GameHasControl && titleFinishedAnimating && !musicPlaying)
            {
                MediaPlayer.Play(titleTheme);
                MediaPlayer.IsRepeating = true;
                musicPlaying = true;
            }

            if (titleFinishedAnimating && input.IsPressed(ActionStart))
            {
                if (MediaPlayer.GameHasControl)
                {
                    MediaPlayer.Stop();
                }
                musicPlaying = false;
                changeScreenDelegate(ScreenState.InGameMenu);
            }
            else if (titleFinishedAnimating && input.IsPressed(ActionExit))
            {
                //soundEffects.PlaySound("SoundEffects/Select");
                changeScreenDelegate(ScreenState.Exit);
            }
            else if (titleFinishedAnimating && input.IsPressed(ActionCredits))
            {
                if (MediaPlayer.GameHasControl)
                {
                    MediaPlayer.Stop();
                }
                musicPlaying = false;
                //soundEffects.PlaySound("SoundEffects/Select");
                changeScreenDelegate(ScreenState.Credits);
            }
            //else if (input.IsPressed(ActionEdit))
            //{
            //    soundEffects.PlaySound("SoundEffects/Select");
            //    changeScreenDelegate(ScreenState.Editor);
            //}
        }

        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
        {
            //background.Draw(batch);

            //draw letters
            for (int i = 0; i < letterSetOne.Length; i++)
            {
                batch.Draw(letterSetOne[i], new Vector2((i * 50 + 200), currentPos1[i]), Color.White);
               
            }

            for (int i = 0; i < letterSetTwo.Length; i++)
            {
                batch.Draw(letterSetTwo[i], new Vector2((i * 50 + 200), currentPos2[i]), Color.White);

            }

            if (titleFinishedAnimating)
            {
                batch.DrawString(legalFont, legal, new Vector2(100, 350), Color.White);
                startButton.Draw(batch);
                exitButton.Draw(batch);
                creditsButton.Draw(batch);
            }
            //editorButton.Draw(batch);
        }
    }
}
