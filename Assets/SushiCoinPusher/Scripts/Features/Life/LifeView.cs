using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Life
{
    public class LifeView : MonoBehaviour
    {
        [SerializeField]
        private Text lifeNumText = null;

        [SerializeField]
        private Text lifeRecoverTimeText = null;
 
        public Text LifeNumText => lifeNumText;

        public Text LifeRecoverTimeText => lifeRecoverTimeText;

        public void SetRecoverTime(string recoverTime)
        {
             string template = I2.Loc.LocalizationManager.GetTranslation("key_heart_recover_time");
             if( string.IsNullOrEmpty( template ) )
             {
                 template = "Next life in {0}";
                 Debug.LogError("[Error] SetRecoverTime" );
             }
             
             if (lifeRecoverTimeText != null)
             {
                 lifeRecoverTimeText.text = template.Replace("{0}", recoverTime);
             }
        }
    }
}
