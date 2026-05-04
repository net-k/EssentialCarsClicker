using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class MenusPreview : MonoBehaviour
    {
        public MenusData MenusData;
        public GameObject BgPrefab;
        public string PathForVariantPrefabs;
    }
}
