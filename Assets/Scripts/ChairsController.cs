using UnityEngine;
using System.Collections;

public class ChairsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//empty now
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
}
