using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiCreator : MonoBehaviour
{

    // 生成するCardオブジェクト
    public GameObject CardPrefab;

    // 「カード」を生成する親オブジェクト
    public RectTransform CardCreateParent;

    void Start () {

        GameObject hai = Instantiate<GameObject> (this.CardPrefab, this.CardCreateParent);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
