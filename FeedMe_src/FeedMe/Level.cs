/* This class is the playing area for Feed Me.  A level consists of a plain field with occaisonal platforms.  Sometimes there 
 * will be pits, but they won't kill the player.  Instead, they're more like warp points; by falling into a pit, the player
 * will fall from the top of the screen, similar to the old 8-bit games with one screen.
 * 
 * A 2D array is used to create the level.  I will use enums to identify the different 
 */

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using FeedMe.Inputs;
using FeedMe.Screens;
using FeedMe.UI;
#endregion

namespace FeedMe
{
    class Level
    {
        /***********graphics************
         * All blocks are 80x80 pixels. Player should be able to accurately select blocks.  
         */
        
        //variables
        const ushort MAX_ROWS = 10;    //if the blocks push past this number, then player takes damage.
        const ushort MAX_COLS = 5;     
        float maxRate = 1.0f;           //how much time must pass before the meter decreases.
        float riseRate = 0.0f;          //the rate at which the food meter decreases.
        float capacityTimer = 0.0f;     //the time the player has to reduce excess food meter before exploding.
        ushort levelNum;                //max level is 999. If the player happens to go past 999, it resets to level 1 but with a star next to it.
        bool gameOver = false;          //if true, then player either exploded or died from hunger
        

        //background
        Texture2D background;   
        Texture2D ground;
        Texture2D platform;


        /* I had to make this enum public so I could use it in the Block class. You cannot make an enum in one class and then use 
         an accessor method to refer to it. */
        public enum Tile
        {
            None,   //empty space
            Ground,
            Platform //the player can jump through this from below and stand on it.
        }


        //2D array used to create the playing field.
        private Tile[,] levelArray = new Tile[MAX_ROWS, MAX_COLS];

        //The following values must be used for the final version of the game so that the levels display properly.
        short offsetX = 100;  //used to position the level
        short offsetY = 30;    //used to position the blocks so that they fill in the empty space at the bottom of the screen.
        short tileSize = 50;

        protected GameInput input = new GameInput();
       
        const string playArea = "Field";

        //touch area of the player's well
        Rectangle levelArea;

        #region Initialize
        /* Whenever a new level is created, random blocks are generated and the stack is set to 3 rows (more depending on difficulty).
         * The blocks will rise at a speed depending on player's level. The higher the level, the faster the blocks rise. */
        public Level(Texture2D player, Texture2D background, Texture2D ground, Texture2D platform)
        {

            //set up rise rate
            riseRate = maxRate;

             //set up controls
            levelArea = new Rectangle(0, 0, Screen.ScreenWidth, Screen.ScreenHeight);
            input.AddTouchGestureInput(playArea, GestureType.HorizontalDrag, levelArea);

           
        }
        #endregion
        #region Getters/Setters


        public ushort MaxRows() { return MAX_ROWS; }
        public ushort MaxCols() { return MAX_COLS; }
        public short OffsetX() { return offsetX; }
        public short OffsetY() { return offsetY; }
        public short TileSize() { return tileSize; }

        #endregion


        #region UpdateGame()
        /* All stats update in here.  This must go into the upate loop in Game1.cs */
        private void UpdateGame()
        {
           
        }
        #endregion

        #region Movement Controls
        /************Movement Controls ****************
        The game must check to see what the player touched. If it's a block, then check for any swaps.
         * Otherwise, the player touched an empty space. By holding down on the empty space, the blocks will rise
         * quickly, and will keep rising until the blocks reach the point touched, or the player releases his finger. */
       
        private void CheckForInput()
        {
            if (input.IsPressed(playArea))
            {
               
            }
          
        }
        #endregion



        #region Draw
        /******** Draw function *********/
        public void Draw(SpriteBatch batch)
        {
           
        }
        #endregion

        //shows debug info
        public void DisplayDebug(SpriteFont font, SpriteBatch batch)
        {
            //rise rate
            batch.DrawString(font, "Rise Rate: " + riseRate + "/" + maxRate, new Vector2(400, 50), Color.White);

            //stop timer
            batch.DrawString(font, "Stop Timer: " + capacityTimer, new Vector2(400, 70), Color.White);

            //touch location for play area
            batch.DrawString(font, "Touch Loc: " + input.CurrentTouchPosition(playArea), new Vector2(400, 90), Color.White);

            //Game over message
            if (gameOver)
            {
                batch.DrawString(font, "Game Over!", new Vector2(400, 150), Color.Red);
            }

        }

        //generates a random integer and produce a block based
        //on the result
        //TODO: Update so that there will never be three matching blocks side by side.
        //private Tile GenerateBlock()
        //{
           
        //}

        /************Update movement and game status***********/
        public void Play()
        {
            //Blocks continually rise and must be done before anything else.
            UpdateGame();
            CheckForInput();      
 
        }
    }
}
