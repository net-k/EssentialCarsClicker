namespace KumaFramework.BuildVersion
{
    public class ApplicationBuildVersion
    {
        // iOSVersion と AndroidVersion を使い分けて
        // ビルド番号を取得する
        public static string GetBuildNumber()
        {
#if UNITY_IOS
            return IOSVersion.GetBuildNumber();
#elif UNITY_ANDROID
        return AndroidVersion.GetVersionCode().ToString();
#else
        return "";
#endif
        }
    }
}
