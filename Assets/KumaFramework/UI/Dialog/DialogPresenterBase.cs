using UnityEngine;

namespace KumaFramework
{
    public class DialogPresenterBase : MonoBehaviour
    {
        DialogPresenterBase _previousDialog;

        public DialogPresenterBase PreviousDialog => _previousDialog;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Show(DialogPresenterBase previousDialog)
        {
            _previousDialog = previousDialog;
            Show();
        }
        public void SetPreviousDialog(DialogPresenterBase previousDialog)
        {
            _previousDialog = previousDialog;
        }
        
        public virtual void ShowAndHidePrevious(DialogPresenterBase previousDialog)
        {
            _previousDialog = previousDialog;
            Show();
            if (_previousDialog != null)
            {
                _previousDialog.Hide();
            }
        }

        // 共通の非表示処理
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        // Hide したら、前のダイアログを表示する
        public void HideAndShowPrevious()
        {
            Hide();
            if (_previousDialog != null)
            {
                _previousDialog.Show();
            }
        }
    }
}
