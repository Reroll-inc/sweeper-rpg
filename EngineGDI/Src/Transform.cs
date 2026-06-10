using System.Numerics;

namespace EngineGDI.Src
{
    public class Transform(Vector2 position, float rotation, Vector2 scale)
    {
        public Vector2 Position = position;
        public float Rotation = rotation;
        public Vector2 Scale = scale;
    }
}
