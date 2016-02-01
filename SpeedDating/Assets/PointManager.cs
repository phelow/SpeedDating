using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointManager : MonoBehaviour {
	public static PointManager instance;
	public GameObject points;
	public GameObject canvas;
	// Use this for initialization
	void Start () {
		instance = this;
	}

	public static void GenPointsStatic(GameObject go){
		instance.GeneratePoints (go);
	}

	private void GeneratePoints(GameObject go){
		int nPoints = Random.Range (2, 10);
		for (int i = 0; i < nPoints; i++) {
			GameObject newPoints = GameObject.Instantiate (points);
			newPoints.transform.position = new Vector3 (newPoints.transform.position.x + Random.Range(-10,10),newPoints.transform.position.y + Random.Range(-10,10),newPoints.transform.position.z);
			newPoints.GetComponent<Rigidbody2D> ().AddForce (new Vector2(Random.Range (1, 10), Random.Range (1, 10)));
			newPoints.GetComponent<Text> ().color = new Color (Random.Range (0.0f, 255.0f), Random.Range (0.0f, 255.0f), Random.Range (0.0f, 255.0f));
			newPoints.transform.parent = canvas.transform;
		}
	}
}
