using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using EngineGDI.Src;

namespace EngineGDI.Src
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class Transform
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Transform(Vector2 position, float rotation, Vector2 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
