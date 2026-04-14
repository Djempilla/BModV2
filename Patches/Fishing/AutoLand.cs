using BModv2.Patches.Impl;
using HarmonyLib;

namespace BModv2.Patches.Fishing;

/**
 * Player.isFishStrikeActive - check if fish is ready to catch
 * Player.activateFishStrike - ??? prob fish catch func
 */

// Патч на SetupMinigame — для логики рыбы
[HarmonyPatch(typeof(FishingGaugeMinigameUI), nameof(FishingGaugeMinigameUI.SetupMinigame))]
public static class SetupMinigamePatch
{
    [HarmonyPrefix]
    public static void Prefix(FishingGaugeMinigameUI __instance, ref World.BlockType rod, ref World.BlockType caughtFish)
    {
        if (__instance == null)
        {
            Plugin.Log.LogInfo("[BMod] Fishing Gauge Minigame UI is null");
            return;
        }
        // Сбрасываем флаг при каждом новом запуске мини-игры
        UpdateGameStatusPatch.EndingTriggered = false;

        if (caughtFish.ToString().EndsWith("Huge"))
            Plugin.Log.LogInfo("[BMod] Huge fish! Don't catch");
        else
        {
            Plugin.Log.LogInfo("[BMod] fish: " + caughtFish);
            __instance.ForceFishPause(50.0f, false); // пример: вызвать SetFishPosition сразу после старта мини-игры
        }
        
    }
}

// Патч на UpdateGameStatus — автолэнд когда готово
[HarmonyPatch(typeof(FishingGaugeMinigameUI), nameof(FishingGaugeMinigameUI.UpdateGameStatus))]
public static class UpdateGameStatusPatch
{
    internal static bool EndingTriggered;

    [HarmonyPostfix]
    public static void Postfix(FishingGaugeMinigameUI __instance)
    {
        if (EndingTriggered)
            return;

        if (!__instance.isReadyToLand)
            return;

        EndingTriggered = true;
        Plugin.Log.LogInfo("[BMod] Landing is ready");
        // PlayerHooker.thePlayer.ActivateFishingLine(); - throws rod in water args: 2dVector [x:y]
        // PlayerHooker.thePlayer
        __instance.LandButtonPressed();
        __instance.ExitButtonPressed();
    }
}