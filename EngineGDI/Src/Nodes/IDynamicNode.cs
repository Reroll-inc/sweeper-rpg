namespace EngineGDI.Src.Nodes
{
    public interface IDynamicNode
    {
        public void Update(float deltaTime);

        public void Draw();
    }
}
