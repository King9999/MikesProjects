/* In this class, food can be defined.  Each item has a name, a texture, and a value that adds to the food meter. Food will
 disappear over time if not touched. Food value is hidden to the player. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FeedMe.Objects
{
    class Food
    {
        protected string mName;
        protected Texture2D mImage;
        protected Vector2 mPosition;
        protected Vector2 mValuePos;        //position of food value display
        protected short mValue;             //how much is added to the food meter
        protected const float DECAY_TIME = 20;         //how long an item lasts before it disappears
        protected float mRate;
        protected Rectangle mHitbox;        //collision box
        protected bool mDisplayValue;       //shows food value on screen when true

        public Food(ContentManager content, string fileName, string itemName, Vector2 location, short value)
        {
            mImage = content.Load<Texture2D>(fileName);
            mHitbox = new Rectangle((int)location.X, (int)location.Y, mImage.Width, mImage.Height);
            mName = itemName;
            //mImage = image;
            mValue = value;
            mRate = 0;
            mValuePos = new Vector2(location.X, location.Y);
        }

        //getters
        public short FoodValue() { return mValue; }
        public string FoodName() { return mName; }
        public Texture2D FoodImage() { return mImage; }
        public string FileName() { return mImage.Name; }
        public Vector2 Position() { return mPosition; }
        public Rectangle Hitbox() { return mHitbox; }
        public bool ValueDisplayed() { return mDisplayValue; }
        public Vector2 ValuePosition() { return mValuePos; }

        //setters
        public void SetPosition(Vector2 newLocation)
        {
            mPosition = newLocation;
            //must move hitbox with the image.
            mHitbox.X = (int)mPosition.X;
            mHitbox.Y = (int)mPosition.Y;
        }

        public void SetValuePos(float x, float y)
        {
            mValuePos = new Vector2(x, y);
        }

        public void Countdown()
        {
            mRate += 0.1f;
        }

        public bool IsDecayed() { return (mRate >= DECAY_TIME); }

        public void ShowValue(bool flag) { mDisplayValue = flag; }

        public void ChangeValuePos(float yValue) { mValuePos.Y += yValue; }

        /*does nothing by default. Any food item that's derived from this class can have special effects
        applied to the player, both positive and negative. These effects are hidden from the player
        until they eat the item. */
        virtual public void ActivateSpecial(Player player) { }
        virtual public string EffectMessage() { return ""; }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(mImage, mPosition, Color.White);
        }

    }
}
