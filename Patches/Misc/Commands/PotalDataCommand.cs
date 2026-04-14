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

    public static void Execute()
    {
        if (Camera.main != null)
        {
            Vector2i v = WorldUtils.ConvertWorldPointToMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            IReadOnlyList<string>? data = PortalUtils.GetPortalData(v);

            if (data != null) Plugin.Log.LogInfo($"Portal [{v.x}|{v.y}]: {string.Join(" ", data)}");
        }
    }
        
}