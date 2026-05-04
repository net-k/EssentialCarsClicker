using UnityEngine;
using System.Collections;
using CoinPusher.Core;

public class CoinSpawner : MonoBehaviour {

	// コインのプレイフィールド
	private Transform coinPlayField;

	// Unityエディタで設定可能なプレイ設定。
	// クリック/タッチ地点ではなく、スポーンエリア内でランダムにコインをスポーンさせるかどうか。
	public bool spawnRandomLocations = false;

	// 垂直ではなく水平面にコインをスポーンさせるか
	public bool spawnCoinHorizontal = false;

	[Header("Regular Coins")]
	// スポーンすべき通常のコインの配列
	public Transform[] coins;

	// アイテムの設定。コモン、レア、エピックの3つの配列を設定。エピックが最もレア。
	[Header("Common Coins")]
	public Transform[] common;
	[Header("Rare Coins")]
	public Transform[] rare;
	[Header("Epic Coins")]
	public Transform[] epic;
	[Header("Dice")]
	public Transform[] dice;

	
	void Awake()
	{
		// プレイフィールドを見つける
		coinPlayField = GameObject.FindWithTag("CoinsPlayField").GetComponent<Transform>();
	}

	void Start()
	{
		InitializeCoinPools();
	}

	private void InitializeCoinPools()
	{
		if (CoinPool.Instance == null)
		{
			Debug.LogError("CoinPool instance not found. Please add CoinPool script to a GameObject in the scene.");
			return;
		}

		const int regularCoinPoolSize = 30;
		const int specialCoinPoolSize = 5;

		foreach (var coinPrefabT in coins)
		{
			if (coinPrefabT != null) CoinPool.Instance.CreatePool(coinPrefabT.gameObject, regularCoinPoolSize);
		}
		foreach (var coinPrefabT in common)
		{
			if (coinPrefabT != null) CoinPool.Instance.CreatePool(coinPrefabT.gameObject, specialCoinPoolSize);
		}
		foreach (var coinPrefabT in rare)
		{
			if (coinPrefabT != null) CoinPool.Instance.CreatePool(coinPrefabT.gameObject, specialCoinPoolSize);
		}
		foreach (var coinPrefabT in epic)
		{
			if (coinPrefabT != null) CoinPool.Instance.CreatePool(coinPrefabT.gameObject, specialCoinPoolSize);
		}
		foreach (var coinPrefabT in dice)
		{
			if (coinPrefabT != null) CoinPool.Instance.CreatePool(coinPrefabT.gameObject, specialCoinPoolSize);
		}
	}

	public void spawnCoin(Vector3 position)
	{
		Vector3 spawnLocation;



		if( spawnRandomLocations )
		{
			spawnLocation = new Vector3(transform.position.x,
			                                    transform.position.y,
			                                    Random.Range (transform.position.z - (transform.localScale.z / 2), transform.position.z + (transform.localScale.z / 2)));
		}
		else
		{
			// クリック/タップ位置でスポーン
			// x = z, y = y, z = x
			spawnLocation = new Vector3(transform.position.x, transform.position.y, position.z);
		}

		// ここでレアリティに基づいてスポーンするアイテムを決定します。
		// コモンアイテムを取得するには: 1 - 5
		// レアアイテムを取得するには: 1 - 15
		// エピックアイテムを取得するには: 1 - 30
		/*
		int findItem = 4;
		int findCommon = Random.Range (1, 5);
		int findRare = Random.Range (1, 30);
		int findEpic = Random.Range (1, 45);
	
		if( findItem == findCommon )
		{
			if( common.Length != 0 )
			{
				Quaternion coinRot = common[0].rotation;
				if (spawnCoinHorizontal)
				{
					coinRot = new Quaternion(180, 0, 0, 0);
				}

				CoinPool.Instance.Get(common[Random.Range (0, common.Length)].gameObject, spawnLocation, coinRot);
			}
		}
		
		if( findItem == findRare )
		{
			if( rare.Length != 0 )
			{
				Quaternion coinRot = rare[0].rotation;
				if (spawnCoinHorizontal)
				{
					coinRot = new Quaternion(180, 0, 0, 0);
				}

				CoinPool.Instance.Get(rare[Random.Range (0, rare.Length)].gameObject, spawnLocation, coinRot);
			}
		}

		if( findItem == findEpic )
		{
			if( epic.Length != 0 )
			{
				Quaternion coinRot = epic[0].rotation;
				if (spawnCoinHorizontal)
				{
					coinRot = new Quaternion(180, 0, 0, 0);
				}

				CoinPool.Instance.Get(epic[Random.Range (0, epic.Length)].gameObject, spawnLocation, coinRot);
			}
		}

		if( findItem != findCommon && findItem != findRare && findItem != findEpic )
		{
		*/
			Quaternion coinRot = coins[0].rotation;
			if (spawnCoinHorizontal)
			{
				coinRot = new Quaternion(180, 0, 0, 0);
			}

			CoinPool.Instance.Get(coins[Random.Range (0, coins.Length)].gameObject, spawnLocation, coinRot);
	    //}
	}
	
