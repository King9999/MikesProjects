/* Code that defines the food meter.  It consists of three rectangles: the empty meter, the filled meter, and the
 capacity marker.  The filled meter constantly decreases; it decreases faster when the player is moving. 
 As the player beats levels, the capacity marker shrinks in width. Once it reaches the minimum width and the player
 keeps the meter within capacity, the width resets and the marker's x position decreases. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FeedMe.UI
{
   
    class FoodMeter
    {
        Texture2D emptyMeter;
        Texture2D foodMeter;      //food remaining.
        Texture2D capacityMarker; 
        Rectangle emptyMeterRect;
        Rectangle foodMeterRect;
        Rectangle capacityMarkerRect;

        //Timer barTimer = new Timer();   //controls the rate of meter reduction.

        const int MAX_WIDTH = 500;
        const int MAX_CAPACITY_WIDTH = 30;
        const int MIN_CAPACITY_WIDTH = 5;
        const int DEFAULT_WIDTH = 100;

        int foodMeterWidth;        //player always starts with a little meter so they don't die.
        int capacityMarkerWidth;
        int emptyMeterWidth;
        float degradeRate;          //the time that must pass before food meter decreases
        float currentRate;          //degrade rate is set to this amount when reset
        const float MAX_RATE = 3;

        public FoodMeter(Texture2D emptyMeter, Texture2D foodMeter, Texture2D capacityMarker, int xPos, int yPos)
        {
            this.emptyMeter = emptyMeter;
            this.foodMeter = foodMeter;
            this.capacityMarker = capacityMarker;

            degradeRate = 0;
            foodMeterWidth = DEFAULT_WIDTH;
            capacityMarkerWidth = MAX_CAPACITY_WIDTH;
            emptyMeterWidth = MAX_WIDTH;
            currentRate = 0;
            emptyMeterRect = new Rectangle(xPos, yPos, emptyMeterWidth, emptyMeter.Height);

            //I use a second rect because the filled life bar is going to be slightly smaller than the empty bar rect
            foodMeterRect = new Rectangle(xPos + 10, yPos + 5, foodMeterWidth, foodMeter.Height);
            capacityMarkerRect = new Rectangle((emptyMeterRect.X + emptyMeterRect.Width) - MAX_CAPACITY_WIDTH, emptyMeterRect.Y - 10, capacityMarkerWidth, capacityMarker.Height);

        }

        //Lifebars decrease and increase by a percentage of its width.
        public void ReduceMeter(int amount)
        {
            foodMeterWidth -= amount;
            if (foodMeterWidth < 0)
            {
                foodMeterWidth = 0;
            }
        }

        public bool IsEmpty() { return foodMeterWidth == 0; }

        public void IncreaseMeter(int amount)
        {
            foodMeterWidth += amount;
            if (foodMeterWidth + foodMeterRect.X > emptyMeterWidth + emptyMeterRect.X)
            {
                foodMeterWidth = emptyMeterWidth;
            }
        }

        public int Width() { return foodMeterWidth; }
        

        public void ResetMeter()
        {
            foodMeterWidth = DEFAULT_WIDTH;
        }

        public int MaxWidth() { return MAX_WIDTH; }

        public void ResetCapacity()
        {
            capacityMarkerWidth = MAX_CAPACITY_WIDTH;
        }

        public void DecreaseCapacity(int amount)
        {
            capacityMarkerWidth -= amount;
            if (capacityMarkerWidth < MIN_CAPACITY_WIDTH)
                capacityMarkerWidth = MIN_CAPACITY_WIDTH;

        }

        public void SetCurrentRate(float rate) 
        { 
            currentRate = rate;
            if (currentRate > MAX_RATE)
                currentRate = 0;
        }

        public bool MarkerAtMinCapacity() { return capacityMarkerWidth == MIN_CAPACITY_WIDTH; }

        //win condition
        public bool LevelWon()
        {
            //return foodMeterRect.Width + foodMeterRect.X >= capacityMarkerRect.X;
            return ((foodMeterRect.Width + foodMeterRect.X) >= capacityMarkerRect.X
                && (foodMeterRect.Width + foodMeterRect.X) <= capacityMarkerRect.X + capacityMarkerRect.Width);
        }

        //lose condition 1
        public bool CapacityExceeded()
        {
            return (foodMeterRect.Width + foodMeterRect.X > capacityMarkerRect.X + capacityMarkerRect.Width);
        }

        public void ChangeMarkerPosition(int amount)
        {
            //decreases by a set amount
            int newXPos = capacityMarkerRect.X + amount;
            capacityMarkerRect = new Rectangle(newXPos, foodMeterRect.Y - 10, capacityMarkerWidth, capacityMarker.Height);
        }

        //updates the bar width
        public void Update()
        {
            //decrease food meter
            float amount = MAX_WIDTH * 0.01f;   //reduced by 1% when not moving
            degradeRate += 0.1f;
            if (degradeRate > MAX_RATE)
            {
                ReduceMeter((int)amount);
                degradeRate = currentRate;
            }

            foodMeterRect = new Rectangle(foodMeterRect.X, foodMeterRect.Y, foodMeterWidth, foodMeterRect.Height);
            capacityMarkerRect = new Rectangle(capacityMarkerRect.X, capacityMarkerRect.Y, capacityMarkerWidth, capacityMarker.Height);
           
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(emptyMeter, emptyMeterRect, Color.White);
            batch.Draw(foodMeter, foodMeterRect, Color.White);
            batch.Draw(capacityMarker, capacityMarkerRect, Color.White);
 
        }

    }
}
