//#region Using Statements
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input.Touch;
//using Microsoft.Xna.Framework.GamerServices;    //used to acquire the onscreen keyboard
//using System;
//using System.IO;
//using System.IO.IsolatedStorage;
//using System.Reflection;

//using TileCrusher.Sprites;
//using TileCrusher.Texts;
//#endregion

///* This is a level editor for the game. It will be integrated into the game, but removed for the release version.
// * The editor has to be in the game due to the way WP7 reads and writes data. */
//namespace TileCrusher.Screens
//{
//    class EditorScreen : Screen
//    {
//        ScrollBackground background, backgroundCopy;
//        int windowWidth;
//        Level level;
//        Level levelCopy;    //copy of currently created level

//        //textures
//        Texture2D ball;

//        //fonts
//        SpriteFont hudFont;
//        SpriteFont tileFont;
        
//        SpriteFont buttonFont;
        
//        //buttons
//        Button newButton;
//        Button loadButton;
//        Button saveButton;
//        Button setButton;
//        Button turnButton;
//        Button testButton;
//        Button exitButton;
      
//        //button textures + positions
//        const int MAX_BUTTONS = 6;

//        Texture2D normalTile;
//        Texture2D emptyTile;
//        Texture2D superTile;
//        Texture2D impassable;
//        Texture2D durableTile;
//        Texture2D targetTile;
//        Texture2D chainTile;
//        Texture2D selection;        //highlights currently selected tile
//        Texture2D levelBoundary;    //shows the edges of the level. Should not be able to draw past
//        Texture2D entryWindow;      //used for entering data.
//        Texture2D winImage;         //displayed when level's completed
//        Texture2D loseImage;        //image when you lose.

//        //tile button touch positions
//        Rectangle normalArea;
//        Rectangle emptyArea;
//        Rectangle durableArea;
//        Rectangle superArea;
//        Rectangle targetArea;
//        Rectangle chainArea;
//        Rectangle impassableArea;
//        Rectangle ballArea;

//        //tile button screen positions
//        Vector2 normalTilePos;
//        Vector2 emptyTilePos;
//        Vector2 superTilePos;
//        Vector2 impassablePos;
//        Vector2 durableTilePos;
//        Vector2 targetTilePos;
//        Vector2 chainTilePos;
//        Vector2 ballPos;

//        //level complete screen positions
//        Vector2 levelCompletePos;

//        //level area touch position
//        Rectangle levelArea;

//        //HUD
//        Vector2 selectionPos;       //this position changes depended on where mouse is clicked.
//        string noticeMessage = "";       //warning messages, etc.
//        bool testingLevel = false;          //game is playable if true


//        //tile ID. Used to change elements in the array.
//        private int tileID = 1; //draws normal tiles by default


//        int target;         //target tile number


//        //actions for the editor buttons
//        const string ActionNew = "New";
//        const string ActionLoad = "Load";
//        const string ActionSave = "Save";
//        const string ActionSetLevel = "Set Level";
//        const string ActionTurn = "Turn Count";
//        const string ActionTest = "Test Level";
//        const string ActionExit = "Exit";

//        //actions for tile buttons
//        const string ActionNormal = "NormalTile";
//        const string ActionEmpty = "EmptyTile";
//        const string ActionDurable = "DurableTile";
//        const string ActionSuper = "SuperTile";
//        const string ActionTarget = "TargetTile";
//        const string ActionChain = "ChainTile";
//        const string ActionBlocked = "BlockedTile";
//        const string ActionBall = "Ball";

//        //touch action for level area
//        const string ActionLevel = "Level";

       
//        public EditorScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen) : base(game, batch, changeScreen, BackButtonScreenType.Editor)
//        {
           
//        }

//        //Only taps are used.
//        protected override void SetupInputs() 
//        {
//            //editor button gestures
//            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.TouchArea);
//            input.AddTouchGestureInput(ActionNew, GestureType.Tap, newButton.TouchArea);
//            input.AddTouchGestureInput(ActionSave, GestureType.Tap, saveButton.TouchArea);
//            input.AddTouchGestureInput(ActionTest, GestureType.Tap, testButton.TouchArea);
//            input.AddTouchGestureInput(ActionSetLevel, GestureType.Tap, setButton.TouchArea);
//            input.AddTouchGestureInput(ActionTurn, GestureType.Tap, turnButton.TouchArea);
//            input.AddTouchGestureInput(ActionLoad, GestureType.Tap, loadButton.TouchArea);

