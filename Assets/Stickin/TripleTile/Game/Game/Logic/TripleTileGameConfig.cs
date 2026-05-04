using System.Collections.Generic;
using UnityEngine;

namespace stickin.tripletile
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Stickin/Triple Tile/Game Config")]
    public class TripleTileGameConfig : LevelsGameConfig
    {
        [Space(20)]
        [Header("Triple Tile")]
        public List<Sprite> Sprites;
        public int MaxCountSelectedCells = 7;

        // public List<Sprite> LevelsSprites;
        
        // [Header("Params")]
        // public float HideCanvasesDuration = 0.3f;
        // public float ShowWinDelay = 0.5f;
        
        private LevelsModel<LevelModel> _levelsModel;
    }
}
