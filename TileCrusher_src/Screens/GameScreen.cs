using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO; 
using System.IO.IsolatedStorage;
using System.Reflection;    //used for recovering embedded resources; in this case, the levels


using TileCrusher.Sprites;
using TileCrusher.Inputs;
using TileCrusher.Texts;



namespace TileCrusher.Screens
{
    class GameScreen : Screen
    {
        //Background background;
        ScrollBackground background, backgroundCopy;
        Level level, levelCopy;
        SpriteFont hudFont;
        SpriteFont buttonFont;
        SpriteFont tileFont;
        int windowWidth;
        
        //timer
        Timer clock;
        
        //Button restartButton;
        Button quitButton;
        Button helpButton;
        Button musicButton;

        /*****Textures*******/
        Texture2D ball;
        Texture2D normalTile;
        Texture2D emptyTile;
        Texture2D superTile;
        Texture2D durableTile;
        Texture2D targetTile;
        Texture2D chainTile;
        Texture2D levelWinImg;
        Texture2D levelLoseImg;

        //music info
        string trackTitle;
        short displayTimer = 0; //displays title for 60 frames.
 
        //used for quitting level
        const string ActionQuit = "Quit";

  
        //tutorial button
        const string ActionTutorial = "Tutorial";

        //change music button
        const string ActionMusic = "Music";

        //bool for loading different levels. When false, a new level will load
        bool levelLoaded = false;

        //controls sound
        bool soundPlaying = false;

        //extra check to make sure player doesn't unlock levels by beating old levels
        bool levelsUnlocked = true;
       
       
        public GameScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen) : base(game, batch, changeScreen, BackButtonScreenType.Gameplay)
        {
           
        }

        //Only taps are used.
        protected override void SetupInputs() 
        {
            //Code for going back to level select
            input.AddTouchGestureInput(ActionQuit, GestureType.Tap, quitButton.TouchArea);

            //tutorial button touch area
            input.AddTouchGestureInput(ActionTutorial, GestureType.Tap, helpButton.TouchArea);

            //music button
            input.AddTouchGestureInput(ActionMusic, GestureType.Tap, musicButton.TouchArea);
            
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            //background = new Background(content);
            background = new ScrollBackground(content);
            backgroundCopy = new ScrollBackground(content);
            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backgroundCopy.Position.X = -windowWidth;

            ball = content.Load<Texture2D>(@"Tiles/WreckingBall");
            normalTile = content.Load<Texture2D>(@"Tiles/NormalTile");
            emptyTile = content.Load<Texture2D>(@"Tiles/EmptyTile");
            superTile = content.Load<Texture2D>(@"Tiles/SuperTile");
            targetTile = content.Load<Texture2D>(@"Tiles/TargetTile");
            durableTile = content.Load<Texture2D>(@"Tiles/DurableTile");
            chainTile = content.Load<Texture2D>(@"Tiles/ChainTile");

            levelWinImg = content.Load<Texture2D>(@"Images/LevelClearImg");
            levelLoseImg = content.Load<Texture2D>(@"Images/LevelFailImg");

            //load fonts
            hudFont = content.Load<SpriteFont>(@"Fonts/screenFont");
            buttonFont = content.Load<SpriteFont>(@"Fonts/buttonFont");
            tileFont = content.Load<SpriteFont>(@"Fonts/tileFont");


            //Button for quickly restarting a level
           // restartButton = new Button(content, "Restart Level", new Vector2(550, 0), Color.White);

            //Button for exiting level
            quitButton = new Button(content, "Level Select", new Vector2(60, 410), Color.White);

            //tutorial button
            helpButton = new Button(content, "Tutorial", new Vector2(300, 410), Color.White);

            //music button
            musicButton = new Button(content, "Change Music", new Vector2(540, 410), Color.White);

            //set up level
            level = new Level(ball, normalTile, emptyTile, hudFont);
            levelCopy = level;

            clock = new Timer();
            
        }

