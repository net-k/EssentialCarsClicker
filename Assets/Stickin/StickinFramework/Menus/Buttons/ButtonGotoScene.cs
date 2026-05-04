using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(Button))]
    public class ButtonGotoScene : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            int sceneIndex = _sceneIndex - 1;
            // SceneLoader.LoadScene(_sceneIndex);
            SceneLoader.LoadScene(sceneIndex);
        }
    }
}