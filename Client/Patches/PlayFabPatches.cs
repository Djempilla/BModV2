// using HarmonyLib;
// using PlayFab;
// using PlayFab.ClientModels;
// using UnityEngine;
// using System;
// using System.Reflection;
// using System.Collections;
// using Il2CppSystem;
// using Il2CppInterop.Runtime; // ⬅️ REQUIRED: For DelegateSupport
//
// namespace BModv2.Client.Patches;
//
// public static class SwitcherConfig
// {
//     // 🔹 TARGET DEVICE ID FOR ACCOUNT SWITCHING
//     public const string TargetDeviceId = "ea0c9543b558f904feb6fec8a429b213d9f94603";
//     public const string OldAccountId = "8EF9753E2D3517B4"; 
// }
//
// // 🔹 1. Force clean state on Start (runs right after Awake)
// [HarmonyPatch(typeof(UserIdent), "Start")]
// public static class ForceCleanStartPatch
// {
//     [HarmonyPrefix]
//     public static bool Prefix(UserIdent __instance)
//     {
//         Plugin.Log.LogInfo("[Switcher] === Start Intercepted ===");
//         
//         // Force logout to break any local session restoration
//         try { PlayFabClientAPI.ForgetAllCredentials(); } catch {}
//         PlayerPrefs.SetInt("logOut", 1);
//         PlayerPrefs.Save();
//         
//         // Nullify internal state fields
//         var t = typeof(UserIdent);
//         foreach (var f in new[] { "sessionTicket", "steamAuthTicket", "authToken", "idState", "inited" })
//         {
//             t.GetField(f, BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(__instance, null);
//         }
//         
//         Plugin.Log.LogInfo("[Switcher] Local state forced to clean. Triggering fresh auth...");
//         
//         // Manually start the credentials login coroutine
//         __instance.StartCoroutine(RunCustomLogin());
//         
//         return false; // Skip original Start() to prevent double-login attempts
//     }
//
//     private static IEnumerator RunCustomLogin()
//     {
//         yield return null;
//         
//         Plugin.Log.LogInfo($"[Switcher] Starting LoginWithCustomID -> {SwitcherConfig.TargetDeviceId}");
//         var req = new LoginWithCustomIDRequest();
//         req.CustomId = SwitcherConfig.TargetDeviceId;
//         req.CreateAccount = new Il2CppSystem.Nullable<bool>(false);
//         
//         // ✅ FIX: Use DelegateSupport.ConvertDelegate to create valid Il2Cpp Actions
//         var successAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action<LoginResult>>(new System.Action<LoginResult>(OnLoginSuccess));
//         var failAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action<PlayFabError>>(new System.Action<PlayFabError>(OnLoginFailure));
//
//         PlayFabClientAPI.LoginWithCustomID(req, successAction, failAction);
//     }
//
//     private static void OnLoginSuccess(LoginResult result)
//     {
//         Plugin.Log.LogInfo($"[Switcher] ✅ Network Login Success -> PlayFabId: {result.PlayFabId}");
//     }
//
//     private static void OnLoginFailure(PlayFabError error)
//     {
//         Plugin.Log.LogError($"[Switcher] ❌ Network Login Failed -> {error.ErrorMessage}");
//     }
// }
//
// // 🔹 2. Fallback: Catch local session restore & force re-login if old account
// [HarmonyPatch(typeof(UserIdent), "OnPlayFabLoginSuccess")]
// public static class SessionHijackPatch
// {
//     [HarmonyPrefix]
//     public static bool Prefix(object result)
//     {
//         var pfId = result.GetType().GetProperty("PlayFabId")?.GetValue(result)?.ToString();
//         Plugin.Log.LogInfo($"[Switcher] === LOGIN SUCCESS (Local or Network) ===");
//         Plugin.Log.LogInfo($"[Switcher] PlayFabId: {pfId}");
//
//         if (pfId == SwitcherConfig.OldAccountId)
//         {
//             Plugin.Log.LogWarning("[Switcher] ⚠️ OLD ACCOUNT RESTORED -> Forcing Logout & Re-auth");
//             
//             // Clear everything aggressively
//             try { PlayFabClientAPI.ForgetAllCredentials(); } catch {}
//             foreach (var k in new[] { "sessionTicket", "sfToken", "sfTokenExpiry", "logOut", "unity.player_sessionid" })
//                 if (PlayerPrefs.HasKey(k)) PlayerPrefs.DeleteKey(k);
//             PlayerPrefs.Save();
//
//             // Trigger fresh login after a short delay
//             var instance = UnityEngine.Object.FindObjectOfType<UserIdent>();
//             instance?.StartCoroutine(DelayedCustomLogin());
//             
//             return false; // Block original handler
//         }
//
//         Plugin.Log.LogInfo("[Switcher] ✅ Account matches target or is new.");
//         return true;
//     }
//
//     private static IEnumerator DelayedCustomLogin()
//     {
//         yield return new WaitForSeconds(1.5f);
//         Plugin.Log.LogInfo("[Switcher] Executing forced LoginWithCustomID...");
//         var req = new LoginWithCustomIDRequest();
//         req.CustomId = SwitcherConfig.TargetDeviceId;
//         req.CreateAccount = new Il2CppSystem.Nullable<bool>(false);
//         
//         // ✅ FIX: Use DelegateSupport.ConvertDelegate here too
//         var successAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action<LoginResult>>(new System.Action<LoginResult>(OnSuccess));
//         var failAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action<PlayFabError>>(new System.Action<PlayFabError>(OnFail));
//
//         PlayFabClientAPI.LoginWithCustomID(req, successAction, failAction);
//     }
//
//     private static void OnSuccess(LoginResult r) => Plugin.Log.LogInfo($"[Switcher] Forced login success -> {r.PlayFabId}");
//     private static void OnFail(PlayFabError e) => Plugin.Log.LogError($"[Switcher] Forced login fail -> {e.ErrorMessage}");
// }