using UnityEngine;
using System.Collections;
using SushiClicker;
using UnityEngine.UI;
using KumaFramework;



public class BC_ItemManager : MonoBehaviour {


//These two Text items allow you to drag in a text object and tell the Item Manager where you would like to display the Name and the Count.
// アイテムの名前と所持数を表示するテキストオブジェクトをインスペクターからドラッグして設定します
public UnityEngine.UI.Text itemInfo;
public UnityEngine.UI.Text itemCount;

//This imports the Click.cs file (Where our click behaviour is) and allows you to call click.bananas and change the value.
// Click.cs（クリック処理とバナナ数の管理）への参照。click.bananas で値を操作できます
public BC_Click click;

//Making the cost/tickvalue/count/itemName public allows it be set at the "Inspector level" which means less coding for you.
// コスト・毎秒tick量・所持数・アイテム名をpublicにすることで、インスペクターから直接設定できます
public double cost;
public double tickValue;
public long count;
public string itemName;
    public bool IsUnlocked = false;


// 購入前の基準コスト（コスト計算のベースとして使用）
public double baseCost;


//This sets 2 colors are public to allow you to change them within the "Inspector" but can still reference it in code.
//The 2 colors are used for "Not affordbale" and "Affordable" So the items change color when you can afford them.
// 「購入不可」と「購入可能」の2色をインスペクターで設定できます。バナナが足りるとアイテムの色が切り替わります
public Color standard;
public Color affordable;


//This is the slider which is what is used to make the "progress bar" look.
// プログレスバーの見た目を実現するスライダー
private Slider _slider;


    // スライダーの値を外部からセットします
    public void SetSlider(float valueToStore)
    {
        if (_slider != null)
            _slider.value = (float)valueToStore;
    }



private bool _isLoaded = false;

// アプリ起動直後に呼ばれる初期化処理
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

// 保存キーはまず itemName を優先し、未設定なら GameObject.name を使う（互換性維持）
string key = string.IsNullOrEmpty(itemName) ? name : itemName;
// ES3 から保存済みの所持数・コスト・アンロック状態を読み込みます
count = LevelSaveDataManager.Instance.LoadCount(key);
cost = LevelSaveDataManager.Instance.LoadCost(key, cost == 0 ? baseCost : cost);
IsUnlocked = LevelSaveDataManager.Instance.LoadIsUnlocked(key);

Debug.Log($"BC_ItemManager.SyncData: key={key}, loaded count={count}, cost={cost}, unlocked={IsUnlocked}");

_isLoaded = true;
}

// アプリ起動直後に1度だけ呼ばれます
void Start(){

//this sets the var baseCost to be equal to the cost.
// baseCost に初期コストを保存しておきます
baseCost = (baseCost == 0) ? cost : baseCost;

//this tells the slider where the slider is using a get component in children.
// 子オブジェクトからスライダーコンポーネントを取得します
_slider = GetComponentInChildren<Slider> ();
        if (_slider == null)
        {
            Debug.LogWarning($"BC_ItemManager: {name} の子にSliderが見つかりません");
        }
}



    //This function runs every frame (really really often more than once a second)
    // 毎フレーム呼ばれます（1秒間に何度も実行されます）
    void Update()
    {

// バナナがコストの半額以上、またはアンロック済みの場合はアイテム情報を表示します
        if (click.bananas > (cost / 2) || IsUnlocked == true)
        {

            //this sets the itemName to be itemname and show the cost of the item and the tickvalue and the count. It is done like this as then you do not need to hard code
            // the value for each item or upgrade, this will display whatever details you place into the inspector.
            // アイテム名とコストを表示します。インスペクターで設定した値がそのまま反映されるため、ハードコードが不要です

            /*
            itemInfo.text = itemName + "\nCost: " + BC_currencyConverter.Instance.GetCurrencyIntoString(cost, false, false) + "\nBananas: " + tickValue + "/s";
            */

            itemInfo.text = itemName + "\nCost: " + BC_currencyConverter.Instance.GetCurrencyIntoString(cost, false, false);

            itemCount.text = count + " ";

            //this converts the double result to a float so that the slider is happy as it only accepts floats as input.
            // スライダーはfloatしか受け付けないため、doubleをfloatにキャストしています
            if (_slider == null) return;
            _slider.value = (float)(click.bananas / cost * 100);

            //If the slider value is greater than or equal to 100% then the item is affordale and this sets the color.
            // スライダーが100%以上（購入可能）ならアフォーダブルカラー、そうでなければ標準カラーにします
            if (_slider.value >= 100)
            {
                GetComponent<Image>().color = affordable;
            }
            else
            {
                GetComponent<Image>().color = standard;
            }

            IsUnlocked = true;




        }
        else
        {
// バナナが不足していてアンロックされていない場合は「???」と表示します
            itemInfo.text = "???";
        }
    }



    //This is a public function that takes the cost that was set in the inspector and checks whether or not you can afford it.
    // インスペクターで設定したコストと現在のバナナ数を比較してアイテムを購入します
    public void PurchasedItem(){
if (click.bananas >= cost) {

            Debug.Log("Bought " + name + " for: " + cost + "\n" +
                      "Before Bananas: " + click.bananas);

            click.bananas -= cost;
count += 1;
            IsUnlocked = true;

            Debug.Log("After Bananas: " + click.bananas);


//This is the same math that cookie clicker uses to determine the cost of the next upgrade.
// Cookie Clicker と同じ計算式で次の購入コストを算出します（baseCost × 1.15^count）
cost = Mathf.Round((float)baseCost * Mathf.Pow(1.15f, count));

            // 所持数・コスト・アンロック状態を ES3 に保存します（ロード側も ES3 を使用）
            string key = string.IsNullOrEmpty(itemName) ? name : itemName;
            LevelSaveDataManager.Instance.SaveCount(key, count);
            LevelSaveDataManager.Instance.SaveCost(key, cost);
            LevelSaveDataManager.Instance.SaveIsUnlocked(key, true);
            
            // BPS更新のため、BC_Click の RefreshBananasPerClick() を呼び出す
            // （VelocityPresenter が毎フレーム GetBananasPerSec() を呼ぶため、自動的にUI更新される）
            if (click != null)
            {
                click.RefreshBananasPerClick();
                Debug.Log($"BC_ItemManager.PurchasedItem: RefreshBananasPerClick() called for {itemName}");
            }
            Debug.Log($"BC_ItemManager.PurchasedItem: saved key={key}, count={count}, cost={cost}");


        }
}

}
