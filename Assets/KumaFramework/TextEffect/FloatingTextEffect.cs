using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace TohoReversi.Effect.TextEffect
{
    public class FloatingTextEffect : MonoBehaviour
    {
        private GameObject _floatingTextPrefab;  // フローティングテキストのプレハブ
        private float floatDuration = 0.6f;       // テキストの表示時間
        private Vector3 floatOffset = new Vector3(0, 0.5f, 0.0f);  // テキストの浮く位置オフセット
        private TextMeshProUGUI _textMeshPro;

        private void Start()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            _floatingTextPrefab = Resources.Load<GameObject>("Prefabs/3DText/ItemGetText");
        }
        
        public async UniTask ShowFloatingTextAsync(string text, int value, Vector3 position)
        {
            if(_floatingTextPrefab == null)
            {
                Debug.LogError("FloatingTextEffect is not initialized.");
                return;
            }
            
            // position の y を上にしたい
            position.y += 0.5f;
            
            GameObject floatingTextInstance = Instantiate(_floatingTextPrefab, position, Quaternion.identity);

            var pre = floatingTextInstance.GetComponent<ItemGetTextPresenter>();
            
            // テキスト内容を "+1" のようにセット
            pre.GetTextMeshPro().text = $"{text}{value}";
        
            // エフェクトの再生
            await FloatAndFadeAsync(floatingTextInstance);

            // エフェクトが終了したらオブジェクトを削除
            Destroy(floatingTextInstance);
        }

        private async UniTask FloatAndFadeAsync(GameObject floatingTextInstance)
        {
            Vector3 startPos = floatingTextInstance.transform.position;
            Vector3 endPos = startPos + floatOffset;
            // 90 y 軸回転
            floatingTextInstance.transform.Rotate( 0, 90, 0);
            // floatingTextInstance.transform.Rotate(-270, 180, 180);
            
            //scale down
            floatingTextInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            float elapsedTime = 0;
            // CanvasGroup canvasGroup = floatingTextInstance.GetComponent<CanvasGroup>();  // フェードアウト用

            while (elapsedTime < floatDuration)
            {
                // 位置を少しずつ上に動かす
                floatingTextInstance.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / floatDuration);
                elapsedTime += Time.deltaTime;
              
                // 1フレーム待つ
                await UniTask.Yield();
            }
        }
    }
}
