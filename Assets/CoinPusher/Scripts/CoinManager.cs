using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using App;
using Quiz.Infrastructure;

// Add namespace for CoinManager

namespace CoinPusher.Core
{
	public class CoinManager : MonoBehaviour {

		[Header("DEBUG ONLY")]
		public bool deleteDataOnStart = false;

		[Header("Coin Manager Settings")]

		// Our save manager
		// セーブマネージャー
		public SaveManager saveManager;

		// Find our level manager
		// レベルマネージャーを探す
		public LevelManager levelManager;

		// This is the amount of coins the player currently has. This is pulled from the saved data.
		// プレイヤーが現在持っているコインの量。これは保存されたデータから取得されます。
		public long currentCoinTotal = 0;

		// This is how many coins the user can drop at once without hitting a cool down
		// ユーザーがクールダウンなしで一度に落とせるコインの数
		public int maxCoinDrop = GameConstants.MaxCoinDrop;

		// This is used to track internally how many coins they have dropped against the maxCoinDrop
		// maxCoinDropに対してどれだけのコインを落としたかを内部的に追跡するために使用されます
		[HideInInspector] public int currentCoinCount;

		[Header("Coin Regen and Hopper Settings")]

		// How often do they get a free coin (in seconds)
		// 無料のコインをどれくらいの頻度で入手できるか（秒単位）
	//	public int nextAvailableCoinSeconds = 30;
		private int nextAvailableCoinSeconds = 30 * 60 * 60; // 30 hours for testing

		// This is used internally for tracking the time that has past
		// 経過時間を追跡するために内部的に使用されます
		private float timeLeftForNextCoin;

		// This is the label in the UI for the current available coins
		// 現在利用可能なコインのUIラベル
		public Text currentCoinTotalLabel;

		// This is the label in the UI for the amount of time until next coin
		// 次のコインまでの時間のUIラベル
		public Text nextCoinInSecondsLabel;

		// This is how fast the user will get a new coin back in seconds
		// ユーザーが新しいコインを取り戻す速さ（秒単位）
		public float coinRefillRate = 0.5f;

		// This is the multiplier that they will get coins, it is multiplied against the level number
		// コインを獲得する倍率。レベル番号と掛け合わされます
		public int refillMultiplier = 2;

		// This is used to track how many coins, equal to their level number, the user can regen over time
		// ユーザーが時間の経過とともに回復できるコインの数（レベル番号に等しい）を追跡するために使用されます
		public int currentLevelCountAmount = 1;

		// This is used internally to track the amount of time that has lapse so we can refill their coin hopper
		// コインホッパーを補充できるように、経過時間を追跡するために内部的に使用されます
		private float timeLeftRefill;

		// Is this our first touch? If so, we disable the touch here area and enabler/disabler
		// 最初のタッチですか？もしそうなら、ここのタッチエリアと有効化/無効化を無効にします
		private bool isFirstTouch = true;

		// The internal timer to control flashing
		// 点滅を制御するための内部タイマー
		private float timeLeftFlasher;

		[Header("UI Settings")]

		// The rate at which we flash
		// 点滅する速度
		public float touchHereFlashRate = 1.0f;

		// The gameobject we want to enable/disable (flash). This is the touch here area label.
		// 有効/無効（点滅）にしたいゲームオブジェクト。これは「ここにタッチ」エリアのラベルです。
		public GameObject touchHereLabel;

		// How much cash the player has earned
		// プレイヤーが稼いだ現金の額
		public long playerCash = 0;

		// The cash UI label
		// 現金UIラベル
		public Text playerCashLabel;

		// This is the area that shows the current picked up coin value
		// 現在拾ったコインの値を表示するエリア
		public Text coinCounterLabel;

		// This is used to clear out the coin counter label after a coin is caught
		// コインがキャッチされた後にコインカウンターラベルをクリアするために使用されます
		[Range(0f, 3f)]
		public float coinCounterLabelTimeout = 1f;

