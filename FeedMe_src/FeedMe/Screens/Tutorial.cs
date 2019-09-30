using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace FeedMe.Screens
{
    class Tutorial : Screen
    {
        bool nextPage;
        const string ActionExit = "Exit";
        const string ActionNext = "Next";

        /**********Tutorial text variables*************/
        const string OBJECTIVE = "Feed the creature without going over capacity. And don't starve!";
        const string OBJECT_TITLE = "***OBJECTS***";
        const string CREATURE_DESC = "You control this little guy. Tap any spot on the screen and it will move to that point.";
        const string JUMP_DESC = "Tap to make the creature jump.";
        const string METER_DESC = "Keep this filled by collecting food!";
        const string CAPACITY_DESC = "Indicates how much you can eat. Stay within its range to complete the level!";
        const string FOOD_DESC = "Eat these to fill the food meter. Not all food is"
            + "\ngood for you, and some even have special effects!";
       

        /***********Textures***************/
        Texture2D creatureImg;
        Texture2D meterImg;
        Texture2D capacityImg;
        Texture2D jumpImg;
        Texture2D foodImgA;
        Texture2D foodImgB;
        Texture2D foodImgC;
        Texture2D backButtonImg;

        Rectangle buttonArea;
        Rectangle creatureSize;
        Rectangle meterSize;
        Rectangle capacitySize;
        Rectangle jumpSize;

        public Tutorial(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Other)
        {
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, buttonArea);
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            creatureImg = content.Load<Texture2D>(@"Images/player");
            meterImg = content.Load<Texture2D>(@"Images/emptymeter");
            capacityImg = content.Load<Texture2D>(@"Images/capacitymarker");
            jumpImg = content.Load<Texture2D>(@"Images/jumpbutton");
            foodImgA = content.Load<Texture2D>(@"Food/carrot");
            foodImgB = content.Load<Texture2D>(@"Food/pepper");
            foodImgC = content.Load<Texture2D>(@"Food/weight");
            backButtonImg = content.Load<Texture2D>(@"Images/backbutton");

            creatureSize = new Rectangle(40, 80, creatureImg.Width / 2, creatureImg.Height / 2);
            meterSize = new Rectangle(40, 140, meterImg.Width / 2, meterImg.Height / 2);
            capacitySize = new Rectangle(40, 200, capacityImg.Width / 2, capacityImg.Height / 2);
            jumpSize = new Rectangle(40, 260, jumpImg.Width / 2, jumpImg.Height / 2);

            buttonArea = new Rectangle(10, 440, backButtonImg.Width, backButtonImg.Height);
            font = content.Load<SpriteFont>(@"Fonts/itemFont");
        }

        public override void Activate()
        {
        }

        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {
 
            if (input.IsPressed(ActionExit))
            {
                changeScreenDelegate(ScreenState.Title);
            }
           
        }

        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
        {
            batch.Draw(backButtonImg, buttonArea, Color.White);

            batch.DrawString(font, OBJECTIVE, new Vector2(110, 20), Color.LightGreen);

            DrawDescription(creatureImg, creatureSize, CREATURE_DESC);
            DrawDescription(meterImg, meterSize, METER_DESC);
            DrawDescription(capacityImg, capacitySize, CAPACITY_DESC);
            DrawDescription(jumpImg, jumpSize, JUMP_DESC);

            //food description
            batch.Draw(foodImgA, new Vector2(100, 320), Color.White);
            batch.Draw(foodImgB, new Vector2(80, 340), Color.White);
            batch.Draw(foodImgC, new Vector2(120, 350), Color.White);
            batch.DrawString(font, FOOD_DESC, new Vector2(160, 340), Color.White);
        }

        void DrawDescription(Texture2D image, Rectangle rect, string description)
        {
            Color color = Color.White;

            //draw tile
            batch.Draw(image, rect, color);

            //draw description
            Vector2 imagePos = new Vector2(rect.X, rect.Y);
            batch.DrawString(font, description, imagePos + new Vector2(image.Width, 0), color);
        }
    }
}
