using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TitleMatch.Scripts.Presentation.TitleLogo
{
    public class TitleLogoView : MonoBehaviour
    {
        public enum LogoIndex
        {
            English,
            Japanese,
        }
        
        [SerializeField]
        List<Image> _images = new List<Image>();
        
        public void Initialize( LogoIndex index )
        {
            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].gameObject.SetActive( i == (int)index );
            }
        }
    }
}
