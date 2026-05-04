using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stickin.menus.type1
{
    public class SelectDifficultMenu : BaseMenu
    {
        [SerializeField] private Transform _popup;
        [SerializeField] private List<Color> _colors;
        [SerializeField] private DifficultBtn _btnPrefab;
        [SerializeField] private RectTransform _buttonsParent;
        [SerializeField] private float _startBorder = 60;
        [SerializeField] private float _border = 20;

        private Action<int> _selectDifficultCallback;
        
        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            var difficults = (DifficultsConfig) data["difficults"];
            if (difficults != null)
                InitButtons(difficults.Difficults);
            else
                Debug.LogError("SelectDifficultMenu: DifficultsConfig is null");
            
            var position = (Vector3) data["position"];
            _selectDifficultCallback = (Action<int>)data["callback"];
            _popup.position = position;
        }

        private void InitButtons(List<DifficultConfig> difficultsDifficults)
        {
            _btnPrefab.gameObject.SetActive(false);
            var y = _startBorder;

            for(var i = difficultsDifficults.Count - 1; i >= 0; i--)
            {
                var difficultConfig = difficultsDifficults[i];
                
                var btn = Instantiate(_btnPrefab, _buttonsParent);
                btn.gameObject.SetActive(true);
                btn.Init(difficultConfig, _colors.GetElement(i), OnClickDifficultBtn);
                
                var btnRt = btn.RectTransform();
                y += btnRt.sizeDelta.y / 2 + _border;
                btnRt.anchoredPosition = new Vector2(0, y);
                y += btnRt.sizeDelta.y / 2;
            }

            _buttonsParent.sizeDelta = new Vector2(_buttonsParent.sizeDelta.x, y + _border);
        }

        public void OnClickDifficultBtn(DifficultConfig config)
        {
            _selectDifficultCallback?.Invoke(config.Number);
        }
    }
}
