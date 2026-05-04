using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    [RequireComponent(typeof(Button))]
    public class ButtonTutorial : MonoBehaviour
    {
        [InjectField] private AppService _appService;

        private void Start()
        {
            InjectService.BindFields(this);
            
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var tutorial = _appService.GameConfig.GetCustomConfig<TutorialConfig>();
            MenusService.Show<TutorialMenu>(new Hashtable {["TutorialConfig"] = tutorial});
        }
    }
}