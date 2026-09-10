namespace BModv2.Patches.Impl;
using UnityEngine;

public class AntiLockedWorld
{

    private static WiringWireController _cachedWiringWireController;
    private static float _nextWiringWireControllerLookupTime;
    
    public static void patchWireCrash()
    {
        WiringWireController www = GetCachedSceneObject<WiringWireController>(ref _cachedWiringWireController, ref _nextWiringWireControllerLookupTime);
        if (www != null)
        {
            Object.Destroy(www);
        }
        
    }
    
    private static T GetCachedSceneObject<T>(ref T cachedObject, ref float nextLookupTime) where T : Object
    {
        if (cachedObject != null)
        {
            return cachedObject;
        }
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        if (realtimeSinceStartup < nextLookupTime)
        {
            return default(T);
        }
        nextLookupTime = realtimeSinceStartup + 0.35f;
        cachedObject = Object.FindObjectOfType<T>();
        return cachedObject;
    }
    
}