//            //tile button gestures
//            input.AddTouchGestureInput(ActionNormal, GestureType.Tap, normalArea);
//            input.AddTouchGestureInput(ActionEmpty, GestureType.Tap, emptyArea);
//            input.AddTouchGestureInput(ActionDurable, GestureType.Tap, durableArea);
//            input.AddTouchGestureInput(ActionSuper, GestureType.Tap, superArea);
//            input.AddTouchGestureInput(ActionTarget, GestureType.Tap, targetArea);
//            input.AddTouchGestureInput(ActionChain, GestureType.Tap, chainArea);
//            input.AddTouchGestureInput(ActionBlocked, GestureType.Tap, impassableArea);
//            input.AddTouchGestureInput(ActionBall, GestureType.Tap, ballArea);

            
//            input.AddTouchGestureInput(ActionLevel, GestureType.Tap, levelArea);

//        }

//        protected override void LoadScreenContent(ContentManager content)
//        {
//            background = new ScrollBackground(content);
//            backgroundCopy = new ScrollBackground(content);
//            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
//            backgroundCopy.Position.X = -windowWidth;
            

//            ball = content.Load<Texture2D>(@"EditorImages/eWreckingBall");
//            normalTile = content.Load<Texture2D>(@"EditorImages/eNormalTile");
//            emptyTile = content.Load<Texture2D>(@"EditorImages/eEmptyTile");
//            superTile = content.Load<Texture2D>(@"EditorImages/eSuperTile");
//            targetTile = content.Load<Texture2D>(@"EditorImages/eTargetTile");
//            durableTile = content.Load<Texture2D>(@"EditorImages/eDurableTile");
//            chainTile = content.Load<Texture2D>(@"EditorImages/eChainTile");
//            impassable = content.Load<Texture2D>(@"EditorImages/eImpassable");
//            selection = content.Load<Texture2D>(@"EditorImages/eSelection");

//            levelBoundary = content.Load<Texture2D>(@"EditorImages/eLevelBoundary");
//            winImage = content.Load<Texture2D>(@"EditorImages/eLevelClearImg");
//            loseImage = content.Load<Texture2D>(@"EditorImages/eLevelFailImg");

//            //load fonts
//            hudFont = content.Load<SpriteFont>(@"Fonts/screenFont");
//            buttonFont = content.Load<SpriteFont>(@"Fonts/buttonFont");
//            tileFont = content.Load<SpriteFont>(@"Fonts/tileFont");

//            //load buttons
//            exitButton = new Button(content, "Exit", new Vector2(Screen.ScreenWidth - 200, Screen.ScreenHeight - 70), Color.White);
//            newButton = new Button(content, "New Level", new Vector2(0, 270), Color.White);
//            loadButton = new Button(content, "Load Level", new Vector2(0, 340), Color.White);
//            saveButton = new Button(content, "Save Level", new Vector2(0, 410), Color.White);
//            setButton = new Button(content, "Level#", new Vector2(200, 270), Color.White);
//            turnButton = new Button(content, "Turn Count", new Vector2(200, 340), Color.White);
//            testButton = new Button(content, "Test", new Vector2(200, 410), Color.White);
            
//            level = new Level(ball, normalTile, emptyTile, hudFont);
//            levelCopy = new Level(ball, normalTile, emptyTile, hudFont);
//            noticeMessage = "";

//            //load positions
//            normalTilePos = new Vector2(500, 270);
//            emptyTilePos = new Vector2(560, 270);
//            durableTilePos = new Vector2(620, 270);
//            superTilePos = new Vector2(680, 270);
//            targetTilePos = new Vector2(500, 340);
//            chainTilePos = new Vector2(560, 340);
//            impassablePos = new Vector2(620, 340);
//            ballPos = new Vector2(680, 340);
//            selectionPos = normalTilePos - new Vector2(5, 5);
//            levelCompletePos = new Vector2(350, 180);


