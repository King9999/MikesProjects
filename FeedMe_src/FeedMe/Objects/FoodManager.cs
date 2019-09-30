/* Generates food objects randomly */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FeedMe.Objects
{
    class FoodManager
    {
        Random randNum = new Random();
        ContentManager mContent;
        Vector2 mFoodPos;       //location of where food will be placed on screen

        public FoodManager(ContentManager content)
        {
            mContent = content;
            //mFoodPos = food.Position();
        }

        public Food GenerateFood(Vector2 pos)
        {
            Food foodGenerated;
            
            int dropChance = randNum.Next(100);

            if (dropChance >= 90)
                foodGenerated = new Watermelon(mContent, pos);
            else if (dropChance >= 75 && dropChance < 90)
                foodGenerated = new Chicken(mContent, pos);
            else if (dropChance >= 40 && dropChance < 80)
                foodGenerated = new Carrot(mContent, pos);
            else if (dropChance >= 26 && dropChance < 40)
                foodGenerated = new Popcorn(mContent, pos);
            else if (dropChance >= 15 && dropChance <= 25)
                foodGenerated = new Pepper(mContent, pos);
            else if (dropChance >= 10 && dropChance <= 15)
                foodGenerated = new Poison(mContent, pos);
            else
                foodGenerated = new Weight(mContent, pos);

            return foodGenerated;
        }
    }
}
