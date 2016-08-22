using UnityEngine;
using System.Collections;

public class ChairsController : MonoBehaviour {

	public int chairCount;
	// Use this for initialization
	void Start () {
		//empty now
		chairCount = transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
		//empty now
	}

	public void clearChairStatus(){
		for (int i = 0; i< transform.childCount; i++) {
			ChairState cs = transform.GetChild(i).GetComponent<ChairState>();
			cs.isCaptured=false;
			cs.occupier=null;
		}

	}
	public bool checkChairsFull(){
		for (int i = 0; i< transform.childCount; i++) {
			ChairState cs = transform.GetChild(i).GetComponent<ChairState>();
			if(cs.isCaptured==false){
				return false;
			}
		}
		return true;
	}
	public int getChairCount(){ return chairCount;}

	public bool deleteAChair(){

		Destroy (transform.GetChild (transform.childCount - 1).gameObject);
		chairCount = transform.childCount-1;
		return true;
	}
}
