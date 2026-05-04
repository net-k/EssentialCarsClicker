using System;
using System.Collections.Generic;
using UnityEngine;

namespace OTooleSoftware {

    /// <summary>
    /// ステージ内で獲得したプライズの個数を管理するシンプルなマネージャー。
    /// - インスペクタに現在のカウントを表示できる
    /// - 他スクリプトから PrizeCollectionManager.Instance.Add(prize) でインクリメント
    /// - 必要なら PlayerPrefs に保存/復元可能（メソッド呼び出しで制御）
    /// </summary>
    public class PrizeCollectionManager : SingletonMonoBehaviour<PrizeCollectionManager>
    {
   #if false
        static PrizeCollectionManager _instance;
        public static PrizeCollectionManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<PrizeCollectionManager>();
                    if (_instance == null) {
                        var go = new GameObject("PrizeCollectionManager");
                        _instance = go.AddComponent<PrizeCollectionManager>();
                    }
                }
                return _instance;
            }
        }
#endif

       
        // 実行時のマップ（キーは prize.name を使用）
        Dictionary<string,int> counts = new Dictionary<string,int>();

        // 値が変化したときに購読できるイベント（プライズ, 新しい合計）
        public event Action<int,int> OnPrizeCountChanged;


    }
}
