using System;
using Moq;
using Xunit;
using TenCrowns.GameCore;
using Mohawk.SystemCore;
using ColonistMod.State;
using TenCrowns.ClientCore;

namespace ColonistModTests
{
    public class ImprovementStateTests
    {
        public ImprovementStateTests()
        {
            ImprovementState.Reset();
        }

        [Fact]
        public void Test_AddOrUpdateColonyOwnerBuilder()
        {
            var munit = new Mock<Unit>();
            var tile = new Mock<Tile>();
            tile.Setup(t => t.getID()).Returns(1337);

            ImprovementState.AddOrUpdateColonyOwnerBuilder(tile.Object, (PlayerType)1, munit.Object);
            var (player, unit) = ImprovementState.GetColonyOwnerBuilder(tile.Object);

            Assert.Equal((PlayerType)1, player);
            Assert.Same(munit.Object, unit);
        }

        [Fact]
        public void Test_GetColonyOwnerBuilder_WhenNotFound()
        {
            var tile = new Mock<Tile>();
            var (player, unit) = ImprovementState.GetColonyOwnerBuilder(tile.Object);

            Assert.Equal(PlayerType.NONE, player);
            Assert.Null(unit);
        }

        [Fact]
        public void Test_GetColonyOwnerBuilder_WhenTileNull()
        {
            var (player, unit) = ImprovementState.GetColonyOwnerBuilder(null);

            Assert.Equal(PlayerType.NONE, player);
            Assert.Null(unit);
        }

        [Fact]
        public void Test_OwnerBuilderAsSerializable_WhenBadUnit()
        {
            var pair = ImprovementState.OwnerBuilderAsSerializable( (PlayerType.NONE, null) );

            Assert.Equal((int)PlayerType.NONE, pair.First);
            Assert.Equal(-1, pair.Second);
        }

        [Fact]
        public void Test_OwnerBuilderFromSerializable_Valid()
        {
            var munit = new Mock<Unit>();

            // The delegate will be used to call Game.unit() since it can't be mocked
            var (player, unit) = ImprovementState.OwnerBuilderFromSerializable(id => munit.Object, new Pair<int, int>(1, 100));

            Assert.Equal((PlayerType)1, player);
            Assert.NotNull(unit);
        }

        [Fact]
        public void Test_OwnerBuilderFromSerializable_Invalid()
        {
            // The delegate will be used to call Game.unit() since it can't be mocked
            var (player, unit) = ImprovementState.OwnerBuilderFromSerializable(id => null, new Pair<int, int>(1, -1));

            Assert.Equal((PlayerType)1, player);
            Assert.Null(unit);
        }
    }
}