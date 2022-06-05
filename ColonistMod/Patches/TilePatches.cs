using System;
using TenCrowns.GameCore;
using HarmonyLib;


namespace ColonistMod.Patches
{
    [HarmonyPatch(typeof(TenCrowns.GameCore.Tile), nameof(TenCrowns.GameCore.Tile.doImprovementFinished))]
    public class DoImprovementFinishedPatches
    {
        ///<summary>Stores the Colony improvement class's ID</summary>
        ///<see cref="M:TenCrowns.GameCore.Infos.improvementClass(TenCrowns.GameCore.ImprovementClassType)"/>
        private static int improvementClassEType = -1;

        public static void Postfix(ref Tile __instance)
        {
            if (improvementClassEType == -1)
            {
                improvementClassEType = __instance.infos().improvementClasses().FindIndex(impClass => impClass.mzType == Constants.ImprovementClassColony);
            }

            ImprovementClassType tilesImpClassType = __instance.getImprovementClass();
            if (tilesImpClassType == (ImprovementClassType) improvementClassEType)
            {
                Utils.DbgLog("A COLONY WAS JUST BUILT!");
            }
        }
    }

    [HarmonyPatch(typeof(TenCrowns.GameCore.Tile), nameof(TenCrowns.GameCore.Tile.clearImprovement))]
    public class ClearImprovementPatches
    {
        ///<summary>Stores the Colony improvement class's ID</summary>
        ///<see cref="M:TenCrowns.GameCore.Infos.improvementClass(TenCrowns.GameCore.ImprovementClassType)"/>
        private static int improvementClassEType = -1;

        public struct State
        {
            public bool WasColony;
        }

        public static void Prefix(ref Tile __instance, out State __state)
        {
            __state = new State();

            if (improvementClassEType == -1)
            {
                improvementClassEType = __instance.infos().improvementClasses().FindIndex(impClass => impClass.mzType == Constants.ImprovementClassColony);
            }

            ImprovementClassType tilesImpClassType = __instance.getImprovementClass();
            __state.WasColony = ( tilesImpClassType == (ImprovementClassType)improvementClassEType );
        }

        public static void Postfix(State __state)
        {
            if (__state.WasColony)
            {
                Utils.DbgLog("A COLONY WAS JUST DESTROYED!");
            }
        }
    }
}
