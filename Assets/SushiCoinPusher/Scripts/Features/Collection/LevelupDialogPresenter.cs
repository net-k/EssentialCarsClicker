using System;
using UnityEngine;

namespace FruitShop
{
    public class LevelupDialogPresenter : MonoBehaviour
    {
        [SerializeField]
        private LevelupDialogView _view;

        private void Awake()
        {
            _view.OkButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }
    }
}