//            //touch positions
//            normalArea = new Rectangle((int)normalTilePos.X, (int)normalTilePos.Y, normalTile.Width, normalTile.Height);
//            emptyArea = new Rectangle((int)emptyTilePos.X, (int)emptyTilePos.Y, normalTile.Width, normalTile.Height);
//            durableArea = new Rectangle((int)durableTilePos.X, (int)durableTilePos.Y, normalTile.Width, normalTile.Height);
//            superArea = new Rectangle((int)superTilePos.X, (int)superTilePos.Y, normalTile.Width, normalTile.Height);
//            targetArea = new Rectangle((int)targetTilePos.X, (int)targetTilePos.Y, normalTile.Width, normalTile.Height);
//            chainArea = new Rectangle((int)chainTilePos.X, (int)chainTilePos.Y, normalTile.Width, normalTile.Height);
//            impassableArea = new Rectangle((int)impassablePos.X, (int)impassablePos.Y, normalTile.Width, normalTile.Height);
//            ballArea = new Rectangle((int)ballPos.X, (int)ballPos.Y, normalTile.Width, normalTile.Height);
//            levelArea = new Rectangle(level.OffsetX(), level.OffsetY(), level.TileSize() * level.MaxCols(), level.TileSize() * level.MaxRows());
            
//        }

//        public override void Activate()
//        {    
           
//        }

//        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
//        {
//            //update tile count
//            //level.UpdateTileCount();

//            CheckInput();
           

//            if (testingLevel)
//                level.Play();


//            //update background
//            float deltaX = 2f;
//            background.Position.X += deltaX;
//            backgroundCopy.Position.X += deltaX;

//            if (background.Position.X > windowWidth)
//                background.Position.X = -windowWidth + deltaX;   //deltaX is added to eliminate gap between images.

//            if (backgroundCopy.Position.X > windowWidth)
//                backgroundCopy.Position.X = -windowWidth + deltaX;

//        }

       
//        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
//        {
          
//            //draw background
//            background.Draw(batch);
//            backgroundCopy.Draw(batch);

//            //draw message
//            batch.DrawString(hudFont, noticeMessage, new Vector2(200, 10), Color.White);

//            //draw level info
//            float infoPosX = 0;
//            float infoPosY = 50;
//            batch.DrawString(hudFont, "Level: " + level.GetLevel(), new Vector2(infoPosX, infoPosY), Color.Yellow);
//            batch.DrawString(hudFont, "Turn Count: " + level.TurnCount(), new Vector2(infoPosX, infoPosY+30), Color.Yellow);
//            batch.DrawString(hudFont, "Tile Count: " + level.TileCount(), new Vector2(infoPosX, infoPosY+60), Color.Yellow);
//            batch.DrawString(hudFont, "Tiles Destroyed: " + level.TilesDestroyed(), new Vector2(infoPosX, infoPosY+90), Color.Yellow);
//            batch.DrawString(hudFont, "Ball Pos: " + level.PlayerXPos() + ", " + level.PlayerYPos(), new Vector2(infoPosX, infoPosY + 120), Color.Yellow);
//            batch.DrawString(hudFont, "Row: " + level.PlayerRowPos() +  "  Col: " + level.PlayerColPos(), new Vector2(infoPosX, infoPosY + 150), Color.Yellow);


//            //draw level
//            batch.Draw(levelBoundary, new Vector2(195, 45), Color.White);
//            level.Draw(batch, tileFont, ball, normalTile, emptyTile, durableTile, superTile, chainTile, targetTile);


//            //draw editor buttons
//            newButton.Draw(batch);
//            loadButton.Draw(batch);
//            saveButton.Draw(batch);
//            setButton.Draw(batch);
//            turnButton.Draw(batch);
//            testButton.Draw(batch);
//            exitButton.Draw(batch);

