using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public static class CubeBoardExtensions
    {
        public static List<T> AddedCells<T>(
            List<Vector3Int> indexes, 
            T prefab, 
            Transform parent,
            Vector3 cellSize,
            bool centered = false) where T : MonoBehaviour
        {
            var result = new List<T>();

            var max = Vector3.zero;
            
            foreach (var index in indexes)
            {
                var cell = GameObject.Instantiate(prefab, parent);
                result.Add(cell);
                
                var pos = GetCellPosition(cellSize, index);
                cell.transform.localPosition = pos;
                
                max.x = Mathf.Max(pos.x, max.x);
                max.y = Mathf.Max(pos.y, max.y);
                max.z = Mathf.Max(pos.z, max.z);
            }

            if (centered)
            {
                foreach (var cell in result)
                {
                    cell.transform.localPosition = cell.transform.localPosition - max / 2f;
                }
            }

            return result;
        }
        
        public static Vector3 GetCellPosition(
            Vector3 cellSize,
            Vector3Int index)
        {
            return new Vector3(cellSize.x * index.x, cellSize.y * index.y, cellSize.z * index.z);
        }
    }
}