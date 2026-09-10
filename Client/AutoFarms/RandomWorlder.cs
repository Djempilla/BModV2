using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using BModv2.Client.Impl.Utils;
using BModv2.Patches.Impl;
using DateTime = Il2CppSystem.DateTime;

namespace BModv2.AutoFarms;

public class RandomWorlder
{
    public static bool isEnabled = false;

    private static int ticksPassed = 0;
    private static int menuDelay = 0;
    private static int joinTimeoutTicks = 0;

    private static bool hasCheckedThisWorld = false;
    private static bool isCheckingWorld = false;
    private static bool isLeavingWorld = false;
    private static bool joinWorldRequested = false;

    private const int CheckDelayTicks = 120;
    private const int MenuDelayTicks = 120;
    private const int JoinTimeoutMaxTicks = 600;

    private static string currentTargetWorld = "BXL";

    private static readonly string OutputFilePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "three_letter_worlds.txt");

    private static readonly HashSet<string> skippedWorlds =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "BUY"
        };

    public static async Task Tick()
    {
        if (!isEnabled)
            return;

        if (isCheckingWorld)
            return;

        if (PlayerUtils.IsPlayerInMenus())
        {
            if (PlayerUtils.IsPlayerJoiningWorld() || PlayerUtils.IsPlayerInLimbo() || PlayerUtils.IsPlayerCheckingGameVerion())
                return;

            ticksPassed = 0;
            isLeavingWorld = false;
            hasCheckedThisWorld = false;

            if (joinWorldRequested)
            {
                joinTimeoutTicks++;

                if (joinTimeoutTicks >= JoinTimeoutMaxTicks)
                {
                    string failedLine = $"{currentTargetWorld} JOIN_FAILED";
                    AppendLineSafe(OutputFilePath, failedLine);
                    Plugin.Log.LogWarning($"[RandomWorlder] Join timeout for '{currentTargetWorld}', skipping");

                    joinWorldRequested = false;
                    joinTimeoutTicks = 0;
                    AdvanceTargetWorld();
                }

                return;
            }

            menuDelay++;
            if (menuDelay < MenuDelayTicks)
                return;

            menuDelay = 0;

            while (skippedWorlds.Contains(currentTargetWorld))
            {
                Plugin.Log.LogInfo($"[RandomWorlder] Skipping blocked world '{currentTargetWorld}'");
                AdvanceTargetWorld();
            }

            joinWorldRequested = true;
            joinTimeoutTicks = 0;

            Plugin.Log.LogInfo($"[RandomWorlder] Joining world '{currentTargetWorld}' from MainMenu");
            SceneLoader.GoFromMainMenuToWorld(currentTargetWorld, "");
            return;
        }

        menuDelay = 0;

        if (Constants.getWorld() != null)
        {
            joinWorldRequested = false;
            joinTimeoutTicks = 0;
        }

        if (isLeavingWorld)
            return;

        ticksPassed++;
        if (ticksPassed < CheckDelayTicks)
            return;

        ticksPassed = 0;

        if (hasCheckedThisWorld)
            return;

        isCheckingWorld = true;

        try
        {
            World world = Constants.getWorld();
            string worldName = world?.worldName ?? currentTargetWorld;
            Plugin.Log.LogInfo($"[RandomWorlder] Checking world: {worldName}");

            string line;

            try
            {
                DateTime lockTime = world.lockWorldDataHelper.GetLastActivatedTime();
                System.DateTime lockTimeSystem = new System.DateTime(
                    lockTime.Year,
                    lockTime.Month,
                    lockTime.Day,
                    lockTime.Hour,
                    lockTime.Minute,
                    lockTime.Second
                );

                int daysSince = (int)(System.DateTime.Now - lockTimeSystem).TotalDays;

                line = $"{worldName} {lockTimeSystem:dd.MM.yyyy HH:mm:ss}";

                if (daysSince > 25)
                {
                    line += $" SOON > 25 DAYS ({daysSince})";
                }
                else
                {
                    line += $" ({daysSince})";
                }
            }
            catch
            {
                line = $"{worldName} FAILED_TO_GET_LAST_ACTIVE";
            }

            AppendLineSafe(OutputFilePath, line);
            Plugin.Log.LogInfo($"[RandomWorlder] Saved: {line}");

            hasCheckedThisWorld = true;
            isLeavingWorld = true;

            AdvanceTargetWorld();

            Plugin.Log.LogInfo("[RandomWorlder] Leaving world to MainMenu");
            SceneLoader.GoFromWorldToMainMenu();
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[RandomWorlder] Failed: {ex}");
        }
        finally
        {
            isCheckingWorld = false;
        }
    }

    private static void AppendLineSafe(string path, string line)
    {
        try
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[RandomWorlder] Failed to write file: {ex}");
        }
    }

    private static void AdvanceTargetWorld()
    {
        char[] chars = currentTargetWorld.ToUpperInvariant().ToCharArray();

        for (int i = 2; i >= 0; i--)
        {
            if (chars[i] < 'Z')
            {
                chars[i]++;
                for (int j = i + 1; j < 3; j++)
                    chars[j] = 'A';

                currentTargetWorld = new string(chars);
                return;
            }
        }

        currentTargetWorld = "BXL";
    }
}