using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus.type1
{
    public class TutorialMenu : BaseMenu
    {
        [SerializeField] private Image _mainImage;
        [SerializeField] private TextLocalization _descriptionText;
        [SerializeField] private Transform _dotsParent;
        [SerializeField] private Transform _dotPrefab;
        [SerializeField] private Transform _mainDot;
        [SerializeField] private Button _nextBtn;
        
        private TutorialConfig _config;
        private int _stepIndex;
        private List<Transform> _dots;

        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            if (data != null && data.ContainsKey("TutorialConfig"))
            {
                _stepIndex = 0;
                _config = (TutorialConfig) data["TutorialConfig"];

                if (_config != null)
                {
                    RefreshDots(_config.Steps.Count);
                    ShowCurrentStep();
                }
                else
                    Debug.LogError("TutorialConfig is null. Need added TutorialConfig in 'GameConfig->CustomConfigs'");
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            _dots = new List<Transform>();
            _nextBtn.onClick.AddListener(OnClickNext);
        }

        private void OnClickNext()
        {
            var isLastStep = _stepIndex >= _config.Steps.Count - 1;

            if (isLastStep)
            {
                Hide();
            }
            else
            {
                _stepIndex = Mathf.Clamp(_stepIndex + 1, 0, _config.Steps.Count - 1);
                ShowCurrentStep();
            }
        }

        private void ShowCurrentStep()
        {
            if (_stepIndex >= 0 && _stepIndex < _config.Steps.Count)
            {
                var step = _config.Steps[_stepIndex];
                _mainImage.sprite = step.Image;
                _descriptionText.SetText(step.Text);

                _mainDot.SetParent(_dots[_stepIndex]);
                _mainDot.transform.localPosition = Vector3.zero;
            }
        }

        private void RefreshDots(int count)
        {
            _mainDot.SetParent(transform);
            
            foreach (var dot in _dots)
                Destroy(dot.gameObject);

            _dots.Clear();

            for (var i = 0; i < count; i++)
            {
                var dot = Instantiate(_dotPrefab, _dotsParent);
                dot.gameObject.SetActive(true);
                _dots.Add(dot);
            }
        }
    }
}