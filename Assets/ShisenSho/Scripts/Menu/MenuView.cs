using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private Button _titleBackButton = null;

    [SerializeField] private Button _newGameButton = null;

    [SerializeField] private Button _closeButton = null;

    public Button TitleBackButton => _titleBackButton;

    public Button NewGameButton => _newGameButton;
    public Button CloseButton => _closeButton;
}
