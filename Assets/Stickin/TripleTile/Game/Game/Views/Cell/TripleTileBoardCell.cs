using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace stickin.tripletile
{
    public class TripleTileBoardCell : MonoBehaviour
    {
        [SerializeField] private Image _bgImage;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteAtlas _atlas;

        private CellGameModel _model;

        public CellGameModel Model => _model;
        
        public void Init(CellGameModel model, List<Sprite> sprites)
        {
            _model = model;
            _model.OnChangeAvailable += OnChangeAvailable;
            _model.OnChangeIndex += OnChangeIndex;
            OnChangeAvailable();

            _iconImage.sprite = sprites.GetElement(_model.Type);
        }

        private void OnChangeIndex()
        {
            var thisRt = this.RectTransform();
            
            var newPos = RectBoardExtensions.GetCellPosition(thisRt, _model.Index, xPosMultuplier: 0.5f, yPosMultuplier: 0.5f);
            thisRt.DOAnchorPos(newPos, 0.5f);
        }

        private void OnChangeAvailable()
        {
            _animator.SetTrigger(_model.IsAvailable ? "open" : "locked");
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _model.Click();
            // Destroy(gameObject); // @TODO temp
        }

        public void Remove()
        {
            _animator.SetTrigger("remove");
        }

        public void EndAnimationRemove()
        {
            Destroy(gameObject);
        }
    }
}