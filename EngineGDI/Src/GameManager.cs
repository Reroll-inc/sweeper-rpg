using EngineGDI.Src.sweeperRpg;
using EngineGDI.Src.SweeperRpg;
using EngineGDI.Src.SweeperRpg.UI;

namespace EngineGDI.Src
{
    public class GameManager : Node
    {
        //refactorizar usando un enum
        //buscar patron maquina estados
        private enum GameState
        {
            MAIN_MENU,

            PLAYING_LEVEL,

            VICTORY,

            DEFEAT,

            QUIT, //falta implementar
        }

        private static readonly LevelManager levelManager = LevelManager.Instance;

        public GameState state = GameState.MAIN_MENU;

        private VictoryScreen();

        private Defeat();

        private MainMenu();

        public static GameManager Instance
        {
            get { return instance; }
        }

        public override void Input()
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    MainMenu.Input();
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Input();
                    break;
                case GameState.VICTORY:
                    VictoryScreen.Input;
                    break;
                case GameState.DEFEAT:
                    Defeat.Input;
                    break;
            }
            //reimplementar como llamados de cambio de estado dentro de cada pantalla
            if (Engine.OnKeyDown(Keys.Z))
                switch (state)
                {
                    case GameState.MAIN_MENU:
                        //muy hardcodeado disculpen
                        levelManager.LoadLevel(1);
                        levelManager.CreateLevel();
                        //termina el hardcodeo
                        state = GameState.PLAYING_LEVEL;
                        break;
                    case GameState.PLAYING_LEVEL:
                        state = GameState.VICTORY; //de momento debugging, pero posible menu de pausa
                        break;
                    case GameState.VICTORY:
                        state = GameState.PLAYING_LEVEL; //posible lugar para cargar el segundo nivel
                        break;
                    case GameState.DEFEAT:
                        state = GameState.MAIN_MENU;
                        break;
                }
            if (Engine.OnKeyDown(Keys.X))
                switch (state)
                {
                    case GameState.MAIN_MENU:
                        state = GameState.QUIT;
                        break;
                    case GameState.PLAYING_LEVEL:
                        state = GameState.DEFEAT; //de momento debbugin, pero posible menu de pausa
                        state = GameState.MAIN_MENU; //si añadimos menu de pausa, lo memeos aca
                        break;
                    case GameState.VICTORY:
                        state = GameState.MAIN_MENU;
                        break;
                    case GameState.DEFEAT:
                        state = GameState.QUIT;
                        break;
                }
        }

        public override void Draw()
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    MainMenu.Draw();
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Draw();
                    break;
                case GameState.VICTORY:
                    VictoryScreen.Draw();
                    break;
                case GameState.DEFEAT:
                    Defeat.Draw();
            }
        }

        public override void Update()
        {
            switch (state)
            {
                case GameState.MAIN_MENU:
                    MainMenu.Update();
                    break;
                case GameState.PLAYING_LEVEL:
                    levelManager.Update(deltaTime: deltaTime);
                    break;
                case GameState.VICTORY:
                    VictoryScreen.Update();
                    break;
                case GameState.DEFEAT:
                    Defeat.Update();
            }
        }
    }
}
