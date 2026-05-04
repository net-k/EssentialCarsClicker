using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class ObjectsPool<T> where T : MonoBehaviour
    {
        private List<T> _list;
        private T _prefab;
        private Transform _parent;
        private bool _autoDisable;

        public static ObjectsPool<T> NewInstance(int capacity, T prefab, Transform parent, bool autoDisable = true)
        {
            return new ObjectsPool<T>(capacity, prefab, parent, autoDisable);
        }

        private ObjectsPool(int capacity, T prefab, Transform parent, bool autoDisable)
        {
            _prefab = prefab;
            _parent = parent;
            _list = new List<T>(capacity);
            _autoDisable = true;

            for (var i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(_prefab, _parent);
                Push(obj);
            }

            _autoDisable = autoDisable;
        }

        public T Pop()
        {
            T result;

            if (_list.Count <= 0)
            {
                if (_parent != null)
                    result = Object.Instantiate(_prefab, _parent);
                else
                    result = Object.Instantiate(_prefab);
            }
            else
            {
                result = _list[0];
                _list.RemoveAt(0);
            }

            result.gameObject.SetActive(true);

            return result;
        }

        public void Push(T obj)
        {
            if (_autoDisable)
                obj.gameObject.SetActive(false);

            _list.Add(obj);
        }
    }
}