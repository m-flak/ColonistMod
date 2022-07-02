using System;
using System.Linq;
using System.Collections.Generic;
using TenCrowns.GameCore;
using HarmonyLib;
using ColonistMod.State;

namespace ColonistMod.Patches
{
    [HarmonyPatch(typeof(TenCrowns.GameCore.Game), "handleAction", new Type[] { typeof(ActionData), typeof(int) })]
    public static class HandleActionPatches
    {
        private static Game gameClass = null;

        public static void Prefix(ref Game __instance)
        {
            gameClass = __instance;
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(
                    AccessTools.Method(typeof(ActionData), nameof(ActionData.getAction)),
                    AccessTools.Method(typeof(HandleActionPatches), nameof(getAction))
                );
        }

        public static ActionType getAction(this ActionData @this)
        {
            var actionValue = @this.getAction();

            Utils.DbgLog(String.Format("Receiving {0}", actionValue));

            ActionType built = (ActionType)Enum.GetValues(typeof(ActionType)).Cast<int>().Max() + Constants.ActionTypeDelta_ColonyBuilt;
            ActionType destroyed = (ActionType)Enum.GetValues(typeof(ActionType)).Cast<int>().Max() + Constants.ActionTypeDelta_ColonyDestroyed;

            if (actionValue == built)
            {
                Utils.DbgLog("A COLONY WAS JUST BUILT!");
                // TODO?: I think actiondata will need to be the dict from improvementstate for MP
                var (player, builder) = ImprovementState.GetColonyOwnerBuilder(gameClass?.tile((int)@this.getValue(0)));
                Utils.DbgLog(String.Format("PLAYER WHO BUILT IT: {0}", player));
                Utils.DbgLog(String.Format("UNIT THAT BUILT IT: {0}", builder));
            }
            else if (actionValue == destroyed)
            {
                var (player, _) = ImprovementState.GetColonyOwnerBuilder(gameClass?.tile((int)@this.getValue(0)));
                Utils.DbgLog("A COLONY WAS JUST DESTROYED!");
                Utils.DbgLog(String.Format("PLAYER WHO LOST IT: {0}", player));
            }

            return @this.getAction();
        }
    }
}
