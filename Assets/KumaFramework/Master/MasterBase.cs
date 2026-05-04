using System;
using System.Collections;
using UnityEngine;

namespace TohoReversi.Master
{
    
    interface IMasterBase
    {
        public bool IsLoaded();
        public bool Load();
    }
    
    public abstract class MasterBase<T> : IMasterBase
    {
        protected T[] _data;
        
        protected bool _isLoaded = false;
        public bool Load(string path)
        {
            TextAsset textasset = new TextAsset();
            textasset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
            if (textasset == null)
            {
                Debug.LogWarning("Failed to load resource at path: " + path);
                return false;
            }

            try
            {
                _data = CSVSerializer.Deserialize<T>(textasset.text);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to deserialize data: " + e.Message);
                return false;
            }

            // Debug.Log("Data count：" + _data.Length);

            _isLoaded = true;

            return true;
        } 

        public bool IsLoaded()
        {
            return _isLoaded;
        }

        public T[] GetDataList()
        {
            return _data;
        }
        
        public virtual bool Load()
        {
            throw new NotImplementedException();
        }
    }
}