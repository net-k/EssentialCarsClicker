using System;
using UnityEngine;

namespace MemoryOnline.Presentation
{
    public class NotEnoughLifePresenter : MonoBehaviour
    {
        [SerializeField] private NotEnoughLifeView _view = null;

        private void Awake()
        {
            _view.OkButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
