using BModv2.Patches.Fishing;
using BModv2.Patches.Impl;
using UnityEngine;

namespace BModv2.Patches.Misc.Commands;

public class AutoFishCommand
{
    public static void Execute()
    {
        AutoFish.isActive = !AutoFish.isActive;
        // Constants.getPlayer().IsPlayingMinigame();
        ConfigData.playerChangeToSleepSeconds = int.MaxValue;
        Plugin.Log?.LogInfo($"AutoFish enabled: {AutoFish.isActive}");
    }
}