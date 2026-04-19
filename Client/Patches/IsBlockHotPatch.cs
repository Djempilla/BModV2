using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace BModv2.Patches;

[HarmonyPatch(typeof(ConfigData), nameof(ConfigData.isBlockHot), MethodType.Getter)]
public class IsBlockHotGetterPatch
{
    [HarmonyPrefix]
    public static bool Prefix(ref Il2CppStructArray<bool> __result)
    {
        __result = null;
        return false;
    }
}