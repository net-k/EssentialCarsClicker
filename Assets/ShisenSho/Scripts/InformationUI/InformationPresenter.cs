using System;
using KumaFramework;
using UnityEngine;
using UniRx;

namespace ShisenSho.InformationUI
{
    public class InformationPresenter : MonoBehaviour
    {
        [SerializeField]
        InformationView _view;
        
        private const int ShowTime = 5; 
        private TouchManager _touchManager;

        void Awake()
        {
            _touchManager = new TouchManager();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            // ５秒で非表示
            Observable.Timer(TimeSpan.FromSeconds(ShowTime))
                .Subscribe(_ => Hide()); 
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        private void Update()
        {
            _touchManager.Update();
            if(_touchManager.isTouched && _touchManager.touchPhase == TouchPhase.Began)
            {
                Hide();
            }
        }
    }
}
