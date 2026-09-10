using BModv2.Client.Farming;
using BModv2.Client.Misc.Commands;
using BModv2.Patches.Impl;
using HarmonyLib;

namespace BModv2.Patches;

[HarmonyPatch(typeof(AuctionHouseUI), "Update")]
public static class Patch_AuctionHouseUI_Update
{
    // Token: 0x06000739 RID: 1849 RVA: 0x00009F2C File Offset: 0x0000812C
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}

[HarmonyPatch(typeof(BaseMenuUI), "Update")]
public static class Patch_BaseMenuUI_Update
{
    // Token: 0x0600073D RID: 1853 RVA: 0x00009F4A File Offset: 0x0000814A
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}

[HarmonyPatch(typeof(ChatUI), "Update")]
public static class Patch_ChatUI_Update
{
    // Token: 0x06000729 RID: 1833 RVA: 0x00009EB4 File Offset: 0x000080B4
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}

[HarmonyPatch(typeof(MainMenuLogic), "Update")]
public static class Patch_MainMenuLogic_Update
{
    // Token: 0x06000731 RID: 1841 RVA: 0x00009EF0 File Offset: 0x000080F0
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}

[HarmonyPatch(typeof(OptionsMenuUI), "Update")]
public static class Patch_OptionsMenuUI_Update
{
    // Token: 0x06000735 RID: 1845 RVA: 0x00009F0E File Offset: 0x0000810E
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}

[HarmonyPatch(typeof(TitleScreenLogic), "Update")]
public static class Patch_TitleScreenLogic_Update
{
    // Token: 0x0600072D RID: 1837 RVA: 0x00009ED2 File Offset: 0x000080D2
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}

[HarmonyPatch(typeof(WorldController), "Update")]
public static class Patch_WorldController_Update
{
    // Token: 0x06000725 RID: 1829 RVA: 0x00009E96 File Offset: 0x00008096
    private static void Postfix()
    {
        AntiLockedWorld.patchWireCrash();
    }
}