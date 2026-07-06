using System.Drawing;
using EngineGDI.Src.Events;
using Moq;
using SweeperRpg.Src;
using Xunit;

namespace Test
{
    public class PlayerTest
    {
        private static readonly EventBus bus = new();

        [Fact]
        public void ShouldDieAndNotify()
        {
            Mock<Action<PlayerDiedEvent>> PlayerDiedHandler = new();

            bus.Subscribe(PlayerDiedHandler.Object);

            Player player = new(x: 1, y: 1, bus: bus);

            Assert.True(player.Hp > 0);

            player.TakeDamage(player.Hp + 1);

            PlayerDiedHandler.Verify(handler => handler(It.IsAny<PlayerDiedEvent>()), Times.Once);

            bus.Unsubscribe(PlayerDiedHandler.Object);
        }

        [Fact]
        public void ShouldNotDieOn0Hp()
        {
            Mock<Action<PlayerDiedEvent>> PlayerDiedHandler = new();

            bus.Subscribe(PlayerDiedHandler.Object);

            Player player = new(x: 1, y: 1, bus: bus);

            player.TakeDamage(player.Hp);

            PlayerDiedHandler.Verify(handler => handler(It.IsAny<PlayerDiedEvent>()), Times.Never);

            bus.Unsubscribe(PlayerDiedHandler.Object);
        }

        [Fact]
        public void ShouldChangePositionOnMove()
        {
            Player player = new(x: 1, y: 1, bus: bus);

            player.Move(new Point(x: 2, y: 3));

            Assert.Equal(2, player.Transform.Position.X);
            Assert.Equal(3, player.Transform.Position.Y);

            player.SetStart(x: 5, y: 7);

            Assert.Equal(5, player.Transform.Position.X);
            Assert.Equal(7, player.Transform.Position.Y);
        }

        private void ShouldNotifyWillToMove()
        {
            Mock<Action<PlayerWantToMoveEvent>> PlayerWantToMoveHandler = new();

            bus.Subscribe(PlayerWantToMoveHandler.Object);

            Player player = new(x: 1, y: 1, bus: bus);

            // TODO: create a movement controller to separate the engine
            // input logic from user movement actions.

            PlayerWantToMoveHandler.Verify(
                handler => handler(It.IsAny<PlayerWantToMoveEvent>()),
                Times.Once
            );

            bus.Unsubscribe(PlayerWantToMoveHandler.Object);
        }

        [Fact]
        public void ShouldReduceHpAndKillEnemyOnCollide()
        {
            Enemy enemy = new(x: 1, y: 1, kind: EnemyKind.GOBLIN_MAGE, bus: bus);
            Player player = new(x: 1, y: 1, bus: bus);
            int OldHp = player.Hp;

            bool didCollide = player.TryCollide(enemy: enemy);

            Assert.True(didCollide);
            Assert.True(player.Hp < OldHp);
            Assert.False(enemy.IsAlive());
        }
    }
}
