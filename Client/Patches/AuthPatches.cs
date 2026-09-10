// using HarmonyLib;
//
// namespace BModv2.Patches;
//
// [HarmonyPatch(typeof(UserIdent), "LoginWithUsernameAndPassword")]
// public static class Patch_LoginWithUsernameAndPassword
// {
//     public static void Prefix(string username, string password)
//     {
//         Plugin.Log.LogInfo(
//             $"LoginWithUsernameAndPassword: user={LogHelpers.Safe(username)}, passLen={LogHelpers.Len(password)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "StartPlayFabCredentialsLoginCoroutine")]
// public static class Patch_CredentialsLogin
// {
//     public static void Prefix(string user, string password)
//     {
//         Plugin.Log.LogInfo(
//             $"StartPlayFabCredentialsLoginCoroutine: user={LogHelpers.Safe(user)}, passLen={LogHelpers.Len(password)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "StartPlayFabLogin")]
// public static class Patch_StartPlayFabLogin
// {
//     public static void Prefix(bool createAccount)
//     {
//         Plugin.Log.LogInfo($"StartPlayFabLogin: createAccount={createAccount}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "LogOut")]
// public static class Patch_LogOut
// {
//     public static void Prefix()
//     {
//         Plugin.Log.LogInfo("LogOut called");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "GetLoginToken")]
// public static class Patch_GetLoginToken
// {
//     public static void Postfix(string __result)
//     {
//         Plugin.Log.LogInfo($"GetLoginToken => {LogHelpers.Safe(__result)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "IsCurrentTokenValid")]
// public static class Patch_IsCurrentTokenValid
// {
//     public static void Postfix(bool __result)
//     {
//         Plugin.Log.LogInfo($"IsCurrentTokenValid => {__result}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "IsTokenValid")]
// public static class Patch_IsTokenValid
// {
//     public static void Prefix(long expiry)
//     {
//         Plugin.Log.LogInfo($"IsTokenValid called: expiry={expiry}");
//     }
//
//     public static void Postfix(long expiry, bool __result)
//     {
//         Plugin.Log.LogInfo($"IsTokenValid => {__result} (expiry={expiry})");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "GetLoginInfo")]
// public static class Patch_GetLoginInfo
// {
//     public static void Postfix(object __result)
//     {
//         Plugin.Log.LogInfo($"GetLoginInfo => {LogHelpers.Dump(__result)}");
//     }
// }
//
// // [HarmonyPatch(typeof(UserIdent), "UpdateLastLogin")]
// // public static class Patch_UpdateLastLogin
// // {
// //     public static void Prefix(string val)
// //     {
// //         Plugin.Log.LogInfo($"UpdateLastLogin: val={LogHelpers.Safe(val)}");
// //     }
// // }
//
// [HarmonyPatch(typeof(UserIdent), "GetDeviceIDFromLoginResult")]
// public static class Patch_GetDeviceIDFromLoginResult
// {
//     public static void Postfix(object result, string __result)
//     {
//         Plugin.Log.LogInfo($"GetDeviceIDFromLoginResult => {LogHelpers.Safe(__result)}");
//         if (result != null)
//             Plugin.Log.LogInfo($"GetDeviceIDFromLoginResult input => {LogHelpers.Dump(result)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnPlayFabLoginSuccess")]
// public static class Patch_OnPlayFabLoginSuccess
// {
//     public static void Prefix(object result)
//     {
//         Plugin.Log.LogInfo("OnPlayFabLoginSuccess called");
//         if (result != null)
//             Plugin.Log.LogInfo($"LoginResult => {LogHelpers.Dump(result)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnPlayFabLoginFailure")]
// public static class Patch_OnPlayFabLoginFailure
// {
//     public static void Prefix(object error)
//     {
//         Plugin.Log.LogWarning("OnPlayFabLoginFailure called");
//         if (error != null)
//             Plugin.Log.LogWarning($"PlayFabError => {LogHelpers.Dump(error)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnPlayFabCredentialsLoginFailure")]
// public static class Patch_OnPlayFabCredentialsLoginFailure
// {
//     public static void Prefix(object error)
//     {
//         Plugin.Log.LogWarning("OnPlayFabCredentialsLoginFailure called");
//         if (error != null)
//             Plugin.Log.LogWarning($"Credentials PlayFabError => {LogHelpers.Dump(error)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "GetRegistrationResult")]
// public static class Patch_GetRegistrationResult
// {
//     public static void Postfix(object __result)
//     {
//         Plugin.Log.LogInfo($"GetRegistrationResult => {LogHelpers.Dump(__result)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "GetPendingEmail")]
// public static class Patch_GetPendingEmail
// {
//     public static void Postfix(string __result)
//     {
//         Plugin.Log.LogInfo($"GetPendingEmail => {LogHelpers.Safe(__result)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "SetPendingEmail")]
// public static class Patch_SetPendingEmail
// {
//     public static void Prefix(string email, object registrationStatus)
//     {
//         Plugin.Log.LogInfo(
//             $"SetPendingEmail: email={LogHelpers.Safe(email)}, status={LogHelpers.Dump(registrationStatus)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnAccountRegistrationTriggered")]
// public static class Patch_OnAccountRegistrationTriggered
// {
//     public static void Prefix()
//     {
//         Plugin.Log.LogInfo("OnAccountRegistrationTriggered called");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "RequestUserRegistrationCoroutine")]
// public static class Patch_RequestUserRegistrationCoroutine
// {
//     public static void Prefix(string a, string b)
//     {
//         Plugin.Log.LogInfo(
//             $"RequestUserRegistrationCoroutine: arg1={LogHelpers.Safe(a)}, arg2={LogHelpers.Safe(b)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "RequestUserRegistrationStatusCoroutine")]
// public static class Patch_RequestUserRegistrationStatusCoroutine
// {
//     public static void Prefix()
//     {
//         Plugin.Log.LogInfo("RequestUserRegistrationStatusCoroutine called");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "GenerateSteamAuthTicket")]
// public static class Patch_GenerateSteamAuthTicket
// {
//     public static void Postfix(string __result)
//     {
//         Plugin.Log.LogInfo($"GenerateSteamAuthTicket => {LogHelpers.Safe(__result)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "StartSteamLoginAuthTicketCoroutine")]
// public static class Patch_StartSteamLoginAuthTicketCoroutine
// {
//     public static void Prefix(bool value)
//     {
//         Plugin.Log.LogInfo($"StartSteamLoginAuthTicketCoroutine: value={value}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnSteamAuthTicketResponse")]
// public static class Patch_OnSteamAuthTicketResponse
// {
//     public static void Prefix(object response)
//     {
//         Plugin.Log.LogInfo($"OnSteamAuthTicketResponse => {LogHelpers.Dump(response)}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "Awake")]
// public static class Patch_Awake
// {
//     public static void Prefix()
//     {
//         Plugin.Log.LogInfo("UserIdent.Awake()");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "Start")]
// public static class Patch_Start
// {
//     public static void Prefix()
//     {
//         Plugin.Log.LogInfo("UserIdent.Start()");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnApplicationPause")]
// public static class Patch_OnApplicationPause
// {
//     public static void Prefix(bool paused)
//     {
//         Plugin.Log.LogInfo($"OnApplicationPause: paused={paused}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnApplicationFocus")]
// public static class Patch_OnApplicationFocus
// {
//     public static void Prefix(bool focused)
//     {
//         Plugin.Log.LogInfo($"OnApplicationFocus: focused={focused}");
//     }
// }
//
// [HarmonyPatch(typeof(UserIdent), "OnDestroy")]
// public static class Patch_OnDestroy
// {
//     public static void Prefix()
//     {
//         Plugin.Log.LogInfo("UserIdent.OnDestroy()");
//     }
// }