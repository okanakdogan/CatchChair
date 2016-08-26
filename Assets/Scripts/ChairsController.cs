using UnityEngine;
using System.Collections;

public class ChairsController : MonoBehaviour {

	public int chairCount;
	private float pointTime;
	private bool pointChairOn;
	private GameControlScript gcs;
	private int pointChairIndex;
	private Color orgColor;
	// Use this for initialization
	void Start () {
		//empty now
		chairCount = transform.childCount;
		pointTime =3f;
		pointChairOn = false;
		gcs = GameObject.Find("GameController").GetComponent<GameControlScript>();
		pointChairIndex = -1;
		orgColor = transform.GetChild (0).GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		//empty now
		if (gcs.gameState == 0) {
			randomPointChair();
		}
	}

	private void randomPointChair(){
		if (pointTime > 0) {
			if (pointChairOn ) {
				SpriteRenderer pcsr= transform.GetChild(pointChairIndex).GetComponent<SpriteRenderer>();
				pcsr.color =Color.yellow;
			}
			pointTime -=Time.deltaTime;
		} else {
			pointTime=3f;
			pointChairOn = !pointChairOn;
			//default value


			if(pointChairOn){

				//select a chair to add value
				pointChairIndex = Random.Range(0,transform.childCount);
			}
			else{
				if(pointChairIndex!=-1){
					SpriteRenderer pcsr= transform.GetChild(pointChairIndex).GetComponent<SpriteRenderer>();
					pcsr.color =orgColor;
				}
			}
		}
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

	public bool isPointChairOn(){ return pointChairOn;}
	public int getPointChairIndex(){return pointChairIndex;}
}
