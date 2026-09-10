using System;
using System.Reflection;
using BModv2.Patches.Impl.Utils;
using HarmonyLib;
using Kernys.Bson;

namespace BModv2.Patches;

[HarmonyPatch(typeof(OutgoingMessages), nameof(OutgoingMessages.AddOneMessageToList), new Type[] { typeof(BSONObject) })]
public static class OutgoingMessagesPatch
{
    [HarmonyPrefix]
    public static void Prefix(BSONObject __0)
    {
        try
        {
            // token auth TODO finish
            // if (__0.ContainsKey("ID"))
            // {
            //     BSONValue idValue = __0["ID"];
            //     
            //     if (idValue.valueType == BSONValue.ValueType.String && idValue.stringValue == "GPd")
            //     {
            //         
            //         __0["AT"] = new BSONValue("eyJhbGciOiJSUzUxMiIsImtpZCI6IlJJV2RkYXh6VlAxNGlyNjM1TDJud3JzNTd2NkxDQV9EMmd3b0Q2UktLZ2siLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJjb20uc29jaWFsZmlyc3QucGl4ZWx3b3JsZHMiLCJ0aXRsZUlkIjoiMTFFRjVDIiwicm9sZSI6ImFub255bW91cyIsIm5pY2tuYW1lIjoiU3ViamVjdF9HMUpKTzM5QyIsImlhdCI6MTc3ODAxMjg1MSwiaXNzIjoiaHR0cHM6Ly9hdXRoLnNjbGZyc3QuY29tIiwiZXhwIjoxNzc4MDE0NjUxLCJzdWIiOiI2OWZhNGEyODU3YjU4OTFkN2E1NmI3MjMifQ.eRr8QXttFtgtVJDqj2TlAoEZKyJF1mBC4UnZedMI-TA3WGiBmGFDqY9vaY3tOuHU5VgvjozqGS4u-mPNOHdLaPXRQHiEbxuoW58gd4Bx2e_qBcPPHASuRq6eNbsIKq1FcPiRPlDx35pYIvUroiZ3SbIrZQyAdPqHjj1a57E3bn_9BFi_SiUCiU9zJtvhfja62Qce_8EQhTfivW2Tio1vyypb-roDLnT9_vonTf3KmgfDPYGmqmiMkv2ZinQ_fde4j3PgwwLiq6-jCNbXk5Wlw7CqLsZyw6Jos5ciICmjwm4To3AMPPom4PvK5spfGnpQonOlmOkyEMPfUSf5p0wg1i9Zo4-jhL9wZVreUFKSfM3GGaoi1r9NuR25Jcgz6JaRIAHJEaVMMgXO8CfiIyaXKKtHW-9etzNFvBq142PFQrjBMOjh8DT9LFjjR3Xsby2tbLEYqQtCp0UeQzxldjRJ3skECZ4tuskHl-TWc9DYz6e-57_PJ151qIAR1I3lnbC2mOCEzHb-9EwWIuf0Czj0-I-FGkhTWFM2BMmxhwqJj9YhRal_98-HdLp7EkZcHj0Hk8SOHl1tYNUAtAoW-UqvcilvqArl5_DzQyM2AtzfwpidygWv2Oige3GqagVF2QU3f6Ba6eyCY0z-4MO_MBdwcnBHJwP_kVRhxUyspls1at4");
            //     }
            // }
            // BSONUtils.PrintBson(__0);
        }
        catch (Exception ex)
        {
            Plugin.Log?.LogError(ex);
        }
    }

    private static string SafeRead(BSONObject obj, string key)
    {
        try
        {
            var value = obj[key];
            return BsonValueToReadableString(value);
        }
        catch
        {
            return "<missing>";
        }
    }

    private static string BsonValueToReadableString(object value)
    {
        if (value == null)
            return "null";

        var type = value.GetType();

        foreach (var propName in new[]
                 {
                     "stringValue", "StringValue", "value", "Value",
                     "intValue", "IntValue",
                     "longValue", "LongValue",
                     "doubleValue", "DoubleValue",
                     "floatValue", "FloatValue",
                     "boolValue", "BoolValue", "booleanValue", "BooleanValue"
                 })
        {
            var prop = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) continue;

            try
            {
                var inner = prop.GetValue(value);
                if (inner != null)
                    return inner.ToString();
            }
            catch { }
        }

        return value.ToString();
    }
}