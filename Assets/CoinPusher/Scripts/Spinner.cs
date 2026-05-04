using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public float rotationSpeed = 1.0f;
    public Vector3 spinDirection;

    private bool isSpinning = true;
    private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (isSpinning)
        {
            transform.RotateAround(GetComponent<BoxCollider>().bounds.center, spinDirection, rotationSpeed * Time.deltaTime);
        }
	}

    /// <summary>
    /// Stops the reel and aligns it to its initial rotation.
    /// </summary>
    [ContextMenu("Align Reel")]
    public void AlignReel()
    {
        isSpinning = false;
        transform.rotation = initialRotation;
    }

    /// <summary>
    /// Resumes spinning.
    /// </summary>
    [ContextMenu("Start Spinning")]
    public void StartSpinning()
    {
        isSpinning = true;
    }
}
