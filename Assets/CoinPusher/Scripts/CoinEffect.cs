using UnityEngine;
using System.Collections;
using CoinPusher;
using Cysharp.Threading.Tasks;
using SushiCoinPusher.Features.Prize;
using TohoReversi.Effect.TextEffect;

[System.Serializable]
public class CoinData
{
    // コインの位置座標
    public float posX;
    public float posY;
    public float posZ;

    // コインの回転（クォータニオン）
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    // コインの価値
    public int coinValue;

    // コインの効果タイプを定義
    public enum Effect
    {
        RegularCoin,            // 通常のコイン
        BumperWallCoin,         // バンパー（壁）が出現するコイン
        BullseyeCoin,           // コインタワー（大量コイン落下）が発動するコイン
        CashCoin,               // キャッシュ（通貨）を獲得できるコイン
        GiftCoin,               // ギフト（レアアイテム）が出現するコイン
        QuakeShakeCoin,         // 地震（フィールド揺らし）が発生するコイン
        StopCoin,               // プッシャーが一時停止するコイン
        CollectableDonut,       // 収集アイテム：ドーナツ
        CollectableCocoDonut,   // 収集アイテム：ココドーナツ
        CollectableDice,        // 収集アイテム：サイコロ
        CollectableOrangeDice,  // 収集アイテム：オレンジサイコロ
        CollectableGoldBar,     // 収集アイテム：金塊
        CollectableSushi,       // 収集アイテム：寿司
        DiaCoin                 // ダイヤコイン：10枚払い出し
    }
    public Effect typeOfCoin;

    // コインの一意なID
    public string id;

    // CoinManagerの配列(allSpawnableCoinsItems)における、この生成可能なコイン（プレハブ）の番号
    public int spawnID;

    // アイテムの生成名（GameObject名）
    public string objectName;
   
}

public class CoinEffect : MonoBehaviour {

	// このコインの価値
	public int coinValue = 1;

	// EffectsManagerへの参照
	private EffectsManager effectsManager;

    // SaveManagerへの参照
    private SaveManager saveManager;

    // FloatingTextEffectへの参照
    [SerializeField]
    private FloatingTextEffect floatingTextEffect;

	// コインの効果タイプを定義
	public enum Effect {
		RegularCoin,            // 通常のコイン
		BumperWallCoin,         // バンパー（壁）が出現するコイン
		BullseyeCoin,           // コインタワー（大量コイン落下）が発動するコイン
		CashCoin,               // キャッシュ（通貨）を獲得できるコイン
		GiftCoin,               // ギフト（レアアイテム）が出現するコイン
		QuakeShakeCoin,         // 地震（フィールド揺らし）が発生するコイン
		StopCoin,               // プッシャーが一時停止するコイン
		CollectableDonut,       // 収集アイテム：ドーナツ
		CollectableCocoDonut,   // 収集アイテム：ココドーナツ
		CollectableDice,        // 収集アイテム：サイコロ
		CollectableOrangeDice,  // 収集アイテム：オレンジサイコロ
		CollectableGoldBar,     // 収集アイテム：金塊
        CollectableSushi,       // 収集アイテム：寿司
        DiaCoin                 // ダイヤコイン：10枚払い出し
	}
	public Effect typeOfCoin;

	// ゲーム開始時に既にフィールド上にあるコインかどうか（SFX再生防止用）
	public bool alreadyOnPlayField;

	// コインが落下した時の効果音
	public AudioClip droppedSound;

	// 着地したかどうか（衝突判定を停止するために使用）
	private bool didLand = false;

	// コインが回収されずに破棄された時の効果音
	public AudioClip destroyedSound;

	[Header("Collectable and Coin Shop Settings")]

	// 景品関連の設定。収集画面で売却可能か、コインショップで購入可能か
	public Sprite prizeImage;

	// コインショップでのアイテム価格
	public int coinShopItemPrice = 1;

    // 生成ID
    public int spawnableID = 0;

    // コインデータ
    public CoinData coinData;

    void OnEnable()
    {
        // プールから再利用される際に状態をリセット
        didLand = false;
        alreadyOnPlayField = false;
        
        // 必要に応じて初期化処理を呼び出す
        InitializeCoin();
    }

	void Start()
	{
        InitializeCoin();
    }

	public void Initialize(FloatingTextEffect floatingTextEffect)
	{
		this.floatingTextEffect = floatingTextEffect;
	}
	
