using System.Collections;
using System.Collections.Generic;
using ShisenSho.Game;
using UnityEngine;

public class ShisenShoHaiSelector : MonoBehaviour
{
    [SerializeField] 
    private GameObject _cursorObject = null;

    [SerializeField] private ShisenShoPresenter _shisenShoPresenter = null;

    public void UpdateCursor(int stateTarget)
    {
        if (stateTarget < 0)
        {
            _cursorObject.SetActive(false);
        }
        else
        {
            _cursorObject.SetActive(true);
        }
       Vector3 position = _shisenShoPresenter.GetHaiPosition(stateTarget);

       _cursorObject.transform.position = position;
    }
}
