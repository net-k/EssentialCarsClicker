using System;
using UnityEngine;

namespace stickin.tripletile
{
    [System.Serializable]
    public class CellGameModel : IComparable
    {
        public Vector2Int Index;
        public int Layer;
        public int Type;
        public GameObject View { get; private set; }

        public bool IsAvailable { get; private set; }

        public event Action OnChangeAvailable;
        public event Action OnChangeIndex;
        public event Action<CellGameModel> OnClick;

        public void SetAvailable(bool available)
        {
            if (IsAvailable != available)
            {
                IsAvailable = available;

                OnChangeAvailable?.Invoke();
            }
        }

        public void Click()
        {
            OnClick?.Invoke(this);
        }

        public void ChangeIndex(Vector2Int index)
        {
            Index = index;

            OnChangeIndex?.Invoke();
        }

        public int CompareTo(object obj)
        {
            if (obj == null) 
                return 1;
            
            var other = obj as CellGameModel;

            // return Layer.CompareTo(other.Layer);

            if (Layer > other.Layer)
                return 1;
            if (Layer < other.Layer)
                return -1;

            return other.Index.y.CompareTo(Index.y);
        }
    }
}