using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public AudioSource audioSource;
	public AudioSource audioSource2;

	public AudioClip clickWord; //
	public AudioClip backgroundAmbience; //
	public AudioClip music; //
	public AudioClip noNumbers; //todo
	public AudioClip GotNumber; //
	public AudioClip NoNumber; //
	public AudioClip TimeRunningOut; //todo
	public AudioClip WordDrop; //todo
	public AudioClip NewRound; //

	public static SoundManager instance;
	// Use this for initialization
	void Start () {
		instance = this;
		instance.audioSource.PlayOneShot(instance.NewRound);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public static void PlayClickWord(){
		instance.audioSource.PlayOneShot(instance.clickWord);
	}

	public static void PlayNoNumber(){
		instance.audioSource.PlayOneShot (instance.NoNumber);
	}

	public static void PlayGotNumber (){
		instance.audioSource.PlayOneShot (instance.GotNumber);
	}

	public static void PlayTimeRunningOut(){
		instance.audioSource2.PlayOneShot (instance.TimeRunningOut);
	}
}