        public override void Activate()
        {    
           
        }

        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {
            if (!levelLoaded)
            {
                //load level
                LoadLevel(LevelSelect.level);
                /***INSERT CODE FOR LOADING CURRENT ELAPSED TIME & TURN COUNT ***/
                LoadRecords();
                levelLoaded = true;
                RevertLevel();
                level.UpdateTileCount();
            }

            //play game until either condition is met
            if (!level.LevelComplete() && !level.LevelFailed())   
            {
                level.Play();
                clock.Tick();
            }
            else if (level.LevelComplete())
            {
                if (!soundPlaying)
                {
                    soundEffects.PlaySound("SoundEffects/Win");
                    soundPlaying = true;
                }
            }
            else if (level.LevelFailed())
            {
                if (!soundPlaying)
                {
                    soundEffects.PlaySound("SoundEffects/Lose");
                    soundPlaying = true;
                }
            }

            //update music display timer
            displayTimer--;
            if (displayTimer < 0)
                displayTimer = 0;


            if (input.IsPressed(ActionQuit) || BackButtonPressed())
            {
                //go back to level select screen
              
                //if the level's not complete and it's not game over, then it's a ragequit.
                //If the player presses the Back button, then it doesn't count.
                if (!level.LevelComplete() && !level.LevelFailed())
                    InGameMenu.rageCount++;

                //if player lost, record the loss
                if (level.LevelFailed())
                    InGameMenu.lossCount++;

                /***INSERT CODE FOR SAVING CURRENT ELAPSED TIME, TOTAL TURN COUNT ***/
                UpdateRecord();

                levelLoaded = false;
                soundPlaying = false;
                changeScreenDelegate(ScreenState.LevelSelect);

            }

            if (input.IsPressed(ActionTutorial))
            {
                //open up the tutorial screen
                //soundEffects.PlaySound("SoundEffects/Select");
                changeScreenDelegate(ScreenState.Tutorial);
            }

            if (MediaPlayer.GameHasControl && input.IsPressed(ActionMusic))
            {
                //change music
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

            level.Draw(batch, tileFont, ball, normalTile, emptyTile, durableTile, superTile, chainTile, targetTile);

            batch.DrawString(font, "Level: " + level.GetLevel(), new Vector2(100, 50), Color.White);
            batch.DrawString(font, "Turn Count: " + level.TurnCount(), new Vector2(220, 50), Color.White);
            batch.DrawString(font, "# of Tiles: " + level.TileCount(), new Vector2(400, 50), Color.White);
            batch.DrawString(font, "Tiles Crushed: " + level.TilesDestroyed(), new Vector2(550, 50), Color.White);

            //display music title
            if (displayTimer > 0)
                batch.DrawString(font, "Now Playing: " + trackTitle, new Vector2(280, 20), Color.LightGreen);
           
            quitButton.Draw(batch);
            helpButton.Draw(batch);
            musicButton.Draw(batch);

            //elapsed time. Used for debug only.
            //batch.DrawString(hudFont, clock.Time() + ":" + clock.Seconds().ToString(), new Vector2(400, 350), Color.White);

            //display win/lose image
            if (level.LevelComplete())
            {
                batch.Draw(levelWinImg, new Vector2(300, 200), Color.White);
            }

            if (level.LevelFailed())
            {
                batch.Draw(levelLoseImg, new Vector2(300, 200), Color.White);
            }
        }

        //reverts current level original state.
        void RevertLevel()
        {
            for (int i = 0; i < level.MaxRows(); i++)
            {
                for (int j = 0; j < level.MaxCols(); j++)
                {
                    level.ChangeTile(i, j, levelCopy.GetValueAtIndex(i, j));
                }
            }

            //copy rest of info
            level.SetPlayerPosition(levelCopy.PlayerRowPos(), levelCopy.PlayerColPos());
            level.SetTurnCount(levelCopy.TurnCount());
            level.SetLevel(levelCopy.GetLevel());
            level.ResetTilesDestroyed();
            level.ResetBoolGrid();
        }


        void LoadLevel(int levelNumber)
        {
            int num = 0;      //used to check what's being read from file
            int num2 = 0;

            //start searching and reading files
            string fileName = "Level" + levelNumber + ".tcl";
            string levelNamespace = "TileCrusher2012.Levels.";

            /*The levels are embedded resources so the following line must be used to retrieve them.*/
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(levelNamespace + fileName);

            /* WP7 uses a different method for writing and reading to file. This is important to remember! */
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

            if (s != null)
            {
                //read data. The file is read in a specific order.
                StreamReader fileRead = new StreamReader(s);


                //level number
                num = Int16.Parse(fileRead.ReadLine());
                level.SetLevel(num);
                levelCopy.SetLevel(num);

                //turn count
                num = Int16.Parse(fileRead.ReadLine());
                level.SetTurnCount(num);
                levelCopy.SetTurnCount(num);

                //player position
                num = Int16.Parse(fileRead.ReadLine());
                num2 = Int16.Parse(fileRead.ReadLine());
                level.SetPlayerPosition(num, num2);
                levelCopy.SetPlayerPosition(num, num2);

                //next is the row of =
                fileRead.ReadLine();

                //level is read next
                for (int i = 0; i < level.MaxRows(); i++)
                {
                    //Take each row of elements and split them into an array so that each
                    //element can be read individually.
                    string[] str = fileRead.ReadLine().Split(' ');

                    for (int j = 0; j < level.MaxCols(); j++)
                    {

                        //convert to an int
                        num = Int16.Parse(str[j]);
                        level.ChangeTile(i, j, num);
                        levelCopy.ChangeTile(i, j, num);
                    }
                }

                //next is the row of -
                fileRead.ReadLine();

                //read in the values from the target array
                for (int i = 0; i < level.MaxRows(); i++)
                {
                    //Take each row of elements and split them into an array so that each
                    //element can be read individually.
                    string[] str = fileRead.ReadLine().Split(' ');

                    for (int j = 0; j < level.MaxCols(); j++)
                    {

                        //convert to an int
                        num = Int16.Parse(str[j]);
                        level.SetTarget(i, j, num);
                        levelCopy.SetTarget(i, j, num);
                    }
                }

                fileRead.Close();      //This line is important!
                s.Close();             //This one too!
            }
           
        }

        /* Used to create a record file one is not found. Can also be used to reset records. */
        void UpdateRecord()
        {
            string directory = "TileCrusher";
            string fileName = "Records.txt";

            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();


            //check if directory exists
            if (file.FileExists(directory + "\\" + fileName))
            {

                IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + "\\" + fileName, System.IO.FileMode.Create, file);
                StreamWriter fileWrite = new StreamWriter(fs);

               
                //save time
                InGameMenu.totalTime = clock.Time();
                InGameMenu.hours = clock.Hour();
                InGameMenu.minutes = clock.Minutes();
                InGameMenu.seconds = clock.Seconds();

                //check if level was completed and write the result
                if (level.LevelComplete() && !InGameMenu.finishedLevel[LevelSelect.level - 1])
                {
                    InGameMenu.finishedLevel[LevelSelect.level - 1] = true;
                    InGameMenu.levelsCompleted++;

                    //calculate completion rate
                    float rate = ((float)InGameMenu.levelsCompleted / (float)level.GetMaxLevels()) * 100;
                    InGameMenu.completionRate = rate.ToString();

                    //prevent player from completing a previously completed level to unlock levels.
                    if (InGameMenu.levelsCompleted % 5 != 0)
                    {
                        levelsUnlocked = false;
                    }
                }

                

                //check if levels need to be unlocked. This check always fails if player is in trial mode
                if (!Guide.IsTrialMode && level.LevelComplete() && (InGameMenu.levelsCompleted > 0 && InGameMenu.levelsCompleted <= 45)
                    && InGameMenu.levelsCompleted % 5 == 0 && !levelsUnlocked)
                {
                    //iterate through the array and unlock more levels until 5 more are unlocked
                    int count = 0;
                    int i = 10; //the iterator. First 10 levels are always unlocked, so no need to check them
                    while (count < 5 && i < level.GetMaxLevels())
                    {
                        if (InGameMenu.lockedLevel[i] == true)
                        {
                            //unlock level
                            InGameMenu.lockedLevel[i] = false;
                            count++;
                        }
                        i++;
                    }

                    levelsUnlocked = true;
                }
               

                //write move count
                InGameMenu.turnTotal = level.MoveCount();

                //write all data
                fileWrite.WriteLine(InGameMenu.hours);
                fileWrite.WriteLine(InGameMenu.minutes);
                fileWrite.WriteLine(InGameMenu.seconds);
                fileWrite.WriteLine(InGameMenu.totalTime);   
                fileWrite.WriteLine(InGameMenu.completionRate);      
                fileWrite.WriteLine(InGameMenu.turnTotal);       
                fileWrite.WriteLine(InGameMenu.levelsCompleted);
                fileWrite.WriteLine(InGameMenu.lossCount);
                fileWrite.WriteLine(InGameMenu.rageCount);

                for (int i = 0; i < level.GetMaxLevels(); i++)
                {
                    fileWrite.Write(InGameMenu.finishedLevel[i] + " ");
                }

                fileWrite.WriteLine();

                for (int j = 0; j < level.GetMaxLevels(); j++)
                {
                    fileWrite.Write(InGameMenu.lockedLevel[j] + " ");
                }

                fileWrite.Close();      
                fs.Close();             
            }
        }

