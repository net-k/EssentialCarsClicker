using UnityEngine;
using System.Collections;
using System;
using SushiClicker; // SushiSaveDataManagerを使用するため

/// <summary>
/// Manages bananas-per-second calculation, auto-click, and save/load processing.
/// バナナ毎秒（BPS）の計算、オートクリック、セーブ/ロード処理を管理するクラス。
/// </summary>
public class BC_bananaPerSec : MonoBehaviour {

	// 現在の日時 / Current date
	DateTime currentDate;
	// 前回アプリを閉じた日時 / Date when the app was last closed
	DateTime oldDate;
	// 経過分数 / Elapsed minutes
	int minutes;
	// 経過秒数 / Elapsed seconds
	int seconds;
	// 経過時間 / Elapsed hours
	int hours;
	// 経過日数 / Elapsed days
	int days;
	// 合計経過秒数 / Total elapsed seconds
	float totalSeconds;
	// オフライン中に得られたバナナ数（収益）/ Revenue earned while offline
	double revenue;

    // セーブ可能かどうかのフラグ（loadGame() 完了前の誤セーブを防ぐ）
    // Flag to allow saving (prevents erroneous saves before loadGame() completes)
    public bool canISave = false;

    // セーブ時に表示するポップアップテキストのプレハブ / Popup text prefab shown on save
    public GameObject popupText;
    // ポップアップを表示する位置のGameObject / GameObject defining the popup spawn location
    public GameObject popupLoc;

    // allows you to refer to the Bananas per second display in the inspector.
    // インスペクターでバナナ毎秒の表示テキストを設定する
    public UnityEngine.UI.Text BpsDisplay;
    // ゴールデンバナナ数の表示テキスト / Golden banana count display text
    public UnityEngine.UI.Text GBananasDisplay;
    // プレステージレベルの表示テキスト / Prestige level display text
    public UnityEngine.UI.Text PrestigeDisplay;
    // 最後にプレイしてからの経過時間を表示するテキスト / Text showing time elapsed since last play
	public UnityEngine.UI.Text TimePassed;
	// オフライン収益を表示するHolder GameObject / Holder GameObject for offline revenue display
	public GameObject revenueHolder;
	// オフライン収益を受け取るボタン / Button to collect offline revenue
	public GameObject revenueButton;
	// オフライン収益のパネル / Offline revenue panel
	public GameObject revenuePanel;

	// reference the Click.cs file to access its functions.
	// Click.csスクリプトへの参照
	public BC_Click click;

	// reference the upgradeManager.cs file to access its functions.
	// upgradeManager.csスクリプトへの参照
	public BC_upgradeManager upgrademanager;

	// creates an array to hold the items that you own, which helps as we need to calculate how many bananas per second we are calculating.
	// 所持しているアイテムの配列（BPS計算に使用）
	public BC_ItemManager[] items;

	// creates an array to hold the upgrades that you own, which helps as we need to calculate how many bananas per click we have.
	// 所持しているアップグレードの配列（クリックあたりのバナナ計算に使用）
	public BC_upgradeManager[] upgrades;

    // デバッグ用タイマー
    private float _bpsDebugTimer = 0f;


	/// <summary>
	/// This starts as soon as the application starts.
	/// アプリ起動時に一度だけ呼ばれる初期化処理。
	/// セーブデータが存在する場合はロードし、オートティックのコルーチンを開始する。
	/// </summary>
	void Start()
	{
        // プレステージデータは常にロードする
        click.PrestigeLevel = SushiSaveDataManager.Instance.LoadPrestigeLevel(0);
        click.GoldenBananas = SushiSaveDataManager.Instance.LoadGoldBananas(0);

        // バナナ数が0でもオフライン収益がある可能性があるため、常にロードを試みる
        loadGame();

		// オートティックコルーチンを開始する
		StartCoroutine (AutoTick ());

        // デバッグ: アイテム情報をログ出力
        DebugLogItems();

        // loadGame() 完了後にセーブを許可する
        canISave = true;
    }

    
    /// <summary>
    /// アイテム情報をコンソールに出力するデバッグ用メソッド
    /// </summary>
    private void DebugLogItems()
    {
        Debug.Log("=== Item Information ===");
        
        // インスペクターで設定されていない場合のみ検索
        if (items == null || items.Length == 0)
        {
            FindAllItemsInScene();
        }
        
        if (items == null || items.Length == 0)
        {
            Debug.LogWarning("Items array is empty or null");
            return;
        }
        
        Debug.Log($"Total items used for calculation: {items.Length}");
        for (int i = 0; i < items.Length; i++)
        {
            BC_ItemManager item = items[i];
            if (item != null)
            {
                item.SyncData();
                Debug.Log($"{i+1} | {item.itemName} | count={item.count} | tick={item.tickValue}");
            }
        }
    }