	public void spawnRandom(Vector3 position, int rarity)
	{
		Vector3 spawnLocation;



		if( spawnRandomLocations )
		{
			spawnLocation = new Vector3(transform.position.x,
			                                    transform.position.y,
			                                    Random.Range (transform.position.z - (transform.localScale.z / 2), transform.position.z + (transform.localScale.z / 2)));
		}
		else
		{
			// クリック/タップ位置でスポーン
			// x = z, y = y, z = x
			spawnLocation = new Vector3(transform.position.x, transform.position.y, position.z);
		}

		// ここでレアリティに基づいてスポーンするアイテムを決定します。
		// コモンアイテムを取得するには: 1 - 5
		// レアアイテムを取得するには: 1 - 15
		// エピックアイテムを取得するには: 1 - 30
		
		int findItem = 4;
		int findCommon = Random.Range (1, 5);
		int findRare = Random.Range (1, 30);
		int findEpic = Random.Range (1, 45);
	
		if( findItem == findCommon )
		{
			if( common.Length != 0 )
			{
				Quaternion coinRot = common[0].rotation;
				if (spawnCoinHorizontal)
				{
					coinRot = new Quaternion(180, 0, 0, 0);
				}

				CoinPool.Instance.Get(common[Random.Range (0, common.Length)].gameObject, spawnLocation, coinRot);
			}
		}
		
		if( findItem == findRare )
		{
			if( rare.Length != 0 )
			{
				Quaternion coinRot = rare[0].rotation;
				if (spawnCoinHorizontal)
				{
					coinRot = new Quaternion(180, 0, 0, 0);
				}

				CoinPool.Instance.Get(rare[Random.Range (0, rare.Length)].gameObject, spawnLocation, coinRot);
			}
		}

		if( findItem == findEpic )
		{
			if( epic.Length != 0 )
			{
				Quaternion coinRot = epic[0].rotation;
				if (spawnCoinHorizontal)
				{
					coinRot = new Quaternion(180, 0, 0, 0);
				}

				CoinPool.Instance.Get(epic[Random.Range (0, epic.Length)].gameObject, spawnLocation, coinRot);
			}
		}

		if( findItem != findCommon && findItem != findRare && findItem != findEpic )
		{
		
			Quaternion coinRot = coins[0].rotation;
			if (spawnCoinHorizontal)
			{
				coinRot = new Quaternion(180, 0, 0, 0);
			}

			CoinPool.Instance.Get(coins[Random.Range (0, coins.Length)].gameObject, spawnLocation, coinRot);
	    }
	}
		
