using Quiz.Presentation.Title;
using ShisenSho.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quiz.Presentation
{

	public class TitlePresenter : MonoBehaviour
	{
		private TitleView _view = null;
//		[SerializeField] private SoundPlayer _soundPlayer = null;
		
		public void Awake()
		{
			_view = GetComponent<TitleView>();

			SetEvents();
			Initialize();
		}

		private void Start()
		{
			if (AdMobObjectHolder.Instance.ADMobBanner != null)
			{
			//	AdMobObjectHolder.Instance.ADMobBanner.Show();
			}
//			_soundPlayer.PlayBGM("bgm_title");
		}

		// 初期化
		public void Initialize()
		{
		}

		// Viewのイベントの設定を行う
		private void SetEvents()
		{
			_view.StartButton.onClick.AddListener(OnStartButtonClicked);
		}


		private void OnStartButtonClicked()
		{
		//	SoundManager.Instance.PlaySE("decision5");
			NextScene();
		}

		private void NextScene()
		{
//			_soundPlayer.StopBGM();
			SceneManager.LoadScene("ShisenSho/Scenes/GameScene");
		}
	}
}
