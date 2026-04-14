using HarmonyLib;

namespace BModv2.Patches.Fishing;

[HarmonyPatch(typeof(FishingGaugeMinigameUI), nameof(FishingGaugeMinigameUI.StartMiniGame))]
public static class StartMiniGamePatch
{
    [HarmonyPostfix]
    public static void Postfix(FishingGaugeMinigameUI __instance)
    {
        __instance.ForceFishPause(50.0f, false);
    }
}