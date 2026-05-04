using System.Collections.Generic;
using UnityEngine;
using Zenject;

[System.Serializable]
public class LocalizeData
{
   public string key = null;
   public string Japanese = "";
   public string English  = "";
};

[CreateAssetMenu( menuName = "Aquarium/CreateLocalizeMaster", fileName = "LocalizeDataMaster" )]
public class LocalizeDataMaster : ScriptableObject
{
    [SerializeField]
    public List<LocalizeData> LocalizeDataList;
}
