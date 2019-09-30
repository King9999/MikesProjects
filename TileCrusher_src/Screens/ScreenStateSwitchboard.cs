using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileCrusher.Screens
{

    public enum ScreenState
    {
        Title,
        Editor,
        InGameMenu,
        Options,
        Tutorial,
        LevelSelect,
        GameScreen,
        About,
        PreviousScreen,
        Credits,
        Exit
    }

    class ScreenStateSwitchboard
    {
        static Game game;
        static SpriteBatch batch;
        static Screen previousScreen;
        static Screen currentScreen;
        static Dictionary<ScreenState, Screen> screens
            = new Dictionary<ScreenState, Screen>();

        private delegate Screen CreateScreen();

        public ScreenStateSwitchboard(Game game, SpriteBatch batch)
        {
            ScreenStateSwitchboard.game = game;
            ScreenStateSwitchboard.batch = batch;
            ChangeScreen(ScreenState.Title);
        }

        private void ChangeScreen(ScreenState screenState)
        {
            switch (screenState)
            {
                case ScreenState.Title:
                    {
                        ChangeScreen(screenState, new CreateScreen(CreateTitleScreen));
                        break;
                    }
                //case ScreenState.Editor:
                //    {
                //        ChangeScreen(screenState, new CreateScreen(CreateEditorScreen));
                //        break;
                //    }
                case ScreenState.InGameMenu:
                    {
                        ChangeScreen(screenState,
                                     new CreateScreen(CreateInGameMenuScreen));
                        break;
                    }
                case ScreenState.Options:
                    ChangeScreen(screenState, new CreateScreen(CreateOptionsScreen));
                    break;
                case ScreenState.Tutorial:
                    {
                        ChangeScreen(screenState, new CreateScreen(CreateTutorialScreen));
                        break;
                    }
                case ScreenState.LevelSelect:
                    {
                        ChangeScreen(screenState, new CreateScreen(CreateLevelSelectScreen));
                        break;
                    }

                case ScreenState.GameScreen:
                    {
                        ChangeScreen(screenState, new CreateScreen(CreateGameScreen));
                        break;
                    }

                case ScreenState.PreviousScreen:
                    {
                        currentScreen = previousScreen;
                        currentScreen.Activate();
                        break;
                    }
                case ScreenState.Credits:
                    {
                        ChangeScreen(screenState, new CreateScreen(CreateCreditsScreen));
                        break;
                    }

                case ScreenState.About:
                    ChangeScreen(screenState, new CreateScreen(CreateAboutScreen));
                    break;

                case ScreenState.Exit:
                    {
                        game.Exit();
                        break;
                    }

            }
        }

        private void ChangeScreen(ScreenState screenState, CreateScreen createScreen)
        {
            previousScreen = currentScreen;

            if (!screens.ContainsKey(screenState))
            {
                screens.Add(screenState, createScreen());
                screens[screenState].LoadContent();
            }
            
            currentScreen = screens[screenState];
           
            currentScreen.Activate();
            
        }

        /* Removes screen from memory once it's no longer in use */
        public void RemoveScreen(ScreenState screenState)
        {
            if (screens.ContainsKey(screenState))
            {
                screens.Remove(screenState);
            }
        }

        private Screen CreateTitleScreen()
        {
            return new Title(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        private Screen CreateAboutScreen()
        {
            return new AboutScreen(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        //private Screen CreateEditorScreen()
        //{
        //    return new EditorScreen(game, batch, new Screen.ChangeScreen(ChangeScreen));
        //}

        private Screen CreateInGameMenuScreen()
        {
            return new InGameMenu(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        private Screen CreateLevelSelectScreen()
        {
            return new LevelSelect(game,batch, new Screen.ChangeScreen(ChangeScreen));
        }

        private Screen CreateGameScreen()
        {
            return new GameScreen(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        private Screen CreateTutorialScreen()
        {
            return new Tutorial(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        private Screen CreateCreditsScreen()
        {
            return new CreditsScreen(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        private Screen CreateOptionsScreen()
        {
            return new OptionsScreen(game, batch, new Screen.ChangeScreen(ChangeScreen));
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void Draw()
        {
            
            currentScreen.Draw();
        }
    }
}
