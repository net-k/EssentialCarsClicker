using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Quiz.Framework.SupportScene
{
    public class SupportSceneButton: MonoBehaviour {
    
        [SerializeField]
        Button supportSceneButton;

        private string scenePath = "ShisenSho/Scenes/";

        // Use this for initialization
        void Start () {
            supportSceneButton.onClick.AddListener(OnSupportSceneButtonClicked);	
        }
	
        void OnSupportSceneButtonClicked()
        {
            SceneManager.LoadScene( scenePath + "SupportScene");
        }
    }
}
