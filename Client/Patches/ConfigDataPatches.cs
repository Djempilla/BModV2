using BModv2.Client.Farming;
using HarmonyLib;

namespace BModv2.Client.Misc.Commands;

[HarmonyPatch(typeof(ConfigData), "CanPlayerPickCollectableFromBlock")]
public class ConfigDataPatches
{
    [HarmonyPrefix]
    static bool Prefix(ref bool __result)
    {
        if (AutoFarm.isEnabled && AutoMove.IsWalking)
        {
            __result = false;
            return false; 
        }

        return true;
    }
}