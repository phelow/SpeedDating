using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DateTextReader : MonoBehaviour {
	public string TextFile;

	public List<Sentence> sentences;

	public enum WordType{
		Adjective,
		Noun,
		Verb,
		Adverb
	}

	public class Sentence{
		public Sentence(List<WordType> slots, List<Word> words){
			_slots = slots;
			_words = words;
		}

		List<WordType> _slots;
		List<Word> _words;
	}

	public class Word{
		public Word(WordType type, string text, int points){
			_type = type;
			_text = text;
			_points = points;
		}

		WordType _type;
		string _text;
		int _points;
	}



	// Use this for initialization
	void Start () {
		sentences = new List<Sentence> ();
		FileInfo sourceFile = new FileInfo ("Assets\\" + TextFile + ".txt");
		StreamReader reader = sourceFile.OpenText ();

		string line;

		line = reader.ReadLine();
		do{
			List<Word> words = new List<Word>();
			List<WordType> slots = new List<WordType>();

			string prompt = line.Substring(0, line.IndexOf(":"));

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
				string wordString = prompt.Substring(nextWord + 1, nextWord - prompt.IndexOf(","));

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
			print("line:" + line + " prompt:" + displayPrompt);
			line = reader.ReadLine();
			sentences.Add(new Sentence(slots,words));
		} while (line != null);

				Debug.Log(sentences);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