//            //draw tile buttons
//            batch.Draw(selection, selectionPos, Color.White);   //must be drawn first
//            DrawTileButton(batch, normalTile, "NOR", normalTilePos);
//            DrawTileButton(batch, emptyTile, "EMP", emptyTilePos);
//            DrawTileButton(batch, durableTile, "DUR", durableTilePos);
//            DrawTileButton(batch, superTile, "SUP", superTilePos);
//            DrawTileButton(batch, targetTile, "TAR", targetTilePos);
//            DrawTileButton(batch, chainTile, "CHA", chainTilePos);
//            DrawTileButton(batch, impassable, "BLO", impassablePos);
//            DrawTileButton(batch, ball, "BAL", ballPos);

//            //check for terminating condition
//            if (testingLevel && level.LevelComplete())
//            {
//                batch.Draw(winImage, levelCompletePos, Color.White);
//            }

//            if (testingLevel && level.LevelFailed())
//            {
//                batch.Draw(loseImage, levelCompletePos, Color.White);
//            }
//        }

//        //Used to draw tile buttons and display text below them
//        void DrawTileButton(SpriteBatch batch, Texture2D texture, string name, Vector2 position)
//        {
//            Vector2 textPos = new Vector2(position.X, position.Y + 40);
//            batch.Draw(texture, position, Color.White);
//            batch.DrawString(hudFont, name, textPos, Color.White);
//        }

//        /* Contains all actions for touching buttons. Must go in the update loop */
//        void CheckInput()
//        {
//            #region Editor Buttons
//            /******Editor buttons ***********/
//            /***New Level Button***/
//            if (!testingLevel && input.IsPressed(ActionNew))
//            {
//                //create new level
//                soundEffects.PlaySound("SoundEffects/Select");

//                noticeMessage = "New level created.";
//                level = new Level(ball, normalTile, emptyTile, hudFont);
//                //set copy of level
//                levelCopy = new Level(ball, normalTile, emptyTile, hudFont);
//            }

//            /***Load Level Button***/
//            if (!testingLevel && input.IsPressed(ActionLoad))
//            {
//                //user must type in the level they want to load, and the game will check
//                //if it exists and load it.
//                soundEffects.PlaySound("SoundEffects/Select");
//                Guide.BeginShowKeyboardInput(PlayerIndex.One, "Choose a level to load", "(Min = 1, Max = 50)", "", LocateLevel, null);
//            }

//            /***Save Level Button***/
//            if (!testingLevel && input.IsPressed(ActionSave))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");

//                //do not allow level to be saved if the player position is not valid
//                if (level.GetValueAtPlayerPosition() == -1)
//                {
//                    noticeMessage = "Player position is invalid!";
//                }
//                else
//                {
//                    SaveLevel();
//                }
//            }

//            /***Set Level Button***/
//            if (!testingLevel && input.IsPressed(ActionSetLevel))
//            {
//                //bring up the keyboard so the user can enter the level number
//                soundEffects.PlaySound("SoundEffects/Select");
//                Guide.BeginShowKeyboardInput(PlayerIndex.One, "What level is this?", "(Min = 1, Max = 50)", "", GetLevel, null);
//            }

//            /***Turn Count Button***/
//            if (!testingLevel && input.IsPressed(ActionTurn))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                Guide.BeginShowKeyboardInput(PlayerIndex.One, "How many turns?", "(Min = 10, Max = 99)", "", GetTurnCount, null);
                
//            }

//            /***Test Button***/
//            if (input.IsPressed(ActionTest))
//            {
//                if (!testingLevel)
//                {
//                    if (level.GetValueAtPlayerPosition() == -1)
//                    {
//                        //can't test level
//                        noticeMessage = "Player is at an invalid position!";
//                    }
//                    else
//                    {
//                        noticeMessage = "Testing level. Press the 'Test' button to stop.";
//                        testingLevel = true;
//                    }
                    
//                }
//                else
//                {
//                    //revert level back to what it was before test started.
//                    RevertLevel();
//                    testingLevel = false;
//                    noticeMessage = "Testing stopped.";
//                }
//            }

//            if (input.IsPressed(ActionExit))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                changeScreenDelegate(ScreenState.Title);
//            }
//            #endregion

//            #region Tile Buttons
//            /*****Tile Buttons********/

