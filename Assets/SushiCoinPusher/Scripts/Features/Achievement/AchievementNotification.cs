using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SushiCatcher.Achievement.AchievementNotificationDialog;

namespace SushiCatcher.Achievement
{
    public class AchievementNotification : MonoBehaviour
    {
        [SerializeField] private GameObject AchievementNotificationDialogPrefab;
        
        AchievementManager _achievementManager;

        private Queue<int> _notificationQueue = new Queue<int>();
        private bool _isShowing = false;

        [Inject]
        void Construct(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }

        private void Awake()
        {
            if (_achievementManager != null)
            {
                _achievementManager.OnAchievementCleared += OnAchievementCleared;
            }
        }

        private void OnDestroy()
        {
            if (_achievementManager != null)
            {
                _achievementManager.OnAchievementCleared -= OnAchievementCleared;
            }
        }

        private void OnAchievementCleared(int achievementId)
        {
            _notificationQueue.Enqueue(achievementId);
            ProcessQueue();
        }

        private void ProcessQueue()
        {
            if (_isShowing || _notificationQueue.Count == 0) return;

            int achievementId = _notificationQueue.Dequeue();
            ShowNotification(achievementId);
        }

        private void ShowNotification(int achievementId)
        {
            _isShowing = true;

            // Prefabからインスタンス生成
            var go = Instantiate(AchievementNotificationDialogPrefab, transform);
            var presenter = go.GetComponent<AchievementNotificationDialogPresenter>();
            presenter.Initialize(_achievementManager);

            presenter.Show(achievementId, () =>
            {
                Destroy(presenter.gameObject);
                _isShowing = false;
                ProcessQueue();
            });
        }
    }
}