		// This is set externally to see if we can spawn a coin, this is used for any popup windows to disable spawning
		// コインをスポーンできるかどうかを確認するために外部から設定されます。これは、スポーンを無効にするためのポップアップウィンドウに使用されます
		public bool coinSpawnerReady = true;

		[Header("Particle Reward Systems")]
		public ParticleSystem coinRewardParticleSystem;

		[Header("Spawnable Coins & Items")]
		public GameObject[] allSpawnableCoinsItems;    // This is the array that holds all objects we can spawn
		// スポーン可能なすべてのオブジェクトを保持する配列です

		// Use this for initialization
		// 初期化に使用します
		void Start ()
		{
			// DEBUG ONLY when set in inspector
			// インスペクターで設定されているかつ、DEBUG_DELETE_DATA が有効の場合のみデバッグ
			if ( deleteDataOnStart )
			{
				#if DEBUG_DELETE_DATA
				// This will delete all of the saved data, only use this for DEBUG!!
				// これにより、保存されたすべてのデータが削除されます。デバッグにのみ使用してください！！
				saveManager.deleteAllSavedGameData();
				Debug.LogError("Delete Data On Start is set to true, all saved data has been deleted, make sure to set this to false after debugging!");
				#endif
			}
		#if false	
			//Check for a saved game, if so, load the last coin amount
			// 保存されたゲームを確認し、存在する場合は最後のコイン量をロードします
			if( saveManager.checkForSavedGame() )
			{
				// Load the data
				// データをロードします
				saveManager.loadData();
			}
			else
			{
				// Set this to the beginning amount since they have not played yet
				// まだプレイしていないため、これを開始量に設定します
				currentCoinTotal = startCoinTotal;	

				// On first play, just set up the defaults. Once we increase a level, this is handled in the update level function in LevelManager
				// 初回プレイ時は、デフォルトを設定するだけです。レベルが上がると、これはLevelManagerのレベル更新関数で処理されます
				currentLevelCountAmount *= refillMultiplier;
			}
		#endif
			currentCoinTotal = CoinSaveDataManager.Instance.LoadCoin();
			
			// Load saved coins
			// 保存されたコインをロードします
			saveManager.loadSavedCoinData();

			// Set up the time left timer with the defined amount they set in the inspector
			// インスペクターで設定された定義量で残り時間タイマーを設定します
			timeLeftForNextCoin = nextAvailableCoinSeconds;

			// Set up the timer for later
			// 後で使用するためにタイマーを設定します
			timeLeftRefill = coinRefillRate;

			// Set up the amount of coins they have at the beginning in their hopper
			// ホッパー内の開始時に持っているコインの量を設定します
			currentCoinCount = maxCoinDrop;

			// Set up the timer for the touch here flasher
			// 「ここにタッチ」フラッシャーのタイマーを設定します
			timeLeftFlasher = touchHereFlashRate;
		}
		
		void Update ()
		{
			// Update the label with the amount of coins they have
			// 持っているコインの量でラベルを更新します
			currentCoinTotalLabel.text = currentCoinTotal.ToString();

			// Update the label with how many seconds are left to next free coin, convert the timer to an int to string
			// 次の無料コインまでの残り秒数でラベルを更新し、タイマーをintからstringに変換します
			nextCoinInSecondsLabel.text = Mathf.FloorToInt(timeLeftForNextCoin).ToString();

			// Update the players cash label
			// プレイヤーの現金ラベルを更新します
			playerCashLabel.text = playerCash.ToString();

			// This is our timer that gives a free coin after X seconds
			// これはX秒後に無料コインを与えるタイマーです
			//nextAvailableCoinGenerator();

			// Run the timer to refill the coin hopper
			// コインホッパーを補充するためにタイマーを実行します
			refillCoinHopper();

			// Check to see if this is our first touch, if so, we display the touch here area
			// これが最初のタッチかどうかを確認し、そうであれば「ここにタッチ」エリアを表示します
			checkFirstTouch();
		}