//            /***Normal Tile***/
//            if (!testingLevel && input.IsPressed(ActionNormal))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = normalTilePos - new Vector2(5, 5);
//                noticeMessage = "Common tiles that must be destroyed to finish a level";
//                tileID = 1;
//            }

//            /***Empty Tile***/
//            if (!testingLevel && input.IsPressed(ActionEmpty))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = emptyTilePos - new Vector2(5, 5);
//                noticeMessage = "Will revert to a normal tile if rolled on!";
//                tileID = 0;
//            }

//            /***Durable Tile***/
//            if (!testingLevel && input.IsPressed(ActionDurable))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = durableTilePos - new Vector2(5, 5);
//                Guide.BeginShowKeyboardInput(PlayerIndex.One, "Enter durable tile's health", "(Min = 2, Max = 10)", "", GetTileHealth, null);
//                noticeMessage = "Requires more than one pass before it's destroyed";
//            }

//            /***Super Tile***/
//            if (!testingLevel && input.IsPressed(ActionSuper))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = superTilePos - new Vector2(5, 5);
//                noticeMessage = "Can't be destroyed, but does not affect turn count!";
//                tileID = 11;
//            }

//            /***Target Tile***/
//            if (!testingLevel && input.IsPressed(ActionTarget))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = targetTilePos - new Vector2(5, 5);
//                Guide.BeginShowKeyboardInput(PlayerIndex.One, "Enter a target number", "(Min = 1, Max = 49)", "", GetTarget, null);
//                noticeMessage = "Tile can be broken if target # is met";
                
//            }

//            /***Chain Tile***/
//            if (!testingLevel && input.IsPressed(ActionChain))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = chainTilePos - new Vector2(5, 5);
//                noticeMessage = "Breaks/reverts entire lines of tiles";
//                tileID = 13;
//            }

//            /***Blocked Tile***/
//            if (!testingLevel && input.IsPressed(ActionBlocked))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = impassablePos - new Vector2(5, 5);
//                noticeMessage = "Ball cannot move onto this space";
//                tileID = -1;
//            }

//            /***Wrecking Ball***/
//            if (!testingLevel && input.IsPressed(ActionBall))
//            {
//                soundEffects.PlaySound("SoundEffects/Select");
//                selectionPos = ballPos - new Vector2(5, 5);
//                noticeMessage = "The player. There can only be one!";
//                tileID = 99;
//            }

//            #endregion

//            #region Level Area
//            /***Level Area
//                 * If the level is touched, the editor should also check which tile in the level was 
//                 * tapped and change it to the currently selected tile.  This should also affect the array 
//                 * that the level uses to display the tiles. ***/
//            if (!testingLevel && input.IsPressed(ActionLevel))
//            {
//                //change the element in the array.  We get the element's position in the array
//                //by taking the mouse's x and y position, divide each by the tile's pixel size.  For the x & y values, we
//                //eliminate the offsets.
//                int colPos = ((int)input.CurrentGesturePosition(ActionLevel).X - level.OffsetX()) / level.TileSize();
//                int rowPos = ((int)input.CurrentGesturePosition(ActionLevel).Y - level.OffsetY()) / level.TileSize();

//                //ensure array doesn't go out of bounds
//                if (colPos > 9)
//                    colPos = 9;
//                if (rowPos > 4)
//                    rowPos = 4;

//                //Console.WriteLine("Row Pos: " + rowPos);
//                //Console.WriteLine("Col Pos: " + colPos);

//                //check what's being drawn
//                if (tileID == 99)
//                {
//                    //draw ball
//                    level.SetPlayerPosition(rowPos, colPos);
//                    levelCopy.SetPlayerPosition(rowPos, colPos);
//                }
//                else if (tileID == 12)
//                {
//                    level.SetTarget(rowPos, colPos, target);
//                    level.ChangeTile(rowPos, colPos, tileID);

//                    levelCopy.SetTarget(rowPos, colPos, target);
//                    levelCopy.ChangeTile(rowPos, colPos, tileID);
//                }
//                else
//                {
//                    //draw tile
//                    level.ChangeTile(rowPos, colPos, tileID);
//                    levelCopy.ChangeTile(rowPos, colPos, tileID);
//                }
//            }
//            #endregion
//        }

