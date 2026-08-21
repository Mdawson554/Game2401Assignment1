using UnityEngine;

namespace _Project.Scripts.Tools
{
    public static class DevLogger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void Log(string message) => Debug.Log(message);
    }
}