		/// <summary>
		/// Check to see if this is our first touch, if so, display the touch area. If not, disable the touch area and the timer that flashes it.
		/// これが最初のタッチかどうかを確認し、そうであればタッチエリアを表示します。そうでない場合は、タッチエリアとそれを点滅させるタイマーを無効にします。
		/// </summary>
		void checkFirstTouch()
		{
			// If this is the first touch, set it inactive
			// これが最初のタッチの場合、非アクティブに設定します
			if( !isFirstTouch )
			{
				touchHereLabel.SetActive(false);
			}
			else if( isFirstTouch )
			{
				// Subtract some time
				// 時間を減算します
				timeLeftFlasher -= Time.deltaTime;

				// If time has run out
				// 時間切れの場合
				if( timeLeftFlasher < 0.0f )
				{
					touchHereLabel.SetActive(!touchHereLabel.activeInHierarchy);

					// Set up the disable rate again, aka, reset the time
					// 無効化レートを再度設定します。つまり、時間をリセットします
					timeLeftFlasher = touchHereFlashRate;
				}
			}
		}

		/// <summary>
		/// This is our timer that will refill their hopper for more coins
		/// これは、より多くのコインのためにホッパーを補充するタイマーです
		/// </summary>
		void refillCoinHopper()
		{
			// タップ中（Fire1が押されている間）はリフィルしない
			if (Input.GetButton("Fire1"))
			{
				return;
			}

			timeLeftRefill -= Time.deltaTime;

			if( timeLeftRefill < 0.0f )
			{
				// Add a coin since we passed our timer
				// タイマーを経過したのでコインを追加します
				if( currentCoinCount < maxCoinDrop )
				{
					// Bump up the coin amount they have dropped
					// 落としたコインの量を増やします
					currentCoinCount++;

					// Reset the timer back to the refill rate
					// タイマーを補充レートにリセットします
					timeLeftRefill = coinRefillRate;
				}
			}
		}
		
		/// <summary>
		/// This gives the user a free coin after X seconds
		/// これにより、X秒後にユーザーに無料のコインが与えられます
		/// </summary>
		void nextAvailableCoinGenerator()
		{
			timeLeftForNextCoin -= Time.deltaTime;

			if( timeLeftForNextCoin < 0.0f )
			{
				// Do we still have coins to spawn?
				// スポーンするコインはまだありますか？
				if( currentLevelCountAmount > 0 )
				{	
					// Spawn a coin
					// コインをスポーンします
					currentCoinTotal++;	

					// Remove one coin from our level amount for regeneration of coins
					// コインの再生のためにレベル量からコインを1つ削除します
					currentLevelCountAmount--;
				}

				// Reset the timer
				// タイマーをリセットします
				timeLeftForNextCoin = nextAvailableCoinSeconds;	
			}
		}

		/// <summary>
		/// This is called from the LevelManager whenever we go up an entire level. This resets the amount of coins the player
		/// gets per level for free.
		/// これは、レベル全体が上がるたびにLevelManagerから呼び出されます。これにより、プレイヤーがレベルごとに無料で入手できるコインの量がリセットされます。
		/// </summary>
		public void resetCoinLevelGenAmount()
		{
			// Save back the current level count amount, this is multiplied by the multplier to give users X coins for free per level
			// 現在のレベルカウント量を保存します。これは、ユーザーにレベルごとにXコインを無料で提供するために乗数で乗算されます
			currentLevelCountAmount = (int)levelManager.currentLevel * refillMultiplier;
		}

		/// <summary>
		/// Check to see if we can spawn a coin or not
		/// コインをスポーンできるかどうかを確認します
		/// </summary>
		/// <returns><c>true</c>, if current total of coins is > 0<c>false</c> otherwise.</returns>
		public bool canSpawnCoin()
		{		
			// Make sure the user has enough coins and enough coins in the 'hopper' aka on deck aka the amount that ticks down each coin drop
			// ユーザーが十分なコインを持っており、「ホッパー」（デッキ上、つまり各コインドロップで減少する量）に十分なコインがあることを確認します
			return (currentCoinTotal > 0) && (currentCoinCount > 0) && (coinSpawnerReady) ? true : false;
		}

