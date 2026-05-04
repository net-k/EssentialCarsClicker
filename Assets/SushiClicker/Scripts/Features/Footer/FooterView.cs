using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SushiClicker
{
    public class FooterView : MonoBehaviour
    {
        [SerializeField] private Button _buildingButton = null;
        [SerializeField] private Button _upgradeButton = null;

        public IObservable<Unit> OnBuildingButtonClick
            => _buildingButton.OnClickAsObservable();

        public IObservable<Unit> OnUpgradeButtonClick
            => _upgradeButton.OnClickAsObservable();
    }
}
