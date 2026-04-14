namespace BModv2.Patches.Impl.Utils;

using System;
using System.Reflection;
using System.Text;
using Kernys.Bson;

public static class BSONUtils
{
    public static string Dump(BSONObject obj)
    {
        if (obj == null)
            return "BSONObject: null";

        var sb = new StringBuilder();

        try
        {
            sb.AppendLine("=== BSONObject Dump ===");
            sb.AppendLine($"Type: {obj.GetType().FullName}");
            sb.AppendLine($"ToString(): {obj}");

            Type t = obj.GetType();

            sb.AppendLine("--- Properties ---");
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object? value = null;
                try
                {
                    if (p.GetIndexParameters().Length == 0)
                        value = p.GetValue(obj);
                    else
                        value = "[indexed property]";
                }
                catch (Exception e)
                {
                    value = $"[error: {e.GetType().Name}]";
                }

                sb.AppendLine($"{p.PropertyType.Name} {p.Name} = {SafeToString(value)}");
            }

            sb.AppendLine("--- Fields ---");
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object? value = null;
                try
                {
                    value = f.GetValue(obj);
                }
                catch (Exception e)
                {
                    value = $"[error: {e.GetType().Name}]";
                }

                sb.AppendLine($"{f.FieldType.Name} {f.Name} = {SafeToString(value)}");
            }
        }
        catch (Exception e)
        {
            sb.AppendLine($"Dump failed: {e}");
        }

        return sb.ToString();
    }

    private static string SafeToString(object? value)
    {
        if (value == null)
            return "null";

        try
        {
            return value.ToString() ?? "null";
        }
        catch (Exception e)
        {
            return $"[ToString error: {e.GetType().Name}]";
        }
    }
}