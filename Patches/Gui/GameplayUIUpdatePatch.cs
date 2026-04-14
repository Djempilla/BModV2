using System.Collections.Generic;
using BasicTypes;
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
        MoveCameraWithArrows();

        // 2) Твой существующий код HUD
        Vector2i v = ControllerHelper.freeSpaceController.currentPlayerMapPoint;
        IReadOnlyList<string>? data = PortalUtils.GetPortalData(v);

        if (data == null || data.Count == 0)
        {
            HudState.Lines = null;
            return;
        }

        Vector3 mouse = Input.mousePosition;

        const float width = 320f;
        float height = 16f + data.Count * 18f;

        float x = mouse.x + 16f;
        float y = Screen.height - mouse.y + 16f;

        if (x + width > Screen.width)
            x = Screen.width - width - 10f;

        if (y + height > Screen.height)
            y = Screen.height - height - 10f;

        HudState.Rect = new Rect(x, y, width, height);
        HudState.Lines = data;
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