using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EngineGDI.Src.SweeperRpg.UI;

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
        private readonly UI.MainMenu mainMenu;
        private readonly LevelManager levelManager = LevelManager.Instance;
        private readonly DefeatScreen defeat;
        private readonly VictoryScreen victory;

        private int level;
        private int maxLvls;

        private GameManager()
        {
            mainMenu = new UI.MainMenu(font: font);
            defeat = new DefeatScreen(font: font);
            victory = new VictoryScreen(font: font);

            ReadLevelCount();

            levelManager.Init(font: font);
        }

        private void ReadLevelCount()
        {
            DirectoryInfo dir = new DirectoryInfo("Assets/Levels");

            maxLvls = dir.GetFiles().Length;
        }

        // Custom methods here
        public void OnPlay()
        {
            level = 1;

            state = GameState.PLAYING_LEVEL;
            levelManager.StartLevel(level);
        }

        public void OnRetry()
        {
            state = GameState.PLAYING_LEVEL;
            levelManager.ResetLevel();
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

        public void OnVictory()
        {
            state = GameState.VICTORY;
        }

        public void NextLevel()
        {
            if (maxLvls == level)
            {
                state = GameState.MAIN_MENU;

                return;
            }
            level++;

            state = GameState.PLAYING_LEVEL;
            levelManager.StartLevel(level);
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
                    victory.Input();
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
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Update(deltaTime: deltaTime);
                    break;
                case GameState.VICTORY:
                    break;
                case GameState.DEFEAT:
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
                    victory.Draw();
                    break;
                case GameState.DEFEAT:
                    defeat.Draw();
                    break;
            }
        }
    }
}