    /// <summary>
    /// シーン内のすべての BC_ItemManager を検索して items にセットする（非アクティブ含む、プレハブ除く）
    /// </summary>
    private void FindAllItemsInScene()
    {
        var allItems = Resources.FindObjectsOfTypeAll<BC_ItemManager>();
        var sceneItems = new System.Collections.Generic.List<BC_ItemManager>();
        
        foreach (var item in allItems)
        {
            // プレハブ（Asset）を除外し、シーン内のオブジェクトのみを取得する
            if (item.gameObject.scene.name != null)
            {
                sceneItems.Add(item);
            }
        }
        items = sceneItems.ToArray();
        Debug.Log($"FindAllItemsInScene: Found {items.Length} scene items.");
    }

	/// <summary>
	/// This updates every frame and updates the amount of Bananas per second.
	/// </summary>
	void Update(){

		if (BpsDisplay != null)
			BpsDisplay.text = BC_currencyConverter.Instance.GetCurrencyIntoString (GetBananasPerSec (), true, false);

	}


    /// <summary>
    /// アイテムの所持数とティック値からバナナ毎秒を計算して返す。
    /// </summary>
    public double GetBananasPerSec(){

        double tick = 0.0;

        if (items == null || items.Length == 0)
        {
            FindAllItemsInScene();
        }

        if (items != null)
        {
            foreach (BC_ItemManager item in items) {
                if (item == null) continue;
                item.SyncData();
                tick += (double)item.count * item.tickValue;
            }
        }

        if (click.PrestigeLevel > 0)
        {
            return tick + (tick * (click.PrestigeLevel / 100));
        }
        else
        {
            return tick;
        }
	}

	/// <summary>
	/// バナナ毎秒の量をバナナ総数に加算する。
	/// </summary>
	public void AutoBananasPerSec(){

        double fullBps = GetBananasPerSec();
        double bpsValue = fullBps / 10;
        
        if (bpsValue > 0)
        {
            click.AddBananas(bpsValue);
            click.CheckLevelUp();
            
            _bpsDebugTimer += 0.10f;
            if (_bpsDebugTimer >= 1.0f)
            {
                Debug.Log($"AutoBananasPerSec: Added {bpsValue} (BPS={fullBps}), Total: {click.bananas}");
                _bpsDebugTimer = 0f;
            }
        }
        else
        {
            // BPSが0の場合のログ（間引き）
            _bpsDebugTimer += 0.10f;
            if (_bpsDebugTimer >= 5.0f)
            {
                Debug.Log("AutoBananasPerSec: BPS is 0.");
                _bpsDebugTimer = 0f;
            }
        }
	}


	/// <summary>
	/// 0.10秒ごとにオートティックを実行するコルーチン。
	/// </summary>
	IEnumerator AutoTick(){
		while (true) {
			AutoBananasPerSec();
			yield return new WaitForSeconds(0.10f);
		}
	}

	/// <summary>
	/// ゲームの状態を保存する。
	/// </summary>
	public void saveGame(){

        SushiSaveDataManager.Instance.SaveBananaCount(click.bananas);
        SushiSaveDataManager.Instance.SaveBananaTrillions(click.bananaTrillionCount);
        SushiSaveDataManager.Instance.SaveBananaTotal(click.BananaCount);
        SushiSaveDataManager.Instance.SaveGoldBananas(click.GoldenBananas);
        SushiSaveDataManager.Instance.SavePrestigeLevel(click.PrestigeLevel);

        SushiSaveDataManager.Instance.SaveCloseTime(System.DateTime.Now);

		double tickPerSec = GetBananasPerSec();
        SushiSaveDataManager.Instance.SaveTickPerSec(tickPerSec);

		click.RefreshBananasPerClick();
        SushiSaveDataManager.Instance.SaveBananasPerClick(click.bananasPerClick);

	    if (popupText && popupLoc)
	    {
            GameObject newObject = (GameObject)Instantiate(popupText, popupLoc.transform.position, Quaternion.identity);
            newObject.GetComponent<RectTransform>().anchoredPosition = popupLoc.GetComponent<RectTransform>().anchoredPosition;
        }
	}


