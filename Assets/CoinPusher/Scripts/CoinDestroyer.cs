using UnityEngine;
using System.Collections;

public class CoinDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		// Make sure we do not trip up the Bumpers since they nest in the destroyer
		// バンパーが破壊装置に入らないようにする
		if( !other.gameObject.CompareTag("Bumpers") )
		{
            var coinEffect = other.gameObject.GetComponent<CoinEffect>();
            if (coinEffect != null)
            {
                // Play the sound effect
                coinEffect.playDestroyedSFX();
                coinEffect.removeCoin();
            }
            else
            {
                // CoinEffectがないオブジェクトが落ちてきた場合も、プールに戻せるなら戻すか、破棄する
                if (CoinPool.Instance != null)
                {
                    CoinPool.Instance.Return(other.gameObject);
                }
                else
                {
                    Destroy(other.gameObject);
                }
            }
		}
	}

	// For drawing inside of the Unity Editor
	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
	}
}