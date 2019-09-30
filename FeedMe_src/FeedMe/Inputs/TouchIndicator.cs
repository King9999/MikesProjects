using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace FeedMe.Inputs
{
    class TouchIndicator
    {
        int alphaValue = 255;
        public int TouchID;

        Texture2D touchCircleIndicatorTexture;
        Texture2D touchCrossHairIndicatorTexture;
        List<Vector2> touchPositions = new List<Vector2>();

        public TouchIndicator(int touchID, ContentManager content)
        {
            TouchID = touchID;

            touchCircleIndicatorTexture = content.Load<Texture2D>("Images/Circle");
            touchCrossHairIndicatorTexture = content.Load<Texture2D>("Images/Crosshair");
        }

        public void Update(GameTime gameTime, TouchCollection touchLocationState)
        {
            Vector2? currentPosition = TouchPosition(touchLocationState);
            if (currentPosition == null)
            {
                if (touchPositions.Count > 0)
                {
                    alphaValue -= 20;
                    if (alphaValue <= 0)
                    {
                        touchPositions.Clear();
                        alphaValue = 255;
                    }
                }
            }
            else
            {
                if (alphaValue != 255)
                {
                    touchPositions.Clear();
                    alphaValue = 255;
                }

                touchPositions.Add((Vector2)currentPosition);
            }
        }

        private Vector2? TouchPosition(TouchCollection touchLocationState)
        {
            TouchLocation touchLocation;
            if (touchLocationState.FindById(TouchID, out touchLocation))
            {
                return touchLocation.Position;
            }

            return null;
        }

        public void Draw(SpriteBatch batch)
        {
            if (touchPositions.Count != 0)
            {
                Vector2 previousPosition = touchPositions[0];
                Vector2 offsetForCenteringTouchPosition = new Vector2(-25, 0);
                foreach (Vector2 aPosition in touchPositions)
                {
                    DrawLine(batch, touchCircleIndicatorTexture, touchCrossHairIndicatorTexture, previousPosition + offsetForCenteringTouchPosition, aPosition + offsetForCenteringTouchPosition, new Color(0, 0, 255, alphaValue));
                    previousPosition = aPosition;
                }
            }

        }

        void DrawLine(SpriteBatch batch, Texture2D lineTexture, Texture2D touchTexture, Vector2 startingPoint, Vector2 endingPoint, Color lineColor)
        {
            batch.Draw(touchTexture, startingPoint, lineColor);

            Vector2 difference = startingPoint - endingPoint;
            float lineLength = difference.Length() / 8;
            for (int i = 0; i < lineLength; i++)
            {
                batch.Draw(lineTexture, startingPoint, lineColor);
                startingPoint.X -= difference.X / lineLength;
                startingPoint.Y -= difference.Y / lineLength;
            }

            batch.Draw(touchTexture, endingPoint, lineColor);
        }

    }
}
