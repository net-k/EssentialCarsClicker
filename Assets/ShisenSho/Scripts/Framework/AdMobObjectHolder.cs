namespace ShisenSho.Framework
{
    public class AdMobObjectHolder : SingletonMonoBehaviour<AdMobObjectHolder>
    {
        private AdMobBanner _adMobBanner = null;

        public AdMobBanner ADMobBanner => _adMobBanner;

        public void Setup(AdMobBanner adMobBanner)
        {
            _adMobBanner = adMobBanner;
        }
    }
}
