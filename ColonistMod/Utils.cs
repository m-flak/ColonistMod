using System;
using HarmonyLib;

namespace ColonistMod
{
    internal sealed class Utils
    {
        internal static void DbgLog(string message)
        {
            FileLog.Debug(String.Format("{0}: {1}", DateTime.Now, message));
        }

        //Revoked
        private Utils() { }
    }
}
