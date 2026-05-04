using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace stickin
{
    public enum LayoutGenerationType
    {
        None,
        SimmetricX,
        SimmetricZ,
        Line
    }
    
    public class LayoutGridCreate : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _prefabs;
        [SerializeField] private LayoutGenerationType _generationType = LayoutGenerationType.None;
        [SerializeField] private Vector2 _minPosition; 
        [SerializeField] private Vector2 _maxPosition;
        [SerializeField] private Vector2Int _countRandom;
        [SerializeField] private float _minDistanceBetween = 1f;

        public LayoutGenerationType GenerationType => _generationType;
        public List<GameObject> Prefabs => _prefabs;
        public Vector2Int CountRandom => _countRandom;

        public void GeneratePositions()
        {
            var count = transform.childCount;
            var positions = GeneratePositions(count, _generationType);
            
            for (var i = 0; i < count; i++)
                transform.GetChild(i).localPosition = positions[i];
        }

        public void Clear()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(
                Random.Range(_minPosition.x, _maxPosition.x), 
                0,
                Random.Range(_minPosition.y, _maxPosition.y));
        }

        private Vector3 GetGridPosition(int index, int count)
        {
            var size = _maxPosition - _minPosition;
            var grid = (int)Mathf.Sqrt(count);
            var step = size / grid;
            var i = index / grid;
            var j = index % grid;

            var result = new Vector3(
                Random.Range(0, step.x) + _minPosition.x + step.x * i, 
                0, 
                Random.Range(0, step.y) + _minPosition.y + step.y * j);
            
            // var result = new Vector3(
            //     _minPosition.x + step.x * i, 
            //     0, 
            //     _minPosition.y + step.y * j);
            
            return result;
        }

        private List<Vector3> GeneratePositions(int count, LayoutGenerationType layoutGenerationType)
        {
            var result = new List<Vector3>();
            var maxSteps = 10000;

            if (layoutGenerationType == LayoutGenerationType.Line)
            {
                var step = (_maxPosition - _minPosition) / count;
                var pos = _minPosition;
                for (var i = 0; i < count; i++)
                {
                    result.Add(new Vector3(pos.x, 0, pos.y));
                    pos += step;
                }
            }
            else
            {
                do
                {
                    maxSteps--;
                    result.Clear();

                    for (var i = 0; i < count; i++)
                    {
                        var position = GetRandomPosition();
                        result.Add(position);

                        if (layoutGenerationType == LayoutGenerationType.SimmetricX)
                        {
                            position.x = -position.x;
                            result.Add(position);
                            i++;
                        }
                        else if (layoutGenerationType == LayoutGenerationType.SimmetricZ)
                        {
                            position.z = -position.z;
                            result.Add(position);
                            i++;
                        }
                    }

                    if (IsCorrectPositions(result, _minDistanceBetween))
                        break;
                } while (maxSteps > 0);

                if (maxSteps == 0)
                    Debug.LogError($"Bad generation");
            }

            return result;
        }

        private bool IsCorrectPositions(List<Vector3> list, float minDistance)
        {
            for (var i = 0; i < list.Count; i++)
            {
                for(var j = i + 1; j < list.Count; j++)
                {
                    if (Vector3.Distance(list[i], list[j]) < minDistance)
                        return false;
                }
            }

            return true;
        }
    }
}