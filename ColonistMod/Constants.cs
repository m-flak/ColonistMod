using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColonistMod
{
    internal sealed class Constants
    {
        internal const string UnitTypeColonist = "UNIT_COLONIST";
        internal const string ImprovementTypeColony = "IMPROVEMENT_COLONY";
        internal const string ImprovementClassColony = "IMPROVEMENTCLASS_COLONY";

        internal const int ActionTypeDelta_ColonyBuilt = 1;
        internal const int ActionTypeDelta_ColonyDestroyed = 2;

        //Revoked
        private Constants() { }
    }
}
