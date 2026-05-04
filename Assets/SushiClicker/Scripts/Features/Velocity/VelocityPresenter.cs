using UnityEngine;

namespace SushiClicker
{
    /// <summary>
    /// バナナ毎秒（BPS）表示のロジックを担当するPresenter。
    /// 毎フレームBPSを取得してViewを更新する。
    /// </summary>
    public class VelocityPresenter : MonoBehaviour
    {
        [SerializeField] private VelocityView _view = null;
        [SerializeField] private BC_bananaPerSec _bcBps = null;

        private void Start()
        {
            // インスペクター未設定の場合はシーン上から自動取得する
            if (_bcBps == null)
                _bcBps = FindObjectOfType<BC_bananaPerSec>();

            // デバッグ: _bcBps が取得できたか確認
            if (_bcBps == null)
                Debug.LogError("VelocityPresenter: _bcBps が null です。BC_bananaPerSec がシーン上に見つかりません。");
            else
                Debug.Log("VelocityPresenter: _bcBps を取得しました → " + _bcBps.gameObject.name);
        }

        private float _debugTimer = 0f;

        private void Update()
        {
            if (_bcBps == null || _view == null) return;

            double bps = _bcBps.GetBananasPerSec();

            // デバッグ: 3秒ごとにBPS値をログ出力
            _debugTimer += Time.deltaTime;
            if (_debugTimer >= 3f)
            {
                _debugTimer = 0f;
                Debug.Log($"VelocityPresenter: GetBananasPerSec() = {bps}");
            }

            _view.SetVelocity(bps);
        }
    }
}
