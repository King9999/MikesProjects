/*Block.cs by Mike Murray 
 Creates a Block object.  Blocks can be moved around and destroyed. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace PuzzleRPG.UI
{
    class Block
    {
        Texture2D blockTexture;
        Vector2 blockPos;   //screen position.  Needed for precise control and gameplay
        Random randBlock = new Random();    //used to generate random blocks

        Level.BlockType block;

        public Block(Level.BlockType block, Texture2D texture, Vector2 pos)
        {
            blockTexture = texture;
            blockPos = pos;
            this.block = block;
        }

        public Vector2 Position()
        {
            return blockPos;
        }

        /* Change position of block by a specified amount */
        public void ChangePosition(float deltaX, float deltaY)
        {
            blockPos.X += deltaX;
            blockPos.Y += deltaY;
        }

        public Texture2D BlockImage()
        {
            return blockTexture;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(blockTexture, blockPos, Color.White);
        }

        public Level.BlockType BlockType()
        {
            return block;
        }

    }
}
