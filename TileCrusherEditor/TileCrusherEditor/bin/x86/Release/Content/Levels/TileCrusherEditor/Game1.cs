using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;        //needed for reading/writing to a file

namespace TileCrusherEditor
{
#if WINDOWS
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level;
        Level levelCopy;    //copy of currently created level

        //textures
        Texture2D ball;
        //Texture2D emptyTile;
        //Texture2D normalTile;

        //fonts
        SpriteFont hudFont;
        SpriteFont tileFont;

        //mouse
        MouseState mouse;

        //button textures + positions
        Texture2D button;
        Texture2D pressedButton;
        Texture2D[] buttonState;      //used to alternate between pressed and depressed 0=new, 1=load, 2=save, 3=level, 4=turn count
        const int MAX_BUTTONS = 6;

        Texture2D normalTile;
        Texture2D emptyTile;
        Texture2D invTile;
        Texture2D impassable;
        Texture2D durableTile;
        Texture2D targetTile;
        Texture2D chainTile;
        Texture2D selection;        //highlights currently selected tile
        Texture2D levelBoundary;    //shows the edges of the level. Should not be able to draw past
        Texture2D background;       //wallpaper for the editor. should scroll.
        Texture2D backgroundCopy;
        Texture2D entryWindow;      //used for entering data.
        Texture2D winImage;         //displayed when level's completed
        Texture2D loseImage;        //image when you lose.

        //blue buttons
        Vector2 newButtonPos;
        Vector2 loadButtonPos;
        Vector2 saveButtonPos;
        Vector2 levelButtonPos;
        Vector2 turnButtonPos;
        Vector2 testButtonPos;

        //tile buttons
        Vector2 norTileButtonPos;
        Vector2 empTileButtonPos;
        Vector2 invTilePos;
        Vector2 impButtonPos;
        Vector2 durTileButtonPos;
        Vector2 tarTileButtonPos;
        Vector2 chaTileButtonPos;
        Vector2 ballButtonPos;

        //HUD
        Vector2 backgroundPos;
        Vector2 backgroundPosCopy;
        Vector2 selectionPos;       //this position changes depended on where mouse is clicked.
        Vector2 entryWindowPos;
        Vector2 noticeMsgPos;
        bool levelWindowOpen;         //when true, entry window pops up
        bool turnWindowOpen;
        bool loadWindowOpen;
        bool durableTileWindowOpen;
        bool targetTileWindowOpen;
        string windowMessage;       //changes depending on which button is clicked.
        string noticeMessage;       //warning messages, etc.
        bool testingLevel;          //game is playable if true
        bool testButtonClicked;     //prevents rapid input by holding down button
       

        int mousePosX = 0;
        int mousePosY = 0;

        //level display area
        private int levelAreaStartX = 100;   //x offset before tiles are displayed
        private int levelAreaEndX = 900;     //(size of tile X number of tiles in a row) + offset
        private int levelAreaStartY = 50;
        private int levelAreaEndY = 450;     //tile pixel size X number of rows

        //tile ID. Used to change elements in the array.
        private int tileID;

        //keyboard state
        KeyboardState keyState;

        //user input
        string stringInput;
        int inputDelay;     //adds a number of frames before you can type again.
        int target;         //target tile number

        //file creation variables. As a test, I will save as a txt file, but I will make a custom file type
        //to prevent unwanted access.
        FileStream fs;
        StreamWriter fileWrite;
        StreamReader fileRead;
        string fileName;
        string directory;

        //screen res variables
        float windowWidth;
        float windowHeight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //set screen resolution
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;

            Content.RootDirectory = "Content";

            //show mouse cursor
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            level = new Level(ball, normalTile, emptyTile, hudFont);
            //set copy of level
            levelCopy = new Level(ball, normalTile, emptyTile, hudFont);
            

            newButtonPos = new Vector2(60, 500);
            loadButtonPos = new Vector2(60, 580);
            saveButtonPos = new Vector2(60, 660);
            levelButtonPos = new Vector2(280, 500);
            turnButtonPos = new Vector2(280, 580);
            testButtonPos = new Vector2(280, 660);


