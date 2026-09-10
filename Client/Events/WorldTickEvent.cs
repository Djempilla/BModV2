using System;
using System.Collections.Generic;
using BasicTypes;
using BModv2.Client.Impl.Utils;
using BModv2.Patches.Impl;
using Cpp2IL.Core.Graphs;
using UnityEngine;

namespace BModv2.Events;

public class WorldTickEvent
{
    private static List<KeyCode> pressedKeys = new();
    
    public static void OnTick()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if(!Input.GetKey(key) && pressedKeys.Contains(key))
                pressedKeys.Remove(key);
            if (Input.GetKey(key))
            {
                KeyHoldEvent.OnKeyHold(key);
                if (pressedKeys.Contains(key))
                    continue;
                pressedKeys.Add(key);
                KeypressEvent.OnKeyPress(key);
            }
        }

        if (Constants.spamWorldLock)
        {
            PlayerUtils.PlaceBlock(Constants.getCurrentPlayerMapPoint().x, Constants.getCurrentPlayerMapPoint().y + 1, World.BlockType.LockWorld);
        }

        if (AutoMove.IsWalking)
        {
            Player player = Constants.getPlayer();
    
            player.deltaMovement = Vector3.zero;
            player.velocity = Vector3.zero;
            Vector3 velocity = player.velocity;
            velocity.y = -player.gravity * Time.deltaTime;
            player.gravity = 0f;
            player.velocity = velocity;
            // player.transform.position = new Vector3((float)Math.Floor(player.transform.position.x), (float)Math.Floor(player.transform.position.y));
            // player.SetVelocity(Vector3.zero);  
        }
        
    }
}