	/// <summary>
	/// PlayerPrefsを全消去してゲームをリセットする。
	/// </summary>
	public void clearGame()
	{
        SushiSaveDataManager.Instance.DeleteAllData();

        click.bananas = 0;
        click.bananasPerClick = 1;
        click.BananaCount = 0;
        click.bananaTrillionCount = 0;
        click.PrestigeLevel = 0;
        click.GoldenBananas = 0;

		foreach (BC_ItemManager item in items) {
            if (item == null) continue;
			item.count = 0;
			item.cost = item.baseCost;
			item.IsUnlocked = false;
            if (item.itemCount != null) item.itemCount.text = item.count.ToString();
            item.SetSlider(0);
        }

		foreach (BC_upgradeManager upgrade in upgrades) {
            if (upgrade == null) continue;
			upgrade.count = 0;
			upgrade.cost = upgrade.baseCost;
			upgrade.IsUnlocked = false;
            upgrade.SetSlider(0);
        }
	}

    /// <summary>
    /// プレステージ用のゲームリセット処理。
    /// </summary>
    public void PrestigeclearGame()
    {
        SushiSaveDataManager.Instance.DeleteDataForPrestige();

        click.bananas = 0;
        click.bananasPerClick = 1;
        click.BananaCount = 0;
        click.bananaTrillionCount = 0;

        foreach (BC_ItemManager item in items)
        {
            if (item == null) continue;
            item.count = 0;
            item.cost = item.baseCost;
            item.IsUnlocked = false;
            if (item.itemCount != null) item.itemCount.text = item.count.ToString();
            item.SetSlider(0);
        }

        foreach (BC_upgradeManager upgrade in upgrades)
        {
            if (upgrade == null) continue;
            upgrade.count = 0;
            upgrade.cost = upgrade.baseCost;
            upgrade.IsUnlocked = false;
            upgrade.SetSlider(0);
        }
    }


    /// <summary>
    /// セーブデータをロードし、オフライン収益を計算する。
    /// </summary>
    public void loadGame(){

        click.bananas = SushiSaveDataManager.Instance.LoadBananaCount(0);
        click.bananaTrillionCount = SushiSaveDataManager.Instance.LoadBananaTrillions(0);
        click.BananaCount = SushiSaveDataManager.Instance.LoadBananaTotal(0);
        click.PrestigeLevel = SushiSaveDataManager.Instance.LoadPrestigeLevel(0);
        click.GoldenBananas = SushiSaveDataManager.Instance.LoadGoldBananas(0);

        currentDate = System.DateTime.Now;
        oldDate = SushiSaveDataManager.Instance.LoadCloseTime(currentDate);

		TimeSpan timeSpan = currentDate - oldDate;
		totalSeconds = (float)timeSpan.TotalSeconds;
		if (totalSeconds < 0) totalSeconds = 0;

		days = timeSpan.Days;
		hours = timeSpan.Hours;
		minutes = timeSpan.Minutes;
		seconds = timeSpan.Seconds;

		double savedBps = SushiSaveDataManager.Instance.LoadTickPerSec(0);
		double savedBPerClick = SushiSaveDataManager.Instance.LoadBananasPerClick(1);

		click.bananasPerClick = savedBPerClick;
		click.RefreshBananasPerClick();

		// オフライン報酬機能を一時的に無効化 / Temporarily disable offline revenue
		revenue = 0; // (savedBps * totalSeconds);

		/* 
		if (revenue > 0) {
			revenueHolder.SetActive (true);
			revenueButton.SetActive (true);
			revenuePanel.SetActive (true);
			TimePassed.text = I2.Loc.LocalizationManager.GetTermTranslation("key_RevenueTrackerPanel_Text")
				.Replace("\\n", "\n")
				.Replace("{days}",    days.ToString())
				.Replace("{hours}",   hours.ToString())
				.Replace("{minutes}", minutes.ToString())
				.Replace("{seconds}", seconds.ToString())
				.Replace("{bananas}", BC_currencyConverter.Instance.GetCurrencyIntoString(revenue, false, false));
		}
		*/
	}


	public void ClearRevenue()
	{
        click.AddBananas(revenue);
        revenue = 0;
		revenueHolder.SetActive (false);
		revenueButton.SetActive (false);
		revenuePanel.SetActive (false);
	}


	void OnApplicationQuit() {
        saveGame();
	}

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (canISave)
            {
                saveGame();
            }
        }
    }

}
