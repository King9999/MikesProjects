/* This is a level editor for my game, Tile Crusher.  Its purpose is to allow me to easily create levels
 * and save them as a custom file that can only be read by the game.  I should be able to make levels of 
 varying sizes, and be able to test the level in the editor itself. 
 
 * What should get saved is the numbers contained in the array.  The game will read those numbers
 * and display the appropriate tiles.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TileCrusherEditor
{
    class Level
    {
        /***********graphics************
         * All graphics are 80x80 pixels for the Windows Phone version. 
         */
        private Texture2D ball;  //player controls this
        private Texture2D normalTile;
        private Texture2D emptyTile;


        //variables
        const int GRID_ROWS = 5;
        const int GRID_COLS = 10;
        
        public const int NUM_LEVELS = 50; //maximum number of levels in the Editor.
        private int turnCount = 0;  //limited number of turns
        private int[,] grid = new int[GRID_ROWS, GRID_COLS];
        private bool[,] landed = new bool[GRID_ROWS, GRID_COLS];    //used to prevent rapid switching between 1 and 0 on grid on each frame.
        private int[,] targetGrid = new int[GRID_ROWS, GRID_COLS];  //tracks all target tiles
        private int playerRowPosition;  //tracks the player's spot on in the array
        private int playerColPosition;
        private int inputDelay = 0;         //amount of frames used to prevent rapid movement.
        private int tileCount = 0;          //tracks number of tiles remaining. Player wins if this equals 0.
        private int tilesDestroyed = 0;     //tracks number of tiles destroyed.
        private int level;
    

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
            playerRowPosition = 0;
            playerColPosition = 0;

            //default turn count is 10
            turnCount = 10;

            //default level is 1
            level = 1; 

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
           
        }

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

        public void SetPlayerPosition(int row, int col)
        {
            playerRowPosition = row;
            playerColPosition = col;
        }

        //used to change the element in the grid.
        public void ChangeTile(int rowPosition, int colPosition, int tileID)
        {
            grid[rowPosition, colPosition] = tileID;
        }

        public int GetMaxLevels()   { return NUM_LEVELS; }

        public bool LevelFailed()   { return (turnCount <= 0 && tileCount > 0); }

        public int TurnCount()      { return turnCount; }

        public int PlayerRowPos()   { return playerRowPosition; }

        public int PlayerColPos()   { return playerColPosition; }

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
                        if (tilesDestroyed < 0)
                            tilesDestroyed = 0;
                        break;

                    case 1: //normal tile
                        grid[playerRowPosition, playerColPosition] = 0;

                        //increase destruction count
                        tilesDestroyed++;
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
                            }
                            else if (grid[i, playerColPosition] == 0)
                            {
                                grid[i, playerColPosition] = 1;
                                tilesDestroyed--;
                                if (tilesDestroyed < 0)
                                    tilesDestroyed = 0;
                            }
                        }

                        for (int j = 0; j < GRID_COLS; j++)
                        {
                            if (grid[playerRowPosition, j] == 1)
                            {
                                grid[playerRowPosition, j] = 0;
                                tilesDestroyed++;
                            }
                            else if (grid[playerRowPosition, j] == 0)
                            {
                                grid[playerRowPosition, j] = 1;
                                tilesDestroyed--;
                                if (tilesDestroyed < 0)
                                    tilesDestroyed = 0;
                            }
                        }
                        break;
                }

                //set bool to true; will not change until player lands on spot again
                landed[playerRowPosition, playerColPosition] = true;
            }
        }

  

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
                playerColPosition--;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;

            }
        }

        public void MoveRight()
        {
            if (playerColPosition != GRID_COLS - 1 && grid[playerRowPosition, playerColPosition + 1] >= 0)    
            {
                //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player right
                playerColPosition++;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;
            }
        }

        public void MoveUp()
        {
            if (playerRowPosition != 0 && grid[playerRowPosition - 1, playerColPosition] >= 0)    
            {
                //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player up
                playerRowPosition--;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;
            }
        }

        public void MoveDown()
        {
            if (playerRowPosition != GRID_ROWS - 1 && grid[playerRowPosition + 1, playerColPosition] >= 0 )    
            {
                //no longer on previous spot, set it to false
                landed[playerRowPosition, playerColPosition] = false;

                //move player down
                playerRowPosition++;

                //reduce the number of turns if player is not on an invincible tile
                if (grid[playerRowPosition, playerColPosition] != 11)
                    turnCount--;
            }
        }


        private void CheckForInput()
        {
            /* get input from either keyboard or 360 pad and move update game accordingly */
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (inputDelay <= 0)
            {
                if (keyState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed)
                {
                    MoveLeft();
                    inputDelay = 15;  //set input delay

                }

                if (keyState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed)
                {
                    MoveRight();
                    inputDelay = 15;
                }

                if (keyState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed)
                {
                    MoveUp();
                    inputDelay = 15;
                }

                if (keyState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed)
                {
                    MoveDown();
                    inputDelay = 15;
                }

             
            }

            //if the button is released at any point, the input delay should be 0.
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.D))
            {
                inputDelay = 0;
            }

            //this should only come into effect if a gamepad is being used!
            if (gamePadState.IsConnected)
            {
                if (gamePadState.DPad.Left == ButtonState.Released && gamePadState.DPad.Right == ButtonState.Released &&
                    gamePadState.DPad.Up == ButtonState.Released && gamePadState.DPad.Down == ButtonState.Released)
                {
                    inputDelay = 0;
                }
            }
        }

        /******** Draw function *********/
        public void Draw(SpriteBatch batch, SpriteFont font, Texture2D player, Texture2D normalTile, Texture2D emptyTile, Texture2D durableTile, Texture2D invTile,
            Texture2D chainTile, Texture2D targetTile)
        {
            
            int offsetX = 100;  //used to position the level
            int offsetY = 50;

            //draw tiles and then player in that order
            for (int i = 0; i < GRID_ROWS; i++)
            {
                for (int j = 0; j < GRID_COLS; j++)
                {
                    //draw tile at position
                    if (grid[i, j] == 0)    //empty tile
                    {
                        batch.Draw(emptyTile, new Vector2(80 * j + offsetX, 80 * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] == 1)   //normal tile
                    {
                        batch.Draw(normalTile, new Vector2(80 * j + offsetX, 80 * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] >= 2 && grid[i, j] <= 10)   //durable tile
                    {
                        batch.Draw(durableTile, new Vector2(80 * j + offsetX, 80 * i + offsetY), Color.White);
                        //draw tile health
                        batch.DrawString(font, grid[i, j].ToString(), new Vector2(80 * j + offsetX+27, 80 * i + offsetY+15), Color.ForestGreen);
                    }
                    else if (grid[i, j] == 11)   //invincible tile
                    {
                        batch.Draw(invTile, new Vector2(80 * j + offsetX, 80 * i + offsetY), Color.White);
                    }
                    else if (grid[i, j] == 12)   //target tile
                    {
                        batch.Draw(targetTile, new Vector2(80 * j + offsetX, 80 * i + offsetY), Color.White);
                        //draw target number
                        batch.DrawString(font, targetGrid[i, j].ToString(), new Vector2(80 * j + offsetX + 27, 80 * i + offsetY + 15), Color.White);
                    }
                    else if (grid[i, j] == 13)   //chain tile
                    {
                        batch.Draw(chainTile, new Vector2(80 * j + offsetX, 80 * i + offsetY), Color.White);
                    }
                }
            }

            //draw player
            batch.Draw(player, new Vector2(80 * playerColPosition + offsetX, 80 * playerRowPosition + offsetY), Color.White);
        }

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
            UpdateTiles();
            inputDelay--;
            CheckForInput();
 
        }
    }
}
