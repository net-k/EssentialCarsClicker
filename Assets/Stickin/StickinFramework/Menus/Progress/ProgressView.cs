using System.Collections.Generic;
using UnityEngine;

namespace stickin.menus
{
    public class ProgressView : MonoBehaviour
    {
        [SerializeField] private ProgressPieceView _piecePrefab;
        
        [Header("Values")]
        [SerializeField] private List<Color> _colors;
        [SerializeField] private List<int> _values;
        [SerializeField] private bool _initOnStart;

        private List<ProgressPieceView> _piecesViews;

        private void Start()
        {
            _piecePrefab.gameObject.SetActive(false);

            if (_initOnStart)
                Refresh(_colors, _values);
        }

        public void Refresh(List<Color> colors, List<int> values)
        {
            if (_piecesViews == null)
                _piecesViews = new List<ProgressPieceView>();
            
            var parentRt = _piecePrefab.transform.parent as RectTransform;
            var size = parentRt.rect.size;
            
            var sum = 0f;
            foreach (var value in values)
                sum += value;

            var x = 0f;
            for(var i = 0; i < values.Count; i++)
            {
                if (i >= _piecesViews.Count)
                {
                    var view = Instantiate(_piecePrefab, parentRt);
                    _piecesViews.Add(view);
                }

                _piecesViews[i].Init(colors[i], values[i], x, sum, size);
                x += values[i];
            }
        }
    }
}