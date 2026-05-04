using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace KumaFramework.OwnAd
{
    public class HttpClient : MonoBehaviour
    {
        [SerializeField] private GameObject _touchGuardPanel = null;
        [SerializeField] private GameObject _dialog = null;
        [SerializeField] private GameObject _textOnlyDialog = null; // TODO テキストのみの自社広告ダイアログのしくみはまだ作ってない
        [SerializeField] private Text _titleText = null;
        [SerializeField] private Text _descriptionText = null;
        [SerializeField] private Image _image = null;
        [SerializeField] private string BundleIdentifier = null;
        
        [SerializeField] private Button _yesButton = null;
        [SerializeField] private Button _noButton = null;
        [SerializeField] private Text _yesButtonText = null;
        [SerializeField] private Text _noButtonText = null;
        [SerializeField] private bool IsDebugMode = false;

        [SerializeField] private Button _debugRequestButton;
        [SerializeField] private Text   _debugRemoteText;
        [SerializeField] private string   _debugOS;

        [SerializeField] private Review _review;
        
        enum ConnectionMode
        {
            Local,
// TODO     Staging,
            Production
        }
        
        [SerializeField]
        private ConnectionMode _connectionMode = ConnectionMode.Local;

        private string HouseAd_LaunchAppCountKey = "HouseAd_LaunchAppKey"; // 何回目の起動で広告を表示するかに使う、起動回数
        private string HouseAd_LastDisplayOwnAdTimeKey = "HouseAd_BeforeDisplayTimerKey"; // 前回起動


        [Serializable]
        public class AppInfo
        {
            public string title;
            public string description;
            public string imageid;
            public string is_open;
            public string start_show_launch_count;
            public string url = "http://net-k.net/kumamon/";
            public string is_text;
            public string display_time = "5";
            public string yes_button_text = "Yes";
            public string no_button_text = "No";
        }

        private AppInfo _appInfo;

        void Awake()
        {
            _dialog.SetActive(false);
            _touchGuardPanel.SetActive(false);
        }

        void Start()
        {
            if (_review.ShouldRequest())
            {
                _review.Request();
                return;
            }
            
            // 起動回数をカウントする
            int launchCount = PlayerPrefs.GetInt(HouseAd_LaunchAppCountKey, 0);
            launchCount += 1;
            PlayerPrefs.SetInt(HouseAd_LaunchAppCountKey, launchCount);
            PlayerPrefs.Save();
            Debug.Log($"Start launch_count={launchCount}");

            if (_debugRequestButton)
            {
                if (IsDebugMode)
                {
                    _debugRequestButton?.gameObject.SetActive(true);
                    _debugRequestButton?.onClick.AddListener(() =>
                    {
                        gameObject.SetActive(true);
                        StartCoroutine(RequestAppInfo());
                    });
                }
                else
                {
                    _debugRequestButton?.gameObject.SetActive(false);
                }
            }

            if (_connectionMode != ConnectionMode.Production && _debugRemoteText != null)
            {
                _debugRemoteText.gameObject.SetActive(true);
                _debugRemoteText.text = "接続先:Local";
            }
            
            // ボタンのイベントハンドラ登録
            _yesButton.onClick.AddListener(() =>
            {
                Debug.Log($"OpenURL={_appInfo.url}");
         //       var uri = new Uri( _appInfo.url );
         //       Application.OpenURL(uri.AbsoluteUri);
                Application.OpenURL(_appInfo.url);
                gameObject.SetActive(false);

            });
            _noButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });

            StartCoroutine(RequestAppInfo());
        }

        /// <summary>
        /// サーバーに相互送客情報を要求する
        /// </summary>
        /// <returns></returns>
        private IEnumerator RequestAppInfo()
        {
            // sample
            // Japanese
            // https://api.net-k.net/appinfo?bundle_identifier=com.netk.rice&os=iOS&lang=Japanese;
            // string URL = $"localhost:3000/appinfo?bundle_identifier={BundleIdentifier}&os={GetOS()}&lang={Application.systemLanguage}";
            var URL = GetAppInfoUrl();

            UnityWebRequest request = UnityWebRequest.Get(URL);

            yield return request.SendWebRequest();

            if (!OnReceiveAppInfo(request))
            {
                yield break;
            }

            bool isSucceeded = ShouldDisplayOwnAd();

            if (isSucceeded)
            {
                yield return StartCoroutine(RequestAppTexture());
            }
        }

        /// <summary>
        /// 最後に自社広告を表示してから、サーバーで指定した一定期間経過したか？
        /// </summary>
        private bool IsElapsedLastLaunchFromDisplayOwnAdTime()
        {
            // 初回はだいぶ昔に自社広告を表示したことにしておくので、かならず表示される
            string displayDate = PlayerPrefs.GetString(HouseAd_LastDisplayOwnAdTimeKey, "1999/01/01 12:34:56");

            var diffTimeSpan = DateTime.Now - DateTime.Parse(displayDate);
            if (diffTimeSpan.TotalMinutes > Double.Parse(_appInfo.display_time))
            {

                return true;
            }

            Debug.Log($"{_appInfo.display_time}分、時間が経過してないので表示しない");
            return false;
        }


        private bool OnReceiveAppInfo(UnityWebRequest request)
        {
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError("GetAppInfo Error" + request.error);
                _dialog.SetActive(false);
                return false;
            }

            Debug.Log(request.downloadHandler.text);
            _appInfo = JsonUtility.FromJson<AppInfo>(request.downloadHandler.text);
            return true;
        }

        /// <summary>
        /// 自社広告を表示するべきか？
        /// </summary>
        /// <returns></returns>
        private bool ShouldDisplayOwnAd()
        {
            if (IsDebugMode)
            {
                return true;
            }
            
            // サーバー設定が、表示する設定になっているか？
            if (Convert.ToInt64(_appInfo.is_open) == 0)
            {
                Debug.Log("表示設定が off になっている");
                return false;
            }

            // 自社広告を表示し始めるアプリの起動回数が、サーバーで設定された起動回数を以上か？
            if (PlayerPrefs.GetInt(HouseAd_LaunchAppCountKey) < Convert.ToInt64(_appInfo.start_show_launch_count))
            {
                Debug.Log($"launch_app_count={PlayerPrefs.GetInt(HouseAd_LaunchAppCountKey)}, start_show_launch_count={Convert.ToInt64(_appInfo.start_show_launch_count)}");
                return false;
            }


            if (!IsElapsedLastLaunchFromDisplayOwnAdTime())
            {
                return false;
            }

            return true;
        }

        private string GetAppInfoUrl()
        {
            string os = GetOs();
            string URL = $"https://api.net-k.net/appinfo?bundle_identifier={BundleIdentifier}&os={os}&lang={Application.systemLanguage}";
            if (_connectionMode == ConnectionMode.Local)
            {
                URL = $"localhost:3000/appinfo?bundle_identifier={BundleIdentifier}&os={os}&lang={Application.systemLanguage}";
            }

            return URL;
        }


        private string GetOs()
        {
            if (IsDebugMode)
            {
                if (_debugOS != "")
                {
                    return _debugOS;
                }
            }
            
            if (Application.platform == RuntimePlatform.Android)
            {
                // Android
                return "Android";
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // iOS
                return "iOS";
            }

            // return _debugOS;
            return "Unknown";
        }

        private IEnumerator RequestAppTexture()
        {
            Debug.Log($"GetAppTexture");
            // string url = "localhost:3000/appimage/" + _appInfo.imageid;
            var url = GetAppTextureUrl();

            Debug.Log($"GetAppTexture url = {url}");

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            OnReceiveAppTexture(www, url);
        }

        private void OnReceiveAppTexture(UnityWebRequest www, string url)
        {
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("GetAppTexture Error" + www.error + ", url=" + url);
                _dialog.SetActive(false);
                return;
            }

            Texture2D texture = ((DownloadHandlerTexture) www.downloadHandler).texture;
            ShowOwnAdDialog(texture);
        }

        private void ShowOwnAdDialog(Texture2D texture)
        {
            _touchGuardPanel.SetActive(true);
            
            // Debug.Log($"Texture w={texture.width}, h={myTexture.height}");
            _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            var titleText = _appInfo.title.Replace("\\n", Environment.NewLine);
            var descriptionText = _appInfo.description.Replace("\\n", Environment.NewLine);
           
            // non break space を打ち込む
            _titleText.text = titleText.Replace(" ", " ");
            _descriptionText.text = descriptionText.Replace(" ", " ");
            
            _yesButtonText.text = _appInfo.yes_button_text;
            _noButtonText.text = _appInfo.no_button_text;

            _dialog.SetActive(true);

            // 最後に表示した日時を更新する
            PlayerPrefs.SetString(HouseAd_LastDisplayOwnAdTimeKey, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        private string GetAppTextureUrl()
        {
            string url = "https://api.net-k.net/appimage/" + _appInfo.imageid;
            if (_connectionMode==ConnectionMode.Local)
            {
                url = "localhost:3000/appimage/" + _appInfo.imageid;
            }

            return url;
        }
    }
}
