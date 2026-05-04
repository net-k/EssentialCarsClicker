using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    GameObject leftWall;
    [SerializeField]
    GameObject rightWall;

    [Header("Animation Settings")]
    [SerializeField]
    float animationDuration = 0.5f;
    [SerializeField]
    float moveDistance = 1.0f;

    private Vector3 leftOriginalPos;
    private Vector3 rightOriginalPos;
    private Coroutine currentCoroutine;

    void Awake()
    {
        if (leftWall != null) leftOriginalPos = leftWall.transform.localPosition;
        if (rightWall != null) rightOriginalPos = rightWall.transform.localPosition;
    }

    public void Show()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        // If inactive, set start position to below so they animate up
        if (leftWall != null && !leftWall.activeSelf)
            leftWall.transform.localPosition = leftOriginalPos - Vector3.up * moveDistance;
        if (rightWall != null && !rightWall.activeSelf)
            rightWall.transform.localPosition = rightOriginalPos - Vector3.up * moveDistance;

        currentCoroutine = StartCoroutine(AnimateWalls(true));
    }

    public void Hide()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AnimateWalls(false));
    }

    private IEnumerator AnimateWalls(bool show)
    {
        float time = 0f;
        
        Vector3 lStart = leftWall != null ? leftWall.transform.localPosition : Vector3.zero;
        Vector3 rStart = rightWall != null ? rightWall.transform.localPosition : Vector3.zero;
        
        Vector3 lEnd = show ? leftOriginalPos : leftOriginalPos - Vector3.up * moveDistance;
        Vector3 rEnd = show ? rightOriginalPos : rightOriginalPos - Vector3.up * moveDistance;

        if (show)
        {
            if (leftWall != null) leftWall.SetActive(true);
            if (rightWall != null) rightWall.SetActive(true);
        }

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / animationDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            if (leftWall != null) leftWall.transform.localPosition = Vector3.Lerp(lStart, lEnd, t);
            if (rightWall != null) rightWall.transform.localPosition = Vector3.Lerp(rStart, rEnd, t);

            yield return null;
        }

        if (leftWall != null) leftWall.transform.localPosition = lEnd;
        if (rightWall != null) rightWall.transform.localPosition = rEnd;

        if (!show)
        {
            if (leftWall != null) leftWall.SetActive(false);
            if (rightWall != null) rightWall.SetActive(false);
        }

        currentCoroutine = null;
    }
}
