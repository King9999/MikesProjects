

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace PuzzleRPG.Texts
{
    class Text
    {
        public Vector2 Position;
        public Color DisplayColor = Color.White;

        public enum Alignment
        {
            None,
            Horizontal,
            Vertical,
            Both
        }

        SpriteFont font;
        string text;
        Vector2 textSize;
        Color outlineColor = Color.White;
        bool isTextOutlined = false;
        //Alignment alignment;
        //Rectangle displayArea;

        public Text(SpriteFont displayFont, string displayText, Vector2 displayPosition)
            : this(displayFont, displayText, displayPosition, Color.White, Color.White,
            false, Alignment.None, Rectangle.Empty)
        {
        }

        public Text(SpriteFont displayFont, string displayText, Vector2 displayPosition,
            Color displayColor)
            : this(displayFont, displayText, displayPosition, displayColor, Color.White,
            false, Alignment.None, Rectangle.Empty)
        {
        }

        public Text(SpriteFont displayFont, string displayText, Vector2 displayPosition,
           Color displayColor, Alignment alignment, Rectangle displayArea)
            : this(displayFont, displayText, displayPosition, displayColor, Color.White,
            false, alignment, displayArea)
        {
        }

        public Text(SpriteFont displayFont, string displayText, Vector2 displayPosition,
          Color displayColor, Color outlineColor)
            : this(displayFont, displayText, displayPosition, displayColor, Color.White,
            true, Alignment.None, Rectangle.Empty)
        {
        }

        public Text(SpriteFont displayFont,
            string displayText,
            Vector2 displayPosition,
          Color displayColor,
            Color outlineColor,
            Alignment alignment,
            Rectangle displayArea)
            : this(displayFont, displayText, displayPosition, displayColor, Color.White,
            true, alignment, displayArea)
        {
        }

        private Text(SpriteFont displayFont, string displayText, Vector2 displayPosition,
          Color displayColor, Color outlineColor, bool isTextOutlined, Alignment alignment, Rectangle displayArea)
        {
            font = displayFont;
            text = displayText;
            Position = displayPosition;

            this.isTextOutlined = isTextOutlined;
            this.outlineColor = outlineColor;

            CenterText(alignment, displayArea);
        }

        private void CenterText(Alignment alignment, Rectangle displayArea)
        {
            textSize = font.MeasureString(text);

            int positionX = (int)Position.X;
            int positionY = (int)Position.Y;

            switch (alignment)
            {
                case Alignment.Horizontal:
                    {
                        positionX = (int)((displayArea.Width / 2) - (textSize.X / 2)) + displayArea.X;
                        break;
                    }

                case Alignment.Vertical:
                    {
                        positionY = (int)((displayArea.Height / 2) - (textSize.Y / 2)) + displayArea.Y;
                        break;
                    }

                case Alignment.Both:
                    {
                        positionX = (int)((displayArea.Width / 2) - (textSize.X / 2)) + displayArea.X;
                        positionY = (int)((displayArea.Height / 2) - (textSize.Y / 2)) + displayArea.Y;
                        break;
                    }

                case Alignment.None:
                    {
                        //Nothing to do
                        break;
                    }
            }

            Position = new Vector2(positionX, positionY);
        }

        public void Draw(SpriteBatch batch)
        {
            if (isTextOutlined)
            {
                int outlineWidth = 3;
                batch.DrawString(font, text, Position - new Vector2(0, outlineWidth), outlineColor);

                batch.DrawString(font, text, Position + new Vector2(0, outlineWidth), outlineColor);

                batch.DrawString(font, text, Position - new Vector2(outlineWidth, 0), outlineColor);

                batch.DrawString(font, text, Position + new Vector2(outlineWidth, 0), outlineColor);

                batch.DrawString(font, text, Position + new Vector2(outlineWidth, outlineWidth), outlineColor);

                batch.DrawString(font, text, Position + new Vector2(outlineWidth, -outlineWidth), outlineColor);

                batch.DrawString(font, text, Position + new Vector2(-outlineWidth, outlineWidth), outlineColor);

                batch.DrawString(font, text, Position + new Vector2(-outlineWidth, -outlineWidth), outlineColor);
            }

            batch.DrawString(font, text, Position, DisplayColor);
        }

        public void ChangeText(string displayText)
        {
            text = displayText;
            CenterText(Alignment.None, Rectangle.Empty);
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)textSize.X, (int)textSize.Y);
            }
        }
    }
}

