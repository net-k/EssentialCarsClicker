using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidReviewDialogPresenter : MonoBehaviour
{
    private AndroidReviewDialogView _view;
    [SerializeField]
    private string storeUrl;
    private string requestUrl = "https://docs.google.com/forms/d/e/1FAIpQLSf8bYFrrT0W_S2DagYW485kMZH3u7E7mf6czErhvmqFXXphOg/viewform?usp=sf_link";
    private KumaFramework.Review _review;
    
    void Awake()
    {
        _view = GetComponent<AndroidReviewDialogView>();
        
        _view.RequestButton.onClick.AddListener(() =>
        {
            Application.OpenURL(requestUrl);
            _review.DoneReview();
            gameObject.SetActive(false);
        });
        
        _view.ReviewButton.onClick.AddListener(() =>
        {
            Application.OpenURL(storeUrl);
            _review.DoneReview();
            gameObject.SetActive(false);
        });
        
        _view.NotReviewButton.onClick.AddListener(() =>
        {
            _review.DoneReview();
            gameObject.SetActive(false);
        });
    }
    
    public void Show(string getUrl, KumaFramework.Review review)
    {
        gameObject.SetActive(true);
        storeUrl = getUrl;
        _review = review;
    }
}