	/// <summary>
    /// 指定された位置にコインを1枚生成します。
    /// </summary>
    /// <param name="position">生成する位置</param>
    /// <param name="coinIndex">使用するコインのインデックス</param>
    /// <returns>生成されたコインのGameObject</returns>
    public GameObject SpawnCoinAt(Vector3 position, int coinIndex = 0)
    {
        if (coins.Length > 0)
        {
            int index = Mathf.Clamp(coinIndex, 0, coins.Length - 1);
            Transform coinTransform = coins[index];
            return CoinPool.Instance.Get(coinTransform.gameObject, position, coinTransform.rotation);
        }
        return null;
    }

	public void coinAttackSpawner(int amount)
	{
		for(int i = 1; i <= amount; i++ )
		{
			spawnSingleCoinRandomly();
		}
	}

	/// <summary>
	/// ランダムな位置にコインを1枚生成します。
	/// </summary>
	public void spawnSingleCoinRandomly()
	{
		float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
		float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

		Vector3 coinAttackSpawnLoc = new Vector3 (newX, 3.0f, newZ);

		CoinPool.Instance.Get(coins[Random.Range (0, coins.Length)].gameObject, coinAttackSpawnLoc, coins[0].rotation);
	}

	public void giftCoinSpawner()
	{
		float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
		float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

		Vector3 coinAttackSpawnLoc = new Vector3 (newX, 3.0f, newZ);

		CoinPool.Instance.Get(rare[Random.Range (0, rare.Length)].gameObject, coinAttackSpawnLoc, rare[0].rotation);
	}

	public void spawnCommon()
	{
		if (common.Length == 0) return;

		float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
		float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

		Vector3 spawnLoc = new Vector3 (newX, 3.0f, newZ);

		CoinPool.Instance.Get(common[Random.Range (0, common.Length)].gameObject, spawnLoc, common[0].rotation);
	}

	public void spawnRare()
	{
		if (rare.Length == 0) return;

		float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
		float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

		Vector3 spawnLoc = new Vector3 (newX, 3.0f, newZ);

		CoinPool.Instance.Get(rare[Random.Range (0, rare.Length)].gameObject, spawnLoc, rare[0].rotation);
	}

	public void spawnEpic()
	{
		if (epic.Length == 0) return;

		float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
		float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

		Vector3 spawnLoc = new Vector3 (newX, 3.0f, newZ);

		CoinPool.Instance.Get(epic[Random.Range (0, epic.Length)].gameObject, spawnLoc, epic[0].rotation);
	}

	public void SpawnDice()
	{
        Debug.Log($"SpawnDice called. dice.Length: {dice.Length}");
		if (dice.Length > 0) 
        {
            float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
            float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

            Vector3 spawnLoc = new Vector3 (newX, 3.0f, newZ);
            Debug.Log($"Spawning dice from dice array at {spawnLoc}");

            CoinPool.Instance.Get(dice[Random.Range (0, dice.Length)].gameObject, spawnLoc, dice[0].rotation);
            return;
        }

        // dice配列が空の場合、CoinManagerから探す
        var coinManager = GameObject.FindWithTag("CoinManager").GetComponent<CoinManager>();
        if (coinManager != null && coinManager.allSpawnableCoinsItems != null)
        {
            foreach(var item in coinManager.allSpawnableCoinsItems)
            {
                var effect = item.GetComponent<CoinEffect>();
                if (effect != null && (effect.typeOfCoin == CoinEffect.Effect.CollectableDice || effect.typeOfCoin == CoinEffect.Effect.CollectableOrangeDice))
                {
                    float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
                    float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);
                    Vector3 spawnLoc = new Vector3 (newX, 3.0f, newZ);
                    
                    CoinPool.Instance.Get(item, spawnLoc, item.transform.rotation);
                    Debug.Log("Spawned Dice from CoinManager list.");
                    return;
                }
            }
        }

        Debug.LogWarning("Dice array is empty and could not find Dice in CoinManager!");
	}

	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(0, 1, 0, 0.5F);
		Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
	}
}