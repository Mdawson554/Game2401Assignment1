using UnityEngine;

public static class DevLogger
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void log(string message) => UnityEngine.Debug.Log(message);
}
