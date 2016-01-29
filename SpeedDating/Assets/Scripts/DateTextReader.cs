using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DateTextReader : MonoBehaviour {
	public static DateTextReader instance;

	public string TextFile;
	[SerializeField]
	private Text DateText;
	[SerializeField]
	private Text ResponseText;
	public List<Sentence> sentences;

	public enum WordType{
		Adjective,
		Noun,
		Verb,
		Adverb
	}

	public class Sentence{
		public Sentence(string datePrompt, string playerResponse, List<WordType> slots, List<Word> words, string goodResponse, string badResponse){
			_slots = slots;
			_words = words;
			_datePrompt = datePrompt;
			_goodResponse = goodResponse;
			_badResponse = badResponse;
			_playerResponse = playerResponse;
		}

		public Sentence(Sentence s){
			_slots = s._slots;
			_words = s._words;
			_datePrompt = s._datePrompt;
			_goodResponse = s._goodResponse;
			_badResponse = s._badResponse;
			_playerResponse = s._playerResponse;
		}

		public List<WordType> _slots;
		public List<Word> _words;
		public string _datePrompt;
		public string _goodResponse;
		public string _badResponse;
		public string _playerResponse;

		public string GetDateText(){
			return _datePrompt;
		}

		public string GetPlayerResponse(){
			return _playerResponse;
		}

	}

	public class Word{
		public Word(WordType type, string text, int points){
			_type = type;
			_text = text;
			_points = points;
		}

		public WordType _type;
		public string _text;
		public int _points;
	}



	// Use this for initialization
	void Start () {
		instance = this;
		sentences = new List<Sentence> ();
		FileInfo sourceFile = new FileInfo ("Assets\\" + TextFile + ".txt");
		StreamReader reader = sourceFile.OpenText ();

		string line;

		line = reader.ReadLine();
		do{
			List<Word> words = new List<Word>();
			List<WordType> slots = new List<WordType>();

			string prompt = line.Substring(0, line.IndexOf(":"));

			string DatePrompt = prompt.Substring(0,line.IndexOf("}"));

			prompt = prompt.Substring(line.IndexOf("}") + 1, prompt.Length - prompt.IndexOf("}") -1);

			string displayPrompt = "";

			int nextWord = 0;

			nextWord = prompt.IndexOf("[");
			while(nextWord != -1){
				displayPrompt += prompt.Substring(0,nextWord) + "_____";

				string wordTypeString = prompt.Substring(nextWord + 1, prompt.IndexOf("]") - nextWord -1);


				Debug.Log(wordTypeString);
				switch(wordTypeString){
				case "adjective":
					slots.Add(WordType.Adjective);
					break;
				case "noun":
					slots.Add(WordType.Noun);
					break;
				case "verb":
					slots.Add(WordType.Verb);
					break;
				case "adverb":
					slots.Add(WordType.Adverb);
					break;
				}


				prompt = prompt.Substring(prompt.IndexOf("]") + 1, prompt.Length - (prompt.IndexOf("]") + 1) );
				nextWord = prompt.IndexOf("[");
			}

			prompt = line.Substring(line.IndexOf(":"),line.Length - line.IndexOf(":"));
			Debug.Log("words:" + prompt);

			nextWord = prompt.IndexOf("<");
			while(nextWord != -1){
				string wordString = prompt.Substring(nextWord + 1, prompt.IndexOf("|") - nextWord - 1);

				Debug.Log("wordString:" + wordString);

				prompt = prompt.Substring(prompt.IndexOf("|") + 1, prompt.Length - prompt.IndexOf("|") - 1);
				Debug.Log(prompt.Substring(0, prompt.IndexOf("|")));
				int points = int.Parse(prompt.Substring(0, prompt.IndexOf("|")));
				prompt = prompt.Substring(prompt.IndexOf("|") + 1, prompt.Length - prompt.IndexOf("|") - 1);

				string wordTypeString = prompt.Substring(0, prompt.IndexOf(">"));
				Debug.Log(wordTypeString);
				switch(wordTypeString){
				case "adjective":
					words.Add(new Word(WordType.Adjective,wordString,points));
					break;
				case "noun":
					words.Add(new Word(WordType.Noun,wordString,points));
					break;
				case "verb":
					words.Add(new Word(WordType.Verb,wordString,points));
					break;
				case "adverb":
					words.Add(new Word(WordType.Adverb,wordString,points));
					break;
				}



				prompt = prompt.Substring(prompt.IndexOf(">") + 1, prompt.Length - (prompt.IndexOf(">") + 1) );
				nextWord = prompt.IndexOf("<");
			}
			string goodResponse = prompt.Substring(prompt.IndexOf("{") + 1, prompt.IndexOf("|") - prompt.IndexOf("{") - 1);
			string badResponse = prompt.Substring(prompt.IndexOf("|") + 1,prompt.Length - prompt.IndexOf("|") - 1);

			Debug.Log("goodResponse:" + goodResponse + " badResponse:" + badResponse);

			print("136 prompt:" + prompt);
			print(DatePrompt);
			print("line:" + line + " prompt:" + displayPrompt);

			line = reader.ReadLine();
			sentences.Add(new Sentence(DatePrompt,displayPrompt,slots,words,goodResponse,badResponse));
		} while (line != null);

		StartCoroutine (Date ());
	}
	public Canvas canvas;
	public GameObject ClickableWord;
	private bool sentenceCompleted = false;

	private Word toAdd;

	public static void AddWord(Word w){
		instance.toAdd = w;
	}

	public IEnumerator Date (){
		//while time is left
		float timeLeft = 120.0f;
		int position = 0;

		while (timeLeft > 0.0f) {
			List<Word> selectedWords = new List<Word>();

			//pick the relevant sentence
			if (position >= sentences.Count) {
				position = 0;
			}

			Sentence s = new Sentence(sentences[position]);

			DateText.text = s.GetDateText();

			ResponseText.text = s.GetPlayerResponse();
			List<GameObject> wordsOnScreen = new List<GameObject>();
			foreach (Word w in s._words) {
				GameObject newGO = GameObject.Instantiate (ClickableWord);
				newGO.transform.parent = canvas.transform;
				RectTransform rt = canvas.GetComponent<RectTransform> ();
				newGO.transform.localPosition = new Vector2 (Random.Range (-350.0f, 350.0f),  Random.Range (-250.0f, 250.0f));
				newGO.transform.localScale = Vector3.one;
				newGO.GetComponent<ClickableWord> ().init (w);
				wordsOnScreen.Add (newGO);
			}

			position++;
			timeLeft -= Time.deltaTime;

			while (sentenceCompleted == false) {
				if (toAdd != null) {
					selectedWords.Add (toAdd);
					string curText = ResponseText.text;

					int posSpace = curText.IndexOf ("_____");

					ResponseText.text = curText.Substring (0, posSpace) + " " + toAdd._text + " " + curText.Substring (posSpace + 5, curText.Length - posSpace-5);

					toAdd = null;
				}

				yield return new WaitForEndOfFrame ();
			}

			foreach (GameObject w in wordsOnScreen) {
				if (w != null) {
					Destroy (w);
				}
			}
		}
	}
}
