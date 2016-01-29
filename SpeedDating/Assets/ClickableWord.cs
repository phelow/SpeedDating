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
	}

	public void OnClick(){
		Debug.LogError ("Click");
		DateTextReader.AddWord (word);
		Destroy (this.gameObject);
	}
		
	public void OnMouseDown()
	{
		OnClick ();
	}

}
