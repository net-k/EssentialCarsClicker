using UnityEngine;

namespace KumaFramework
{
    /// <summary>
    /// タッチ管理クラス
    /// </summary>
    public class TouchManager
    {
        public bool isTouched { get; private set; } // タッチの有無
        public Vector2 touchPosition { get; private set; } = Vector2.zero; // タッチ座標
        public TouchPhase touchPhase { get; private set; } = TouchPhase.Began; // タッチ状態

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TouchManager(bool flag = false, Vector2 position = default, TouchPhase phase = TouchPhase.Began)
        {
            isTouched = flag;
            touchPosition = position;
            touchPhase = phase;
        }

        /// <summary>
        /// タッチ情報の更新
        /// </summary>
        public void Update()
        {
            isTouched = false;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;
                touchPhase = touch.phase;
                isTouched = true;
            }
            else if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer ||
                     Application.platform == RuntimePlatform.OSXPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isTouched = true;
                    touchPhase = TouchPhase.Began;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isTouched = true;
                    touchPhase = TouchPhase.Ended;
                }
                else if (Input.GetMouseButton(0))
                {
                    isTouched = true;
                    touchPhase = TouchPhase.Moved;
                }

                if (isTouched)
                {
                    touchPosition = Input.mousePosition;
                }
            }
        }
    }
    
    #if false
     public class TouchExample : MonoBehaviour
     {
         private TouchManager touchManager;

         void Start()
         {
             touchManager = new TouchManager();
         }

         void Update()
         {
             touchManager.Update();

             if (touchManager.isTouched && touchManager.touchPhase == TouchPhase.Began)
             {
                 Debug.Log("タッチ開始: " + touchManager.touchPosition);
             }

             if (touchManager.isTouched && touchManager.touchPhase == TouchPhase.Moved)
             {
                 Debug.Log("タッチ移動: " + touchManager.touchPosition);
             }

             if (touchManager.isTouched && touchManager.touchPhase == TouchPhase.Ended)
             {
                 Debug.Log("タッチ終了: " + touchManager.touchPosition);
             }
         }
     } 
    #endif
}
