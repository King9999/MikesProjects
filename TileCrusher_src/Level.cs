/* This is the Level class for the game. 
 
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

using TileCrusher.Inputs;
using TileCrusher.Sounds;
#endregion

namespace TileCrusher
{
    class Level
    {
        /***********graphics************
         * All graphics are 80x80 pixels for the Windows Phone version.  
         * The editor uses smaller tiles to make space for other elements.
         */
        private Texture2D ball;  //player controls this
        private Texture2D normalTile;
        private Texture2D emptyTile;


        //variables
        const int GRID_ROWS = 5;
        const int GRID_COLS = 10;
        
        private const int NUM_LEVELS = 50; //maximum number of levels in the Editor.
        private int turnCount = 0;  //limited number of turns
        private int[,] grid = new int[GRID_ROWS, GRID_COLS];
        private bool[,] landed = new bool[GRID_ROWS, GRID_COLS];    //used to prevent rapid switching between 1 and 0 on grid on each frame.
        private int[,] targetGrid = new int[GRID_ROWS, GRID_COLS];  //tracks all target tiles
        private int playerRowPosition;  //tracks the player's spot on in the array
        private int playerColPosition;
        private int tileCount;          //tracks number of tiles remaining. Player wins if this equals 0.
        private int tilesDestroyed;     //tracks number of tiles destroyed.
        private int level;
        private int moveCount;      //number of moves the player made in the current level.
        float currentCol;               //grabs player's current position
        float currentRow;
        //int offsetX = 200;  //used to position the level
        //int offsetY = 50;
        //int tileSize = 40;

        //The following values must be used for the final version of the game so that the levels display properly.
        int offsetX = 100;  //used to position the level
        int offsetY = 100;
        int tileSize = 60;

        protected GameInput input = new GameInput();
       
        const string ActionLevel = "Level";

        //touch area of the level
        Rectangle levelArea;

        #region Initialize
        /* Whenever a new level is created, a level with all normal tiles should be displayed by default, with no starting player position. */
        public Level(Texture2D playerTexture, Texture2D normalTileTexture, Texture2D emptyTileTexture, SpriteFont font)
        {
            ball = playerTexture;
            normalTile = normalTileTexture;
            emptyTile = emptyTileTexture;

            
            /* The level parameter determines what kind of grid the player gets. A case statement is used to 
             * create the level and pass it on to the global 2D array called "grid."
             *
             Level contents
             ****************
             * 1 = normal tile
             * 0 = empty tile node
             * -1 = blocked area. Used to vary the shapes of the puzzles
             * 2-10 = durable tile. Requires more than 1 pass to destroy.
             * 11 = invincible tile. cannot be destroyed. Does not reduce the turn count. Does not count towards tile count
             * 12 = target tile. can only be destroyed if the player destroyed a given number of tiles.
             * 13 = chain tile. Changes normal tiles to empty tiles in all rows and/or columns adjacent to the tile.
             * Has the opposite effect if rolled on again. Excluded from tile count
             * 
             * The player can start at different positions which can make puzzles harder or easier.
             */
            

            //set the player's position
           // playerRowPosition = 0;
            //playerColPosition = 0;
            //currentRow = 0;
            //currentCol = 0;

            //default turn count is 10
            turnCount = 10;

            //default level is 1
            level = 1; 

            //set move count
            moveCount = 0;

             int[,] newLevel = 
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            
            
            //copy the level to the global array
            for (int i = 0; i < GRID_ROWS; i++)
            {
                for (int j = 0; j < GRID_COLS; j++)
                {
                    grid[i, j] = newLevel[i, j];
                    

                    //initialize target grid
                    targetGrid[i, j] = 0;

                    //keep track of number of nodes
                    //if (grid[i, j] == 1)
                    //    tileCount++;
                }
            }

            //set up controls
            levelArea = new Rectangle(offsetX, offsetY, tileSize * GRID_COLS, tileSize * GRID_ROWS);
            input.AddTouchGestureInput(ActionLevel, GestureType.Tap, levelArea);
           
        }
        #endregion
        #region Getters/Setters
        public int GetValueAtPlayerPosition()
        {
            //returns the value of the player's current position
            return grid[playerRowPosition, playerColPosition];
        }

        public int GetValueAtIndex(int row, int col)
        {
            //returns the value at the given index. Used to write data to file
            return grid[row, col];
        }

        public int TilesDestroyed() { return tilesDestroyed; }

        public int GetTargetAtIndex(int row, int col)
        {
            return targetGrid[row, col];
        }

        public void SetTarget(int row, int col, int target)
        {
            if (target < 1 || target > 49)
                target = 1;

            targetGrid[row, col] = target;
        }

        public int MaxRows()    { return GRID_ROWS; }
        public int MaxCols() { return GRID_COLS; }
        public int OffsetX() { return offsetX; }
        public int OffsetY() { return offsetY; }
        public int TileSize() { return tileSize; }
        public int MoveCount() { return moveCount; }

        public void SetPlayerPosition(int row, int col)
        {
            playerRowPosition = row;
            playerColPosition = col;

            //set player's current position
            currentCol = playerColPosition * tileSize + offsetX;
            currentRow = playerRowPosition * tileSize + offsetY;
        }

        //used to change the element in the grid.
        public void ChangeTile(int rowPosition, int colPosition, int tileID)
        {
            grid[rowPosition, colPosition] = tileID;
        }

        public int GetMaxLevels()   { return NUM_LEVELS; }

        public bool LevelFailed()   { return (tileCount > 0 && turnCount <= 0); }

        public int TurnCount()      { return turnCount; }

        public int PlayerRowPos()   { return playerRowPosition; }

        public int PlayerColPos()   { return playerColPosition; }

        public int PlayerXPos() { return tileSize * playerColPosition + offsetX; }

        public int PlayerYPos() { return tileSize * playerRowPosition + offsetY; }

       
        public int TileCount() { return tileCount; }

        public void ResetTilesDestroyed() { tilesDestroyed = 0; }

        public void SetTurnCount(int turns)
        {
            turnCount = turns;
            if (turnCount < 1)
            {
                turnCount = 10;
            }
        }

        public int GetLevel()   { return level; }

        public void SetLevel(int num)
        {
            level = num;
            if (level > NUM_LEVELS || level < 1)
            {
                level = 1;
            }
        }

        public bool LevelComplete()     { return (tileCount == 0); }
        #endregion

        //resets any movement on the grid to an untouched state.
        public void ResetBoolGrid()
        {
            for (int i = 0; i < GRID_ROWS; i++)
            {
                for (int j = 0; j < GRID_COLS; j++)
                {
                    landed[i, j] = false;
                }
            }
        }

        #region UpdateTiles()
        private void UpdateTiles()
        {
            if (!landed[playerRowPosition, playerColPosition])
            {
                switch (grid[playerRowPosition, playerColPosition])
                {
                    case 0: //empty tile
                        grid[playerRowPosition, playerColPosition] = 1;

                        //decrease destruction count
                        tilesDestroyed--;
                        tileCount++;
                        if (tilesDestroyed < 0)
                            tilesDestroyed = 0;
                        break;

                    case 1: //normal tile
                        grid[playerRowPosition, playerColPosition] = 0;

                        //increase destruction count and reduce tile count
                        tilesDestroyed++;
                        tileCount--;
                        break;

                    case 2: case 3: case 4: case 5: case 6: case 7: case 8: case 9:
                    case 10:    //durable tile
                        //reduce tile health by 1
                        grid[playerRowPosition, playerColPosition]--;
                        break;

                    case 12:    //target tile
                        //this tile only gets destroyed if player destroyed tiles equal to target number
                        if (tilesDestroyed >= targetGrid[playerRowPosition, playerColPosition])
                        {
                            grid[playerRowPosition, playerColPosition] = 0;
                            tilesDestroyed++;
                            tileCount--;
                        }
                        break;

                    case 13:    //chain tile
                        //change all normal tiles in a row and column to 0 if 1, otherwise change to 1.
                        //any other tiles encountered should stop the process.
                        for (int i = 0; i < GRID_ROWS; i++)
                        {
                            if (grid[i, playerColPosition] == 1)
                            {
                                grid[i, playerColPosition] = 0;
                                tilesDestroyed++;
                                tileCount--;
                            }
                            else if (grid[i, playerColPosition] == 0)
                            {
                                grid[i, playerColPosition] = 1;
                                tilesDestroyed--;
                                if (tilesDestroyed < 0)
                                    tilesDestroyed = 0;
                                tileCount++;
                            }
                        }

                        for (int j = 0; j < GRID_COLS; j++)
                        {
                            if (grid[playerRowPosition, j] == 1)
                            {
                                grid[playerRowPosition, j] = 0;
                                tilesDestroyed++;
                                tileCount--;
                            }
                            else if (grid[playerRowPosition, j] == 0)
                            {
                                grid[playerRowPosition, j] = 1;
                                tilesDestroyed--;
                                if (tilesDestroyed < 0)
                                    tilesDestroyed = 0;
                                tileCount++;
                            }
                        }
                        break;
                }

                //set bool to true; will not change until player lands on spot again
                landed[playerRowPosition, playerColPosition] = true;
            }
        }
        #endregion

        #region Movement Controls
        /************Movement Controls ****************
         * The following functions handle player movement in the array. They will check for impassable spots
         * and make sure the player doesn't go out of range.*/
        public void MoveLeft()
        {
            //check ahead to make sure player can move to destination. The order of the test is important!
            if(playerColPosition != 0 && grid[playerRowPosition, playerColPosition - 1] >= 0)    
            {
                 //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player left
                currentCol = playerColPosition * tileSize + offsetX;
                playerColPosition--;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;

                //+1 to move
                moveCount++;
            }
        }

        public void MoveRight()
        {
            if (playerColPosition != GRID_COLS - 1 && grid[playerRowPosition, playerColPosition + 1] >= 0)    
            {
                //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player right
                currentCol = playerColPosition * tileSize + offsetX;
                playerColPosition++;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;

                //+1 to move
                moveCount++;
            }
        }

        public void MoveUp()
        {
            if (playerRowPosition != 0 && grid[playerRowPosition - 1, playerColPosition] >= 0)    
            {
                //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player up
                currentRow = playerRowPosition * tileSize + offsetY;
                playerRowPosition--;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;

                //+1 to move
                moveCount++;
            }
        }

        public void MoveDown()
        {
            if (playerRowPosition != GRID_ROWS - 1 && grid[playerRowPosition + 1, playerColPosition] >= 0 )    
            {
                //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player down
                currentRow = playerRowPosition * tileSize + offsetY;
                playerRowPosition++;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;

                //+1 to move
                moveCount++;
            }
        }


        private void CheckForInput()
        {
            if (input.IsPressed(ActionLevel))
            {
                /* compare the touch area with one of the tiles adjacent to the ball */
                if (TappedLeft())
                {
                    MoveLeft();
                }

                else if (TappedRight())
                {
                    MoveRight();
                }

                else if (TappedUp())
                {
                    MoveUp();
                }

                else if (TappedDown())
                {
                    MoveDown();
                }

            }
          
        }

        #region Tap Bools
        /* The following code ensures that the ball will only move if the player taps the tile directly adjacent
         * to the ball.  They cannot tap a tile on a different row/column and have the ball move that way. */
        private bool TappedLeft()
        {
            return (input.CurrentGesturePosition(ActionLevel).X <= PlayerXPos() &&
                    input.CurrentGesturePosition(ActionLevel).X >= PlayerXPos() - tileSize) &&
                    (input.CurrentGesturePosition(ActionLevel).Y >= PlayerYPos() && input.CurrentGesturePosition(ActionLevel).Y <= PlayerYPos() + tileSize);
        }

        private bool TappedRight()
        {
            return (input.CurrentGesturePosition(ActionLevel).X >= PlayerXPos() + tileSize &&
                    input.CurrentGesturePosition(ActionLevel).X <= PlayerXPos() + tileSize * 2) &&
                    (input.CurrentGesturePosition(ActionLevel).Y >= PlayerYPos() && input.CurrentGesturePosition(ActionLevel).Y <= PlayerYPos() + tileSize);
        }


        private bool TappedUp()
        {
            return (input.CurrentGesturePosition(ActionLevel).Y <= PlayerYPos() &&
                    input.CurrentGesturePosition(ActionLevel).Y >= PlayerYPos() - tileSize) &&
                    (input.CurrentGesturePosition(ActionLevel).X >= PlayerXPos() && input.CurrentGesturePosition(ActionLevel).X <= PlayerXPos() + tileSize);
        }

        private bool TappedDown()
        {
            return (input.CurrentGesturePosition(ActionLevel).Y >= PlayerYPos() + tileSize &&
                    input.CurrentGesturePosition(ActionLevel).Y <= PlayerYPos() + tileSize * 2) &&
                    (input.CurrentGesturePosition(ActionLevel).X >= PlayerXPos() && input.CurrentGesturePosition(ActionLevel).X <= PlayerXPos() + tileSize);
        }
        #endregion

        #endregion

        #region Draw
        /******** Draw function *********/
        public void Draw(SpriteBatch batch, SpriteFont font, Texture2D player, Texture2D normalTile, Texture2D emptyTile, Texture2D durableTile, Texture2D invTile,
            Texture2D chainTile, Texture2D targetTile)
        {
            //draw tiles and then player in that order
            for (int i = 0; i < GRID_ROWS; i++)
            {
                for (int j = 0; j < GRID_COLS; j++)
                {
                    //draw tile at position
                    if (grid[i, j] == 0)    //empty tile
                    {
                        batch.Draw(emptyTile, new Vector2(tileSize * j + offsetX, tileSize * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] == 1)   //normal tile
                    {
                        batch.Draw(normalTile, new Vector2(tileSize * j + offsetX, tileSize * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] >= 2 && grid[i, j] <= 10)   //durable tile
                    {
                        batch.Draw(durableTile, new Vector2(tileSize * j + offsetX, tileSize * i + offsetY), Color.White);
                        //draw tile health
                        if (grid[i, j] > 9)
                            batch.DrawString(font, grid[i, j].ToString(), new Vector2(tileSize * j + offsetX + 12, tileSize * i + offsetY + 8), Color.Blue);
                        else
                            batch.DrawString(font, grid[i, j].ToString(), new Vector2(tileSize * j + offsetX + 20, tileSize * i + offsetY + 8), Color.Blue);

                        //NOTE: This line is for the edtior only! The above commented line should be used for main game!
                        //batch.DrawString(font, grid[i, j].ToString(), new Vector2(tileSize * j + offsetX + 10, tileSize * i + offsetY), Color.Blue);
                    }
                    else if (grid[i, j] == 11)   //invincible tile
                    {
                        batch.Draw(invTile, new Vector2(tileSize * j + offsetX, tileSize * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] == 12)   //target tile
                    {
                        batch.Draw(targetTile, new Vector2(tileSize * j + offsetX, tileSize * i + offsetY), Color.White);
                        //draw target number
                        if (targetGrid[i, j] > 9)
                            batch.DrawString(font, targetGrid[i, j].ToString(), new Vector2(tileSize * j + offsetX + 12, tileSize * i + offsetY + 8), Color.White);
                        else
                            batch.DrawString(font, targetGrid[i, j].ToString(), new Vector2(tileSize * j + offsetX + 20, tileSize * i + offsetY + 8), Color.White);
                        //NOTE: This line is for the edtior only! The above commented line should be used for main game!
                        //batch.DrawString(font, targetGrid[i, j].ToString(), new Vector2(tileSize * j + offsetX + 10, tileSize * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] == 13)   //chain tile
                    {
                        batch.Draw(chainTile, new Vector2(tileSize * j + offsetX, tileSize * i + offsetY), Color.White);
                    }
                }
            }

            //draw player
            float playerDestinationX = tileSize * playerColPosition + offsetX;
            float playerDestinationY = tileSize * playerRowPosition + offsetY;
           
            //move the player in increments to simulate rolling
            if (currentCol < playerDestinationX)
                currentCol += 15;
            else if (currentCol > playerDestinationX)
                currentCol -= 15;

            if (currentRow < playerDestinationY)
                currentRow += 15;
            else if (currentRow > playerDestinationY)
                currentRow -= 15;

            batch.Draw(player, new Vector2(currentCol, currentRow), Color.White);
            
        }
        #endregion

        //check how many tiles must be destroyed
        public void UpdateTileCount()
        {
            tileCount = 0;  //start fresh
            for (int i = 0; i < GRID_ROWS; i++)
            {
                for (int j = 0; j < GRID_COLS; j++)
                {
                    if ((grid[i, j] >= 1 && grid[i, j] <= 10) || grid[i, j] == 12)
                    {
                        tileCount++;
                    }
                }
            }
        }

        /************Update movement and game status***********/
        public void Play()
        {
            //the order in which the game checks for input and updates is important. This fixed a bug in which
            //if the player was on the last tile and on the last turn, the game would end in failure, which is not intended.
            CheckForInput();
            UpdateTiles();
 
        }
    }
}
