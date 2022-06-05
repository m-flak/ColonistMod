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

        public static bool Prefix(ref Unit __instance, ref bool __result, ImprovementType eImprovement)
        {
            if (improvementEType == -1)
            {
                // The xType enums are indices in each of the Infos lists, ex: maImprovements[eImprovement]
                Infos info = (Infos) AccessTools.Method(typeof(Unit), "infos").Invoke(__instance, null);
                improvementEType = info.improvements().FindIndex(improvement => improvement.mzType == Constants.ImprovementTypeColony);
            }

            // Only show the colony as buildable for colonists
            // Also don't allow them to build shrines n stuff
            string unitType = __instance.info().mzType;
            if (eImprovement == (ImprovementType) improvementEType)
            {
                __result = (unitType == Constants.UnitTypeColonist);
                return false;
            }
            else if (unitType == Constants.UnitTypeColonist && eImprovement != (ImprovementType) improvementEType)
            {
                // This only seems to disable the options in the UI. :/
               __result = false;
                return false;
            }

            return true;
        }
    }
}
