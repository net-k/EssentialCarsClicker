using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SushiClicker;


public class BC_Click : MonoBehaviour {

	//this allows you to reference the bananas per click and the main banana display by dragging them into the inspector.
	// クリックあたりのバナナ数表示とメインのバナナ数表示をインスペクターからドラッグして設定します
	public UnityEngine.UI.Text bpc;
	public UnityEngine.UI.Text bananaDisplay;
    //public UnityEngine.UI.Text guiText;
    public GameObject popupText;
    public GameObject popupLoc;
    private Canvas _popupCanvas;

    public GameObject StatsWindow;


	//this is your main total of bananas
	// バナナの現在総数
	public double bananas = 0;

    //this will keep track of how many trillions of cookies we made TOTAL.
    // 1兆単位のバナナ獲得総数を記録します
    public double bananaTrillionCount = 0;

    //this will be our total Count, and every time it goes over a trillion
    //we will clear it and increment the trillion counter
    // this will make it easier when working out how many golden bananas the user gets.
    // 1兆未満の端数バナナ獲得数。1兆を超えると bananaTrillionCount に繰り上げます（ゴールデンバナナ計算に使用）
    public double BananaCount = 0;

	// クリックあたりのバナナ獲得量
	public double bananasPerClick = 1;

	/// <summary>
	/// クリックあたりの獲得量を再計算します。
	/// ベース(1) + 全アップグレードのパワー合計。
	/// </summary>
	public void RefreshBananasPerClick()
	{
		// もし bcbps が null ならここで探す
		if (bcbps == null)
		{
			var bpsGo = GameObject.Find("BPSmanager");
			if (bpsGo != null) bcbps = bpsGo.GetComponent<BC_bananaPerSec>();
		}

		double power = 1.0;
		// ダイアログが閉じていても全アップグレードを見つける（非アクティブを含む）
		var allUpgrades = Resources.FindObjectsOfTypeAll<BC_upgradeManager>();
		
		foreach (var upgrade in allUpgrades)
		{
			if (upgrade != null)
			{
				// 同期処理
				upgrade.SyncData();
				power += (double)upgrade.count * upgrade.clickPower;
			}
		}

		bananasPerClick = power;
		Debug.Log($"BC_Click.RefreshBananasPerClick: New BPC = {bananasPerClick}, total upgrades = {allUpgrades.Length}");
	}


    public double PrestigeLevel = 0;
    public double GoldenBananas = 0;

    public BC_bananaPerSec bcbps;

    // 自動保存用タイマー（5秒ごとに保存）
    private float _saveTimer = 0f;
    private const float SAVE_INTERVAL = 5f;

    public double GetCubeRoot( double number)
    {
        double root = (System.Math.Pow(number, (1.0 / 3.0)));
        return Math.Floor(root);

    }

    public void PrintCubeRoot()
    {
        double root = (System.Math.Pow(30, (1.0 / 3.0)));
        Debug.Log(Math.Floor(root));

    }



    void Start()
    {
        var bpsGo = GameObject.Find("BPSmanager");
        if (bpsGo != null)
            bcbps = bpsGo.GetComponent<BC_bananaPerSec>();
        else
            Debug.LogError("BC_Click: BPSmanager が見つかりません。シーン構成を確認してください。");

        // 起動時に保存されたアップグレードを反映する
        RefreshBananasPerClick();

        // セーブデータから前回のバナナ数を読み込む
        LoadBananaData();
    }


    //this updates your total bananas constantly. eg every frame.
    // 毎フレーム呼ばれ、バナナ総数の表示を常に更新します
    void Update()
	{
        // バナナ総数の表示は小数切り捨て（floor）で表示する
        bananaDisplay.text = BC_currencyConverter.Instance.GetCurrencyIntoString (Math.Floor(bananas), false, false);
		bpc.text = BC_currencyConverter.Instance.GetCurrencyIntoString (bananasPerClick, false, true);

        // 自動保存タイマー（5秒ごと）
        _saveTimer += Time.deltaTime;
        if (_saveTimer >= SAVE_INTERVAL)
        {
            SaveBananaData();
            _saveTimer = 0f;
        }
	}



