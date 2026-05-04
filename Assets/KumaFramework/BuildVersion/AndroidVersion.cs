using UnityEngine;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class AndroidVersion {
    #if false
    public static int GetVersionCode() {
#if UNITY_EDITOR
        return PlayerSettings.Android.bundleVersionCode;
#elif UNITY_ANDROID
        using (var packageInfo = GetPackageInfo()) {
            return packageInfo.Get<int>("versionCode");
        }
#else
        return 0;
#endif
    }

#if UNITY_ANDROID
    static AndroidJavaObject GetPackageInfo() {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var context = currentActivity.Call<AndroidJavaObject>("getApplicationContext"))
        using (var packageManager = context.Call<AndroidJavaObject>("getPackageManager"))
        using (var packageManagerClass = new AndroidJavaClass("android.content.pm.PackageManager")) {
            string packageName = context.Call<string>("getPackageName");
            int activities = packageManagerClass.GetStatic<int>("GET_ACTIVITIES");
            return packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, activities);
        }
    }
#endif
    #endif


        public static int GetVersionCode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var versionCodeClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var currentActivity = versionCodeClass.GetStatic<AndroidJavaObject>("currentActivity");
                var packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
                var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", currentActivity.Call<string>("getPackageName"), 0);
                return packageInfo.Get<int>("versionCode");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to get Android build version: " + e.Message);
            return -1; // エラー時のデフォルト値
        }
            
#elif UNITY_ANDROID && UNITY_EDITOR
            return PlayerSettings.Android.bundleVersionCode;
#else
            Debug.LogWarning("This code is only supported on Android devices.");
            return -1; // Android以外ではデフォルト値
#endif
        }
}
