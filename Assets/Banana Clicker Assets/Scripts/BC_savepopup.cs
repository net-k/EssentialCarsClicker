using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BC_savepopup : MonoBehaviour {

    public float clickRate;
    public float popupScrollSpeed = 10f;
    public UnityEngine.UI.Text popupText;
    public Transform _canvas;


    //this is the first function to run when the app gets started.
    // アプリ起動時に最初に呼ばれる処理です
    void Awake()
    {

        popupText = GetComponent<Text>();

        //set the text component to + with the clickrate.
        // テキストに「Game Saved!」を設定します
        popupText.text = "Game Saved!";

        //set the parent of the object to the canvas, so that the position of the popus is right.
        // ポップアップの位置を正しくするために Canvas の子オブジェクトに設定します
        transform.SetParent(GameObject.Find("Canvas").transform, false);
        //destroy the popup after 1 second.
        // 1秒後にポップアップを自動で破棄します
        Destroy(gameObject, 1f);
    }

    // Use this for initialization
    // 初期化処理（Start は空のため処理なし）
    void Start()
    {

    }

    // Update is called once per frame
    // 毎フレーム呼ばれます
    void Update()
    {
        //every fram smoothly move up
        // 毎フレーム上方向にスムーズに移動させます
        transform.Translate(Vector2.up * popupScrollSpeed * Time.deltaTime);



    }
}