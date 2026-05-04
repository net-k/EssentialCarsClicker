using UnityEngine;
using System.Collections;
using CoinPusher.Core;

public class TouchClickManager : MonoBehaviour {

	// Our save manager
	public SaveManager saveManager;

	// Our reference to the CoinSpawner which is based on a tag
	public CoinSpawner coinSpawner;			

	// The reference to the CoinManager for use later
	public CoinManager coinManager;

	// Our out of coin manager
	public OutOfCoinsManager outOfCoinsManager;

	void Update()
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			// Make sure we found our Coin Spawner
			if( coinSpawner != null )
			{
				RaycastHit hit;
				if( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) )
				{
					// Make sure the user has coins to actually use
					if( coinManager.canSpawnCoin() )
					{
						// Spawn a coin
						coinSpawner.spawnCoin(hit.point);		

						// Remove the coin from the CoinManager
						coinManager.removeCoin();

						// Save the game data
						saveManager.saveData();

						// コインを消費した直後にコインがなくなったかチェックする
						if( coinManager.currentCoinTotal <= 0 && !outOfCoinsManager.isWindowOpen )
						{
							// コイン補充が必要なポップアップを表示する
							outOfCoinsManager.showWindow();
						}
					}
					else
					{
						// コインをスポーンできない場合、コインが足りないかチェックし、足りなければ補充を促す
						if( coinManager.currentCoinTotal <= 0 && !outOfCoinsManager.isWindowOpen )
						{
							// コイン補充が必要なポップアップを表示する
							outOfCoinsManager.showWindow();
						}
					}
				}
			}
			else
				Debug.LogError ("You need to assign a coin spawner!");
		}
	}
}