            norTileButtonPos = new Vector2(550, 500);
            empTileButtonPos = new Vector2(650, 500);
            durTileButtonPos = new Vector2(750, 500);
            invTilePos = new Vector2(850, 500);
            tarTileButtonPos = new Vector2(550, 620);
            chaTileButtonPos = new Vector2(650, 620);
            impButtonPos = new Vector2(750, 620);
            ballButtonPos = new Vector2(850, 620);

            selectionPos = new Vector2(545, 495);   //position should always be tile button's position - 5 for both x and y.
            backgroundPos = new Vector2(0, 0);
            backgroundPosCopy = new Vector2(-windowWidth, 0); //1 is added to remove space between images
            entryWindowPos = new Vector2(windowWidth / 3.5f, windowHeight / 3.8f);
            noticeMsgPos = new Vector2(windowWidth / 3f, windowHeight / 1.65f);


            buttonState = new Texture2D[MAX_BUTTONS];

            //by default, normal tiles are drawn.
            tileID = 1;

            turnWindowOpen = false;
            levelWindowOpen = false;
            loadWindowOpen = false;
            durableTileWindowOpen = false;
            targetTileWindowOpen = false;
            windowMessage = "";
            noticeMessage = "";

            stringInput = "";
            inputDelay = 0;
            target = 0;

            testingLevel = false;
            testButtonClicked = false;
            
            
           
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = this.Content.Load<Texture2D>(@"Tiles/WreckingBall");
            emptyTile = this.Content.Load<Texture2D>(@"Tiles/EmptyTile");
            normalTile = this.Content.Load<Texture2D>(@"Tiles/NormalTile");

            //buttons
            button = this.Content.Load<Texture2D>(@"Buttons/Button");
            pressedButton = this.Content.Load<Texture2D>(@"Buttons/ButtonPressed");

            for (int i = 0; i < MAX_BUTTONS; i++)
            {
                buttonState[i] = button;
            }

            normalTile = this.Content.Load<Texture2D>(@"Tiles/NormalTile");
            emptyTile = this.Content.Load<Texture2D>(@"Tiles/EmptyTile");
            impassable = this.Content.Load<Texture2D>(@"Tiles/Impassable");
            invTile = this.Content.Load<Texture2D>(@"Tiles/InvincibleTile");
            targetTile = this.Content.Load<Texture2D>(@"Tiles/TargetTile");
            durableTile = this.Content.Load<Texture2D>(@"Tiles/DurableTile");
            chainTile = this.Content.Load<Texture2D>(@"Tiles/ChainTile");


            selection = this.Content.Load<Texture2D>(@"HUD/Selection");
            levelBoundary = this.Content.Load<Texture2D>(@"HUD/LevelBoundary");
            background = this.Content.Load<Texture2D>(@"HUD/Background");
            backgroundCopy = this.Content.Load<Texture2D>(@"HUD/Background");
            entryWindow = this.Content.Load<Texture2D>(@"HUD/EntryWindow");
            winImage = this.Content.Load<Texture2D>(@"HUD/LevelClearImg");
            loseImage = this.Content.Load<Texture2D>(@"HUD/LevelFailImg");

