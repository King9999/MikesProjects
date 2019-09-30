/* The title screen will have the words "TILE" and "CRUSHER" appear from the top and bottom of the screen, respectively.
 * The letters appear individually, one after the other, and move into their designated place. */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace FeedMe.Screens
{
    class Title : Screen
    {

        Texture2D logoImg;
        Texture2D startImg;
        Texture2D creditsImg;   //only me, but I must also thank XMG for GCA.
        Texture2D tutorialImg;

        Rectangle logoArea;
        Rectangle startArea;
        Rectangle creditsArea;
        Rectangle helpArea;

        SpriteFont screenFont;

        const string ActionStart = "Start";
        const string ActionCredits = "Credits";
        const string ActionTutorial = "Tutorial";

        const string legal = "(C)2012 Mike Murray - mmking9999.com";
        const string version = "Version 1.0";

        public Title(Game game,SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.First)
        {
           
        }

        protected override void SetupInputs()
        {
            input.AddTouchGestureInput(ActionStart, GestureType.Tap, startArea);
            input.AddTouchGestureInput(ActionCredits, GestureType.Tap, creditsArea);
            input.AddTouchGestureInput(ActionTutorial, GestureType.Tap, helpArea);
        }

        public override void Activate()
        {

        }

        protected override void LoadScreenContent(ContentManager content)
        {
            logoImg = content.Load<Texture2D>(@"Images/Logo");
            startImg = content.Load<Texture2D>(@"Images/startbutton");
            creditsImg = content.Load<Texture2D>(@"Images/creditsbutton");
            tutorialImg = content.Load<Texture2D>(@"Images/tutorial");

            logoArea = new Rectangle(Screen.ScreenWidth / 3, 60, logoImg.Width + 50, logoImg.Height + 50);
            startArea = new Rectangle(150, 350, startImg.Width, startImg.Height);
            helpArea = new Rectangle(startArea.X + 140, 350, tutorialImg.Width, tutorialImg.Height);
            creditsArea = new Rectangle(startArea.X + 300, 350, creditsImg.Width, creditsImg.Height);
            

            screenFont = content.Load<SpriteFont>(@"Fonts/titleFont");
        }

        protected override void UpdateScreen(GameTime gameTime, DisplayOrientation displayOrientation)
        {
            if (input.IsPressed(ActionStart))
            {
                changeScreenDelegate(ScreenState.GameScreen);
            }

            if (input.IsPressed(ActionCredits))
                changeScreenDelegate(ScreenState.Credits);

            if (input.IsPressed(ActionTutorial))
                changeScreenDelegate(ScreenState.Tutorial);
        }

        protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
        {
            batch.Draw(logoImg, logoArea, Color.White);
            batch.Draw(startImg, startArea, Color.White);
            batch.Draw(tutorialImg, helpArea, Color.White);
            batch.Draw(creditsImg, creditsArea, Color.White);
            batch.DrawString(screenFont, legal, new Vector2(160, 420), Color.White);
            batch.DrawString(screenFont, version, new Vector2(Screen.ScreenWidth / 3 + 60, 450), Color.Salmon);
        }
    }
}
