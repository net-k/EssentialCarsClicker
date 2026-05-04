#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class IOSVersion {
    public static string GetBuildNumber() {
#if UNITY_EDITOR
        return PlayerSettings.iOS.buildNumber;
#elif UNITY_IOS
        return GetBundleVersion();
#else
        return null;
#endif
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    static extern string GetBundleVersion();
#endif
}