//        //reverts current level to what it was before testing began.
//        void RevertLevel()
//        {
//            for (int i = 0; i < level.MaxRows(); i++)
//            {
//                for (int j = 0; j < level.MaxCols(); j++)
//                {
//                    level.ChangeTile(i, j, levelCopy.GetValueAtIndex(i, j));
//                }
//            }

//            //copy rest of info
//            level.SetPlayerPosition(levelCopy.PlayerRowPos(), levelCopy.PlayerColPos());
//            level.SetTurnCount(levelCopy.TurnCount());
//            level.SetLevel(levelCopy.GetLevel());
//            level.ResetTilesDestroyed();
//            level.ResetBoolGrid();
//        }

//        #region Callback methods
//        /* Callback method used to get keyboard input and apply it to wherever it needs to go. */
//        void GetLevel(IAsyncResult result)
//        {
//            try
//            {
//                int levelNum = Int16.Parse(Guide.EndShowKeyboardInput(result));
//                level.SetLevel(levelNum);
//                levelCopy.SetLevel(levelNum);
//                noticeMessage = "Level number set.";
//            }
//            catch
//            {
//                //warn the user if there is no input
//                noticeMessage = "You must enter a valid level number.";
//            }
//        }

//        void GetTurnCount(IAsyncResult result)
//        {
//            try
//            {
//                int turnCount = Int16.Parse(Guide.EndShowKeyboardInput(result));
//                level.SetTurnCount(turnCount);
//                levelCopy.SetTurnCount(turnCount);
//                noticeMessage = "Turn count set.";
//            }
//            catch
//            {
//                //warn the user if there is no input
//                noticeMessage = "You must enter a valid turn number.";
//            }
//        }

//        void LocateLevel(IAsyncResult result)
//        {
//            try
//            {
//                int levelNum = Int16.Parse(Guide.EndShowKeyboardInput(result));
//                LoadLevel(levelNum);
//            }
//            catch
//            {
//                //warn the user if there is no input
//                noticeMessage = "You must enter a valid level number.";
//            }
//        }

//        void GetTileHealth(IAsyncResult result)
//        {
//            try
//            {
//                int tileHp = Int16.Parse(Guide.EndShowKeyboardInput(result));

//                //enforce the health rule
//                if (tileHp < 2 || tileHp > 10)
//                    tileHp = 2;

//                tileID = tileHp;
//                noticeMessage = "Health set. Re-tap the DUR tile to change its health.";
//            }
//            catch
//            {
//                //warn the user if there is no input
//                noticeMessage = "You must enter a valid number.";
//            }
//        }

//        void GetTarget(IAsyncResult result)
//        {
//            try
//            {
//                //set the target number. This changes the global variable
//                target = Int16.Parse(Guide.EndShowKeyboardInput(result));

//                //tile ID must be set here, in case things go wrong. Don't want user to be able to drop tiles regardless.
//                tileID = 12;
//                noticeMessage = "Target set. Re-tap the TAR tile to change target.";
//            }
//            catch
//            {
//                //warn the user if there is no input
//                noticeMessage = "You must enter a valid number.";
//            }
//        }
//        #endregion

//        #region Read/Write Functions
//        //write level to a file.  File name cannot be adjusted; this makes it easier for the game to find 
//        //the files it needs to load levels. All files will be saved as "levelX.tcl," where X is the level
//        //number the user specified before saving the level. The level can only be saved if the player's position is valid.
//        void SaveLevel()
//        {

//            string fileName = "Level" + level.GetLevel() + ".tcl";
//            string directory = @"TileCrusher/Levels";

//            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

//            //check if directory exists.  If not, create it
//            if (!file.FileExists(directory))
//                file.CreateDirectory(directory);


//            //write data to file
//            IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + fileName, System.IO.FileMode.Create, file);
//            StreamWriter fileWrite = new StreamWriter(fs);


//            //write info to file
//            fileWrite.WriteLine(level.GetLevel());  //level number
//            fileWrite.WriteLine(level.TurnCount()); //turn count
//            fileWrite.WriteLine(level.PlayerRowPos());  //player row position
//            fileWrite.WriteLine(level.PlayerColPos());  //player col position

