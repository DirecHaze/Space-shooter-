using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
	
	public float shakeAmount = 0.7f;
	public float TimeofShake = 0.5f;
 
	public void CamShakeOn()
	{
		StartCoroutine(CamShakeNow());
	}
    IEnumerator CamShakeNow()
	{
		Vector3 OrginPosition = transform.position;
		float elapsed = 0f;
		while (elapsed < TimeofShake)
		{

			float UpShake = Random.Range(-1f, 1f) * shakeAmount;
			float DownShake = Random.Range(-1f, 1f) * shakeAmount;
			transform.position = new Vector3(UpShake, DownShake, -10f);
			elapsed += Time.deltaTime;
			yield return null; 
		}
		transform.position = OrginPosition;
	}
}


