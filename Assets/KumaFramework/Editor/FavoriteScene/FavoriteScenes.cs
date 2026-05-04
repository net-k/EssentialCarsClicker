using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FavoriteScenes", menuName = "ScriptableObjects/FavoriteScenes", order = 1)]
public class FavoriteScenes : ScriptableObject
{
    public List<SceneAsset> scenes = new List<SceneAsset>();
}
