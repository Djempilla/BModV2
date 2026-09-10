using BModv2.Patches.Impl;
using BModv2.Patches.Impl.Utils;
using UnityEngine;

namespace BModv2.Events;

public class KeyHoldEvent
{
    public static void OnKeyHold(KeyCode key)
    {
        if (Constants.IsFlyEnabled && key == KeyCode.F)
        {
            Vector2 v2 = default(Vector2);
            if(Camera.main == null)
                return;
            Vector2 v3 = Camera.main.WorldToScreenPoint(Constants.getPlayer().transform.position);
            v2.x = Input.mousePosition.x - v3.x;
            v2.y = -(Input.mousePosition.y - v3.y);

            float num9 = 1f / 10f * 1000f;
            Vector3 velocity = new Vector3(v2.x / num9, v2.y / ((num9 - num9 / 4f) / -1f), 0f);
            
            Constants.getPlayer().SetVelocity(velocity);
        }
        
        
        if (Constants.IsKeyflyEnabled)
        {

            float FlySpeed = 2.5f;
            
            Player player = Constants.getPlayer();
            
            player.isDoubleJumpFirstJumpDone = true;
            player.isTripleJumpFirstJumpDone = true;
            player.isTripleJumpSecondJumpDone = true;
            player.IsGrounded();
            
            
            if (key == KeyCode.LeftArrow)
            {
            }
            else if (key == KeyCode.RightArrow)
            {
            }
            else if (key == KeyCode.UpArrow)
            {
            }
            else if (key == KeyCode.DownArrow)
            {
            }
            else
            {
                player.SetVelocity(Vector3.zero);
            }
        }
        
        
    }
}