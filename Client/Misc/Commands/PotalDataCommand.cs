using System;
using System.Collections.Generic;
using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using JetBrains.Annotations;
using Kernys.Bson;
using UnityEngine;

namespace BModv2.Patches.Misc.Commands;

public static class PortalDataCommand
{

    // 0  for entry 1 for target
    public static void Execute(bool teleport, int target)
    {
        if (Camera.main != null)
        {
            Vector2i v = WorldUtils.ConvertWorldPointToMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            IReadOnlyList<string>? data = PortalUtils.GetPortalData(v);

            if (data != null) Plugin.Log.LogInfo($"Portal [{v.x}|{v.y}]: {string.Join(" ", data)}");
            if (teleport)
            {
                switch (target)
                {
                    case 0:
                        SceneLoader.CheckIfWeCanGoFromWorldToWorld(PortalUtils.lastEntryWorld, PortalUtils.lastEntryPortalID, null);
                        break;
                    case 1:
                        SceneLoader.CheckIfWeCanGoFromWorldToWorld(PortalUtils.lastTargetWorld, PortalUtils.lastTargetPortalID, null);
                        break;
                }
            }
        }
    }
        
}