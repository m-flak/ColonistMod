using System;
using TenCrowns.GameCore;
using HarmonyLib;


namespace ColonistMod.Patches
{
    [HarmonyPatch(typeof(TenCrowns.GameCore.Unit), nameof(TenCrowns.GameCore.Unit.canBuildImprovementType))]
    public class CanBuildImprovementTypePatches
    {
        ///<summary>Stores the Colony improvement's ID</summary>
        ///<see cref="M:TenCrowns.GameCore.Infos.improvement(TenCrowns.GameCore.ImprovementType)"/>
        private static int improvementEType = -1;

        public struct State
        {
            public bool UnitIsColonist;
            public bool ImprovementIsColony;
        }

        public static void Prefix(ref Unit __instance, out State __state, ImprovementType eImprovement)
        {
            if (improvementEType == -1)
            {
                // The xType enums are indices in each of the Infos lists, ex: maImprovements[eImprovement]
                Infos info = (Infos) AccessTools.Method(typeof(Unit), "infos").Invoke(__instance, null);
                improvementEType = info.improvements().FindIndex(improvement => improvement.mzType == Constants.ImprovementTypeColony);
            }

            string unitType = __instance.info().mzType;
            __state = new State();
            __state.UnitIsColonist = ( unitType == Constants.UnitTypeColonist );
            __state.ImprovementIsColony = ( eImprovement == (ImprovementType) improvementEType );
        }

        public static void Postfix(ref bool __result, State __state)
        {
            if (__state.UnitIsColonist && __state.ImprovementIsColony)
            {
                // Only show the colony as buildable for colonists
                __result = true;
            }
            else if (__state.UnitIsColonist && !__state.ImprovementIsColony)
            {
                // Colonists should only be able to build colonies.
                // This only seems to disable the options in the UI. :/
                __result = false;
            }
            else if (__state.ImprovementIsColony)
            {
                // Regular workers shouldn't be able to build colonies.
                // This only seems to disable the options in the UI. :/
                __result = false;
            }
        }
    }
}