            hudFont = this.Content.Load<SpriteFont>(@"Fonts/HUD");
            tileFont = this.Content.Load<SpriteFont>(@"Fonts/tileFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
          
            //update type speed
            inputDelay--;

            if (inputDelay < 0)
                inputDelay = 0;

            //update tile count
            level.UpdateTileCount();

           

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //scroll the background infinitely
            backgroundPos.X++;
            backgroundPosCopy.X++;

            if (backgroundPos.X > windowWidth)
                backgroundPos.X = -windowWidth+1;   //1 is added to eliminate gap between images.

            if (backgroundPosCopy.X > windowWidth)
                backgroundPosCopy.X = -windowWidth+1;


            //check mouse/keyboard input
            CheckInput();


            //check if testing level. No mouse input is allowed except to stop testing.
            if (testingLevel)
                level.Play();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //draw background
            spriteBatch.Draw(background, backgroundPos, Color.White);
            spriteBatch.Draw(backgroundCopy, backgroundPosCopy, Color.White);

            //draw boundary before drawing level
            DrawTileButton(levelBoundary, "", new Vector2(90, 40));
            level.Draw(spriteBatch, tileFont, ball, normalTile, emptyTile, durableTile, invTile, chainTile, targetTile);
            
            //draw buttons and the appropriate texture
            DrawButton(buttonState[0], "New Level", newButtonPos);
            DrawButton(buttonState[1], "Load Level", loadButtonPos);
            DrawButton(buttonState[2], "Save Level", saveButtonPos);
            DrawButton(buttonState[3], "Set Level#", levelButtonPos);
            DrawButton(buttonState[4], "Turn Count", turnButtonPos);
            DrawButton(buttonState[5], "Test Level", testButtonPos);

            //draw selection icon over selected tile.
            DrawTileButton(selection, "", selectionPos);
           

            //draw tile buttons
            DrawTileButton(normalTile, "Normal", norTileButtonPos);
            DrawTileButton(emptyTile, "Empty", empTileButtonPos);
            DrawTileButton(durableTile, "Durable", durTileButtonPos);
            DrawTileButton(invTile, "Invincible", invTilePos);
            DrawTileButton(targetTile, "Target", tarTileButtonPos);
            DrawTileButton(chainTile, "Chain", chaTileButtonPos);
            DrawTileButton(impassable, "Blocked", impButtonPos);
            DrawTileButton(ball, "Ball", ballButtonPos);

            //draw level information
            spriteBatch.DrawString(hudFont, "Level: " + level.GetLevel().ToString() +
                "    Turn Count: " + level.TurnCount().ToString() + "    Tile Count: " +
            level.TileCount().ToString() + "    Tiles Destroyed: " + level.TilesDestroyed().ToString(),
            new Vector2(220, 10), Color.White);

            //draw version information
            spriteBatch.DrawString(hudFont, "Version 1.0 by Mike Murray", new Vector2(5, windowHeight-30), Color.Salmon);

            //draw mouse position
            //spriteBatch.DrawString(hudFont, "Mouse Coords: (" + mousePosX.ToString() + 
            //    ", " + mousePosY.ToString() + ")", new Vector2(400, 460), Color.LightYellow);

            //draw notice messages
            spriteBatch.DrawString(hudFont, noticeMessage, noticeMsgPos, Color.White);

            //draw entry window
            if (loadWindowOpen || turnWindowOpen || levelWindowOpen
                || durableTileWindowOpen || targetTileWindowOpen)
            {
                DrawEntryWindow(windowMessage);
            }

            //if level is finished then display win message
            if (testingLevel)
            {
                if (level.LevelComplete())
                {
                    spriteBatch.Draw(winImage, new Vector2(windowWidth / 3, windowHeight / 3), Color.White);
                }
                else if (level.LevelFailed())
                {
                    spriteBatch.Draw(loseImage, new Vector2(windowWidth / 3, windowHeight / 3), Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //reverts current level to what it was before testing began.
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

        /* Used to draw buttons and to display text on them */
        void DrawButton(Texture2D texture, string name, Vector2 position)
        {
            Vector2 textPos = new Vector2(position.X + 50, position.Y + 20);
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(hudFont, name, textPos, Color.White);
        }

        //Used to draw tile buttons and display text below them
        void DrawTileButton(Texture2D texture, string name, Vector2 position)
        {
            Vector2 textPos = new Vector2(position.X, position.Y + 80);
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(hudFont, name, textPos, Color.White);
        }

        //draws entry window to the screen.
        void DrawEntryWindow(string message)
        {
            Vector2 textPos = new Vector2(entryWindowPos.X * 1.1f, entryWindowPos.Y * 1.2f);
            Vector2 inputPos = new Vector2(entryWindowPos.X * 1.2f, entryWindowPos.Y * 1.71f);
            spriteBatch.Draw(entryWindow, entryWindowPos, Color.White);
            spriteBatch.DrawString(hudFont, message, textPos, Color.White);

            //get user input
            try
            {
                spriteBatch.DrawString(hudFont, GetKeyboardInput(), inputPos, Color.White);
            }
            catch
            {
                stringInput = "";
            }
   
        }

        //write level to a file.  File name cannot be adjusted; this makes it easier for the game to find 
        //the files it needs to load levels.Should be able to specify a filename before saving.  Also, can only 
        //save if the player's position is valid.
        void SaveLevel()
        {
          
            fileName = "Level"+ level.GetLevel().ToString() + ".tcl";
            directory = @"Content\\Levels\\";

            //check if directory exists.  If not, create it
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            
            fs = new FileStream(directory + fileName, FileMode.Create);
            fileWrite = new StreamWriter(fs);

            //write info to file
            fileWrite.WriteLine(level.GetLevel());  //level number
            fileWrite.WriteLine(level.TurnCount()); //turn count
            //fileWrite.WriteLine(level.TileCount()); //tile count
            fileWrite.WriteLine(level.PlayerRowPos());  //player row position
            fileWrite.WriteLine(level.PlayerColPos());  //player col position

            //past this line, the level should be written
            fileWrite.WriteLine("===========================");

            for (int i = 0; i < level.MaxRows(); i++)
            {
                for (int j = 0; j < level.MaxCols(); j++)
                {
                    fileWrite.Write(level.GetValueAtIndex(i, j) + " ");
                }

                //add new line
                fileWrite.WriteLine();
            }

            //- sign is the end of the level
            fileWrite.WriteLine("---------------------------");

            //write the target grid to file
            for (int i = 0; i < level.MaxRows(); i++)
            {
                for (int j = 0; j < level.MaxCols(); j++)
                {
                    fileWrite.Write(level.GetTargetAtIndex(i, j) + " ");
                }

                //add new line
                fileWrite.WriteLine();
            }

            //end of file when % is read
            fileWrite.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            fileWrite.Close();      //This line is important!
            fs.Close();             //This one too!
           
        }

        //Searches for a file and reads its contents.  If I can figure this out, I can
        //use it in the game itself.
        void LoadLevel(int levelNumber)
        {

            int num = 0;      //used to check what's being read from file
            int num2 = 0;

            //start searching and reading files
            fileName = "Level" + levelNumber + ".tcl";
            directory = @"Content\\Levels\\";

            if (File.Exists(directory + fileName))
            {
                //read data. The file is read in a specific order.
                fs = new FileStream(directory + fileName, FileMode.Open);
                fileRead = new StreamReader(fs);

                

                //level number
                //num = Convert.ToInt16(fileRead.ReadLine());
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
                fs.Close();             //This one too!
                noticeMessage =  fileName + " loaded successfully.";
            }
            else
            {
                //can't find file
                noticeMessage = "Unable to locate " + fileName + "!";
            }
        }

        //This function takes keyboard input from the user and outputs the result.
        //This only accepts numbers.
        string GetKeyboardInput()
        {
            Keys[] pressedKey = Keyboard.GetState().GetPressedKeys();

            if (inputDelay <= 0)
            {
                for (int i = 0; i < pressedKey.Length; i++)
                {

                    int keyCode = (int)pressedKey[i];
                    if (stringInput.Length < 2)
                    {
                       
                        stringInput += (char)keyCode;
                        inputDelay = 9;
                       
                        //TODO: make the typing better
                    }

                    if (keyState.IsKeyDown(Keys.Back) || keyState.IsKeyDown(Keys.Delete))
                        //remove whatever was typed
                        stringInput = "";
                }               
            }

           
            //remove any whitespace. TODO: currently does not work
            stringInput.Trim();

            return stringInput;
        }


        /* Checks which buttons are pressed and performs action. Should go in the Update function. */
        void CheckInput()
        {
            // Get mouse and keyboard state
            mouse = Mouse.GetState();
            keyState = Keyboard.GetState();

            //get mouse coordinates; for debugging only
            mousePosX = mouse.X;
            mousePosY = mouse.Y;

            //If the ESC key is pressed, the entry window is closed, and any data in the window is erased.
            if (keyState.IsKeyDown(Keys.Escape))
            {
                loadWindowOpen = false;
                turnWindowOpen = false;
                levelWindowOpen = false;
                durableTileWindowOpen = false;
                targetTileWindowOpen = false;
                stringInput = "";
                noticeMessage = "";

                //stop testing
                testingLevel = false;
                RevertLevel();
            }

            /******Checking the window states******/

            if (keyState.IsKeyDown(Keys.Enter) && loadWindowOpen)
            {
                //TODO: insert code to ensure that the user input is valid and change level number
                //try
                //{
                    int input = Convert.ToInt16(stringInput);

                    LoadLevel(input);
                    //noticeMessage = fileName + " loaded successfully.";
                    loadWindowOpen = false;
               // }
                //catch
                //{
                //    noticeMessage = "What you entered was not a number!";
                //    loadWindowOpen = false;
                //}

            }
     
            if (keyState.IsKeyDown(Keys.Enter) && levelWindowOpen)
            {
               //TODO: insert code to ensure that the user input is valid and change level number
                try
                {
                    int input = Convert.ToInt16(stringInput);

                    level.SetLevel(input);
                    levelCopy.SetLevel(input);
                    noticeMessage = "Level number set.";
                    levelWindowOpen = false;
                }
                catch
                {
                    noticeMessage = "What you entered was not a number!";
                    levelWindowOpen = false;
                }

            }

            if (keyState.IsKeyDown(Keys.Enter) && turnWindowOpen)
            {
                //TODO: insert code to ensure that the user input is valid and change turn count
                try
                {
                    int input = Convert.ToInt16(stringInput);
                    level.SetTurnCount(input);
                    levelCopy.SetTurnCount(input);
                    noticeMessage = "Turn count set.";
                    turnWindowOpen = false;
                }
                catch
                {
                    noticeMessage = "What you entered was not a number!";
                    turnWindowOpen = false;
                }
               
            }

            if (keyState.IsKeyDown(Keys.Enter) && durableTileWindowOpen)
            {
                //TODO: insert code to ensure that the user input is valid and change tile health
                try
                {
                    int input = Convert.ToInt16(stringInput);


                    if (input >= 2 && input <= 10)
                        tileID = input;
                    else
                        tileID = 2;


                    noticeMessage = "To set the health for a different tile, re-select the Durable Tile.";
                    durableTileWindowOpen = false;
                }
                catch
                {
                    noticeMessage = "What you entered was not a number!";
                    durableTileWindowOpen = false;
                }
            }

            if (keyState.IsKeyDown(Keys.Enter) && targetTileWindowOpen)
            {
                //TODO: insert code to ensure that the user input is valid and change target number
                try
                {
                    target = Convert.ToInt16(stringInput);  //max number is 49
                    tileID = 12;
                    noticeMessage = "To set the target for a different tile, re-select the Target Tile.";

                    targetTileWindowOpen = false;
                }
                catch
                {
                    noticeMessage = "What you entered was not a number!";
                    targetTileWindowOpen = false;
                }
            }

            //Left mouse button input
            if (mouse.LeftButton == ButtonState.Pressed && !loadWindowOpen && !levelWindowOpen
                && !turnWindowOpen && !durableTileWindowOpen && !targetTileWindowOpen)
            {
                if (!testingLevel)
                {
                    //prevent feedback from disappearing if testing. Lets the user know 
                    //that testing is in progress
                    noticeMessage = "";
                }
 
                /*******What was clicked?*******/



                /***New Level Button***/
                if (!testingLevel && (mouse.X >= newButtonPos.X && mouse.X <= newButtonPos.X + 200) && 
                    (mouse.Y >= newButtonPos.Y && mouse.Y <= newButtonPos.Y + 70))
                   
                {
                    //highlight the button with other texture
                    buttonState[0] = pressedButton;

                    noticeMessage = "New level created.";
                    level = new Level(ball, normalTile, emptyTile, hudFont);
                    //set copy of level
                    levelCopy = new Level(ball, normalTile, emptyTile, hudFont);
                }


                /***Load Level Button***/
                if (!testingLevel && (mouse.X >= loadButtonPos.X && mouse.X <= loadButtonPos.X + 200) && 
                    (mouse.Y >= loadButtonPos.Y && mouse.Y <= loadButtonPos.Y + 70))
                {
                    //highlight the button with other texture
                    buttonState[1] = pressedButton;
                    windowMessage = "Choose a level to load \n(Min = 1, Max = 50)";
                    stringInput = "";
                    loadWindowOpen = true;      //causes a window to open in the update loop

                }


                /***Save Level Button***/
                if (!testingLevel && (mouse.X >= saveButtonPos.X && mouse.X <= saveButtonPos.X + 200) &&
                    (mouse.Y >= saveButtonPos.Y && mouse.Y <= saveButtonPos.Y + 70))
                {
                    //highlight the button with other texture
                    buttonState[2] = pressedButton;

                    //do not allow level to be saved if the player position is not valid
                    if (level.GetValueAtPlayerPosition() == -1)
                    {
                        noticeMessage = "Player position is invalid!";
                    }
                    else
                    {
                        SaveLevel();
                        noticeMessage = fileName + " saved.";
                    }
                  
                }

                /***Set Level Button***/
                if (!testingLevel && (mouse.X >= levelButtonPos.X && mouse.X <= levelButtonPos.X + 200) && 
                    (mouse.Y >= levelButtonPos.Y && mouse.Y <= levelButtonPos.Y + 70))
                {
                    //highlight the button with other texture
                    buttonState[3] = pressedButton;
                    windowMessage = "What level is this? (Min = 1, Max = 50) \n Press ESC to cancel";
                    stringInput = "";
                    levelWindowOpen = true;
                }

                /***Turn Count Button***/
                if (!testingLevel && (mouse.X >= turnButtonPos.X && mouse.X <= turnButtonPos.X + 200) && 
                    (mouse.Y >= turnButtonPos.Y && mouse.Y <= turnButtonPos.Y + 70))
                {
                    //highlight the button with other texture
                    buttonState[4] = pressedButton;
                    windowMessage = "How many turns? (Min = 10, Max = 99) \n Press ESC to cancel";
                    stringInput = "";
                    turnWindowOpen = true;
                }

                /***Test Button***/
                if (!testButtonClicked && (mouse.X >= testButtonPos.X && mouse.X <= testButtonPos.X + 200) && 
                    (mouse.Y >= testButtonPos.Y && mouse.Y <= testButtonPos.Y + 70))
                {
                    

                    //highlight the button with other texture
                    buttonState[5] = pressedButton;
                    if (!testingLevel)
                    {
                        if (level.GetValueAtPlayerPosition() == -1)
                        {
                            //can't test level
                            noticeMessage = "Player is at an invalid position!";
                        }
                        else
                        {
                            
                            noticeMessage = "Testing level. Press the 'Test Level' button or ESC key to stop.";
                        }
                        testingLevel = true;
                    }
                    else
                    {
                        //revert level back to what it was before test started.
                        RevertLevel();
                        testingLevel = false;    
                    }
                    testButtonClicked = true;
                }

                

                //*******TILE BUTTONS********

                /***Normal Tile Button *****/
                if (!testingLevel && (mouse.X >= norTileButtonPos.X && mouse.X <= norTileButtonPos.X + 80) && 
                    (mouse.Y >= norTileButtonPos.Y && mouse.Y <= norTileButtonPos.Y + 80))
                {
                    selectionPos = norTileButtonPos - new Vector2(5, 5);
                    noticeMessage = "Common tiles that must be destroyed to finish a level";
                    tileID = 1;
                }

                /***Empty Tile Button *****/
                if (!testingLevel && (mouse.X >= empTileButtonPos.X && mouse.X <= empTileButtonPos.X + 80) && 
                    (mouse.Y >= empTileButtonPos.Y && mouse.Y <= empTileButtonPos.Y + 80))
                {
                    selectionPos = empTileButtonPos - new Vector2(5, 5);
                    noticeMessage = "Will revert to a normal tile if rolled on!";
                    tileID = 0;
                }

                /***Durable Tile Button *****/
                if (!testingLevel && (mouse.X >= durTileButtonPos.X && mouse.X <= durTileButtonPos.X + 80) && 
                    (mouse.Y >= durTileButtonPos.Y && mouse.Y <= durTileButtonPos.Y + 80))
                {
                    selectionPos = durTileButtonPos - new Vector2(5, 5);
                    durableTileWindowOpen = true;
                    stringInput = "";
                    windowMessage = "Enter durable tile's health \n(Min = 2, Max = 10) \n Press ESC to cancel";
                    noticeMessage = "Requires more than one pass before it's destroyed";
                }

                /***Invincible Tile Button *****/
                if (!testingLevel && (mouse.X >= invTilePos.X && mouse.X <= invTilePos.X + 80) && 
                    (mouse.Y >= invTilePos.Y && mouse.Y <= invTilePos.Y + 80))
                {
                    selectionPos = invTilePos - new Vector2(5, 5);
                    noticeMessage = "Can't be destroyed, but does not affect turn count!";
                    tileID = 11;
                }

                /***Target Tile Button *****/
                if (!testingLevel && (mouse.X >= tarTileButtonPos.X && mouse.X <= tarTileButtonPos.X + 80) && 
                    (mouse.Y >= tarTileButtonPos.Y && mouse.Y <= tarTileButtonPos.Y + 80))
                {
                    selectionPos = tarTileButtonPos - new Vector2(5, 5);
                    targetTileWindowOpen = true;
                    stringInput = "";
                    windowMessage = "Enter a target number \n(Min = 1, Max = 49) \n Press ESC to cancel";
                    noticeMessage = "Can only be destroyed if target number of broken tiles is met";
                }

                /***Chain Tile Button *****/
                if (!testingLevel && (mouse.X >= chaTileButtonPos.X && mouse.X <= chaTileButtonPos.X + 80) && 
                    (mouse.Y >= chaTileButtonPos.Y && mouse.Y <= chaTileButtonPos.Y + 80))
                {
                    selectionPos = chaTileButtonPos - new Vector2(5, 5);
                    noticeMessage = "Breaks/reverts entire lines of tiles";
                    tileID = 13;
                }

                /***Impassable Tile Button *****/
                if (!testingLevel && (mouse.X >= impButtonPos.X && mouse.X <= impButtonPos.X + 80) && 
                    (mouse.Y >= impButtonPos.Y && mouse.Y <= impButtonPos.Y + 80))
                {
                    selectionPos = impButtonPos - new Vector2(5, 5);
                    noticeMessage = "Ball cannot move onto this space";
                    tileID = -1;
                }

                /***Ball Button *****/
                if (!testingLevel && (mouse.X >= ballButtonPos.X && mouse.X <= ballButtonPos.X + 80) && 
                    (mouse.Y >= ballButtonPos.Y && mouse.Y <= ballButtonPos.Y + 80))
                {
                    selectionPos = ballButtonPos - new Vector2(5, 5);
                    noticeMessage = "The player. There can only be one!";
                    tileID = 99;
                }


                /***Level Area
                 * If the level is clicked, the editor should also check which tile in the level was 
                 * clicked and change it to the currently selected tile.  This should also affect the array 
                 * that the level uses to display the tiles. ***/
                if (!testingLevel && (mouse.X >= levelAreaStartX && mouse.X <= levelAreaEndX) && 
                    (mouse.Y >= levelAreaStartY && mouse.Y <= levelAreaEndY))
                {
                    //change the element in the array.  We get the element's position in the array
                    //by taking the mouse's x and y position, divide each by 80 (the tile's pixel size).  For the x value, we
                    //must first subtract 100 to eliminate the offset.  For Y, I subtract 50 for the same reason.
                    int colPos = (mouse.X - 100) / 80;
                    int rowPos = (mouse.Y - 50) / 80;

                    //ensure array doesn't go out of bounds
                    if (colPos > 9)
                        colPos = 9;
                    if (rowPos > 4)
                        rowPos = 4;

                    //Console.WriteLine("Row Pos: " + rowPos);
                    //Console.WriteLine("Col Pos: " + colPos);

                    //check what's being drawn
                    if (tileID == 99)
                    {
                        //draw ball
                        level.SetPlayerPosition(rowPos, colPos);
                        levelCopy.SetPlayerPosition(rowPos, colPos);
                    }
                    else if (tileID == 12)
                    {
                        level.SetTarget(rowPos, colPos, target);
                        level.ChangeTile(rowPos, colPos, tileID);

                        levelCopy.SetTarget(rowPos, colPos, target);
                        levelCopy.ChangeTile(rowPos, colPos, tileID);
                    }
                    else
                    {
                        //draw tile
                        level.ChangeTile(rowPos, colPos, tileID);
                        levelCopy.ChangeTile(rowPos, colPos, tileID);
                    }
                }

            }
            else
            {
                //remove button highlighting
                for (int i = 0; i < MAX_BUTTONS; i++)
                {
                    buttonState[i] = button;
                }

                //allow test button to be clicked if test button is released.
                testButtonClicked = false;
            }
        }
    }
#endif
}
