using UnityEngine;
using System.Collections;

public class PeriodicShake : MonoBehaviour {
	
	public float duration = 0.5f;
	public float speed = 3.0f;
	public float magnitude = 0.1f;
	
	public bool test = false;
	
	// -------------------------------------------------------------------------
	public void PlayShake() {
		
		StopAllCoroutines();
		StartCoroutine("Shake");
	}
	
	// -------------------------------------------------------------------------
	void Update() {
		if (test) {
			test = false;
			PlayShake();
		}
	}
	
	// -------------------------------------------------------------------------
	IEnumerator Shake() {
		
		float elapsed = 0.0f;
		float randomStartX = Random.Range(-1000.0f, 1000.0f);
		float randomStartY = Random.Range(-1000.0f, 1000.0f);
		
		Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map noise to [-1, 1]
			float x = Mathf.Sin(randomStartX + percentComplete * speed);
			float y = Mathf.Cos(randomStartY + percentComplete * speed);
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);
				
			yield return null;
		}
		
		Camera.main.transform.position = originalCamPos;
	}
}
