using System;
using System.Runtime.CompilerServices;
using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using Object = UnityEngine.Object;

namespace BModv2.Patches.Fishing;

public class AutoFish
{

    public static bool isActive = false;
    private static int ticksPassed = 0;
    
    
    [Obsolete("Obsolete")]
    public static void onTick()
    {
        if(!isActive) return;

        // auto strike
        if (Constants.getPlayer().IsFishStrikeActive())
        {
            ControllerHelper.gameplayUI.OnButtonDownNew(4);
        }
        
        FishingResultsPopupUI fishReslt =  Object.FindObjectOfType<FishingResultsPopupUI>();
        if(fishReslt != null)
            fishReslt.ClosePopup();

        if (Constants.getPlayer().IsFishing() || Constants.getPlayer().IsFishingLineActive() ||
            Constants.getPlayer().IsFishStrikeActive())
        {
            ticksPassed = 0;
            return;
        }
        
        // delay between casting rod
        ticksPassed++;
        if(ticksPassed < 60) return;
        ticksPassed = 0;
        
        bool attempt = FishingUtils.CastAutoFishLine(Constants.getInventory());

        if (!attempt)
        {
            Plugin.Log?.LogInfo("Failed to cast auto fish. Disabling!");
            isActive = false;
        }
    }
}