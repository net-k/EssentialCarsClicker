using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidReviewDialogView : MonoBehaviour
{
    [SerializeField]
    private Button _requestButton;

    public Button RequestButton => _requestButton;

    [SerializeField]
    private Button _reviewButton;
    public Button ReviewButton => _reviewButton;
    
    [SerializeField]
    private Button _notReviewButton;
    public Button NotReviewButton => _notReviewButton;
}
