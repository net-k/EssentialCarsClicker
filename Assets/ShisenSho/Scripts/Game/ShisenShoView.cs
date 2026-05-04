using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShisenShoView : MonoBehaviour
{
    [SerializeField]
    private GameObject _haiParent = null;

    public GameObject HaiParent => _haiParent;


}
