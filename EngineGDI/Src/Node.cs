namespace EngineGDI.Src
{
    public abstract class Node
    {
        public virtual void Input() { }

        public virtual void Update(float deltaTime) { }

        public abstract void Draw();
    }
}
