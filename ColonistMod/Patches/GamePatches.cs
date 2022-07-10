using System;
using System.Linq;
using System.Collections.Generic;
using TenCrowns.GameCore;
using HarmonyLib;
using ColonistMod.State;
using Mohawk.SystemCore;

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

                var (player, builder) = ImprovementState.OwnerBuilderFromSerializable(id => gameClass?.unit(id), (Pair<int, int>)@this.getValue(0));
                Tile colonyTile = gameClass?.tile((int)@this.getValue(1));

                Utils.DbgLog(String.Format("PLAYER WHO BUILT IT: {0}", player));
                Utils.DbgLog(String.Format("UNIT THAT BUILT IT: {0}", builder));

                // Consume the builder
                builder?.kill();
                // Give the colony its territory
                if (colonyTile != null)
                {
                    AddToNearestCity(colonyTile, player);
                }

                // Update, disassociate builder
                ImprovementState.AddOrUpdateColonyOwnerBuilder(colonyTile, player, null);
            }
            else if (actionValue == destroyed)
            {
                Tile colonyTile = gameClass?.tile((int)@this.getValue(0));
                var (player, _) = ImprovementState.GetColonyOwnerBuilder(colonyTile);

                Utils.DbgLog("A COLONY WAS JUST DESTROYED!");
                Utils.DbgLog(String.Format("PLAYER WHO LOST IT: {0}", player));

                // Remove the territory & clear from state
                colonyTile.setOwner(PlayerType.NONE, TribeType.NONE, -1);
                ImprovementState.RemoveColonyOwnerBuilder(colonyTile);
            }

            return @this.getAction();
        }

        private static void AddToNearestCity(Tile tile, PlayerType player)
        {
            List<int> nearestCities = new List<int>();

            try
            {
                AccessTools.Method(typeof(Player), "getCitiesByDistance", new Type[] { typeof(Tile), typeof(List<int>), typeof(Predicate<City>) })
                    .Invoke(GetPlayerFromId(player), new object[] { tile, nearestCities, null });

                tile.setOwner(player, TribeType.NONE, nearestCities[0]);
            }
            catch (Exception e)
            {
                Utils.DbgLog(String.Format("UNABLE TO ASSIGN TILE {0} TO NEAREST CITY.\n{1}", tile, e));
            }
        }

        private static Player GetPlayerFromId(PlayerType playerType)
        {
            Player[] players = gameClass?.getPlayers() ?? new Player[]{};
            Player playa = null;

            for (int i = 0; i < players.Length; ++i)
            {
                if (players[i].getPlayer() == playerType)
                {
                    playa = players[i];
                    break;
                }
            }

            return playa;
        }
    }

    //[HarmonyPatch(typeof(TenCrowns.GameCore.Game), "handleUpdate", new Type[] { typeof(UpdateData)})]
    //public static class HandleUpdatePatches
    //{
    //    [HarmonyTranspiler]
    //    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    //    {
    //        return instructions.MethodReplacer(
    //                AccessTools.Method(typeof(UpdateData), nameof(UpdateData.getUpdate)),
    //                AccessTools.Method(typeof(HandleUpdatePatches), nameof(getUpdate))
    //            );
    //    }

    //    public static UpdateType getUpdate(this UpdateData @this)
    //    {

    //        return @this.getUpdate();
    //    }
    //}
}
