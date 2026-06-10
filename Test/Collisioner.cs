using System.Drawing;
using EngineGDI.Src;
using Xunit;

namespace Test
{
    public class CollisionerTest
    {
        [Fact]
        public void ShouldCollideOrNotAsExpected()
        {
            Collisioner collisionerOne = new(position: new Point(1, 1), size: new Size(2, 2));
            Collisioner collisionerTwo = new(position: new Point(1, 1), size: new Size(2, 2));

            Assert.True(collisionerOne.CheckCollision(collisionerTwo));

            collisionerTwo.UpdatePosition(position: new Point(10, 10));

            Assert.False(collisionerOne.CheckCollision(collisionerTwo));
        }
    }
}
