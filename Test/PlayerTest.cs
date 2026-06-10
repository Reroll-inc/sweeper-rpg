using System.Drawing;
using Moq;
using SweeperRpg.Src;
using Xunit;

namespace Test
{
    public class PlayerTest
    {
        [Fact]
        public void ShouldDieAndNotify()
        {
            Mock<PlayerEventIsDead> isDeadHandler = new();

            isDeadHandler.Setup(d => d());

            Player player = new(x: 1, y: 1);
            player.OnDeath += isDeadHandler.Object;

            Assert.True(player.Hp > 0);

            player.TakeDamage(player.Hp + 1);

            isDeadHandler.Verify(d => d(), Times.Once);
        }

        [Fact]
        public void ShouldNotDieOn0Hp()
        {
            Mock<PlayerEventIsDead> isDeadHandler = new();

            isDeadHandler.Setup(d => d());

            Player player = new(x: 1, y: 1);
            player.OnDeath += isDeadHandler.Object;

            player.TakeDamage(player.Hp);

            isDeadHandler.Verify(d => d(), Times.Never);
        }

        [Fact]
        public void ShouldChangePositionOnMove()
        {
            Player player = new(x: 1, y: 1);

            player.Move(new Point(x: 2, y: 3));

            Assert.Equal(2, player.Position.X);
            Assert.Equal(3, player.Position.Y);

            player.SetStart(x: 5, y: 7);

            Assert.Equal(5, player.Position.X);
            Assert.Equal(7, player.Position.Y);
        }

        private void ShouldNotifyWillToMove()
        {
            Mock<PlayerEventWillMove> willMoveHandler = new();

            willMoveHandler.Setup(d => d(It.IsAny<Point>()));

            Player player = new(x: 1, y: 1);
            player.OnWillMove += willMoveHandler.Object;

            // TODO: create a movement controller to separate the engine
            // input logic from user movement actions.

            willMoveHandler.Verify(d => d(It.IsAny<Point>()), Times.Once);
        }

        [Fact]
        public void ShouldReduceHpAndKillEnemyOnCollide()
        {
            Enemy enemy = new(x: 1, y: 1, EnemyKind.GOBLIN_MAGE);
            Player player = new(x: 1, y: 1);
            int OldHp = player.Hp;

            bool didCollide = player.TryCollide(enemy: enemy);

            Assert.True(didCollide);
            Assert.True(player.Hp < OldHp);
            Assert.False(enemy.IsAlive());
        }
    }
}
