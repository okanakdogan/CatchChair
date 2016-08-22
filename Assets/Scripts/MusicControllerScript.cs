using UnityEngine;
using System.Collections;

public class MusicControllerScript : MonoBehaviour {


	private bool isMusicOn;
	// Use this for initialization
	void Start () {
		isMusicOn = false;
	}


	public bool getMusicStatus(){ return isMusicOn;}
	public void stopMusic(){
		if (isMusicOn) {
			isMusicOn = false;
			GetComponent<AudioSource>().Pause();
		}
	}
	public void startMusic(){
		if (!isMusicOn) {
			isMusicOn = true;
			GetComponent<AudioSource>().Play();
		}
	}
}
