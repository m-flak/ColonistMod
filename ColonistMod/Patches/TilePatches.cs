using System;
using System.Linq;
using System.Collections.Generic;
using TenCrowns.GameCore;
using HarmonyLib;
using ColonistMod.State;

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
            if (tilesImpClassType == (ImprovementClassType)improvementClassEType)
            {
                Utils.DbgLog("in doImprovementFinished after the colony was built");
                var ownerBuilder = ImprovementState.GetColonyOwnerBuilder(__instance);
                if (ownerBuilder.Item1 != PlayerType.NONE)
                {
                    ActionType built = (ActionType)Enum.GetValues(typeof(ActionType)).Cast<int>().Max() + Constants.ActionTypeDelta_ColonyBuilt;
                    Utils.DbgLog(String.Format("Sent {0}", built));
                    ActionData action = new ActionData(built, ownerBuilder.Item1);

                    // must be serializable
                    // replicate improvementstate thru here 
                    action.addValue(ImprovementState.OwnerBuilderAsSerializable(ownerBuilder));
                    action.addValue(__instance.getID());
                    ModGameFactory.CurrentClientManager?.sendAction(action);
                }
            }
        }
    }

    [HarmonyPatch(typeof(TenCrowns.GameCore.Tile), nameof(TenCrowns.GameCore.Tile.setImprovement))]
    public static class SetImprovementPatches
    {
        ///<summary>Stores the Colony improvement's ID</summary>
        ///<see cref="M:TenCrowns.GameCore.Infos.improvement(TenCrowns.GameCore.ImprovementType)"/>
        private static int improvementEType = -1;

        public static void Prefix(ref Tile __instance, ImprovementType eNewImprovement, PlayerType ePlayer, Unit pUnit)
        {
            Utils.DbgLog("In setImprovement");

            if (improvementEType == -1)
            {
                improvementEType = __instance.infos().improvements().FindIndex(improvement => improvement.mzType == Constants.ImprovementTypeColony);
            }

            // True when invoked from Unit.buildImprovement
            if (eNewImprovement == (ImprovementType)improvementEType && ePlayer != PlayerType.NONE && pUnit != null)
            {
                Utils.DbgLog("True when invoked from Unit.buildImprovement");
                ImprovementState.AddOrUpdateColonyOwnerBuilder(__instance, ePlayer, pUnit);
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

        public static void Postfix(ref Tile __instance, State __state)
        {
            if (__state.WasColony)
            {
                var (buildingPlayer, _) = ImprovementState.GetColonyOwnerBuilder(__instance);
                if (buildingPlayer != PlayerType.NONE)
                {
                    ActionType destroyed = (ActionType)Enum.GetValues(typeof(ActionType)).Cast<int>().Max() + Constants.ActionTypeDelta_ColonyDestroyed;
                    Utils.DbgLog(String.Format("Sent {0}", destroyed));
                    ActionData action = new ActionData(destroyed, buildingPlayer);

                    // must be serializable
                    action.addValue(__instance.getID());
                    ModGameFactory.CurrentClientManager?.sendAction(action);
                }
            }
        }
    }
}
