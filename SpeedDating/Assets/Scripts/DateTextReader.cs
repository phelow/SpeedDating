using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DateTextReader : MonoBehaviour {
	public string TextFile;

	public List<Sentence> sentences;

	public enum wordType{
		Adjective,
		Noun,
		Verb
	}

	public class Sentence{
		List<wordType> slots;
		List<Word> words;
	}

	public class Word{
		wordType type;
		string text;
		int points
	}



	// Use this for initialization
	void Start () {
		FileInfo sourceFile = new FileInfo ("Assets\\" + TextFile + ".txt");
		StreamReader reader = sourceFile.OpenText ();

		string line;

		line = reader.ReadLine();
		do{
			string prompt = line.Substring(0, line.IndexOf(":"));

			string displayPrompt = "";

			int nextWord = 0;

			nextWord = prompt.IndexOf("[");
			while(nextWord != -1){
				displayPrompt += prompt.Substring(0,nextWord) + "_____";
				prompt = prompt.Substring(prompt.IndexOf("]") + 1, prompt.Length - (prompt.IndexOf("]") + 1) );
				nextWord = prompt.IndexOf("[");
			}

			print("line:" + line + " prompt:" + displayPrompt);

			line = reader.ReadLine();
		} while (line != null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
