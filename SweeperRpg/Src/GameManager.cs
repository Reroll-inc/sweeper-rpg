using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EngineGDI.Src.Events;
using EngineGDI.Src.Nodes;
using PlantUmlClassDiagramGenerator.Attributes;
using SweeperRpg.Src.UI;

namespace SweeperRpg.Src
{
    public class GameManager : IDynamicNode
    {
        [PlantUmlIgnore]
        private enum GameState
        {
            MAIN_MENU,

            PLAYING_LEVEL,

            VICTORY,

            DEFEAT,

            QUIT,
        }

        [PlantUmlIgnoreAssociation]
        private GameState state = GameState.MAIN_MENU;

        public static GameManager Instance { get; } = new();

        [PlantUmlIgnoreAssociation]
        private readonly Font font = new("Assets/Fonts/pixel.ttf", 16);
        private readonly LevelManager levelManager;
        private readonly UI.MainMenu mainMenu;
        private readonly DefeatScreen defeat;
        private readonly VictoryScreen victory;
        private readonly EventBus bus = new();

        private int level;
        private int maxLvls;

        private GameManager()
        {
            mainMenu = new UI.MainMenu(font: font);
            defeat = new DefeatScreen(font: font);
            victory = new VictoryScreen(font: font);

            ReadLevelCount();

            levelManager = new(font: font, bus: bus);

            bus.Subscribe<LevelWinEvent>(handler: HandleVictory);
            bus.Subscribe<PlayerDiedEvent>(handler: HandleLose);
        }

        private void ReadLevelCount()
        {
            DirectoryInfo dir = new("Assets/Levels");

            maxLvls = dir.GetFiles().Length;
        }

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

        private void HandleLose(PlayerDiedEvent data)
        {
            state = GameState.DEFEAT;
            defeat.EnableDefeat();
        }

        public void HandleVictory(LevelWinEvent data)
        {
            state = GameState.VICTORY;
            victory.EnableVictory();
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

        public void Input()
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
                case GameState.QUIT:
                    break;
                default:
                    break;
            }
        }

        public void Update(float deltaTime)
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
                default:
                    break;
            }
        }

        public void Draw()
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
                case GameState.QUIT:
                    break;
                default:
                    break;
            }
        }
    }
}
