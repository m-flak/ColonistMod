using Xunit;
using TenCrowns.GameCore;
using ColonistMod.State;

namespace ColonistModTests
{
    public class ImprovementStateTests
    {
        [Fact]
        public void Test_GetColonyOwnerBuilder_WhenNotFound()
        {
            var tile = new Tile();
            var (player, unit) = ImprovementState.GetColonyOwnerBuilder(tile);

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
    }
}