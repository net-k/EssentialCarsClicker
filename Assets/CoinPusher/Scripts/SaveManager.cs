using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using CoinPusher.Core;
using App; // CoinSaveDataManagerを使用するために追加

public class SaveManager : MonoBehaviour {

	// This is the list of things that need data to be saved
	// これは、保存する必要があるデータのリストです
	public CoinManager coinManager;
	public LevelManager levelManager;
	public CollectableManager collectableManager;

    // Objects for spawning new / saved coins
    // 新規/保存されたコインを生成するためのオブジェクト
    public GameObject coinPlayField;
    public GameObject coinPreloadedTable;

    // Our saved coins that are searlized
    // シリアライズされた保存済みのコイン
    public Dictionary<string, CoinData> savedCoins;

    public void Awake()
    {
        Debug.Log("SaveManager Awake called. Instance ID: " + GetInstanceID()); // 追加
        // Init our dictionary of serialized data
        // シリアライズされたデータのディクショナリを初期化します
        if (savedCoins == null) // nullチェックを追加
        {
            savedCoins = new Dictionary<string, CoinData>();
        }

        // Used to continously save the coin data
        // コインデータを継続的に保存するために使用されます
        InvokeRepeating("saveCoinData", 1.0f, 5.0f);
    }

    /// <summary>
    /// Loads the data.
    /// </summary>
    // データをロードします
    public void loadData()
	{
		// Get data for the CoinManager
		// CoinManagerのデータを取得します
		// CoinSaveDataManagerからロードするように変更
		coinManager.currentCoinTotal = CoinSaveDataManager.Instance.LoadCoin();
		coinManager.playerCash = CoinSaveDataManager.Instance.LoadPlayerCash();

		// Get data for the LevelManager
		// LevelManagerのデータを取得します
		if( PlayerPrefs.HasKey("currentLevel") )
			levelManager.currentLevel = PlayerPrefs.GetFloat ("currentLevel");

		if( PlayerPrefs.HasKey("currentLevelAmount") )
			levelManager.currentLevelAmount = PlayerPrefs.GetFloat ("currentLevelAmount");

		// Get the data for the CollectableManager
		// CollectableManagerのデータを取得します
		if (PlayerPrefs.HasKey ("collectables")) 
			collectableManager.inventory = PlayerPrefsSerialize<Dictionary<CoinEffect.Effect, int>>.Load ("collectables");
		else 
			collectableManager.inventory = new Dictionary<CoinEffect.Effect, int>();

    }

    /// <summary>
    /// Saves the data.
    /// </summary>
    // データを保存します
    public void saveData()
    {
        // Save data for the CoinManager
        // CoinManagerのデータを保存します
        // CoinSaveDataManagerに保存するように変更
        CoinSaveDataManager.Instance.SaveCoin(coinManager.currentCoinTotal);
        CoinSaveDataManager.Instance.SavePlayerCash(coinManager.playerCash);

        // Save data for the LevelManager
        // LevelManagerのデータを保存します
        PlayerPrefs.SetFloat("currentLevel", levelManager.currentLevel);
        PlayerPrefs.SetFloat("currentLevelAmount", levelManager.currentLevelAmount);

        // Save data for the CollectableManager
        // CollectableManagerのデータを保存します
        PlayerPrefsSerialize<Dictionary<CoinEffect.Effect, int>>.Save(collectableManager.collectableSaveName, collectableManager.inventory);

        saveCoinData();
    }

