using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour {
	public int sceneNum;
	public void Start(){
		if (PlayerPrefs.GetInt ("" + sceneNum, 0) == 1) {
			this.GetComponent<Text> ().color = Color.blue;
		}
	}

	public void OnMouseDown()
	{
		SceneManager.LoadScene (sceneNum);
		Destroy (this.gameObject);
	}
}