    private void InitializeCoin()
    {
        // coinDataがnullの場合は初期化
        if (coinData == null)
        {
            coinData = new CoinData();
        }

        // 有効なcoinData.idを持って開始した場合
        if ( !string.IsNullOrEmpty(coinData.id) )
        {
            // 前回のコイン設定を読み込む
            this.coinValue = coinData.coinValue;
            this.typeOfCoin = (CoinEffect.Effect)coinData.typeOfCoin;
            this.name = coinData.objectName;
        }
        else
        {
            // 新しいコインの基本的な初期値を設定
            coinData.coinValue = this.coinValue;
            coinData.typeOfCoin = (CoinData.Effect)this.typeOfCoin;

            // 新しいコインを生成する際、その名前を保存する
            coinData.objectName = this.name;

            // 新規作成なので、このコインに新しいIDを生成する
            coinData.id = System.Guid.NewGuid().ToString();
        }
        
        // オブジェクトを取得
        if (effectsManager == null)
        {
            var effectManagerObj = GameObject.FindWithTag("EffectManager");
            if (effectManagerObj != null)
            {
                effectsManager = effectManagerObj.GetComponent<EffectsManager>();
            }
        }

        // SaveManagerを取得
        if (saveManager == null)
        {
            var saveManagerObj = GameObject.FindGameObjectWithTag("SaveManager");
            if (saveManagerObj != null)
            {
                saveManager = saveManagerObj.GetComponent<SaveManager>();
            }
        }

        // FloatingTextEffectを取得
        if (floatingTextEffect == null)
        {
            var floatingTextEffectObj = GameObject.FindObjectOfType<FloatingTextEffect>();
            if (floatingTextEffectObj != null)
            {
                floatingTextEffect = floatingTextEffectObj;
            }
        }

        // SaveManagerに追加
        if (saveManager != null)
        {
            saveManager.addCoin(coinData);
        }

		// この新しいコインの親を設定
        var coinsPlayField = GameObject.FindWithTag("CoinsPlayField");
        if (coinsPlayField != null)
        {
            this.gameObject.transform.parent = coinsPlayField.transform;
        }
    }

    public void Update()
    {
        updateCoinData();
    }

    /// <summary>
    /// 価値を取得します。
    /// </summary>
    /// <returns>価値。</returns>
    public int getValue()
	{
		return coinValue;
	} 

    /// <summary>
    /// 効果をトリガーする必要がある場所から呼び出され、EffectsManagerに渡されます。
    /// </summary>
    public void effect()
	{
		if (effectsManager != null)
		{
            PrizeData prizeData = null;
            var prizeInfo = GetComponent<PrizeInfo>();
            if (prizeInfo != null)
            {
                prizeData = new PrizeData();
                prizeData.prizeId = prizeInfo.prizeID;
            }

			effectsManager.runEffect(typeOfCoin, coinValue, prizeData);
		}

        // フローティングテキストを表示
        if (floatingTextEffect != null)
        {
            // コインの価値が0より大きい場合のみ表示
            if (coinValue > 0)
            {
                floatingTextEffect.ShowFloatingTextAsync("+", coinValue, this.transform.position).Forget();
            }
        }

		// コインを削除
		removeCoin();
	}

	/// <summary>
	/// 外部から呼び出されます。コインデストロイヤーで、落下して回収されなかった場合に呼ばれます。
	/// </summary>
	public void playDestroyedSFX()
	{
		// 変数がnullでないことを確認し、再生する
		if( destroyedSound != null && Camera.main != null )
			Camera.main.GetComponent<AudioSource>().PlayOneShot(destroyedSound);
	}

	// 完了時にこのオブジェクトを削除
	public void removeCoin()
	{
        if (CoinPool.Instance != null)
        {
            CoinPool.Instance.Return(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject, 0.1f);
        }
	}

	void OnCollisionEnter (Collision col)
	{
        // まだ着地していない場合
        if ( !didLand )
		{
			// 既にプレイフィールド上にあった場合（開始時のコインなど）は除外
			if( !alreadyOnPlayField )
			{
				// プッシュバーまたは床に当たった場合
				if( col.gameObject.CompareTag("PushBar") ||  col.gameObject.CompareTag("Floor") )
				{
					// 再生する効果音があるか確認
					if( droppedSound != null && Camera.main != null )
						Camera.main.GetComponent<AudioSource>().PlayOneShot(droppedSound);

					// 着地済みとしてマーク
					didLand = true;
                }
			}
		}     
	}
    

    public void updateCoinData()
    {
        if (saveManager == null || coinData == null) return;

        coinData.posX = this.gameObject.transform.localPosition.x;
        coinData.posY = this.gameObject.transform.localPosition.y;
        coinData.posZ = this.gameObject.transform.localPosition.z;

        coinData.rotX = this.gameObject.transform.rotation.x;
        coinData.rotY = this.gameObject.transform.rotation.y;
        coinData.rotZ = this.gameObject.transform.rotation.z;
        coinData.rotW = this.gameObject.transform.rotation.w;

        saveManager.updateCoin(coinData);
    }
}