    /// <summary>
    /// Used for loading saved coin data
    /// </summary>
    // 保存されたコインデータをロードするために使用されます
    public void loadSavedCoinData()
    {
        // Load any saved coins on the table
        // テーブルに保存されているコインをロードします
        // If we have save data, load it
        // 保存データがある場合はロードします
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_CoinSaveData"))
        {
            // Disable the preloaded coin top
            // プレロードされたコイントップを無効にします
            coinPreloadedTable.SetActive(false);

            // Load the saved data
            // 保存されたデータをロードします
            savedCoins = PlayerPrefsSerialize<Dictionary<string, CoinData>>.Load(SceneManager.GetActiveScene().name + "_CoinSaveData");
            
            foreach (KeyValuePair<string, CoinData> coin in savedCoins)
            {
                // Get the object name we need to spawn
                // スポーンする必要があるオブジェクト名を取得します
                // Look for the first blank whitespace and (
                // 最初の空白と（を探します
                int index = coin.Value.objectName.IndexOf(" (");

                // If we found enough of a string to parse
                // 解析するのに十分な文字列が見つかった場合
                if (index > 0)
                {
                    // Pull out everything UP to the " (" part
                    // 「（」までのすべてを取り出します
                    coin.Value.objectName = coin.Value.objectName.Substring(0, index);
                }

                // Now search and destroy for (Clone)
                // （Clone）を検索して削除します
                index = coin.Value.objectName.IndexOf("(Clone)");

                // If we found enough of a string to parse
                // 解析するのに十分な文字列が見つかった場合
                if (index > 0)
                {
                    // Pull out everything UP to the "(Clone)" part
                    // 「（Clone）」までのすべてを取り出します
                    coin.Value.objectName = coin.Value.objectName.Substring(0, index);
                }

                // Loop through all available spawnables
                // 使用可能なすべてのスポーン可能オブジェクトをループします
                for (int i = 0; i < coinManager.allSpawnableCoinsItems.Length; i++)
                {
                    // If we find our gameobject name in the array of spawnable items / objects
                    // スポーン可能なアイテム/オブジェクトの配列にゲームオブジェクト名が見つかった場合
                    if (String.Compare(coinManager.allSpawnableCoinsItems[i].name, coin.Value.objectName) == 0)
                    {
                        // Save this spawnID index
                        // このspawnIDインデックスを保存します
                        coin.Value.spawnID = i;
                    }
                }

                // Spawn our new item
                // 新しいアイテムをスポーンします
                GameObject go = Instantiate(coinManager.allSpawnableCoinsItems[coin.Value.spawnID]);

                // Reparent it
                // 親を変更します
                go.transform.SetParent(coinPlayField.transform);

                // Adjust the position & rotation from the saved data
                // 保存されたデータから位置と回転を調整します
                go.transform.localPosition = new Vector3(coin.Value.posX, coin.Value.posY, coin.Value.posZ);
                go.transform.rotation = Quaternion.Euler(coin.Value.rotX, coin.Value.rotY, coin.Value.rotZ);
                
                // Update settings for this coin
                // このコインの設定を更新します
                go.GetComponent<CoinEffect>().coinData.id = coin.Value.id;
                go.GetComponent<CoinEffect>().coinData.coinValue = coin.Value.coinValue;
                go.GetComponent<CoinEffect>().coinData.typeOfCoin = coin.Value.typeOfCoin;
                go.GetComponent<CoinEffect>().coinData.objectName = coin.Value.objectName;
            }
        }
        else
        {
            // Show the stock preloaded coin table top
            // ストックのプレロードされたコイントップを表示します
            coinPreloadedTable.SetActive(true);
        }
    }

    /// <summary>
    /// Used for saving just coin data on the table
    /// </summary>
    // テーブル上のコインデータのみを保存するために使用されます
    public void saveCoinData()
    {
        if (savedCoins == null) // nullチェックを追加
        {
            savedCoins = new Dictionary<string, CoinData>();
        }
        // Save the seralized data
        // シリアライズされたデータを保存します
        PlayerPrefsSerialize<Dictionary<string, CoinData>>.Save(SceneManager.GetActiveScene().name + "_CoinSaveData", savedCoins);
    }

    public void addCoin(CoinData coinData)
    {
	    if (savedCoins == null)
	    {
		    Debug.LogWarning( "SaveManager.addCoin: savedCoins dictionary is null. Cannot add coin data.");
		    savedCoins = new Dictionary<string, CoinData>(); // nullの場合に初期化
	    }
	    
        // If we do not have this coin added already
        // このコインがまだ追加されていない場合
        if (savedCoins.ContainsKey(coinData.id) == false)
        {
            // Let's add it
            // 追加しましょう
            savedCoins.Add(coinData.id, coinData);
        }
    }

    public void updateCoin(CoinData coinData)
    {
	    if (savedCoins == null)
	    {
		    Debug.LogWarning( "SaveManager.updateCoin: savedCoins dictionary is null. Cannot update coin data.");
		    savedCoins = new Dictionary<string, CoinData>(); // nullの場合に初期化
	    }
        // Make sure we have the coin to update
        // 更新するコインがあることを確認します
        if (savedCoins.ContainsKey(coinData.id) == true)
        {
            // Let's update this coin since it has changed
            // 変更されたので、このコインを更新しましょう
            savedCoins[coinData.id] = coinData;
        }
    }

	/// <summary>
	/// Checks for saved game.
	/// </summary>
	// 保存されたゲームがあるかどうかを確認します
	/// <returns><c>true</c>, if for saved game was checked, <c>false</c> otherwise.</returns>
	public bool CheckForSavedGame()
	{
		// See what the return value is, if it is -1 we have no value
		// 戻り値が-1の場合は値がないことを確認します
		// CoinSaveDataManagerからロードするように変更
		return CoinSaveDataManager.Instance.ExistsSaveData();
	}

	/// <summary>
	/// Deletes all saved game data.
	/// </summary>
	// 保存されたすべてのゲームデータを削除します
	public void deleteAllSavedGameData()
	{
		PlayerPrefs.DeleteAll ();
		CoinSaveDataManager.Instance.DeleteAllCoinData(); // CoinSaveDataManagerのデータも削除
	}
}
