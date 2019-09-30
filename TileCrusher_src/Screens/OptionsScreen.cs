/* Options screen.  The player can choose music, reset records, and view the tutorial. A file will save settings. */

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
    class OptionsScreen : Screen
    {

        ScrollBackground background, backgroundCopy;
        Button exitButton;
        Button tutorialButton;
        Button deleteButton;        //deletes records permanently. must double tap to delete, so there's no confirmation
        Button musicButton;         //changes music playing. Affects all screens
        Button aboutButton;         //shows contact info and version number
        SpriteFont screenFont;

        int windowWidth;

        const string ActionMusic = "Music";
        const string ActionDelete = "Delete";
        const string ActionTutorial = "Tutorial";
        const string ActionExit = "Exit";
        const string ActionAbout = "About";

        //record deletion
        bool recordReset = false;

        //music + settings   
        short displayTimer = 0;           //displays music name when track is selected.
        Color displayColor = Color.LightGreen;
        string trackTitle;

        
        

        public OptionsScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Options)
        {
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionTutorial, GestureType.Tap, tutorialButton.TouchArea);
            input.AddTouchGestureInput(ActionMusic, GestureType.Tap, musicButton.TouchArea);
            input.AddTouchGestureInput(ActionDelete, GestureType.Hold, deleteButton.TouchArea); //prevents accidental deletion
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.TouchArea);
            input.AddTouchGestureInput(ActionAbout, GestureType.Tap, aboutButton.TouchArea);
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            background = new ScrollBackground(content);
            backgroundCopy = new ScrollBackground(content);
            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backgroundCopy.Position.X = -windowWidth;

            tutorialButton = new Button(content, "Tutorial", new Vector2(100, 100), Color.White);
            musicButton = new Button(content, "Choose BGM", new Vector2(500, 100), Color.White);
            deleteButton = new Button(content, "Reset Records", new Vector2(100, 200), Color.White);
            exitButton = new Button(content, "Main Menu", new Vector2(300, 400), Color.White);
            aboutButton = new Button(content, "About", new Vector2(500, 200), Color.White);
            screenFont = content.Load<SpriteFont>(@"Fonts/screenFont");           
        }

        public override void Activate()
        {
        }

        //Vector2 touchPos;
        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {

            if (input.IsPressed(ActionTutorial))
            {
                changeScreenDelegate(ScreenState.Tutorial);
                displayTimer = 0;
            }

            if (input.IsPressed(ActionAbout))
            {
                displayTimer = 0;
                recordReset = false;
                changeScreenDelegate(ScreenState.About);   
            }

            if (MediaPlayer.GameHasControl && input.IsPressed(ActionMusic)) //game MUST have control in order to play music
            {
                //change music or turn it off.
                if (InGameMenu.trackNumber == 3)
                {
                    InGameMenu.trackNumber = -1;  //no music should play
                    MediaPlayer.Stop();
                    trackTitle = "Nothing";
                    InGameMenu.musicPlaying = false;
                }
                else if (InGameMenu.trackNumber < 0) 
                {
                    InGameMenu.trackNumber = 0;    //start over
                    MediaPlayer.Play(InGameMenu.tracklist[InGameMenu.trackNumber]);
                    MediaPlayer.IsRepeating = true;
                    trackTitle = InGameMenu.tracklist[InGameMenu.trackNumber].Name.Substring(6);    //eliminates "Music/" from the name
                    InGameMenu.musicPlaying = true;
                }
                else
                {
                    InGameMenu.trackNumber++;
                    MediaPlayer.Play(InGameMenu.tracklist[InGameMenu.trackNumber]);
                    MediaPlayer.IsRepeating = true;
                    trackTitle = InGameMenu.tracklist[InGameMenu.trackNumber].Name.Substring(6);
                    InGameMenu.musicPlaying = true;
                }

                //save settings
                InGameMenu.UpdateTrackFile();

                //set display timer
                displayTimer = 60;
            }

            if (!recordReset && input.IsPressed(ActionDelete))
            {
                //reset record
                recordReset = true;
                soundEffects.PlaySound("SoundEffects/Win");
                InGameMenu.CreateRecord();
            }

          
            if (input.IsPressed(ActionExit) || BackButtonPressed())
            {
                recordReset = false;
                displayTimer = 0;
                changeScreenDelegate(ScreenState.InGameMenu);
            }

            //update timer
            displayTimer--;
            if (displayTimer < 0)
                displayTimer = 0;

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

            if (!MediaPlayer.GameHasControl)
                batch.DrawString(screenFont, "Music is currently disabled. Turn off Zune to enable game music.", new Vector2(70, 10), Color.LightGreen);

            //draw track title
            if (displayTimer > 0)
                batch.DrawString(screenFont, "Now playing: " + trackTitle, new Vector2(280, 10), displayColor);
          
            //warning message for record deletion
            batch.DrawString(screenFont, "Hold button to reset", new Vector2(100, 270), Color.White);

            //reset confirmation
            if (recordReset)
                batch.DrawString(screenFont, "Record Reset.", new Vector2(130, 290), Color.Red);

            //buttons
            tutorialButton.Draw(batch);
            musicButton.Draw(batch);
            deleteButton.Draw(batch);
            aboutButton.Draw(batch);
            exitButton.Draw(batch);
        }

       

    }
}
