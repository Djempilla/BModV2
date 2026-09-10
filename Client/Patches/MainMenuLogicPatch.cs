using BModv2.AutoFarms;
using BModv2.Client.Farming;
using BModv2.Farming;
using HarmonyLib;

namespace BModv2.Patches;

[HarmonyPatch(nameof(MainMenuLogic), nameof(MainMenuLogic.Update))]
public class MainMenuLogicPatch
{
    public static void Postfix(MainMenuLogic __instance)
    {
        LEGACY_AutoBreak.onTick();
        RandomWorlder.Tick();
        AutoFarm.onTick();
    }
}