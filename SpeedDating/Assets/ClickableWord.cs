using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickableWord : MonoBehaviour {
	public Text t;
	public DateTextReader.Word word;

	public void init(DateTextReader.Word w){
		gameObject.AddComponent<BoxCollider2D> ();
		t.text = w._text;
		word = w;
		StartCoroutine (InterpolateRandomly ());
		t.color = Random.ColorHSV ();
	}

	public void OnClick(){
		DateTextReader.AddWord (word);
		Destroy (this.gameObject);
		SoundManager.PlayClickWord ();
	}
		
	public void OnMouseDown()
	{
		OnClick ();
	}

	public IEnumerator InterpolateRandomly(){
		while (true) {
			//pick a random color
			Color lastColor = t.color;
			Color randomColor = Random.ColorHSV();
			//interpolate to that color

			float lerpTime = Random.Range (.2f, 1.0f);
			float timePassed = 0.0f;
			while (timePassed < lerpTime) {
				timePassed += Time.deltaTime;
				t.color = Color.Lerp (lastColor, randomColor, timePassed / lerpTime);

				yield return new WaitForEndOfFrame ();
			}

		}

	}

}
