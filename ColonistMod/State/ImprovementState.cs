using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenCrowns.GameCore;

namespace ColonistMod.State
{
    public static class ImprovementState
    {
        // Keys are the Tile ID
        private static Dictionary<int, (PlayerType, Unit)> colonyOwnerBuilder = new Dictionary<int, (PlayerType, Unit)>();

        public static void Reset()
        {
            colonyOwnerBuilder.Clear();
        }

        public static void AddOrUpdateColonyOwnerBuilder(Tile colony, PlayerType ownerId, Unit builderUnit)
        {
            colonyOwnerBuilder[colony.getID()] = (ownerId, builderUnit);
        }

        public static (PlayerType, Unit) GetColonyOwnerBuilder(Tile colony)
        {
            (PlayerType, Unit) ownerBuilder;

            if (colony == null || !colonyOwnerBuilder.TryGetValue(colony.getID(), out ownerBuilder))
            {
                ownerBuilder.Item1 = PlayerType.NONE;
                ownerBuilder.Item2 = null;
            }

            return ownerBuilder;
        }

        public static void RemoveColonyOwnerBuilder(Tile colony)
        {
            colonyOwnerBuilder.Remove(colony.getID());
        }
    }
}
