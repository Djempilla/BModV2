using System;

namespace BModv2;

internal static class LogHelpers
{
    internal static string Safe(string value)
    {
        if (value == null) return "<null>";
        if (value.Length == 0) return "<empty>";
        if (value.Length <= 256) return value;
        return value.Substring(0, 256) + $"... [len={value.Length}]";
    }

    internal static int Len(string value) => value?.Length ?? 0;

    internal static string Dump(object obj)
    {
        if (obj == null) return "<null>";

        try
        {
            var s = obj.ToString();
            if (string.IsNullOrEmpty(s))
                s = $"<{obj.GetType().FullName}>";

            if (s.Length > 512)
                s = s.Substring(0, 512) + $"... [len={s.Length}]";

            return $"{obj.GetType().FullName}: {s}";
        }
        catch (Exception ex)
        {
            return $"<dump_error {ex.GetType().Name}: {ex.Message}>";
        }
    }
}