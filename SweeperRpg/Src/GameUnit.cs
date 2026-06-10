using System.Drawing;
using EngineGDI.Src;

namespace SweeperRpg.Src
{
    interface IGameUnit
    {
        protected Point Position { get; }

        public Collisioner Collisioner { get; }
    }
}
