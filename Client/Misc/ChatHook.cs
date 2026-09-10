using HarmonyLib;
using System;
using BasicTypes;
using BModv2.AutoFarms;
using BModv2.Client.Misc.Commands;
using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using BModv2.Patches.Misc.Commands;
using UnityEngine;

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

            if (commandLine.StartsWith("world"))
            {
                string[] line = commandLine.Split(" ");
                if (line.Length != 2) return false;
                WorldCommand.Execute(line[1]);
            }
            
            if (commandLine.StartsWith("warp"))
            {
                WarpCommand.Execute(false);
            }

            if (commandLine.StartsWith("spawn"))
            {
                WarpCommand.Execute(true);
            }
            
            if (commandLine.StartsWith("fish"))
            {
                AutoFishCommand.Execute();
            }

            if (commandLine.StartsWith("test"))
            {
                testCommand.Execute();
            }
            
            if (commandLine.StartsWith("parse"))
            {
                ParseCommand.Execute();
            }
            
            if (commandLine.StartsWith("break"))
            {
                string[] line = commandLine.Split(" ");
                if (line.Length < 2)
                {
                    AutoFarmCommand.Execute(false);    
                } else if (line[1].Contains("bg"))
                {
                    AutoFarmCommand.Execute(true);
                }
            }
            if (commandLine.StartsWith("reload"))
            {
                ReloadCommand.Execute();
            }
            
            // Modules toggles
            
            if (commandLine.StartsWith("fly"))
            {
                Constants.IsFlyEnabled = !Constants.IsFlyEnabled;
            }

            if (commandLine.StartsWith("keyfly"))
            {
                Plugin.Log.LogInfo("KeyFly enabled");
                Constants.IsKeyflyEnabled = !Constants.IsKeyflyEnabled;
            }
            if (commandLine.StartsWith("freecam"))
            {
                Constants.IsFreecamEnabled = !Constants.IsFreecamEnabled;
            }

            if (commandLine.StartsWith("inv"))
            {
                InventoryCommand.Execute();
            }

            if (commandLine.StartsWith("goto"))
            {
                Vector2i v = WorldUtils.ConvertWorldPointToMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                GotoCommand.Execute(v.x, v.y);
            }
            
            if (commandLine.StartsWith("vortex"))
            {
                if (ConfigData.vortexPortalActivateDistance == 0.0f)
                {
                    ConfigData.vortexPortalActivateDistance = 1.0f;    
                }
                else
                {
                    ConfigData.vortexPortalActivateDistance = 0.0f;
                }
                
            }

            if (commandLine.StartsWith("forcelock"))
            {
                Constants.spamWorldLock = !Constants.spamWorldLock;
                Plugin.Log.LogInfo($"Forcelock {Constants.spamWorldLock}");
            }

            if (commandLine.StartsWith("snipe"))
            {
                RandomWorlder.isEnabled =  !RandomWorlder.isEnabled;
            }
            
            return false;
        }

        return true;
    }
}