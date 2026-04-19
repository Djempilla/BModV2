using HarmonyLib;
using System;

namespace BModv2.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.HitPlayerFromAIEnemy),
    new Type[] { typeof(AIBase), typeof(AIDamageModelType) })]
public class HitPlayerFromAIEnemyPatch
{
    [HarmonyPrefix]
    public static bool Prefix(ref AIBase aiEnemy, ref AIDamageModelType aiDamageModelType)
    {
        return false;
    }
}