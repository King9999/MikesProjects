/* Player class. contains movement controls.  The player moves the creature by tapping a space, and it will move
 toward the location on the X axis only.  The player can swipe upwards to jump.  **It might be better to make a jump
 button instead** */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using FeedMe.Inputs;
using FeedMe.Screens;

namespace FeedMe.Objects
{
    class Player
    {
        Texture2D mImage;
        Vector2 mPosition;
        Rectangle mHitbox;      //collision box
        short health;           //food meter
        short mMoveMod;          //movement modifier
        float mEffectTimer;       //duration of any effects.
        float currentTime;          //current duration timer
        const short MAX_HEALTH = 500;   //same as the food meter's length
        //private GameInput input = new GameInput();  //touch gesture
        //const string touchStr = "Player";
        //Rectangle playArea = new Rectangle(0, 0, Screen.ScreenWidth, Screen.ScreenHeight);

        public Player(Texture2D image, Vector2 position)
        {
            mImage = image;
            mPosition = position;
            mHitbox = new Rectangle((int)position.X, (int)position.Y, mImage.Width, mImage.Height);
            mMoveMod = 0;

            //set up gestures
           // input.AddTouchGestureInput(touchStr, GestureType.Tap, playArea);

        }

        public Vector2 Position() { return mPosition; }
        public Rectangle Hitbox() { return mHitbox; }
        public short MoveMod() { return mMoveMod; }
        public void SetMod(short amount) { mMoveMod = amount; }
        public bool EffectEnded { get { return currentTime == 0 && mEffectTimer == 0; } }

        public void SetTimer(float amount) 
        { 
            mEffectTimer = amount;
            currentTime = 0;
        }

        public void ReduceTimer()
        {
            currentTime += 0.1f;
            if (currentTime > mEffectTimer)
            {
                //effect is finished.
                currentTime = 0;
                mEffectTimer = 0;
            }
        }

        public void SetPosition(Vector2 newPos) 
        { 
            mPosition = newPos;
            mHitbox.X = (int)mPosition.X;
            mHitbox.Y = (int)mPosition.Y;
        }

        //checks for collision between player and food
        public bool Collides(Food food)
        {
            bool result = false;

            if (mHitbox.Intersects(food.Hitbox()))
            {
                result = true;
            }
            return result;
        }
    }
}
