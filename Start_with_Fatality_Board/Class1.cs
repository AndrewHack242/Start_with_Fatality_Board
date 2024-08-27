using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Start_with_Fatality_Board
{
    [BepInPlugin(mod_GUID, mod_name, mod_version)]
    public class Start_with_fatality_board_base : BaseUnityPlugin
    {
        private const string mod_GUID = "Hackattack242.Start_with_Fatality_Board";
        private const string mod_name = "Start_with_Fatality_Board";
        private const string mod_version = "1.0.0";

        private readonly Harmony harmony = new Harmony(mod_GUID);

        private static Start_with_fatality_board_base Instance;

        private ManualLogSource logger;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            logger = BepInEx.Logging.Logger.CreateLogSource(mod_name);

            logger.LogInfo(mod_name + " has awoken");

            harmony.PatchAll();

            logger.LogInfo("Patches Complete");
        }

        internal static void LogDebug(string message)
        {
            Instance.Log(message, (LogLevel)32);
        }

        internal static void LogInfo(string message)
        {
            Instance.Log(message, (LogLevel)16);
        }

        internal static void LogWarning(string message)
        {
            Instance.Log(message, (LogLevel)4);
        }

        internal static void LogError(string message)
        {
            Instance.Log(message, (LogLevel)2);
        }

        internal static void LogError(Exception ex)
        {
            Instance.Log(ex.Message + "\n" + ex.StackTrace, (LogLevel)2);
        }

        private void Log(string message, LogLevel logLevel)
        {
            ((Start_with_fatality_board_base)this).logger.Log(logLevel, (object)message);
        }

    }
}
