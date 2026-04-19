namespace BModv2.Patches.Impl.Hud;

using System.Collections.Generic;
using UnityEngine;


public static class SimpleTextPanelDraw
{
    public static void Draw(Rect rect, IReadOnlyList<string> lines)
    {
        if (lines == null || lines.Count == 0)
            return;

        GUI.Box(rect, GUIContent.none);

        const float padding = 8f;
        const float lineHeight = 18f;

        float x = rect.x + padding;
        float y = rect.y + padding;
        float width = rect.width - padding * 2f;

        for (int i = 0; i < lines.Count; i++)
        {
            GUI.Label(new Rect(x, y + i * lineHeight, width, lineHeight), lines[i] ?? string.Empty);
        }
    }
}