    /// <summary>
    /// アプリがバックグラウンドに移行する時、またはフォーカスが失われる時に呼ばれる
    /// </summary>
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // ポーズされたので（バックグラウンド移行）、バナナ数を保存
            SaveBananaData();
            Debug.Log("BC_Click: OnApplicationPause - バナナ数を保存しました");
        }
    }

    /// <summary>
    /// アプリ終了時に呼ばれる
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveBananaData();
        Debug.Log("BC_Click: OnApplicationQuit - バナナ数を保存しました");
    }

    /// <summary>
    /// セーブデータからバナナ数を読み込む
    /// </summary>
    private void LoadBananaData()
    {
        bananas = SushiSaveDataManager.Instance.LoadBananaCount(0);
        bananaTrillionCount = SushiSaveDataManager.Instance.LoadBananaTrillions(0);
        // BananaTotal（累積総数）が保存されていればそちらを使う。なければ bananas で代替
        double savedTotal = SushiSaveDataManager.Instance.LoadBananaTotal(-1);
        BananaCount = savedTotal >= 0 ? savedTotal : bananas;
        PrestigeLevel = SushiSaveDataManager.Instance.LoadPrestigeLevel(0);
        GoldenBananas = SushiSaveDataManager.Instance.LoadGoldBananas(0);
        bananasPerClick = SushiSaveDataManager.Instance.LoadBananasPerClick(1);
        
        Debug.Log($"BC_Click.LoadBananaData: bananas={bananas}, trillions={bananaTrillionCount}, prestige={PrestigeLevel}");
    }

    /// <summary>
    /// バナナ数をセーブデータに保存する
    /// </summary>
    private void SaveBananaData()
    {
        SushiSaveDataManager.Instance.SaveBananaCount(bananas);
        SushiSaveDataManager.Instance.SaveBananaTrillions(bananaTrillionCount);
        SushiSaveDataManager.Instance.SaveBananaTotal(BananaCount);
        SushiSaveDataManager.Instance.SaveBananasPerClick(bananasPerClick);
        
        Debug.Log($"BC_Click.SaveBananaData: bananas={bananas}, trillions={bananaTrillionCount}");
    }


    public void AddBananas(double bananasToAdd)
    {
        bananas += bananasToAdd;
        BananaCount += bananasToAdd;

        // 1兆を超えた場合は bananaTrillionCount に繰り上げ処理を行います
        if (BananaCount >= 1e12)
        {
            double trillions = Math.Floor(BananaCount / 1e12);
            bananaTrillionCount += trillions;
            BananaCount -= trillions * 1e12;
            Debug.Log($"AddBananas: Trillion Rollover! New total trillions: {bananaTrillionCount}");
        }
    }

    /// <summary>
    /// 累積バナナ総数でレベルアップを確認する。
    /// Clicked() などの主要な操作の最後で一度だけ呼び出す。
    /// AddBananas() では呼ばない（報酬などで複数回呼ぶと重複判定される）。
    /// </summary>
    public void CheckLevelUp()
    {
        var totalBananas = (bananaTrillionCount * 1e12) + BananaCount;
        PlayerLevelManager.Instance.CheckLevelUp(totalBananas);
    }

    //pretty simply when you click this happens and adds the bananas perclick to your total.
    // クリック時に呼ばれ、クリックあたりのバナナ数を総数に加算します
	public void Clicked()
	{
        //check if prestiged
        // プレステージ済みの場合はボーナスを加算します
	    if (PrestigeLevel >= 1)
	    {
            var num = (bananasPerClick * (PrestigeLevel / 100));
            //give bonus
            // プレステージボーナスを加えて加算します
            AddBananas(bananasPerClick + num);

        }
	    else
	    {
            //calls the function above to add the bananas without bonus.
            // ボーナスなしでバナナを加算します
            AddBananas(bananasPerClick);
        }


	    // クリック数を表示するポップアップを生成する
        if (popupText == null || popupLoc == null) return;
        if (_popupCanvas == null) _popupCanvas = popupLoc.GetComponentInParent<Canvas>();
        Instantiate(popupText, popupLoc.transform.position, Quaternion.identity, _popupCanvas != null ? _popupCanvas.transform : null);

        //this is the old way i use to show the message.
        // これはメッセージを表示する古い方法です（現在は上記の方法に置き換え済み）
        //StartCoroutine(ShowMessage("+" + bananasPerClick, 1));


        // レベルアップをチェック
        CheckLevelUp();

	}

    //this has been replaced in the new version with a better method of showing the popup, but i have left it here so you learn the same way i did.
    // 新バージョンではより良い方法に置き換えられましたが、学習のために残してあります
	IEnumerator ShowMessage (string message, float delay) {
		GetComponent<Text>().text = message;
		GetComponent<Text>().enabled = true;
		yield return new WaitForSeconds(delay);
		GetComponent<Text>().enabled = false;
	}

    public void OpenStatsWindow()
    {
        // PrestigeButton の interactable 制御は StatsPanelPresenter が OnEnable で行う
        StatsWindow.SetActive(true);
    }

    public void CloseStatsWindow()
    {
        StatsWindow.SetActive(false);
    }


    public void BuyPrestige()
    {

        var gb = GetCubeRoot(bananaTrillionCount);
        if (gb >= 1)
        {
            // リセットより先にプレステージレベルを加算・保存する
            // （PrestigeclearGame() 内で例外が発生しても値が失われないようにする）
            PrestigeLevel += gb;
            GoldenBananas += gb;
            SushiSaveDataManager.Instance.SavePrestigeLevel(PrestigeLevel);
            SushiSaveDataManager.Instance.SaveGoldBananas(GoldenBananas);

            // ゲームの進行状況をリセットします
            bcbps.PrestigeclearGame();

            Debug.Log("Game Prestiged! - Golden bananas/Prestige Levels: " + gb);



        }



    }

}