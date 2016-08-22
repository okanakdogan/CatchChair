using UnityEngine;
using System.Collections;

public class ChairState : MonoBehaviour {

	public bool isCaptured;
	public GameObject occupier;

	// Use this for initialization
	void Start () {
		isCaptured = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void  OnTriggerEnter2D(Collider2D other){
		if (isCaptured == false) {
			isCaptured = true;
			//other.gameObject.GetComponent<PlayerControlScript> ().onChair = true;
			occupier = other.gameObject;
		}
	}
	void  OnTriggerExit2D(Collider2D other){
		if(other.gameObject==occupier)
			isCaptured = false;
		//occupier = null;
	}
}
