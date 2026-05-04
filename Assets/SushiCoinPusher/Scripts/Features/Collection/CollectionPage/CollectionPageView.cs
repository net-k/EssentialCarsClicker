using UnityEngine;
using UnityEngine.UI;

namespace SushiCatcher.Collection.CollectionPage
{
    public class CollectionPageView : MonoBehaviour
    {
        [SerializeField]
        private Button _okButton = null;
        
        [SerializeField]
        Image _collectionImage;
        
        [SerializeField]
        Text _collectionNameText;
        
        [SerializeField]
        Text _collectionDescriptionText;

        public Button OkButton => _okButton;
        
        public Image CollectionImage => _collectionImage;
        
        public Text CollectionNameText => _collectionNameText;
        
        public Text CollectionDescriptionText => _collectionDescriptionText;
        
        
    }
}
