using System.Collections.Generic;
using UnityEngine;

namespace BModv2.Client.Impl.Utils;

public static class RenderUtils
{
    private static GameObject? _lineObj;
    private static LineRenderer? _line;

    public static void RenderPath(List<Vector3> worldPoints)
    {
        if (worldPoints == null || worldPoints.Count < 2)
            return;

        EnsureRenderer();

        _line!.positionCount = worldPoints.Count;

        for (int i = 0; i < worldPoints.Count; i++)
        {
            var p = worldPoints[i];
            p.z = 0f;
            _line.SetPosition(i, p);
        }

        _line.enabled = true;
    }

    public static void Clear()
    {
        if (_line != null)
            _line.enabled = false;
    }

    private static void EnsureRenderer()
    {
        if (_lineObj != null && _line != null)
            return;

        _lineObj = new GameObject("BMod_PathRenderer");
        Object.DontDestroyOnLoad(_lineObj);

        _line = _lineObj.AddComponent<LineRenderer>();
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.startColor = Color.red;
        _line.endColor = Color.red;
        _line.startWidth = 0.08f;
        _line.endWidth = 0.08f;
        _line.useWorldSpace = true;
        _line.alignment = LineAlignment.View;
        _line.textureMode = LineTextureMode.Stretch;
        _line.numCapVertices = 4;
        _line.numCornerVertices = 4;
        _line.sortingOrder = 9999;
    }
}