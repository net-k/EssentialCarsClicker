using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BC_PopupScript : MonoBehaviour {

    public double clickRate;
    public float popupScrollSpeed = 100f;
    public UnityEngine.UI.Text popupText;
    public Transform _canvas;

    public BC_Click click;


    //this is the first function to run when the app gets started.
    // アプリ起動時に最初に呼ばれる処理です
    /// <summary>
    /// BC_Click の静的キャッシュ（毎回 FindObjectOfType を避ける）
    /// </summary>
    private static BC_Click _cachedClick;

    void Awake()
    {
        // BC_Click のキャッシュを再利用する（シーン内に1つしかない前提）
        if (_cachedClick == null)
            _cachedClick = FindObjectOfType<BC_Click>();
        click = _cachedClick;

        if (click == null)
        {
            Destroy(gameObject, 1f);
            return;
        }
        clickRate = click.bananasPerClick;

        //get reference to the text componet on this object
        // このオブジェクトの Text コンポーネントへの参照を取得します
        popupText = GetComponent<Text>();
        if (popupText == null)
        {
            Debug.LogError("[BC_PopupScript] Text コンポーネントが見つかりません");
            Destroy(gameObject, 1f);
            return;
        }

        if (BC_currencyConverter.Instance == null)
        {
            Debug.LogError("[BC_PopupScript] BC_currencyConverter.Instance が null です");
            popupText.text = "+ " + clickRate;
        }
        else
        {
            // プレステージボーナスを含めた実際の加算量を表示する（BC_Click.Clicked() と同じ計算）
            double totalPerClick = clickRate;
            if (click.PrestigeLevel >= 1)
            {
                totalPerClick += clickRate * (click.PrestigeLevel / 100);
            }
            popupText.text = "+ " + BC_currencyConverter.Instance.GetCurrencyIntoString(totalPerClick, false, false);
        }

        //destroy the popup after 1 second.
        // 1秒後にポップアップを自動で破棄します
        Destroy(gameObject, 1f);
    }

	// Use this for initialization
	// 初期化処理（サイズ拡大と位置のランダム化を行います）
	void Start () {
        // ポップアップのサイズを1.5倍に大きくする
        transform.localScale *= 1.5f;

        // 表示位置を横・縦に大きくランダムに散らす（表示被りを防ぐ）
        float randomX = UnityEngine.Random.Range(-100f, 100f);
        float randomY = UnityEngine.Random.Range(-100f, 100f);
        transform.localPosition = new Vector3(
            transform.localPosition.x + randomX, 
            transform.localPosition.y + randomY, 
            transform.localPosition.z
        );
	}

	// Update is called once per frame
	// 毎フレーム呼ばれます
	void Update () {
        //every fram smoothly move up
        // 毎フレーム上方向にスムーズに移動させます
        transform.Translate(Vector2.up * popupScrollSpeed * Time.deltaTime);



    }
}