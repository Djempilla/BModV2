using BModv2.AutoFarms;
using BModv2.Client.Farming;
using BModv2.Client.Misc.Commands;
using BModv2.Events;
using BModv2.Farming;
using BModv2.Mining;
using BModv2.Patches.Fishing;
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
        LEGACY_AutoBreak.onTick();
        WorldTickEvent.OnTick();
        
        AutoMove.Tick();
        RandomWorlder.Tick();
        AutoFarm.onTick();

    }
    

}



[HarmonyPatch]
public class PlayerIsGroundedPatch()
{
    
}
