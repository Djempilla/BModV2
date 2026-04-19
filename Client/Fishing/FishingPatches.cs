using System;
using BModv2.Patches.Impl;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BModv2.Patches.Fishing;


// Used to check if fish currenly fishing is huge or not, to prevent insta-ban
[HarmonyPatch(typeof(FishingGaugeMinigameUI), nameof(FishingGaugeMinigameUI.SetupMinigame))]
public static class SetupMinigamePatch
{
    [HarmonyPrefix]
    public static void Prefix(FishingGaugeMinigameUI __instance, ref World.BlockType rod, ref World.BlockType caughtFish)
    {
        Plugin.Log?.LogInfo($"[BMod] Fish type {caughtFish.ToString()}");
        if (caughtFish.ToString().EndsWith("Huge"))
            StartMiniGamePatch.isHuge = true;
    }
}


// auto claim fish
// [HarmonyPatch(typeof(FishingResultsPopupUI), nameof(FishingResultsPopupUI.UpdateSpriteIconAndItsDimensions))]
// public static class FishingResultsPopupUIReadyPatch
// {
//     [HarmonyPostfix]
//     public static void Postfix(FishingResultsPopupUI __instance)
//     {
//         __instance.TakeFishPressed();
//         __instance.ClosePopup();
//     }
// }


// auto land fish
[HarmonyPatch(typeof(FishingGaugeMinigameUI), nameof(FishingGaugeMinigameUI.UpdateGameStatus))]
public static class UpdateGameStatusPatch
{
    [HarmonyPostfix]
    public static void Postfix(FishingGaugeMinigameUI __instance)
    {
        if (__instance.isReadyToLand)
            __instance.LandButtonPressed();
        
        // Plugin.Log.LogInfo("Target pos: " + __instance.targetAreaPosition);
        // Plugin.Log.LogInfo("Fish velocity: " + __instance.fishVelocity);
        // Plugin.Log.LogInfo("Fish position: " + __instance.fishPosition);
        // Plugin.Log.LogInfo("Delta time: " + Time.deltaTime);
        
        
        __instance.targetAreaPosition = __instance.fishPosition + __instance.fishVelocity * Time.deltaTime;
    }
}

// auto stop fish, except HUGE
[HarmonyPatch(typeof(FishingGaugeMinigameUI), nameof(FishingGaugeMinigameUI.StartMiniGame))]
public static class StartMiniGamePatch
{

    public static bool isHuge = false;
    
    [HarmonyPostfix]
    public static void Postfix(FishingGaugeMinigameUI __instance)
    { 
        // if(!isHuge)
        //     __instance.ForceFishPause(50.0f, false); // пример: вызвать SetFishPosition сразу после старта мини-игры
    }
}