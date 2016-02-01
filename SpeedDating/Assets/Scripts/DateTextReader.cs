using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DateTextReader : MonoBehaviour {
	public static DateTextReader instance;

	public int minConfusedAffection;
	public int minVictoryAffection;

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
		Adverb,
		Weird
	}

	public class Sentence{
		public Sentence(string comparison, int affectionNeeded, string datePrompt, string playerResponse, List<WordType> slots, List<Word> words, string goodResponse, string badResponse, string confusedText){
			_slots = slots;
			_words = words;
			_datePrompt = datePrompt;
			_goodResponse = goodResponse;
			_badResponse = badResponse;
			_playerResponse = playerResponse;
			_comparison = comparison;
			_affectionNeeded = affectionNeeded;
			_confusedText = confusedText;
		}

		public Sentence(Sentence s){
			_slots = s._slots;
			_words = s._words;
			_datePrompt = s._datePrompt;
			_goodResponse = s._goodResponse;
			_badResponse = s._badResponse;
			_playerResponse = s._playerResponse;
			_affectionNeeded = s._affectionNeeded;
			_comparison = s._comparison;
			_confusedText = s._confusedText;
		}

		public List<WordType> _slots;
		public List<Word> _words;
		public string _confusedText;
		public string _datePrompt;
		public string _goodResponse;
		public string _badResponse;
		public string _playerResponse;
		public string _comparison;
		public int _affectionNeeded;

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
		wordsAdded = new Stack<Word> ();

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
			string comparison = prompt.Substring(0,1);
			prompt = prompt.Substring(line.IndexOf("|") + 1, prompt.Length - prompt.IndexOf("|") -1);
			string affectionString = prompt.Substring(0, prompt.IndexOf("|"));
			string promptString = prompt.Substring(prompt.IndexOf("|") + 1,prompt.Length - prompt.IndexOf("|") -1);
			int affectionNeeded = int.Parse(affectionString);

			prompt = prompt.Substring(line.IndexOf("|") + 1, prompt.Length - prompt.IndexOf("|") -1);

			string DatePrompt = prompt.Substring(1,prompt.IndexOf("}")-1);

			prompt = prompt.Substring(prompt.IndexOf("}") + 1, prompt.Length - prompt.IndexOf("}") - 1);

			string displayPrompt = "";

			int nextWord = 0;

			nextWord = prompt.IndexOf("[");
			bool buffer = true;
			while(nextWord != -1 || buffer){
				if(nextWord == -1){
					buffer = false;
					displayPrompt += prompt;

				}
				else{
					displayPrompt += prompt.Substring(0,nextWord) + "_____";
					string wordTypeString = prompt.Substring(nextWord + 1, prompt.IndexOf("]") - nextWord -1);

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
			}

			prompt = line.Substring(line.IndexOf(":"),line.Length - line.IndexOf(":"));

			nextWord = prompt.IndexOf("<");
			while(nextWord != -1){
				string wordString = prompt.Substring(nextWord + 1, prompt.IndexOf("|") - nextWord - 1);

				prompt = prompt.Substring(prompt.IndexOf("|") + 1, prompt.Length - prompt.IndexOf("|") - 1);
				int points = int.Parse(prompt.Substring(0, prompt.IndexOf("|")));
				prompt = prompt.Substring(prompt.IndexOf("|") + 1, prompt.Length - prompt.IndexOf("|") - 1);

				string wordTypeString = prompt.Substring(0, prompt.IndexOf(">"));
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
				case "weird":
					words.Add(new Word(WordType.Weird,wordString,points));
					break;
				}



				prompt = prompt.Substring(prompt.IndexOf(">") + 1, prompt.Length - (prompt.IndexOf(">") + 1) );
				nextWord = prompt.IndexOf("<");
			}
			string goodResponse = prompt.Substring(prompt.IndexOf("{") + 1, prompt.IndexOf("|") - prompt.IndexOf("{") - 1);
			string badResponse = prompt.Substring(prompt.IndexOf("|") + 1,prompt.IndexOf("+") - prompt.IndexOf("|") - 1);
			string confusedRespons = prompt.Substring(prompt.IndexOf("+") + 1, prompt.Length - prompt.IndexOf("+")-1);

			line = reader.ReadLine();
			sentences.Add(new Sentence(comparison, affectionNeeded, DatePrompt,displayPrompt,slots,words,goodResponse,badResponse, confusedRespons));
		} while (line != null);

		StartCoroutine (Date ());
	}
	public Canvas canvas;
	public GameObject ClickableWord;
	private bool sentenceCompleted = false;

	private Word toAdd;

	private Stack<Word> wordsAdded;

	public static void AddWord(Word w){
		instance.toAdd = w;
		instance.wordsAdded.Push (w);
	}

	private bool removeLastWord = false;

	public void Update(){
		/*if (Input.GetKeyDown (KeyCode.Backspace)) {
			removeLastWord = true;
		}*/
	}


	public Text points;
	public Text responseText;
	public Text timerText;

	public float timeLeft = 120.0f;
	public int curScene = 1;
	public IEnumerator Date (){
		//while time is left
		int position = 0;
		int totalPoints = 0;
		bool quit = false;
		while (timeLeft > 0.0f) {
			int roundPoints = 0;
			points.text = "" +totalPoints;
			List<Word> selectedWords = new List<Word>();

			Sentence s;
			do{
				//pick the relevant sentence
				if (position >= sentences.Count) {
					position = 0;
					quit = true;
				}
				s = new Sentence(sentences[position]);
				position++;
			} while((s._affectionNeeded < totalPoints && s._comparison == "<" )||( s._affectionNeeded > totalPoints && s._comparison == ">"));

			if (quit) {
				break;
			}

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

			timeLeft -= Time.deltaTime;
			timerText.text = "" + timeLeft;
			int sentencePosition = 0;
			bool confused = false;

			while (sentencePosition < s._slots.Count && timeLeft > 0.0f) {
				if (removeLastWord) {
					Word w = wordsAdded.Pop ();

					GameObject newGO = GameObject.Instantiate (ClickableWord);
					newGO.transform.parent = canvas.transform;
					RectTransform rt = canvas.GetComponent<RectTransform> ();
					newGO.transform.localPosition = new Vector2 (Random.Range (-350.0f, 350.0f),  Random.Range (-250.0f, 250.0f));
					newGO.transform.localScale = Vector3.one;
					newGO.GetComponent<ClickableWord> ().init (w);
					wordsOnScreen.Add (newGO);
					ResponseText.text.Replace (w._text, "_____");
					sentencePosition--;
				}

				if (toAdd != null) {
					selectedWords.Add (toAdd);
					string curText = ResponseText.text;

					int posSpace = curText.IndexOf ("_____");


					ResponseText.text = curText.Substring (0, posSpace) + " " + toAdd._text + " " + curText.Substring (posSpace + 5, curText.Length - posSpace-5);

					int pointsToAdd = 0;
					if (s._slots [sentencePosition] == toAdd._type) {
						pointsToAdd = toAdd._points;
					} else {
						pointsToAdd = -5;
						confused = true;
					}

					totalPoints += pointsToAdd;
					roundPoints = pointsToAdd;
					sentencePosition++;
					points.text = "" +totalPoints;
					toAdd = null;
				}
				timeLeft -= Time.deltaTime;
				timerText.text = "" + timeLeft;
				yield return new WaitForEndOfFrame ();
			}

			if (confused) {
				responseText.text = s._confusedText;
			}
			else if (roundPoints < 0) {
				responseText.text = s._badResponse;

			} else {

				responseText.text = s._goodResponse;
			}

			timeLeft -= Time.deltaTime;
			timerText.text = "" + timeLeft;
			yield return new WaitForEndOfFrame ();

			foreach (GameObject w in wordsOnScreen) {
				if (w != null) {
					Destroy (w);
				}
			}
		}

		if (!quit) { //sentences were completed
			ResponseText.text = "";
			responseText.text = "";

			if (totalPoints < minConfusedAffection) {
				DateText.text = "This is a bad date.";
			} else {
				DateText.text = "This is a confused date";
			}

		} else { //we ran out of time

			ResponseText.text = "";
			responseText.text = "";

			if (totalPoints < minConfusedAffection) {
				DateText.text = "This is a bad date.";
			} else if(totalPoints > minVictoryAffection){
				DateText.text = "you won the date";
				PlayerPrefs.SetInt ("" + curScene, 1); 
			}

			else {
				DateText.text = "This is a confused date";
			}
		}

		yield return new WaitForSeconds (2.0f);
		SceneManager.LoadScene (0);
	}
}
