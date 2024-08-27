using System;
using System.Collections.Generic;
using HarmonyLib;

namespace Start_with_Fatality_Board.Patches
{
    internal class Board_Patch
    {
        [HarmonyPatch(typeof(StartOfRound))]
        internal class NewSaveStartPatch
        {
            [HarmonyPatch("firstDayAnimation")]
            [HarmonyPostfix]
            internal static void LoadTelevisionFromConfig()
            {
                if (StartOfRound.Instance.gameStats.daysSpent == 0 && !StartOfRound.Instance.isChallengeFile)
                {
                    Start_with_fatality_board_base.LogInfo("New save detected, unlocking Fatalities Sign.");
                }
                else
                {
                    Start_with_fatality_board_base.LogInfo("Not a new save, not unlocking Fatalities Sign.");
                    return;
                }

                // check if the player is a host, if not return
                if (!GameNetworkManager.Instance.isHostingGame) return;

                List<UnlockableItem> unlockablesList = StartOfRound.Instance.unlockablesList.unlockables;

                // Search for Fatalities Sign in unlockables list
                foreach (UnlockableItem unlockable in unlockablesList)
                {
                    var unlockableName = unlockable.unlockableName;
                    var unlockableID = unlockablesList.IndexOf(unlockable);

                    Start_with_fatality_board_base.LogDebug($"Checking unlockableName {unlockableName} and ID {unlockableID}.");

                    var televisionResult = CheckConfig(unlockableID, unlockableName);

                    if (televisionResult) break;
                }
            }

            private static bool CheckConfig(int unlockableID, string unlockableName)
            {
                if (unlockableName.Equals("Fatalities Sign"))
                {
                    Start_with_fatality_board_base.LogInfo($"Found Fatalities Sign with unlockableName {unlockableName} and ID {unlockableID}. Unlocking.");
                    UnlockShipItem(StartOfRound.Instance, unlockableID, unlockableName);
                    return true;
                }
                return false;
            }

            private static void UnlockShipItem(StartOfRound instance, int unlockableID, string name)
            {
                try
                {
                    Start_with_fatality_board_base.LogInfo($"Attempting to unlock {name}");
                    var unlockShipMethod = instance.GetType().GetMethod("UnlockShipObject",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    unlockShipMethod.Invoke(instance, new object[] { unlockableID });
                    Start_with_fatality_board_base.LogInfo($"Spawning {name}");
                }
                catch (NullReferenceException ex)
                {
                    Start_with_fatality_board_base.LogError($"Could not invoke UnlockShipObject method: {ex}");
                }
            }
        }
    }
}