		/// <summary>
		/// This function removes a coin from the total the player has. This is called from the CoinSpawner.
		/// この関数は、プレイヤーが持っている合計からコインを削除します。これはCoinSpawnerから呼び出されます。
		/// </summary>
		public void removeCoin()
		{
			// Check to see if this is our first coin, if so, mark it now false since this is not our first coin
			// これが最初のコインかどうかを確認し、そうであれば、これが最初のコインではないため、falseとしてマークします
			if( isFirstTouch ) isFirstTouch = false;

			currentCoinCount--;			// Remove one coin from their hopper // ホッパーからコインを1つ削除します
			currentCoinTotal--;			// Remove one coin // コインを1つ削除します
			
			// Save the coin total immediately
			CoinSaveDataManager.Instance.SaveCoin(currentCoinTotal);
		}

		/// <summary>
		/// This is the function that will add a coin to their total coins
		/// これは、合計コインにコインを追加する関数です
		/// </summary>
		/// <param name="coinType">Coin type.</param>
		public void addCoin(int coinValue, bool isReward)
		{
			// Add the coin value they got to the score
			// 獲得したコインの値をスコアに追加します
			currentCoinTotal += coinValue;	
			CoinSaveDataManager.Instance.SaveCoin(currentCoinTotal);

			// If this is a reward coin, show them some fancy particles
			// これが報酬コインの場合、派手なパーティクルを表示します
			if( isReward )
			{
				coinRewardParticleSystem.Play();
				coinRewardParticleSystem.Stop();
			}

			// Fire up the coroutine
			// コルーチンを起動します
			StartCoroutine (addCoinCounterLabelUpdate (coinValue));
		}

		IEnumerator addCoinCounterLabelUpdate(int coinValue)
		{
			// Show the value of the coin they picked up!
			// 拾ったコインの値を表示します！
			coinCounterLabel.text = coinValue.ToString ();

			// Wait
			// 待機します
			yield return new WaitForSeconds(coinCounterLabelTimeout);

			// Clear out label
			// ラベルをクリアします
			coinCounterLabel.text = "";
		}

		/// <summary>
		/// Called to add cash to the player for buying things later
		/// 後で物を買うためにプレイヤーに現金を追加するために呼び出されます
		/// </summary>
		/// <param name="amount">Amount of cash to add</param>
		public void addCash( int amount )
		{
			// Add the player their cash
			// プレイヤーに現金を追加します
			playerCash += amount;

			saveManager.saveData();
		}

		/// <summary>
		/// This rewards the user some coins
		/// これにより、ユーザーにいくつかのコインが報酬として与えられます
		/// </summary>
		/// <param name="amount">Amount of coins to reward the user</param>
		public void addRewardCoin(int rewardAmount)
		{
			// Add the coin
			// コインを追加します
			addCoin(rewardAmount, true);

			// Thank the user
			// ユーザーに感謝します
		}

		/// <summary>
		/// This rewards the user some cash
		/// これにより、ユーザーにいくつかの現金が報酬として与えられます
		/// </summary>
		/// <param name="amount">Amount of cash to reward the user</param>
		public void addRewardCash(int rewardAmount)
		{
			// Add the cash
			// 現金を追加します
			addCash(rewardAmount);
		}

		/*
		public float pausedTime;
		public float unpausedTime;
		void OnApplicationPause(bool pauseStatus)
		{
			// If we're paused, log what time we paused
			if( pauseStatus )
			{
				// Save the time we paused
				Debug.Log("Pause Time = " + DateTime.Now.ToString());
			}
			else if( !pauseStatus )
			{
				Debug.Log("Unpause Time = " + DateTime.Now.ToString());
			}
		}
		*/
	}
}
