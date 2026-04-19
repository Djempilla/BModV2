using BModv2.Farming;
using BModv2.Patches.Fishing;
using BModv2.Patches.Impl;
using BModv2.Patches.Misc.Commands;
using HarmonyLib;

namespace BModv2.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.Update))]
public class PlayerPatch
{
    private static int ticksPassed = 0;

    // Called every frame only when IN THE WORLD
    [HarmonyPrefix]
    public static void Prefix(Player __instance)
    {
        AutoFish.onTick();
        AutoBreak.onTick();
        // ticksPassed++;
        // if (ticksPassed % 100 == 0)
        // {
        
        // } 
    }
}
