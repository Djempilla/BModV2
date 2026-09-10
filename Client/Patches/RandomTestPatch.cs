using System.Collections.Generic;
using Cpp2IL.Core.Extensions;
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

[HarmonyPatch(typeof(ConfigData), nameof(ConfigData.IsBlockTrap))]
public class RandomTestPatch3
{
    [HarmonyPostfix]
    public static bool Prefix(World.BlockType blockType)
    {
        return false;
    }
}

[HarmonyPatch(typeof(SignWorldMessages), "Populate")]
public static class PatchSignPopulate
{
    [HarmonyPrefix]
    public static bool Prefix()
    {
        using (IEnumerator<SignWorldMessages.SignMessage> enumerator = SignWorldMessages.messages.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                SignWorldMessages.SignMessage signMessage = enumerator.Current;

                if (signMessage != null &&
                    (signMessage.bubbleText.text.Contains("<size=-") ||
                     signMessage.signBubble.textBubbleText.text.Contains("<size=-")))
                {
                    return false;
                }
            }

            return true;
        }
    }
}