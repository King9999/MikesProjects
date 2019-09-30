/* The level select screen.  Not all levels should be available from the start, but the player
 * can unlock levels quickly.  The first 10 levels are available; for every 5 levels that are completed,
 * 5 more levels are unlocked. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.GamerServices;

using TileCrusher.Sprites;

namespace TileCrusher.Screens
{
    class LevelSelect : Screen
    {
        ScrollBackground background, backgroundCopy;
        LevelButton levelButton;
        List<LevelButton> levelList;
        Button exitButton;
        SpriteFont levelFont;
        SpriteFont titleFont;
        Texture2D levelTile;
        Texture2D padlock;      //used to display locked levels
        Texture2D checkmark;    //used to show completed levels

        int windowWidth;
        const short MAX_LEVELS = 50;
        const short MAX_ROWS = 5;
        const short MAX_COLS = 10;
        public static short level;                    //this is accessed by the GameScreen class.
        bool trialModeEnabled = Guide.IsTrialMode;  //checks for trial status

        List<string> ActionSelectList = new List<string>();
        
        const string ActionSelect = "Select";
        const string ActionExit = "Exit";

        

        public LevelSelect(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.LevelSelect)
        {
        }

        protected override void SetupInputs()
        {
            for (int i = 0; i < MAX_LEVELS; i++)
            {

                input.AddTouchGestureInput(ActionSelect, GestureType.Tap, levelList[i].TouchArea);
                
            }
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.TouchArea);
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            background = new ScrollBackground(content);
            backgroundCopy = new ScrollBackground(content);
            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backgroundCopy.Position.X = -windowWidth;

            //resumeButton = new Button(content, "Go!", new Vector2(180, 400), Color.White);

            exitButton = new Button(content, "Main Menu", new Vector2(300, 400), Color.White);
            levelFont = content.Load<SpriteFont>(@"Fonts/tileFont");
            titleFont = content.Load<SpriteFont>(@"Fonts/screenFont");
            levelTile = content.Load<Texture2D>(@"Images/LevelSelectTile");
            padlock = content.Load<Texture2D>(@"Images/Padlock");
            checkmark = content.Load<Texture2D>(@"Images/Checkmark");

            levelList = new List<LevelButton>();

            //draw level buttons
            short level = 1;
            int xOffset = 70;
            int yOffset = 45;

            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    
                    levelButton = new LevelButton(content, level.ToString(), level, 
                        new Vector2(j * 70f + xOffset, i * 70f + yOffset), Color.Red);
                    levelList.Add(levelButton); //there should be 50 of these
                    level++;
                    
                }
            }

            
        }

        public override void Activate()
        {
        }

        //Vector2 touchPos;
        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {

            if (input.IsPressed(ActionSelect))
            {
                //check which level was selected by checking the tap position
                foreach (LevelButton b in levelList)
                {
                    //touchPos = input.CurrentGesturePosition(ActionSelect);
                        
                    //System.Console.WriteLine("Touch Position " + i + ": " + input.CurrentGesturePosition(ActionSelect));

                    if ((input.CurrentGesturePosition(ActionSelect).X >= b.Position.X && 
                        input.CurrentGesturePosition(ActionSelect).X <= b.Position.X + b.Width) &&
                        (input.CurrentGesturePosition(ActionSelect).Y >= b.Position.Y && 
                        input.CurrentGesturePosition(ActionSelect).Y <= b.Position.Y + b.Height)) 
                    {
                        //get the level number
                        level = b.LevelNumber();

                        //if the level is locked, can't continue
                        if (InGameMenu.lockedLevel[level - 1])
                        {
                            //play a sound effect indicating player can't continue
                            soundEffects.PlaySound("SoundEffects/Noaccess");
                        }
                        else
                        {
                            changeScreenDelegate(ScreenState.GameScreen);
                        }
                    }
                }

   
            }

          
            if (input.IsPressed(ActionExit))
            {
                changeScreenDelegate(ScreenState.InGameMenu);
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

            batch.DrawString(titleFont, "Tap a level to play", new Vector2(300, 0), Color.Yellow);

            if (trialModeEnabled)
            {
                batch.DrawString(titleFont, "Purchase Tile Crusher to access level 11 and beyond!", new Vector2(150, 370), Color.LightGreen);
            }

            //draw touch location for debugging purposes
            //batch.DrawString(titleFont, touchPos.ToString(), new Vector2(0, 440), Color.White);


            for (int i = 0; i < levelList.Count; i++)
            {
                levelList[i].Draw(batch);

                //draw a padlock for each locked level
                if (InGameMenu.lockedLevel[i])
                {
                    batch.Draw(padlock, levelList[i].Position, Color.White);
                }

                //draw a checkmark if player finished level
                if (InGameMenu.finishedLevel[i])
                {
                    batch.Draw(checkmark, levelList[i].Position, Color.White);
                }
            }

            exitButton.Draw(batch);
        }

        public int GetLevel() { return level; }
    }
}