        void LoadRecords()
        {

            //start searching and reading files
            string directory = "TileCrusher";
            string fileName = "Records.txt";


            /* WP7 uses a different method for writing and reading to file. This is important to remember! */
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

           
            //read data. The file is read in a specific order.
            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + "\\" + fileName, System.IO.FileMode.Open, fileStorage);
            StreamReader fileRead = new StreamReader(fs);


            //extract the data from the time and set the timer
            string hour = fileRead.ReadLine();
            ushort hourNum = UInt16.Parse(hour);

            string mins = fileRead.ReadLine();
            ushort minNum = UInt16.Parse(mins);

            string secs = fileRead.ReadLine();
            ushort secNum = UInt16.Parse(secs);

            clock.SetTimer(hourNum, minNum, secNum);

            InGameMenu.totalTime = fileRead.ReadLine();

            //completion rate
            InGameMenu.completionRate = fileRead.ReadLine();

            //total turns
            string num2 = fileRead.ReadLine();
            InGameMenu.turnTotal = Int16.Parse(num2);

            //level complete count
            string num = fileRead.ReadLine();
            InGameMenu.levelsCompleted = Int16.Parse(num);

            //loss count
            string loss = fileRead.ReadLine();
            InGameMenu.lossCount = Int16.Parse(loss);

            //rage count
            string rage = fileRead.ReadLine();
            InGameMenu.rageCount = Int16.Parse(rage);


            string[] str = fileRead.ReadLine().Split(' ');
            for (int i = 0; i < level.GetMaxLevels(); i++)
            {
                InGameMenu.finishedLevel[i] = Boolean.Parse(str[i]);
            }


            string[] str2 = fileRead.ReadLine().Split(' ');
            for (int j = 10; j < level.GetMaxLevels(); j++)   //checking from level 10 and onward
            {
                InGameMenu.lockedLevel[j] = Boolean.Parse(str2[j]);
            } 


            fileRead.Close();      //This line is important!
            fs.Close();             //This one too!


        }
    }
}
