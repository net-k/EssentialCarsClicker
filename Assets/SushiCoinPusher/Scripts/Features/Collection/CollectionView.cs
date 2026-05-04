using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FruitShop.Scripts
{
    public class CollectionView : MonoBehaviour
    {
        [SerializeField]        
        private Button _backButton;
        public Button BackButton => _backButton;
        
        [SerializeField]
        private List<GameObject> _fruitsList;

        public void VisibleFruits( int level, bool visible )
        {
            int index = level - 1;
            _fruitsList[index].gameObject.SetActive(visible);
        }

        public Button GetFruitButton(int index)
        {
            if (index >= 0 && index < _fruitsList.Count)
            {
                return _fruitsList[index].GetComponent<Button>();
            }
            return null;
        }

        public int GetFruitsCount()
        {
            return _fruitsList.Count;
        }
    }
}
