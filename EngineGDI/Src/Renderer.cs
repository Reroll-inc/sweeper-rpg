using EngineGDI.Src.Drawing;

namespace EngineGDI.Src
{
    public class Renderer(IDrawCommand command)
    {
        private readonly IDrawCommand Command = command;

        public void Draw()
        {
            Engine.DrawACommand(command: Command);
        }
    }
}
