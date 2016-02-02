using UnityEngine;
using System.Collections;

public class Reposition : MonoBehaviour {
	Vector2 target;
	// Use this for initialization
	void Start () {
		StartCoroutine (LerpToTarget ());	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator LerpToTarget(){

		while(true){
			Vector2 startPos = transform.localPosition;
			Vector2 target = new Vector2 (Random.Range (-299.0f, -37.0f),  Random.Range (-182.0f, 58.4f));
			float lerpTime = Random.Range (3.0f, 9.0f);
			float t = 0;
			while (t < lerpTime) {
				t += Time.deltaTime;
				transform.localPosition = Vector2.Lerp (startPos, target, t/lerpTime);
				yield return new WaitForEndOfFrame ();
			}
		}
	}

}
