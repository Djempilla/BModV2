using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace BModv2;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log = null!;
    private Harmony? _harmony;
    private static GameObject? _hudObject;

    public override void Load()
    {
        Log = base.Log;

        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
        
        // ControllerHelper.rootUI.RemoveWorldLighting();
        ConfigData.playerChangeToSleepSeconds = int.MaxValue;
        Log.LogInfo("BModv2 loaded");
    }
}