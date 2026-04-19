using System;
using BasicTypes;

namespace BModv2.Patches.Impl.Utils;

public class FishingUtils
{

    // todo paste from charon
    public static void handleAutoFishRejoin()
    {
        
    }

    public static bool CastAutoFishLine(PlayerData.InventoryKey[]? inventory)
    {
        if (inventory == null)
        {
            Plugin.Log?.LogInfo("[CastAutoFishLine] Fail: inventory is null");
            return false;
        }
        if (Constants.getPlayer() == null)
        {
            Plugin.Log?.LogInfo("[CastAutoFishLine] Fail: player is null");
            return false;
        }

        var playerPos = Constants.getCurrentPlayerMapPoint();
        var world = Constants.getWorld();

        Vector2i[] fishPos = new[]
        {
            new Vector2i(playerPos.x - 1, playerPos.y - 1),
            new Vector2i(playerPos.x + 1, playerPos.y - 1)
        };

        foreach (Vector2i v in fishPos)
        {
            if (!world.AreMapPointsValidForFishing(playerPos, v.x, v.y))
            {
                Plugin.Log?.LogInfo($"[CastAutoFishLine] Pos ({v.x},{v.y}) invalid for fishing, skipping");
                continue;
            }

            bool foundLure = false;
            foreach (PlayerData.InventoryKey invKey in inventory)
            {
                if (!ConfigData.IsFishingLure(invKey.blockType))
                {
                    Plugin.Log?.LogInfo($"[CastAutoFishLine] Item {invKey.blockType} is not a fishing lure, skipping");
                    continue;
                }
                if (Constants.getPlayerData().GetCount(invKey) <= 0)
                {
                    Plugin.Log?.LogInfo($"[CastAutoFishLine] Lure {invKey.blockType} count is 0, skipping");
                    continue;
                }

                foundLure = true;
                Plugin.Log?.LogInfo($"[CastAutoFishLine] Casting at ({v.x},{v.y}) with lure {invKey.blockType}");
                ControllerHelper.worldController.SetBaitWithTool(invKey.blockType, v, 0f);
                return true;
            }

            if (!foundLure)
                Plugin.Log?.LogInfo($"[CastAutoFishLine] No valid lure found for pos ({v.x},{v.y})");
        }

        Plugin.Log?.LogInfo("[CastAutoFishLine] Fail: no valid position + lure combination found");
        return false;
    }
    
}