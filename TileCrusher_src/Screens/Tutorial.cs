using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using TileCrusher.Sprites;

namespace TileCrusher.Screens
{
    class Tutorial : Screen
    {
        ScrollBackground background, backgroundCopy;
        Button exitButton;
        Button pageButton;  //used to change what's displayed on screen
        int windowWidth;
        bool nextPage;
        const string ActionExit = "Exit";
        const string ActionNext = "Next";

        /**********Tutorial text variables*************/
        const string TITLE = "How To Play";
        const string OBJECTIVE = "Crush all destructible tiles by rolling over them with the Wrecking Ball.\n" +
        "You have a limited number of turns, so mind your movements!";
        const string OBJECT_TITLE = "***OBJECTS***";
        const string BALL_DESC = "This is what you control.\nTap any tile next to the ball to move it.";
        const string NORMAL_DESC = "These are standard tiles.\nSmash them to finish the level!";
        const string EMPTY_DESC = "Rolling over these will restore tiles,\nwasting precious turns. Sometimes it's necessary...";
        const string DURABLE_DESC = "Requires more than one crush.\nThe tile's number will tell you!";
        const string SUPER_DESC = "Cannot be destroyed, but\nthey won't use up a turn.";
        const string TARGET_DESC = "Break the required number \nof tiles to break this one!";
        const string CHAIN_DESC = "Breaks entire lines of tiles,\nbut will also restore \nbroken ones!";

        /***********Textures***************/
        Texture2D ball;
        Texture2D normalTile;
        Texture2D emptyTile;
        Texture2D superTile;
        Texture2D durableTile;
        Texture2D targetTile;
        Texture2D chainTile;
        Texture2D ballName;
        Texture2D normalTileName;
        Texture2D emptyTileName;
        Texture2D superTileName;
        Texture2D durableTileName;
        Texture2D targetTileName;
        Texture2D chainTileName;

        public Tutorial(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Other)
        {
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionExit, GestureType.Tap, exitButton.TouchArea);
            input.AddTouchGestureInput(ActionNext, GestureType.Tap, pageButton.TouchArea);
        }

        protected override void LoadScreenContent(ContentManager content)
        {
            background = new ScrollBackground(content);
            backgroundCopy = new ScrollBackground(content);
            windowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            backgroundCopy.Position.X = -windowWidth;

            exitButton = new Button(content, "Quit Tutorial", new Vector2(440, 400), Color.White);
            pageButton = new Button(content, "Change Page", new Vector2(180, 400), Color.White);
            nextPage = false;

            ball = content.Load<Texture2D>(@"Tiles/WreckingBall");
            normalTile = content.Load<Texture2D>(@"Tiles/NormalTile");
            emptyTile = content.Load<Texture2D>(@"Tiles/EmptyTile");
            superTile = content.Load<Texture2D>(@"Tiles/SuperTile");
            targetTile = content.Load<Texture2D>(@"Tiles/TargetTile");
            durableTile = content.Load<Texture2D>(@"Tiles/DurableTile");
            chainTile = content.Load<Texture2D>(@"Tiles/ChainTile");

            ballName = content.Load<Texture2D>(@"Images/BallName");
            normalTileName = content.Load<Texture2D>(@"Images/NormalTileName");
            emptyTileName = content.Load<Texture2D>(@"Images/EmptyTileName");
            superTileName = content.Load<Texture2D>(@"Images/SuperTileName");
            durableTileName = content.Load<Texture2D>(@"Images/DurableTileName");
            targetTileName = content.Load<Texture2D>(@"Images/TargetTileName");
            chainTileName = content.Load<Texture2D>(@"Images/ChainTileName");

            font = content.Load<SpriteFont>(@"Fonts/screenFont");
        }

        public override void Activate()
        {
        }

        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {
 
            if (input.IsPressed(ActionExit))
            {
                changeScreenDelegate(ScreenState.PreviousScreen);
                nextPage = false;
            }

            if (input.IsPressed(ActionNext))
            {
                //change screen info
                nextPage = !nextPage;
                
            }

            
            //update background
            background.Position.X += 2;
            backgroundCopy.Position.X += 2;

            if (background.Position.X > windowWidth)
                background.Position.X = -windowWidth + 2;   //2 is added to eliminate gap between images.

            if (backgroundCopy.Position.X > windowWidth)
                backgroundCopy.Position.X = -windowWidth + 2;
        }

        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
        {
            
            background.Draw(batch);
            backgroundCopy.Draw(batch);
            batch.DrawString(font, OBJECTIVE, new Vector2(50, 10), Color.Yellow);
            if (!nextPage)
            {
                DrawDescription(ball, ballName, new Vector2(20, 100), BALL_DESC);
                DrawDescription(normalTile, normalTileName, new Vector2(20, 190), NORMAL_DESC);
                DrawDescription(emptyTile, emptyTileName, new Vector2(20, 280), EMPTY_DESC);
                
            }
            else
            {
                //show additional stuff
                DrawDescription(durableTile, durableTileName, new Vector2(20, 100), DURABLE_DESC);
                DrawDescription(superTile, superTileName, new Vector2(20, 190), SUPER_DESC);
                DrawDescription(targetTile, targetTileName, new Vector2(20, 280), TARGET_DESC);
                DrawDescription(chainTile, chainTileName, new Vector2(420, 190), CHAIN_DESC);
                
            }
            exitButton.Draw(batch);
            pageButton.Draw(batch);
            
        }

        void DrawDescription(Texture2D image, Texture2D tileName, Vector2 imagePos, string description)
        {
            Color color = Color.White;

            //draw tile
            batch.Draw(image, imagePos, color);

            //draw tile name
            batch.Draw(tileName, imagePos + new Vector2(-10, 40), color);

            //draw description
            batch.DrawString(font, description, imagePos + new Vector2(100, 0), color);
        }
    }
}
