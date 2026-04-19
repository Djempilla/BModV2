using HarmonyLib;

namespace BModv2.Patches;

// [HarmonyPatch(typeof(DoorData), nameof(DoorData.GetIsLocked))]
// public class RandomTestPatch
// {
//     [HarmonyPostfix]
//     public static void Postfix(ref bool __result)
//     {
//         __result = false;
//     }
// }
//
//
[HarmonyPatch(typeof(WorldItemBase), nameof(WorldItemBase.SetAnimationOn))]
public class RandomTestPatch2
{
    [HarmonyPostfix]
    public static void Prefix(WorldItemBase __instance)
    {
        Plugin.Log?.LogInfo("Animation called for ID:" + __instance.itemId + " ClassKey: " + __instance.ToString());
    }
}