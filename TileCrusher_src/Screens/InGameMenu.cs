/* The in-game menu allows access to the tutorial and level select.  The screen will also display various records, so a record file must be loaded here and its contents displayed. 
 * The records
 * ------------
 * Total time: total time spent in levels. This is hidden from the player and only displayed on this screen
 * Completion %: shows total number of levels completed as a percentage.
 * Turn Count: total number of turns taken 
 * 
 * The records are stored in a file.  If the file is not found, the game creates it and initilizes the values.
 */


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;    //used to check for trial mode
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using Microsoft.Phone;

using TileCrusher.Sprites;

namespace TileCrusher.Screens
{
    class InGameMenu : Screen
    {
        ScrollBackground background, backgroundCopy;
        Button resumeButton;
        Button exitButton;
        Button tutorialButton;
        Button marketButton;        //only displayed in trial mode
        int windowWidth;

        const short MUSIC_COUNT = 4;       //4 music tracks total
        public static Song[] tracklist = new Song[MUSIC_COUNT];
        
        const string ActionResume = "Resume";
        const string ActionExit = "Exit";
        const string ActionOptions = "Options";
        const string ActionMarket = "Marketplace";

        //records data. These are made static so that they are available to other classes for file manipulation.
        public static ushort hours;    //used to save/load individual units of time
        public static ushort minutes;
        public static ushort seconds;
        public static string totalTime;
        public static string completionRate;
        public static int turnTotal;
        public static short levelsCompleted;   //hidden from player, tracks number of levels completed, and used to calculate completion%
        public static short lossCount;         //number of times players lost a level
        public static short rageCount;        //number of rage quits (pressing the level select button while level's not complete)
        public static short trackNumber;       //tracks selected music.
       

        //level tracking
        const int MAX_LEVELS = 50;
        public static bool[] finishedLevel = new bool[MAX_LEVELS];  //used to track which levels are complete
        public static bool[] lockedLevel = new bool[MAX_LEVELS];   //used to check which levels are locked. The first 10 are always unlocked.

        //bool fileCreated = false;   //checks if record file was created. Used for testing only

        /*this variable checks if game is in trial mode. I'm using a separate variable for this check
        because making the call directly affects performance. It's better to get the result before
        the update loop is called.*/
        bool trialModeEnabled = Guide.IsTrialMode;
        bool marketplaceLoading = false;

        //files & music should only be loaded once in an update loop.
        bool filesLoaded = false;
        public static bool musicPlaying = false;

        public InGameMenu(Game game,SpriteBatch batch,ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.InGameMenu)
        {
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionResume, GestureType.Tap, resumeButton.TouchArea);
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.TouchArea);
            input.AddTouchGestureInput(ActionOptions, GestureType.Tap, tutorialButton.TouchArea);
            if (trialModeEnabled)
                input.AddTouchGestureInput(ActionMarket, GestureType.Tap, marketButton.TouchArea);
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            background = new ScrollBackground(content);
            backgroundCopy = new ScrollBackground(content);
            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backgroundCopy.Position.X = -windowWidth;
            

            resumeButton = new Button(content, "Level Select", new Vector2(60, 400), Color.White);
            tutorialButton = new Button(content, "Help & Options", new Vector2(300, 400), Color.White);
            exitButton = new Button(content, "Back to Title", new Vector2(540, 400), Color.White);
            if (trialModeEnabled)
                marketButton = new Button(content, "Marketplace", new Vector2(300, 50), Color.White);

