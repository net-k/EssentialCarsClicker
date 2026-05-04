using System.Collections;
using UnityEngine;

namespace stickin.menus.type1
{
    public class TextMessageMenu : BaseMenu
    {
        public const string AdsNotReady = "strNoAds";
            
        [SerializeField] private TextLocalization _text;

        public static void ShowWithText(string text)
        {
            // Ads not ready
            MenusService.Show<TextMessageMenu>(new Hashtable { ["text"] = text });
        }
        
        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            if (data != null && data.ContainsKey("text"))
                _text.SetText((string) data["text"]);
        }

        protected override void ShowComplete()
        {
            base.ShowComplete();
            
            Hide();
        }
    }
}