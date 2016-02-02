using UnityEngine;
using System.Collections;

public class Reposition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D(Collision2D col){

		transform.localPosition = new Vector2 (Random.Range (-299.0f, -37.0f),  Random.Range (-182.0f, 58.4f));
	}
}
