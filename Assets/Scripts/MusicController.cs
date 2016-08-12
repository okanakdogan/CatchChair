using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public bool isMusicOn;

	// Use this for initialization
	void Start () {
		isMusicOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		//empty now
	}

	public void stopMusic(){
		BoxCollider2D chColl = GameObject.Find("Chairs").GetComponent<BoxCollider2D>();
		if(isMusicOn){
			isMusicOn=false;
			GetComponent<AudioSource>().Stop();
			chColl.isTrigger = true;
			
		}
	}
	public void startMusic(){
		if(isMusicOn==false){
			BoxCollider2D chColl = GameObject.Find("Chairs").GetComponent<BoxCollider2D>();
			isMusicOn=true;
			GetComponent<AudioSource>().Play();
			chColl.isTrigger = true;
			GameObject.Find("Chairs").GetComponent<ChairsController>().clearChairStatus();
		}

	}
}