//            //past this line, the level should be written
//            fileWrite.WriteLine("===========================");

//            for (int i = 0; i < level.MaxRows(); i++)
//            {
//                for (int j = 0; j < level.MaxCols(); j++)
//                {
//                    fileWrite.Write(level.GetValueAtIndex(i, j) + " ");
//                }

//                //add new line
//                fileWrite.WriteLine();
//            }

//            //- sign is the end of the level
//            fileWrite.WriteLine("---------------------------");

//            //write the target grid to file
//            for (int i = 0; i < level.MaxRows(); i++)
//            {
//                for (int j = 0; j < level.MaxCols(); j++)
//                {
//                    fileWrite.Write(level.GetTargetAtIndex(i, j) + " ");
//                }

//                //add new line
//                fileWrite.WriteLine();
//            }

//            //end of file when % is read
//            fileWrite.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

//            fileWrite.Close();      //This line is important!
//            fs.Close();             //This one too!

//            noticeMessage = fileName + " saved successfully.";

//        }

//        void LoadLevel(int levelNumber)
//        {
//            int num = 0;      //used to check what's being read from file
//            int num2 = 0;

//            //start searching and reading files
//            string fileName = "Level" + levelNumber + ".tcl";
//            //string directory = @"Levels";
//            string levelNamespace = "TileCrusher2012.Levels.";

//            /*The levels are embedded resources so the following line must be used to retrieve them.*/
//            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(levelNamespace + fileName); 
               
//            /* WP7 uses a different method for writing and reading to file. This is important to remember! */
//            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

//            //if (file.FileExists(directory + fileName))
//            if (s != null)
//            {
//                //read data. The file is read in a specific order.
//                //IsolatedStorageFileStream fs = new IsolatedStorageFileStream(directory + fileName, System.IO.FileMode.Open, file);
//                //StreamReader fileRead = new StreamReader(fs);

//                //FileStream fs = new FileStream(directory + fileName, FileMode.Open);
//                StreamReader fileRead = new StreamReader(s);


//                //level number
//                num = Int16.Parse(fileRead.ReadLine());
//                level.SetLevel(num);
//                levelCopy.SetLevel(num);

//                //turn count
//                num = Int16.Parse(fileRead.ReadLine());
//                level.SetTurnCount(num);
//                levelCopy.SetTurnCount(num);

//                //player position
//                num = Int16.Parse(fileRead.ReadLine());
//                num2 = Int16.Parse(fileRead.ReadLine());
//                level.SetPlayerPosition(num, num2);
//                levelCopy.SetPlayerPosition(num, num2);

//                //next is the row of =
//                fileRead.ReadLine();

//                //level is read next
//                for (int i = 0; i < level.MaxRows(); i++)
//                {
//                    //Take each row of elements and split them into an array so that each
//                    //element can be read individually.
//                    string[] str = fileRead.ReadLine().Split(' ');

//                    for (int j = 0; j < level.MaxCols(); j++)
//                    {

//                        //convert to an int
//                        num = Int16.Parse(str[j]);
//                        level.ChangeTile(i, j, num);
//                        levelCopy.ChangeTile(i, j, num);
//                    }
//                }

//                //next is the row of -
//                fileRead.ReadLine();

//                //read in the values from the target array
//                for (int i = 0; i < level.MaxRows(); i++)
//                {
//                    //Take each row of elements and split them into an array so that each
//                    //element can be read individually.
//                    string[] str = fileRead.ReadLine().Split(' ');

//                    for (int j = 0; j < level.MaxCols(); j++)
//                    {

//                        //convert to an int
//                        num = Int16.Parse(str[j]);
//                        level.SetTarget(i, j, num);
//                        levelCopy.SetTarget(i, j, num);
//                    }
//                }

//                fileRead.Close();      //This line is important!
//                s.Close();             //This one too!

//                noticeMessage = fileName + " loaded successfully.";
//            }
//            else
//            {
//                //could not find file, let user know
//                noticeMessage = "Unable to locate " + fileName + "!";
//            }

//        }

//        #endregion
//    }
//}
