using System;
using System.Collections.Generic;
using BasicTypes;
using Kernys.Bson;

namespace BModv2.Patches.Impl.Utils;

public class PortalUtils
{
    public static IReadOnlyList<string>? GetPortalData(Vector2i pos)
    {
        try
        {
            WorldItemBase? itemData = null;
            BSONObject? asBSON = null;

            // all down under try-catch case
            itemData = Constants.getWorld().GetWorldItemData(pos);
            if (itemData == null)
            {
                // Plugin.Log.LogWarning("[Portal] itemData is null");
                return new[] { "No item data" };
            }

            asBSON = itemData.GetAsBSON();
            if (asBSON == null)
            {
                // Plugin.Log.LogWarning("[Portal] asBSON is null");
                return new[] { "Bson is null" };
            }

            if (!asBSON.ContainsKey("class"))
            {
                // Plugin.Log.LogWarning("[Portal] no 'class' key in BSON");
                return null;
            }

            // string dd = BSONUtils.Dump(asBSON);
            // Plugin.Log.LogInfo(dd);
            
            if (asBSON["class"].stringValue != "PortalData")
            {
                // Plugin.Log.LogInfo($"[Portal] class = {asBSON["class"].stringValue}, not PortalData");
                return new[] {"Not a portal -> " +  asBSON["class"].stringValue};
            }

            string itemBlockType = asBSON["blockType"].stringValue;
            string name = asBSON["name"].stringValue;
            string entryWorld = "W: " + Constants.getWorld().worldName + " ";
            string entryPoint = asBSON["entryPointID"].stringValue;
            string targetWorld = asBSON["targetWorldID"].stringValue;
            string targetEntryPoint = asBSON["targetEntryPointID"].stringValue;
            string isLocked = asBSON["isLocked"].stringValue;

            return new[]
            {
                $"blockType {itemBlockType}",
                $"name {name}",
                $"entryWorld {entryWorld}",
                $"entryPoint {entryPoint}",
                $"targetWorld {targetWorld}",
                $"targetEntryPoint {targetEntryPoint}",
                $"isLocked {isLocked}",
            };
        }
        catch(Exception e)
        {
            Plugin.Log.LogError("[PortalUtils] " + e.Message);
            return null;
        }
    } 
}