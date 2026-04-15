using HarmonyLib;
using System;
using BModv2.Patches.Impl;
using BModv2.Patches.Misc.Commands;

namespace BModv2.Patches.Misc;

[HarmonyPatch(typeof(ChatUI), nameof(ChatUI.Submit))]
public static class PortalDataCtorPatch
{
    [HarmonyPrefix]
    public static bool Prefix(ref string text)
    {
        if (text.StartsWith("//"))
        {
            // faking message :"D
            String Newtext = text.Remove(0, 1);
            OutgoingMessages.SubmitClanChatMessage(Newtext);
            NetworkPlayers.ShowSpeechBubbleFor(Constants.thePlayer.myPlayerData.playerId, Newtext);
            return false;
        }

        if (text.StartsWith("/"))
        {
            String commandLine = text.Remove(0, 1);
            if (commandLine.StartsWith("portal"))
            {
                if (commandLine.EndsWith("0") || commandLine.EndsWith("1"))
                {
                    if (commandLine.EndsWith("0"))
                    {
                        PortalDataCommand.Execute(true, 0);   
                    }
                    if (commandLine.EndsWith("1"))
                    {
                        PortalDataCommand.Execute(false, 1);
                    }   
                }
                else
                {
                    PortalDataCommand.Execute(false, 0);
                }
                
            }
            
            if (commandLine.StartsWith("pos"))
            {
                PositionCommand.Execute();
            }

            if (commandLine.StartsWith("warp"))
            {
                WarpCommand.Execute(false);
            }

            if (commandLine.StartsWith("spawn"))
            {
                WarpCommand.Execute(true);
            }

            return false;
        }

        return true;
    }
}