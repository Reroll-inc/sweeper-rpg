using System.Drawing;
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

        private static readonly GameManager instance = new GameManager();

        public static GameManager Instance
        {
            get { return instance; }
        }
        private readonly Font font = new Font("Assets/Fonts/pixel.ttf", 16);
        private readonly MainMenu mainMenu;
        private readonly LevelManager levelManager = LevelManager.Instance;
        private readonly Defeat defeat;

        private GameManager()
        {
            mainMenu = new MainMenu(font: font);
            defeat = new Defeat(font: font);

            levelManager.Init(font: font);
        }

        // Custom methods here
        public void OnPlay()
        {
            levelManager.ResetLevel();
            state = GameState.PLAYING_LEVEL;
            defeat.DisableDefeat();
            levelManager.StartLevel(1);
        }

        public void OnExit()
        {
            state = GameState.QUIT;
        }

        public void OnDefeat()
        {
            state = GameState.DEFEAT;
            defeat.EnableDefeat();
        }

        public void OnWin()
        {
            state = GameState.VICTORY;
        }

        public void OnMainMenu()
        {
            levelManager.ResetLevel();
            state = GameState.MAIN_MENU;
        }

        public override void Input()
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    mainMenu.Input();
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Input();
                    break;
                case GameState.VICTORY:
                    // VictoryScreen.Input;
                    break;
                case GameState.DEFEAT:
                    defeat.Input();
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
    }
}
