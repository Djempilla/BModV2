using BModv2.Patches.Impl;
using HarmonyLib;
using UnityEngine;

namespace BModv2.Patches;

[HarmonyPatch(typeof(FogOfWar), nameof(FogOfWar.UpdateFogOpacity), new[]
{
    typeof(int),
    typeof(int),
    typeof(float)
})]
public class FogOfWarPatch
{
    
    [HarmonyPatch]
    public static void Prefix(FogOfWar __instance, int x, int y, ref float opacityValue)
    {
        opacityValue = 0;
        if (Camera.main != null)
            Plugin.Log.LogInfo($"pos: {Camera.main.ScreenToWorldPoint(Input.mousePosition)}");
    }
}
