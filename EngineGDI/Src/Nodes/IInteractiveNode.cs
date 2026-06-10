namespace EngineGDI.Src.Nodes
{
    public interface IInteractiveNode
    {
        public void Input();

        public void Update(float deltaTime);

        public void Draw();
    }
}
