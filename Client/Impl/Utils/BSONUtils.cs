using System;
using Kernys.Bson;

namespace BModv2.Patches.Impl.Utils;

public static class BSONUtils
{
    public static void PrintBson(BSONObject packet)
    {
        if (packet == null || packet.mMap == null)
            return;

        foreach (string key in packet.mMap.Keys)
        {
            try
            {
                BSONValue value = packet[key];
                if (value == null)
                {
                    Console.WriteLine($"{key} : null");
                    continue;
                }

                string valueStr = FormatBsonValue(value);
                Console.WriteLine($"{key} : {valueStr}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{key} : <error: {ex.Message}>");
            }
        }
    }

    private static string FormatBsonValue(BSONValue value)
    {
        try
        {
            switch (value.valueType)
            {
                case BSONValue.ValueType.String:
                    return $"\"{value.stringValue}\"";

                case BSONValue.ValueType.Int32:
                    return value.int32Value.ToString();

                case BSONValue.ValueType.Int64:
                    return value.int64Value.ToString();

                case BSONValue.ValueType.Double:
                    return value.doubleValue.ToString();

                case BSONValue.ValueType.Boolean:
                    return value.boolValue.ToString();

                case BSONValue.ValueType.Object:
                    return "<nested object>";

                case BSONValue.ValueType.Array:
                    return "<array>";

                case BSONValue.ValueType.Binary:
                    return $"<binary, {value.binaryValue?.Length ?? 0} bytes>";

                case BSONValue.ValueType.None:
                    return "null";

                case BSONValue.ValueType.UTCDateTime:
                    return value.dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");

                default:
                    return $"<type: {value.valueType}>"; // показать неизвестный тип
            }
        }
        catch (Exception ex)
        {
            return $"<error: {ex.Message}, type: {value?.valueType}>"; // показать тип и ошибку
        }
    }
}