using UnityEngine;
using System.Collections;
using CoinPusher.Core;
using UnityEngine.UI;

/// <summary>
/// バケット（手前の受け皿）に入ったコインを検知し、コインの獲得処理やレベル更新を行うクラス
/// </summary>
public class BucketCoinCollector : MonoBehaviour {

	// コイン管理マネージャーへの参照
	public CoinManager coinManager;

	// レベル管理マネージャーへの参照
	public LevelManager levelManager;

	public void Start()
	{
		// LevelManagerが未設定の場合、タグ検索して取得する
		if (!levelManager)
			levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); 
		
		// CoinManagerが未設定の場合、タグ検索して取得する
		if (!coinManager)
			coinManager = GameObject.FindGameObjectWithTag("CoinManager").GetComponent<CoinManager>();
	}

	// コインやオブジェクトがバケット（トリガー）に入った時に呼ばれる
	void OnTriggerEnter(Collider other) 
	{
		// 衝突したオブジェクトからCoinEffectコンポーネントを取得
		CoinEffect coinEffect = other.gameObject.GetComponent<CoinEffect>();

		// CoinEffectコンポーネントが存在する場合のみ処理を行う
		if (coinEffect != null)
		{
			// コインの価値を取得
			int value = coinEffect.coinValue;
			
			// コイン獲得時のエフェクト（音やパーティクルなど）を再生
			// 注意: effect()メソッド内で Destroy(gameObject, 0.1f) が呼ばれ、コインは消滅します
			coinEffect.effect();

			// CoinManagerに獲得したコインの価値を加算する（falseはボーナスコインではないことを示す）
			coinManager.addCoin(value, false);

			// 獲得した価値に基づいてレベル（経験値）を更新する
			levelManager.updateLevel(value);
		}
	}
}