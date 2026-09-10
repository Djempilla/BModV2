using System.Collections.Generic;
using BasicTypes;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Hud;
using BModv2.Patches.Impl.Utils;
using HarmonyLib;
using UnityEngine;

namespace BModv2.Patches.Gui;

[HarmonyPatch(typeof(GameplayUI), "Update")]
public static class GameplayUIUpdatePatch
{
    private const float CameraSpeed = 10f;

    static void Postfix(GameplayUI __instance)
    {
        // 1) Движение камеры по стрелкам
        if(!Constants.IsFreecamEnabled) return;
        MoveCameraWithArrows();
    }

    private static void MoveCameraWithArrows()
    {
        var cam = Camera.main;
        if (cam == null)
            return;

        Vector3 pos = cam.transform.position;
        Vector3 delta = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            delta += Vector3.up;        // или Vector3.forward, если камера в 3D
        if (Input.GetKey(KeyCode.DownArrow))
            delta += Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow))
            delta += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            delta += Vector3.right;

        if (delta.sqrMagnitude > 0f)
        {
            delta.Normalize();
            pos += delta * CameraSpeed * Time.deltaTime;
            cam.transform.position = pos;
        }
    }
}