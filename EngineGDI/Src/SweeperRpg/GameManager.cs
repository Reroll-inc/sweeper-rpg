using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EngineGDI.Src.SweeperRpg
{
    public class GameManager : Node
    {
        // Buscar patron maquina estados
        private enum GameState
        {
            MAIN_MENU,

            PLAYING_LEVEL,

            VICTORY,

            DEFEAT,

            QUIT,
        }

        private GameState state = GameState.MAIN_MENU;

        // private VictoryScreen();
        // private Defeat();
        // private MainMenu();

        private static readonly GameManager instance = new GameManager();

        private MainMenu mainMenu = new MainMenu();
        public static GameManager Instance
        {
            get { return instance; }
        }
        private static readonly LevelManager levelManager = LevelManager.Instance;
        private static readonly Defeat defeat = new Defeat();

        private GameManager() { }

        public override void Input()
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    mainMenu.Input();
                    var menuResult = mainMenu.GetResult();
                    if (menuResult == MainMenu.MenuResult.Play)
                    {
                        // levelManager.LoadLevel(1);
                        // levelManager.CreateLevel();
                        levelManager.StartLevel(1); // Todavia hardcodeado...
                        state = GameState.PLAYING_LEVEL;
                    }
                    else if (menuResult == MainMenu.MenuResult.Exit)
                    {
                        state = GameState.QUIT;
                    }
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Input();
                    break;
                case GameState.VICTORY:
                    // VictoryScreen.Input;
                    break;
                case GameState.DEFEAT:
                    defeat.Input();
                    var loseResult = defeat.GetResult();

                    if (loseResult == Defeat.DefeatResult.Retry)
                    {
                        defeat.DisableDefeat();
                        levelManager.ResetLevel();
                        levelManager.StartLevel(1); // Hardcodeado otra vez...
                        state = GameState.PLAYING_LEVEL;
                    }
                    else if (loseResult == Defeat.DefeatResult.MainMenu)
                    {
                        defeat.DisableDefeat();
                        levelManager.ResetLevel();
                        state = GameState.MAIN_MENU;
                    }
                    break;
            }
            //reimplementar como llamados de cambio de estado dentro de cada pantalla
            // if (Engine.OnKeyDown(Keys.Z))
            //     switch (state)
            //     {
            //         case GameState.MAIN_MENU:
            //             //muy hardcodeado disculpen
            //             levelManager.LoadLevel(1);
            //             levelManager.CreateLevel();
            //             //termina el hardcodeo
            //             state = GameState.PLAYING_LEVEL;
            //             break;
            //         case GameState.PLAYING_LEVEL:
            //             state = GameState.VICTORY; //de momento debugging, pero posible menu de pausa
            //             break;
            //         case GameState.VICTORY:
            //             state = GameState.PLAYING_LEVEL; //posible lugar para cargar el segundo nivel
            //             break;
            //         case GameState.DEFEAT:
            //             state = GameState.MAIN_MENU;
            //             break;
            //     }
            // if (Engine.OnKeyDown(Keys.X))
            //     switch (state)
            //     {
            //         case GameState.MAIN_MENU:
            //             state = GameState.QUIT;
            //             break;
            //         case GameState.PLAYING_LEVEL:
            //             state = GameState.DEFEAT; //de momento debbugin, pero posible menu de pausa
            //             state = GameState.MAIN_MENU; //si añadimos menu de pausa, lo memeos aca
            //             break;
            //         case GameState.VICTORY:
            //             state = GameState.MAIN_MENU;
            //             break;
            //         case GameState.DEFEAT:
            //             state = GameState.QUIT;
            //             break;
            //     }
        }

        public override void Draw()
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    mainMenu.Draw();
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Draw();
                    break;
                case GameState.VICTORY:
                    // VictoryScreen.Draw();
                    break;
                case GameState.DEFEAT:
                    defeat.Draw();
                    break;
            }
        }

        public override void Update(float deltaTime)
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    // MainMenu.Update(deltaTime: deltaTime);
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Update(deltaTime: deltaTime);

                    if (levelManager.IsPlayerDead())
                    {
                        state = GameState.DEFEAT;
                        defeat.EnableDefeat();
                    }
                    break;
                case GameState.VICTORY:
                    // VictoryScreen.Update(deltaTime: deltaTime);
                    break;
                case GameState.DEFEAT:
                    // Defeat.Update(deltaTime: deltaTime);
                    break;
                case GameState.QUIT:
                    // Close the game window.
                    Application.Exit();
                    break;
            }
        }
    }
}
