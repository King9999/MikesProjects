using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using PuzzleRPG.Inputs;

namespace PuzzleRPG.Screens
{
    class Screen
    {
     
        protected static Game game;
        protected static ContentManager content;
        protected static SpriteBatch batch;
        protected static Random random = new Random();
        protected SpriteFont font;

        protected static Song music;

        public ChangeScreen changeScreenDelegate;
        public delegate void ChangeScreen(ScreenState screen);

        protected GameInput input = new GameInput();
        public bool IsTouchIndicatorEnabled = false;
        private TouchIndicatorCollection touchIndicator;


        public enum BackButtonScreenType
        {
            First,
            Gameplay,
            Editor,
            InGameMenu,
            Options,
            LevelSelect,
            Other
        }

        private BackButtonScreenType backButtonScreenType;
        const string ActionBack = "Back";

        public Screen(Game game, SpriteBatch batch, ChangeScreen changeScreen, BackButtonScreenType backButtonScreenType)
        {
            Screen.game = game;
            Screen.content = game.Content;
            Screen.batch = batch;

            changeScreenDelegate = changeScreen;
            touchIndicator = new TouchIndicatorCollection();

            this.backButtonScreenType = backButtonScreenType;
        }

        public virtual void Activate()
        {
        }

        public void LoadContent()
        {
            font = content.Load<SpriteFont>(@"Fonts/screenFont");
            LoadScreenContent(content);
            input.AddGamePadInput(ActionBack, Buttons.Back, true);
            SetupInputs();
            
        }

        protected virtual void SetupInputs()
        {
        }

        protected virtual void LoadScreenContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
            input.BeginUpdate();

            HandleBackButtonInput();

            if (IsTouchIndicatorEnabled)
            {
                touchIndicator.Update(gameTime, content);
            }

            UpdateScreen(gameTime, game.GraphicsDevice.PresentationParameters.DisplayOrientation);

            input.EndUpdate();
        }

        private void HandleBackButtonInput()
        {
            if (input.IsPressed(ActionBack))
            {
                switch (backButtonScreenType)
                {
                    case BackButtonScreenType.First:
                        {
                            game.Exit();
                            break;
                        }

                    case BackButtonScreenType.Gameplay:
                        {
                            changeScreenDelegate(ScreenState.LevelSelect);
                            break;
                        }

                    case BackButtonScreenType.LevelSelect:
                        {
                            changeScreenDelegate(ScreenState.InGameMenu);
                            break;
                        }

                    case BackButtonScreenType.Options:
                        changeScreenDelegate(ScreenState.InGameMenu);
                        break;

                    case BackButtonScreenType.InGameMenu:
                        {
                            changeScreenDelegate(ScreenState.Title);
                            break;
                        }

                    case BackButtonScreenType.Other:
                        {
                            changeScreenDelegate(ScreenState.PreviousScreen);
                            break;
                        }
                }
            }
        }

        public bool BackButtonPressed()
        {
            return (input.IsPressed(ActionBack));
        }
        protected virtual void UpdateScreen(GameTime gameTime, DisplayOrientation screenOrientation)
        {
        }

        public void Draw()
        {
            batch.Begin();
            
            DrawScreen(batch, game.GraphicsDevice.PresentationParameters.DisplayOrientation);

            if (IsTouchIndicatorEnabled)
            {
                touchIndicator.Draw(batch);
            }


            batch.End();
        }

        protected virtual void DrawScreen(SpriteBatch batch, DisplayOrientation screenOrientation)
        {
           // batch.Draw(blackScreen, fadeScreen, fadeColor); //used for fading to black
        }

        public void SaveState()
        {
            SaveScreenState();
        }

    
        
        protected virtual void SaveScreenState()
        {
        }

        static public int ScreenWidth
        {
            get { return game.GraphicsDevice.PresentationParameters.BackBufferWidth; }
        }

        static public int ScreenHeight
        {
            get { return game.GraphicsDevice.PresentationParameters.BackBufferHeight; }
        }

        static public Rectangle ScreenRectangle
        {
            get { return new Rectangle(0, 0, ScreenWidth, ScreenHeight); }
        }

        static public Rectangle ScreenLeftHalf
        {
            get { return new Rectangle(0, 0, (int)(ScreenWidth / 2), ScreenHeight); }
        }

        static public Rectangle ScreenRightHalf
        {
            get { return new Rectangle((int)(ScreenWidth / 2), 0, (int)(ScreenWidth / 2), ScreenHeight); }
        }

    }
}
