﻿#nullable enable
using System.Reflection;
using TenCrowns.AppCore;
using TenCrowns.GameCore;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

using ColonistMod.State;

namespace ColonistMod
{
    public class ColonistMod : ModEntryPointAdapter
    {
        private ModSettings? Owrld = default;

        private static bool harmonyLoaded = false;

        public static readonly Harmony harmony = new Harmony("org.matthew.colonistmod");

        public ColonistMod()
            : base()
        {
            Harmony.DEBUG = true;
        }

        public override void Initialize(ModSettings modSettings)
        {
            base.Initialize(modSettings);
            this.Owrld = modSettings;
            modSettings.Factory = new ModGameFactory();

            if (!harmonyLoaded)
            {
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
                harmonyLoaded = true;
                Utils.DbgLog("Harmony Patches Applied");
            }

            Utils.DbgLog("MOD INITIALIZED");
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            ImprovementState.Reset();
        }

        public override void OnExitGame()
        {
            base.OnGameOver();

            ImprovementState.Reset();
        }
    }
}
