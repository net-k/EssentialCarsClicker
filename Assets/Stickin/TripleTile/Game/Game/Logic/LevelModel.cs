using System.Collections.Generic;
using UnityEngine;

namespace stickin.tripletile
{
    [System.Serializable]
    public class LayerModel
    {
        // short names for small size file with levels
        [SerializeField] private int w;
        [SerializeField] private int h;
        // [SerializeField] private List<int> d;
        [SerializeField] private string v;

        public int Width => w;
        public int Height => h;
        public string Value => v;

        public void Init(List<List<int>> l)
        {
            w = l.Count;
            if (w > 0)
                h = l[0].Count;

            // d = new List<int>();
            v = string.Empty;
            foreach (var sL in l)
            {
                if (h != sL.Count)
                    Debug.LogError("error. Not square layer");
                foreach (var ssL in sL)
                {
                    v += ssL.ToString();
                    // d.Add(ssL);
                }
            }
        }

        public int GetIntValue(int i, int j)
        {
            var index = RectBoardExtensions.GetIndex(i, j, Height);
            return int.Parse(Value[index].ToString());
        }
    }
    
    [System.Serializable]
    public class LevelModel
    {
        // short names for small size file with levels
        [SerializeField] private int[] l;
        [SerializeField] private int m; // monetization
        
        public List<CellGameModel> Cells { get; private set; }
        public int CountTiles { get; private set; }
        
        public void SetData(int t, List<CellGameModel> cells, int m1, int m2, int j1, int j2)
        {
            l = new int[cells.Count * 3 + 1];

            l[0] = t;
            var i = 1;
            foreach (var cell in cells)
            {
                l[i] = cell.Index.x;
                l[i + 1] = cell.Index.y;
                l[i + 2] = cell.Layer;

                i += 3;
            }
        }

        public void Init()
        {
            Cells = new List<CellGameModel>();
            CountTiles = l[0];

            var countBoardTiles = (l.Length -1) / 3;

            var types = GenerateTypes(countBoardTiles, CountTiles);

            for (var i = 1; i < l.Length; i += 3)
            {
                var cell = new CellGameModel();
                cell.Index = new Vector2Int(l[i], l[i + 1]);
                cell.Layer = l[i + 2];
                
                var randomTypeIndex = Random.Range(0, types.Count);
                cell.Type = types[randomTypeIndex];
                types.RemoveAt(randomTypeIndex);
                
                Cells.Add(cell);
            }
        }

        private List<int> GenerateTypes(int count, int countTypes)
        {
            var types = new List<int>(countTypes);
            for(var i = 0; i < countTypes; i++)
                types.Add(i);
            
            var result = new List<int>(count);
            for (var i = 0; i < count; i+=3)
            {

                var type = Random.Range(0, countTypes);
                if (types.Count > 0)
                {
                    type = types[0];
                    types.RemoveAt(0);
                }

                result.Add(type);
                result.Add(type);
                result.Add(type);

                if (result.Count == count)
                    break;
                else if(result.Count > count)
                    Debug.LogError("TripleTile levelModel: ERROR GenerateTypes count > tiles count");
            }

            result.Shuffle();
            return result;
        }
    }
}