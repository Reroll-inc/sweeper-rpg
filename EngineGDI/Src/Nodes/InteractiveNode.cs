namespace EngineGDI.Src.Nodes
{
    public abstract class InteractiveNode
    {
        public abstract void Input();

        public virtual void Update(float deltaTime) { }

        public abstract void Draw();
    }
}
