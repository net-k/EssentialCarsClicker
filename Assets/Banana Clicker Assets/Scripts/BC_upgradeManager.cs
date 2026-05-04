using UnityEngine;
using System.Collections;
using SushiClicker;
using UnityEngine.UI;

public class BC_upgradeManager : MonoBehaviour {


	//These two Text items allow you to drag in a text object and tell the Upgrade Manager where you would like to display the Name and the Count.
	// アップグレード名を表示するテキストオブジェクトをインスペクターからドラッグして設定します
	public string itemName;

	public UnityEngine.UI.Text itemInfo;

    public bool IsUnlocked = false;

    public GameObject Myself;


	//This allows you to import the Click.cs File and access it's functions.
	// Click.cs スクリプトへの参照。クリック処理や各種値にアクセスするために使用します
	public BC_Click click;


	//This allows you to change the cost, count and clickPower in the Inspector.
	// コスト・所持数・クリック倍率をインスペクターで設定できます
	public double cost;

	public long count = 0;
	public double clickPower;


	//private variable used only for the math in increasing the cost of each upgrade after you buy one.
	// 購入後のコスト計算に使う基準コスト（インスペクターからは直接変更不可）
	public double baseCost;


	//allows you to reference the 2 colors in the in the inspector and set the colors that they upgrades become when affordbale.
	// 「購入不可」と「購入可能」の2色をインスペクターで設定できます
	public Color standard;
	public Color affordable;


	//this is the slider which determines the value of the progress style bar.
	// プログレスバーの値を管理するスライダー
	private Slider _slider;



    public void SetSlider(double valueToStore)
    {
        if (_slider != null)
            _slider.value = (float)valueToStore;
    }



	private bool _isLoaded = false;

    // 起動時にデータを読み込む
    void Awake()
    {
        SyncData();
    }

	/// <summary>
	/// 保存されている所持数やコストを同期します。
	/// オブジェクトが非アクティブでも外部から呼び出し可能です。
	/// </summary>
	public void SyncData()
	{
		if (_isLoaded) return;

		// 保存キーは itemName を優先し、未設定なら GameObject.name を使う（互換性維持）
		string key = string.IsNullOrEmpty(itemName) ? name : itemName;
		count = LevelSaveDataManager.Instance.LoadCount(key);
		cost = LevelSaveDataManager.Instance.LoadCost(key, cost == 0 ? baseCost : cost);
		IsUnlocked = LevelSaveDataManager.Instance.LoadIsUnlocked(key);

		Debug.Log($"BC_upgradeManager.SyncData: key={key}, loaded count={count}, cost={cost}, unlocked={IsUnlocked}");

		_isLoaded = true;
	}

    //this function runs as soon as the application starts.
    // アプリ起動直後に1度だけ呼ばれる初期化処理

    void Start(){

		//sets basecost to cost as basecost is private and not accesible from the inspector and cost is.
		// baseCost に初期コストを保存しておきます（Inspector でアクセスできる cost を使用）
		baseCost = (baseCost == 0) ? cost : baseCost;
        if (cost == 0) {
            cost = baseCost;
        }

		_slider = GetComponentInChildren<Slider> ();

        // アクティブになった際に最新のパワーを再計算させる
        if (click != null)
            click.RefreshBananasPerClick();
    }

	//this functions runs alot like really really fast, so it's useful for displaying information that changes a lot.
	// 毎フレーム呼ばれます。頻繁に変わる表示内容の更新に使用します
	void Update(){

        if (click == null || itemInfo == null) return;

        if (click.bananas > (cost / 2) || IsUnlocked == true)
        {

            //this sets the item name of the item to whatever is written in the inspector plus it adds on the cost and the clickpower from the inspector.
            // アップグレード名・所持数・コスト・クリック倍率を表示します。インスペクターで設定した値がそのまま反映されます
            itemInfo.text = itemName + " " + count + "\nCost: " + BC_currencyConverter.Instance.GetCurrencyIntoString(cost, false, false) + "\nPower: +" + clickPower;

            if (_slider != null)
            {
                _slider.value = (float)(click.bananas / cost * 100);
                if (_slider.value >= 100)
                {
                    GetComponent<Image>().color = affordable;
                }
                else
                {
                    GetComponent<Image>().color = standard;
                }
            }
            IsUnlocked = true;



        }
        else
        {
            itemInfo.text = "???";

        }







	}


	//same as the itemManager one, checks if you can afford the upgrade then takes the cost from your banana count and increase the clickpower based on the click power of the upgrade.
	// BC_ItemManager の購入処理と同様に、コストを確認してバナナを消費し、クリック倍率を加算します
	public void PurchasedUpgrade()
	{
        // コストとバナナ数の比較（浮動小数点誤差を考慮して小さな許容値を加える）
		if (click.bananas + 0.0001 >= cost)
		{
            Debug.Log("Bought " + name + " for: " + cost + "\n" +
                      "Before Bananas: " + click.bananas);

            click.bananas -= cost;
			count += 1;

            Debug.Log("After Bananas: " + click.bananas);

            // 保存キーは itemName を優先し、未設定なら GameObject.name を使う（互換性維持）
            string key = string.IsNullOrEmpty(itemName) ? name : itemName;
            LevelSaveDataManager.Instance.SaveCount(key, count);
            LevelSaveDataManager.Instance.SaveCost(key, cost);
            LevelSaveDataManager.Instance.SaveIsUnlocked(key, true);

            if (click != null)
                click.RefreshBananasPerClick();

            cost = Mathf.Round((float)baseCost * Mathf.Pow (1.15f, count));

            SetDouble(name + "c", cost);
		}
        else
        {
            Debug.LogWarning($"PurchasedUpgrade: {itemName} のバナナが不足しています (bananas={click.bananas}, cost={cost})");
        }
	}


    //the following lines of code allow you to save a double to player prefs.
    // 以下は double 型を PlayerPrefs に保存・読み込みするためのユーティリティ関数です


    //this function takes a double and a string
    // double 値を指定したキーで PlayerPrefs に保存します
    public static void SetDouble(string key, double value)
    {
        //this saves the double you passed into the key you provided.
        //but first it runs the function called DoubleToString to turn it into a string with the format R
        // DoubleToString で文字列に変換してから PlayerPrefs に保存します
        PlayerPrefs.SetString(key, DoubleToString(value));
    }

    //this retrieves a double stored under the key you provided and takes a default value.
    // 指定したキーの double 値を PlayerPrefs から読み込みます（デフォルト値あり）
    public static double GetDouble(string key, double defaultValue)
    {
        //store the default value incase its not stored.
        // 未保存の場合に備えてデフォルト値を文字列化しておきます
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
    }

    //this gets a double without a default value.
    // デフォルト値なしで double を読み込みます（デフォルトは 0）
    public static double GetDouble(string key)
    {
        return GetDouble(key, 0d);
    }

    //this turned the double into a string with the format R
    //which microsoft states will ensure it converts back the same.
    // double を "R"（ラウンドトリップ）形式の文字列に変換します。元の値に正確に復元できることを保証します
    /* link - https://msdn.microsoft.com/en-us/library/dwhawy9k.aspx#RFormatString */

    private static string DoubleToString(double target)
    {
        return target.ToString("R");
    }
    private static double StringToDouble(string target)
    {
        if (string.IsNullOrEmpty(target))
            return 0d;

        return double.Parse(target);
    }

}