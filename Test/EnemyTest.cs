using SweeperRpg.Src;
using Xunit;

namespace Test
{
    public class EnemyTest
    {
        [Fact]
        public void ShouldChangeStatesCorrectly()
        {
            Enemy enemy = new(x: 1, y: 1, EnemyKind.GOBLIN_MAGE);

            Assert.True(enemy.IsAlive());

            enemy.Defeat();

            Assert.False(enemy.IsAlive());

            enemy.Reset();

            Assert.True(enemy.IsAlive());
        }

        [Fact]
        public void ShouldDoAFullClone()
        {
            Enemy enemy = new(x: 1, y: 1, EnemyKind.GOBLIN_MAGE);
            Enemy clone = enemy.Clone(x: 2, y: 2);

            Assert.False(enemy.Equals(clone));
            Assert.False(enemy.Transform.Equals(clone.Transform));
            Assert.False(enemy.Collisioner.Equals(clone.Collisioner));
            Assert.False(enemy.Collisioner.CheckCollision(clone.Collisioner));
        }
    }
}
