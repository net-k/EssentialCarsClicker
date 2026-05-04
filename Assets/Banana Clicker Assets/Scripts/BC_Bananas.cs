using UnityEngine;
using System.Collections;

public class BC_Bananas : MonoBehaviour {

	public float delay = 0.1f;

	public GameObject bananap;



	// Use this for initialization
	// 初期化処理（起動時に1度だけ呼ばれる）
	void Start () {

            //this means repeat Spawn every Delay, with the first delay being set here too.
			// Spawn を Delay 秒ごとに繰り返し呼び出します（最初の実行も Delay 秒後）
			InvokeRepeating ("Spawn", delay, delay);

	}

	// Update is called once per frame
	// 毎フレーム呼ばれます（バナナを生成するメソッドです）
	public void Spawn () {
        //this instantiate the bananas. old code but i have left it here for you to learn from
		// バナナをランダムなX座標で生成します。学習のために残している古いコードです
		Instantiate (bananap, new Vector3 (Random.Range (25, 1020), 500 ,524 ), Quaternion.identity);

	}
}