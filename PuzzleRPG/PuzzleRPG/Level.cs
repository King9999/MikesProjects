/* This class is the foundation of Puzzle RPG. Here, a level containing the puzzle is generated. The challenge
 * will be making the blocks large enough for accurate touch screen input, especially if I plan to have
 * an AI opponent with its own well.
 * 
 * A "well" is the space containing the blocks.
 
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

using PuzzleRPG.Inputs;
using PuzzleRPG.Screens;
using PuzzleRPG.UI;
#endregion

namespace PuzzleRPG
{
    class Level
    {
        /***********graphics************
         * All blocks are 50x50 pixels. Player should be able to accurately select blocks.  
         */
        
        //variables
        const ushort MAX_ROWS = 10;    //if the blocks push past this number, then player takes damage.
        const ushort MAX_COLS = 5;     
        float maxRate = 1.0f;           //the maximum rate at which blocks rise. Changes when player levels up.
        float riseRate = 0.0f;          //the rate at which the blocks rise automatically. Decreases by 0.02 per level.
        float stopTimer = 0.0f;         //prevents blocks from rising.  The more chains/combos, the higher this number is.
        ushort playerLevel;             //max level is 50.
        Random randBlock = new Random();    //used to generate random blocks
        const ushort MAX_BLOCKTYPES = 6;
        bool gameOver = false;          //if true, then blocks hit the top of the well
        bool drawReady = true;          //if true, will draw blocks offscreen then change to false.
        

        //blocks
        Block block;
        Texture2D blockTexture;
        Texture2D attackBlock;
        Texture2D shieldBlock;
        Texture2D redBlock;
        Texture2D greenBlock;
        Texture2D blueBlock;
        Texture2D multiBlock;

        //background
        Texture2D wellBackground;   //background for the block well

        /* I had to make this enum public so I could use it in the Block class. You cannot make an enum in one class and then use 
         an accessor method to refer to it. */
        public enum BlockType
        {
            None,   //empty space
            Attack,
            Shield,
            Red,
            Green,
            Blue,
            Multi //multicoloured block; can be matched with any mana block
        }

        BlockType blockTouched;     //used for debugging.

        //Texture2D attackBlock =

        //2D array used to create the playing field.
        //private Block[,] well = new Block[MAX_ROWS, MAX_COLS];
        private List<Block> blockList = new List<Block>();                                            //tracks all of the blocks in play.
       // private Dictionary<Vector2, BlockType> blockTable = new Dictionary<Vector2, BlockType>();    //tracks all of the blocks in play.
        //private BlockType[,] well = new BlockType[MAX_ROWS, MAX_COLS];
        //private Vector2[,] blockLocation = new Vector2[MAX_ROWS, MAX_COLS];
        //private ushort[,] blockID = new ushort[MAX_ROWS, MAX_COLS];
        private ushort initRowCount;    //controls the starting number of rows of blocks.
        private ushort buffer;                //iterator used for drawing blocks off-screen.
        ushort currentRowCount;         //keeps tracks of how many rows are currently on the screen.
       

        //The following values must be used for the final version of the game so that the levels display properly.
        short offsetX = 100;  //used to position the level
        short offsetY = 30;    //used to position the blocks so that they fill in the empty space at the bottom of the screen.
        short blockSize = 50;

        protected GameInput input = new GameInput();
       
        const string playArea = "Well";

        //touch area of the player's well
        Rectangle wellArea;

        #region Initialize
        /* Whenever a new level is created, random blocks are generated and the stack is set to 3 rows (more depending on difficulty).
         * The blocks will rise at a speed depending on player's level. The higher the level, the faster the blocks rise. */
        public Level(Texture2D atk, Texture2D shield, Texture2D red, Texture2D green, Texture2D blue, Texture2D multi,
            Texture2D wellBG, ushort rowCount)
        {
            attackBlock = atk;
            shieldBlock = shield;
            redBlock = red;
            greenBlock = green;
            blueBlock = blue;
            multiBlock = multi;
            wellBackground = wellBG;
            initRowCount = rowCount;
            currentRowCount = rowCount;

            //set up rise rate
            riseRate = maxRate;

             //set up controls
            wellArea = new Rectangle(offsetX, 0, blockSize * MAX_COLS, Screen.ScreenHeight);
            input.AddTouchGestureInput(playArea, GestureType.HorizontalDrag, wellArea);

            //fill up well with blocks
            for (int i = 0; i < MAX_ROWS; i++)
            {
                //we need to check if a block occupies the current space
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (i >= (MAX_ROWS - initRowCount) - 1) //I'm subtracting 1 because of the Y offset. Without it, the number of rows displayed is inaccurate.
                    {
                        //Create a random block and store it in the list
                        Vector2 blockLocation = new Vector2((j * blockSize) + offsetX, (i * blockSize) + offsetY); //note the i and j
                        //block = new Block(GenerateBlock(), blockTexture, blockLocation);

                        //check the previous block to see if it's the same as the current block. If true, then generate another block.
                        do
                        {
                            block = new Block(GenerateBlock(), blockTexture, blockLocation);
                        }
                        while (blockList.Count > 0 && block.BlockType() == blockList[blockList.Count - 1].BlockType());
                      
                        blockList.Add(block);                    
                    } 
                }
            }

            //set up the buffer
            buffer = MAX_ROWS;
           
        }
        #endregion
        #region Getters/Setters


        public ushort MaxRows() { return MAX_ROWS; }
        public ushort MaxCols() { return MAX_COLS; }
        public short OffsetX() { return offsetX; }
        public short OffsetY() { return offsetY; }
        public short BlockSize() { return blockSize; }

        #endregion


        #region UpdateBlocks()
        /* The blocks rise constantly.  New blocks should be created offscreen before they're displayed. This should happen before
         * the player raises the blocks or otherwise. */
        private void UpdateBlocks()
        {
            stopTimer += -0.1f;
            if (stopTimer < 0)
            {
                stopTimer = 0;
                riseRate += 0.1f;
            }

            int matchCount = 0;     //if this number is 3 or higher, then there's a match and blocks need to be destroyed.
            //if the rate is exceeded, raise the blocks
            if (riseRate > maxRate)
            {
                int i = 0;
                List<Block> matchBlocks = new List<Block>();    //used to check if any blocks are matching.
                foreach (Block b in blockList)
                {
                    b.ChangePosition(0, -1);

                    //if the blocks reach the top of the well and time hasn't stopped, then the game is over.
                    if (b.Position().Y < 0)
                    {
                        gameOver = true;
                    }

                    //check if the offscreen blocks are fully visible so we can draw the next row of blocks.
                    //The offscreen blocks are always the last 5 blocks in the list.
                    if (blockList.Count - i <= MAX_COLS && !drawReady)
                    {
                        if (b.Position().Y < Screen.ScreenHeight)
                        {
                            drawReady = true;
                            currentRowCount++;
                        }
                    }
                    i++;

                    /*check for a match of at least three blocks.  There's a match when there are 3 or more consecutive 
                    //blocks of the same type in the list.  For vertical matches, we must check the current block against
                    //every 5th block to see if there's a match of at least three.
                     If there's any match, the blocks must stop moving for 1 second (30 frames).  If there's a match of
                     4 or more, additional time is added to the stop timer. */

                    //horizontal matches
                    if (b != blockList[0] && b.BlockType() == blockList[i - 1].BlockType())
                    {
                        matchBlocks.Add(b);
                        matchBlocks.Add(blockList[i - 1]);
                        matchCount += 2;
                    }

                    //vertical matches
                    for (int k = 0; k < currentRowCount; k++)
                    {

                    }
                  
                }
                
                riseRate = 0.0f;
            }

            
            

            /*blocks should be drawn off-screen so that they're always available to the player
            //when they need them.  Off-screen blocks should only be drawn when the previous row
            //of offscreen blocks are fully visible on screen.  This is to prevent performance issues. */
            if (drawReady)
            {
                //int i = MAX_ROWS;   //always want to draw a row of blocks off screen, but one row at a time.
                for (int j = 0; j < MAX_COLS; j++)
                {
                    Vector2 blockLocation = new Vector2((j * blockSize) + offsetX, Screen.ScreenHeight + blockSize);
                    Block b = new Block(GenerateBlock(), blockTexture, blockLocation);
                    blockList.Add(b);
                }
               // buffer++;
                drawReady = false;
            }
        }
        #endregion

        private BlockType GetBlockLocation()
        {
            BlockType block = BlockType.None;
            foreach (Block b in blockList)
            {
                if ((input.CurrentGesturePosition(playArea).X >= b.Position().X && input.CurrentGesturePosition(playArea).X <= b.Position().X + blockSize) &&
                    (input.CurrentGesturePosition(playArea).Y >= b.Position().Y && input.CurrentGesturePosition(playArea).Y <= b.Position().Y + blockSize))
                {
                    block = b.BlockType();
                }
            }
            
            return block;
        }

        #region Movement Controls
        /************Movement Controls ****************
        The game must check to see what the player touched. If it's a block, then check for any swaps.
         * Otherwise, the player touched an empty space. By holding down on the empty space, the blocks will rise
         * quickly, and will keep rising until the blocks reach the point touched, or the player releases his finger. */
       
        private void CheckForInput()
        {
            if (input.IsPressed(playArea))
            {
                blockTouched = GetBlockLocation();
            }
          
        }
        #endregion



        #region Draw
        /******** Draw function *********/
        public void Draw(SpriteBatch batch)
        {
            //draw well
            batch.Draw(wellBackground, new Rectangle(offsetX, 0, blockSize * MAX_COLS, Screen.ScreenHeight), Color.White);

            //draw blocks on screen
            foreach (Block b in blockList)
            {
                //b.Draw(batch);
                batch.Draw(b.BlockImage(), b.Position(), Color.White);
            }
            
            
        }
        #endregion

        //shows debug info
        public void DisplayDebug(SpriteFont font, SpriteBatch batch)
        {
            //rise rate
            batch.DrawString(font, "Rise Rate: " + riseRate + "/" + maxRate, new Vector2(400, 50), Color.White);

            //stop timer
            batch.DrawString(font, "Stop Timer: " + stopTimer, new Vector2(400, 70), Color.White);

            //Y offset; causes blocks to rise
           // batch.DrawString(font, "Y Offset: " + offsetY, new Vector2(400, 70), Color.White);

            //touch location for play area
            batch.DrawString(font, "Touch Loc: " + input.CurrentTouchPosition(playArea), new Vector2(400, 90), Color.White);

            //position of block that was touched
            batch.DrawString(font, "Touched Block: " + blockTouched, new Vector2(400, 110), Color.White);

            //row count
            batch.DrawString(font, "Row Count: " + currentRowCount, new Vector2(400, 130), Color.White);

            //Game over message
            if (gameOver)
            {
                batch.DrawString(font, "Game Over!", new Vector2(400, 150), Color.Red);
            }

        }

        //generates a random integer and produce a block based
        //on the result
        //TODO: Update so that there will never be three matching blocks side by side.
        private BlockType GenerateBlock()
        {
            BlockType block;
            int num = randBlock.Next(100);

            //blocks are generated based on a range of values. Multi blocks are rare, so 
            //they should occur the least.
            if (num >= 90)
            {
                block = BlockType.Attack;
                blockTexture = attackBlock;
            }
            else if (num >= 70)
            {
                block = BlockType.Shield;
                blockTexture = shieldBlock;
            }
            else if (num >= 50)
            {
                block = BlockType.Red;
                blockTexture = redBlock;
            }
            else if (num >= 30)
            {
                block = BlockType.Green;
                blockTexture = greenBlock;
            }
            else if (num >= 5)
            {
                block = BlockType.Blue;
                blockTexture = blueBlock;
            }
            else
            {
                block = BlockType.Multi;
                blockTexture = multiBlock;
            }
          
            return block;
        }

        /************Update movement and game status***********/
        public void Play()
        {
            //Blocks continually rise and must be done before anything else.
            UpdateBlocks();
            CheckForInput();      
 
        }
    }
}