            //set up music tracks
            tracklist[0] = content.Load<Song>(@"Music/Street Lights");
            tracklist[1] = content.Load<Song>(@"Music/Slipped");
            tracklist[2] = content.Load<Song>(@"Music/Lost");
            tracklist[3] = content.Load<Song>(@"Music/Trade Off");
        }

        public override void Activate()
        {
        }

        protected override void UpdateScreen(GameTime gameTime,DisplayOrientation displayOrientation)
        {
            //load up files
            if (!filesLoaded)
            {
                LoadRecords();
                LoadTrackFile();
                filesLoaded = true;
               
            }

            if (MediaPlayer.GameHasControl && !musicPlaying && trackNumber >= 0)
            {
                //play music
                MediaPlayer.Play(tracklist[trackNumber]);
                MediaPlayer.IsRepeating = true;
                musicPlaying = true;
            }

              

            if (input.IsPressed(ActionResume))
            {
                //FadeToBlack();
                filesLoaded = false;
                changeScreenDelegate(ScreenState.LevelSelect);
            }

            if (input.IsPressed(ActionExit) || BackButtonPressed())
            {
                filesLoaded = false;
                musicPlaying = false;   //the only time music should stop playing
                if (MediaPlayer.GameHasControl)
                {
                    MediaPlayer.Stop();
                }
                changeScreenDelegate(ScreenState.Title);
            }

            if (input.IsPressed(ActionOptions))
            {
                filesLoaded = false;
                changeScreenDelegate(ScreenState.Options);
            }

            if (trialModeEnabled && input.IsPressed(ActionMarket))
            {
                try
                {
                    //go to marketplace
                    Guide.ShowMarketplace(PlayerIndex.One);
                }
                catch   //if we get here, the player pressed the button more than once
                {
                    marketplaceLoading = true;
                }
            }

            //update background
            background.Position.X += 2;
            backgroundCopy.Position.X += 2;

            if (background.Position.X > windowWidth)
                background.Position.X = -windowWidth + 2;   //2 is added to eliminate gap between images.

            if (backgroundCopy.Position.X > windowWidth)
                backgroundCopy.Position.X = -windowWidth + 2;
        }

        protected override void DrawScreen(SpriteBatch batch,DisplayOrientation displayOrientation)
        {
           
            //background
            background.Draw(batch);
            backgroundCopy.Draw(batch);

            //records
            batch.DrawString(font, "RECORDS", new Vector2(330, 140), Color.Red);
            batch.DrawString(font, "Total Time: " + totalTime, new Vector2(300, 170), Color.White);
            batch.DrawString(font, "Completion: " + completionRate + "%", new Vector2(300, 200), Color.White);
            batch.DrawString(font, "Turn Total: " + turnTotal, new Vector2(300, 230), Color.White);
            batch.DrawString(font, "Loss Count: " + lossCount, new Vector2(300, 260), Color.White);
            batch.DrawString(font, "Ragequits: " + rageCount, new Vector2(300, 290), Color.White);

            //if (fileCreated)
            //    batch.DrawString(font, "Record file created!", new Vector2(300, 260), Color.LightGreen);

            //if the player is in trial mode, display a message on the top of the screen, and
            //show a button that exits game and sends player to Marketplace.
            if (trialModeEnabled)
            {
                batch.DrawString(font, "You are in Trial Mode. Click the button below to purchase Tile Crusher!", new Vector2(40, 10), Color.LightGreen);
                marketButton.Draw(batch);

                if (marketplaceLoading)
                {
                    batch.DrawString(font, "Loading Marketplace...", new Vector2(520, 70), Color.LightGreen);
                }
            }


            //buttons
            resumeButton.Draw(batch);
            exitButton.Draw(batch);
            tutorialButton.Draw(batch);
           
        }

        /* Used to create a record file one is not found. Can also be used to reset records. */
        public static void CreateRecord()
        {
            string directory = "TileCrusher";
            string fileName = "Records.txt";

            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //create directory
            fileStorage.CreateDirectory(directory);

            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + "\\" + fileName, System.IO.FileMode.Create, fileStorage);
            StreamWriter fileWrite = new StreamWriter(fs);
               
            //(re)set initial data
            hours = 0;
            minutes = 0;
            seconds = 0;
            totalTime = "00:00";
            completionRate = "0";
            turnTotal = 0;
            levelsCompleted = 0;
            lossCount = 0;
            rageCount = 0;
            
           


            fileWrite.WriteLine(hours);
            fileWrite.WriteLine(minutes);
            fileWrite.WriteLine(seconds);  
            fileWrite.WriteLine(totalTime);         //total time played
            fileWrite.WriteLine(completionRate);    //completion rate
            fileWrite.WriteLine(turnTotal);         //total turns
            fileWrite.WriteLine(levelsCompleted);   //number of levels completed
            fileWrite.WriteLine(lossCount);         //number of losses
            fileWrite.WriteLine(rageCount);         //number of quits before level's complete. If player finished level previously, this doesn't apply
           

            //set up bool arrays & write to file
            for (int i = 0; i < MAX_LEVELS; i++)
            {
                finishedLevel[i] = false;
                fileWrite.Write(finishedLevel[i] + " ");
            }

            //move to next line
            fileWrite.WriteLine();

            for (int j = 0; j < MAX_LEVELS; j++)
            {
                if (j <= 9) //first 10 levels are always unlocked.
                    lockedLevel[j] = false;
                else
                    lockedLevel[j] = true;

                fileWrite.Write(lockedLevel[j] + " ");
            }

            //fileCreated = true;
              
            fileWrite.Close();      //This line is important!
            fs.Close();             //This one too!
       
        }

        void LoadRecords()
        {

            //start searching and reading files
            string directory = "TileCrusher";
            string fileName = "Records.txt";


            /* WP7 uses a different method for writing and reading to file. This is important to remember! */
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //locate the file.  If it doesn't exist, it will be created.
            if (!fileStorage.FileExists(directory + "\\" + fileName))
            {
                CreateRecord();
            }
            
             //read data. The file is read in a specific order.
            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + "\\" + fileName, System.IO.FileMode.Open, fileStorage);
            StreamReader fileRead = new StreamReader(fs);

            //skip the first two lines because we don't need them
            fileRead.ReadLine();
            fileRead.ReadLine();
            fileRead.ReadLine();

            //total time
            totalTime = fileRead.ReadLine();
               
            //completion rate
            completionRate = fileRead.ReadLine();

            //total turns
            string turns = fileRead.ReadLine();
            turnTotal = Int16.Parse(turns);

            //level complete count
            string num = fileRead.ReadLine();
            levelsCompleted = Int16.Parse(num);

            //loss count
            string loss = fileRead.ReadLine();
            lossCount = Int16.Parse(loss);

            //rage count
            string rage = fileRead.ReadLine();
            rageCount = Int16.Parse(rage);
       
            //read the arrays
            string[] str = fileRead.ReadLine().Split(' ');
            for (int i = 0; i < MAX_LEVELS; i++)
            {
                finishedLevel[i] = Boolean.Parse(str[i]);    
            }


            string[] str2 = fileRead.ReadLine().Split(' ');
            for (int j = 10; j < MAX_LEVELS; j++)   //checking from level 10 and onward
            {    
                lockedLevel[j] = Boolean.Parse(str2[j]); 
            } 
                
            //done   
            fileRead.Close();      
            fs.Close();            
            

        }

        /* Create/update a file to save track number. */
        public static void UpdateTrackFile()
        {
            string directory = "TileCrusher";
            string fileName = "Trackfile.txt";

            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //initialize track number if file doesn't exist.
            if (!fileStorage.FileExists(directory + "\\" + fileName))
            {
                trackNumber = 0;
            }

            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + "\\" + fileName, System.IO.FileMode.OpenOrCreate, fileStorage);
            StreamWriter fileWrite = new StreamWriter(fs);

            //write track number to file
            fileWrite.WriteLine(trackNumber);

            fileWrite.Close();      
            fs.Close();             

        }

        //Load track from file
        void LoadTrackFile()
        {

            //start searching and reading files
            string directory = "TileCrusher";
            string fileName = "Trackfile.txt";


            /* WP7 uses a different method for writing and reading to file. This is important to remember! */
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            //locate the file.  If it doesn't exist, it will be created.
            if (!fileStorage.FileExists(directory + "\\" + fileName))
            {
                UpdateTrackFile();
            }

            //read data. The file is read in a specific order.
            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + "\\" + fileName, System.IO.FileMode.Open, fileStorage);
            StreamReader fileRead = new StreamReader(fs);

            //load track number
            string track = fileRead.ReadLine();
            trackNumber = Int16.Parse(track);

            //done   
            fileRead.Close();
            fs.Close();


        }
    }
}
