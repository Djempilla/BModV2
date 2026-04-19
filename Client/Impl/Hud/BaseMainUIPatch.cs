using HarmonyLib;

namespace BModv2.Patches.Impl.Hud;

[HarmonyPatch(typeof(BaseMenuUI), "Update")]
public class BaseMainUIPatch
{
    public static void Postfix(BaseMenuUI __instance)
    {
        // Plugin.Log.LogInfo($"New ui: {__instance.GetInstanceID()} {__instance.name}");
        
    }
}