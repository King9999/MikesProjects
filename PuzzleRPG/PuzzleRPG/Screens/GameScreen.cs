using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO; 
using System.IO.IsolatedStorage;
using System.Reflection;    //used for recovering embedded resources; in this case, the levels


using PuzzleRPG.UI;
using PuzzleRPG.Inputs;
using PuzzleRPG.Texts;



namespace PuzzleRPG.Screens
{
    class GameScreen : Screen
    {
        Texture2D atkBlock;
        Texture2D shldBlock;
        Texture2D redBlock;
        Texture2D greenBlock;
        Texture2D blueBlock;
        Texture2D multiBlock;
        Texture2D well;         //background for the blocks

        Lifebar playerBar;       //player's health bar
        Texture2D emptyBarTexture;
        Texture2D lifeBarTexture;
        Texture2D redLifeTexture;

        SpriteFont debugFont;

        GameInput input = new GameInput();
        const string screenTapped = "ScreenTap";
        const string screenHold = "Hold";
        Rectangle screenArea;

        Level level;
         public GameScreen(Game game, SpriteBatch batch, ChangeScreen changeScreen)
            : base(game, batch, changeScreen, BackButtonScreenType.Gameplay)
        {
        }

         protected override void LoadScreenContent(ContentManager content)
         {
             atkBlock = content.Load<Texture2D>(@"Blocks/attack");
             shldBlock = content.Load<Texture2D>(@"Blocks/shield");
             redBlock = content.Load<Texture2D>(@"Blocks/red");
             greenBlock = content.Load<Texture2D>(@"Blocks/green");
             blueBlock = content.Load<Texture2D>(@"Blocks/blue");
             multiBlock = content.Load<Texture2D>(@"Blocks/multi");
             well = content.Load<Texture2D>(@"Images/wellbackground");

             debugFont = content.Load<SpriteFont>(@"Fonts/debugFont");

             //lifebar
             emptyBarTexture = content.Load<Texture2D>(@"Images/lifebar_empty");
             lifeBarTexture = content.Load<Texture2D>(@"Images/lifebar_solid");
             redLifeTexture = content.Load<Texture2D>(@"Images/lifebar_red");

              screenArea = new Rectangle(0, 0, Screen.ScreenWidth, Screen.ScreenHeight);
              input.AddTouchGestureInput(screenTapped, GestureType.Tap, screenArea);
              input.AddTouchGestureInput(screenHold, GestureType.Hold, screenArea);

             level = new Level(atkBlock, shldBlock, redBlock, greenBlock, blueBlock, multiBlock, well, 3);
             playerBar = new Lifebar(emptyBarTexture, lifeBarTexture, redLifeTexture, 100, 20);
         }

         protected override void DrawScreen(SpriteBatch batch, DisplayOrientation displayOrientation)
         {
             level.Draw(batch);
             level.DisplayDebug(debugFont, batch);
             playerBar.Draw(batch);
         }

         protected override void UpdateScreen(GameTime gameTime, DisplayOrientation screenOrientation)
         {
             level.Play();

             if (input.IsPressed(screenTapped))
             {
                 //change lifebar
                 playerBar.DecreaseBar(10);
             }

             if (input.IsPressed(screenHold))
             {
                 //restore lifebar
                 playerBar.IncreaseBar(100);
             }
             playerBar.Update();
             base.UpdateScreen(gameTime, screenOrientation);
         }
    }
}
