using TenCrowns.AppCore;
using TenCrowns.GameCore;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

namespace ColonistMod.Info
{
    internal class InfosListener : IInfosListener
    {
        private ModSettings Mod
        {
            get;
            set;
        }

        public InfosListener(ModSettings modSettings)
        {
            Mod = modSettings;
        }

        public void OnInfosLoaded(Infos infos)
        {
            Utils.DbgLog("INFOS LOADED");
        }
    }
}
