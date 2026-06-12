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
            Collisioner collisionerOne = new(transform: new(position: new(1, 1)));
            Collisioner collisionerTwo = new(transform: new(position: new(1, 1)));

            Assert.True(collisionerOne.CheckCollision(element: collisionerTwo));

            collisionerTwo.UpdatePosition(transform: new(position: new(2, 2)));

            Assert.False(collisionerOne.CheckCollision(element: collisionerTwo));
        }
    }
}
