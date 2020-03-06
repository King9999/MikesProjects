/* A lifebar that is used to display player or enemy health. Health bars consist of three images: the empty bar,
 the life remaining bar, and the red life bar.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PuzzleRPG.UI
{
    class Lifebar
    {
        Texture2D emptyBar;
        Texture2D lifeBar;      //HP remaining.
        Texture2D redLife;      //shows damage received.
        Rectangle emptyBarRect;
        Rectangle lifeBarRect;
        Rectangle redBarRect;

        Timer barTimer = new Timer();

        const int MAX_WIDTH = 100;
        int lifeBarWidth = MAX_WIDTH;
        int redBarWidth = MAX_WIDTH;

        public Lifebar(Texture2D emptyBar, Texture2D lifeBar, Texture2D redLife, int xPos, int yPos)
        {
            this.emptyBar = emptyBar;
            this.lifeBar = lifeBar;
            this.redLife = redLife;

            emptyBarRect = new Rectangle(xPos, yPos, emptyBar.Width, emptyBar.Height);

            //I use a second rect because the filled life bar is going to be slightly smaller than the empty bar rect
            lifeBarRect = new Rectangle(xPos, yPos + 1, lifeBarWidth, lifeBar.Height);
            redBarRect = lifeBarRect;

        }

        //Lifebars decrease and increase by a percentage of its width.
        public void DecreaseBar(int amount)
        {
            lifeBarWidth -= amount;
            if (lifeBarWidth < 0)
            {
                lifeBarWidth = 0;
            }
        }

        public void IncreaseBar(int amount)
        {
            lifeBarWidth += amount;
            if (lifeBarWidth > MAX_WIDTH)
            {
                lifeBarWidth = MAX_WIDTH;
            }
            redBarWidth = lifeBarWidth;
        }

        //updates the bar width
        public void Update()
        {
            lifeBarRect = new Rectangle(lifeBarRect.X, lifeBarRect.Y, lifeBarWidth, lifeBarRect.Height);

            //red life decreases gradually after 1 second.
            barTimer.Tick();
            if (barTimer.Seconds() > 1)
            {
                redBarWidth--;
                if (redBarWidth < lifeBarWidth)
                {
                    redBarWidth = lifeBarWidth;
                    barTimer.SetTimer(0, 0, 0);
                }
            }
            redBarRect = new Rectangle(redBarRect.X, redBarRect.Y, redBarWidth, redBarRect.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(redLife, redBarRect, Color.White);
            batch.Draw(lifeBar, lifeBarRect, Color.White);
            batch.Draw(emptyBar, emptyBarRect, Color.White);
            //batch.Draw(redLife, lifeBarRect, Color.White);
            //batch.Draw(lifeBar, lifeBarRect, Color.White);
        }

    }
}
