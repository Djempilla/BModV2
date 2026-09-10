using System;
using System.Numerics;
using BModv2;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;

public static class AutoMove
{
    private static Vector2[] _path = Array.Empty<Vector2>();
    private static int _pathIndex = 0;

    private static int _moveCooldownFrames = 6;
    private static int _cooldownLeft = 0;

    private static bool _wasWalkingLastTick = false;

    public static bool IsWalking => _path != null && _pathIndex < _path.Length;

    public static int MoveCooldownFrames
    {
        get => _moveCooldownFrames;
        set
        {
            int newValue = Math.Max(1, value);

            if (_moveCooldownFrames != newValue)
            {
                _moveCooldownFrames = newValue;
                Plugin.Log.LogInfo($"[AutoMove] MoveCooldownFrames set to {_moveCooldownFrames}");
            }
        }
    }

    public static void SetPath(Vector2[] path)
    {
        _path = path ?? Array.Empty<Vector2>();
        _pathIndex = 0;
        _cooldownLeft = 0;

        SkipCurrentPointIfNeeded();

        if (_path.Length == 0 || _pathIndex >= _path.Length)
        {
            Plugin.Log.LogInfo("[AutoMove] Received empty or already completed path");
            return;
        }

        Vector2 start = _path[_pathIndex];
        Vector2 end = _path[_path.Length - 1];

        Plugin.Log.LogInfo(
            $"[AutoMove] New path set. Length={_path.Length}, Start=({(int)Math.Round(start.X)},{(int)Math.Round(start.Y)}), End=({(int)Math.Round(end.X)},{(int)Math.Round(end.Y)})"
        );
    }

    public static void Stop()
    {
        bool wasWalking = IsWalking;

        _path = Array.Empty<Vector2>();
        _pathIndex = 0;
        _cooldownLeft = 0;

        if (wasWalking)
        {
            Plugin.Log.LogInfo("[AutoMove] Movement stopped manually");
        }
    }

    public static void Tick()
    {
        if (!IsWalking)
        {
            if (_wasWalkingLastTick)
            {
                Plugin.Log.LogInfo("[AutoMove] Path completed");
                _wasWalkingLastTick = false;
            }

            return;
        }

        _wasWalkingLastTick = true;

        if (_cooldownLeft > 0)
        {
            _cooldownLeft--;
            return;
        }

        Vector2 nextPoint = _path[_pathIndex];
        MoveToMapPoint(nextPoint);

        _pathIndex++;
        _cooldownLeft = _moveCooldownFrames;

        SkipCurrentPointIfNeeded();

        if (_pathIndex >= _path.Length)
        {
            Plugin.Log.LogInfo("[AutoMove] Reached final path point");
        }
    }

    private static void SkipCurrentPointIfNeeded()
    {
        var current = Constants.getCurrentPlayerMapPoint();
        int skipped = 0;

        while (_pathIndex < _path.Length)
        {
            Vector2 p = _path[_pathIndex];
            int px = (int)Math.Round(p.X);
            int py = (int)Math.Round(p.Y);

            if (current.x == px && current.y == py)
            {
                _pathIndex++;
                skipped++;
            }
            else
            {
                break;
            }
        }

        if (skipped > 0)
        {
            Plugin.Log.LogInfo($"[AutoMove] Skipped {skipped} already reached path point(s)");
        }
    }

    private static void MoveToMapPoint(Vector2 mapPoint)
    {
        int x = (int)Math.Round(mapPoint.X);
        int y = (int)Math.Round(mapPoint.Y);

        Constants.getPlayer().transform.position =
            WorldUtils.ConvertMapPointToWorldPoint(x, y);
    }
}