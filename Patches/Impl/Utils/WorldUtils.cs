using BasicTypes;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace BModv2.Patches.Impl.Utils;

public class WorldUtils
{
    public static Vector2i ConvertWorldPointToMapPoint(Vector3 worldPoint)
    {
        Vector2i result;
        result.x = 0;
        result.y = 0;
        result.x = (int)Mathf.Round(worldPoint.x / ConfigData.tileSizeX);
        result.y = (int)((worldPoint.y + ConfigData.tileSizeX / 2f) / ConfigData.tileSizeY);
        return result;
    }
}