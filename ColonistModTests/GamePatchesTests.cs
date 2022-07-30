using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ColonistMod.Patches;
using TenCrowns.GameCore;

namespace ColonistModTests
{
    public class GamePatchesTests
    {
        [Fact]
        public void Test_RemoveTerritoryAroundColony()
        {
            const int rings = 32;
            var colony = new Mock<Tile>();
            var adjacent = new Mock<Tile>();

            colony.Setup(t => t.getX()).Returns(0);
            colony.Setup(t => t.getY()).Returns(0);
            adjacent.Setup(t => t.setOwner(It.Is<PlayerType>(p => p == PlayerType.NONE), 
                                           It.Is<TribeType>(tt => tt == TribeType.NONE), 
                                           It.Is<int>(i => i == -1), 
                                           It.IsAny<bool>(), 
                                           It.IsAny<bool>()));

            HandleActionPatches.RemoveTerritoryAroundColony(colony.Object, (x, y) => adjacent.Object, rings);

            colony.Verify(t => t.getX(), Times.Once());
            colony.Verify(t => t.getY(), Times.Once());
            adjacent.Verify(t => t.setOwner(It.Is<PlayerType>(p => p == PlayerType.NONE),
                                           It.Is<TribeType>(tt => tt == TribeType.NONE),
                                           It.Is<int>(i => i == -1),
                                           It.IsAny<bool>(),
                                           It.IsAny<bool>()),
                            Times.Exactly(8*rings));
        }
    }
}
