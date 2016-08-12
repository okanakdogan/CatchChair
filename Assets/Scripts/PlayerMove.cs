using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public bool musicPlaying;

	public Vector3 walkWay;
	public Vector3[] cornersInColl;

	private Transform closestChairTrans;
	private Vector3 standPosition;

	public bool onChair;
	int lastArea;
	// Use this for initialization
	void Start () {
		musicPlaying = true;
		lastArea = -1;
		//getAWalkTarget (transform.position);
		walkWay =new Vector3(0,0,0);

		cornersInColl = new Vector3[4];
		getCollCorners ("Chairs");
	}
	
	// Update is called once per frame
	void Update () {
	
		if (GameObject.Find("MusicSystem").GetComponent<MusicController>().isMusicOn) {

			//if on chair go away
			if(onChair){
				getAwayFromChair();
			}

			MoveAround();
			standPosition=transform.position;
		} 
		else {
			//close box collider

			CatchAChairv2();
		}

	}

	void MoveAround ()
	{
		//Turn around clockwise
		// or random points
		int area = getAreaID (transform.position);

		if ( lastArea!=area) {

			//select a new target
			walkWay=getAWalkWay("Chairs",transform.position);
			//walkTarget=getAWalkTarget(transform.position);
			Debug.Log (walkWay);
			lastArea=area;
			GetComponent<Rigidbody2D>().velocity=new Vector2(0,0);
		} else {
			//go one step to target
			//test
			//transform.position=Vector3.Lerp(transform.position,transform.position+walkWay,0.1f);
			GetComponent<Rigidbody2D>().AddForce(walkWay*0.05f,ForceMode2D.Impulse);
			closestChairTrans= getClosestChair();
			//correct z value
			closestChairTrans.position.Set(closestChairTrans.position.x,closestChairTrans.position.y,transform.position.z);
			//transform.position=walkTarget;
		}
	}
	void getAwayFromChair(){
		/*transform.position = Vector3.Lerp (transform.position,
		                                   transform.position+ ((closestChairTrans.right) * -80),1f*Time.deltaTime);*/
		Vector3 goVec = closestChairTrans.right* -1.5f;
		goVec.z = transform.position.z;
		GetComponent<Rigidbody2D> ().MovePosition (transform.position + goVec );

		onChair = false;

	}

	void CatchAChairv2(){

		if (closestChairTrans != null ) {
			//go chair
			ChairState cs= closestChairTrans.gameObject.GetComponent<ChairState> ();
			if (!cs.isCaptured) {
				//Debug.Log ("closestChairpos " + closestChairTrans.position);
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				//GetComponent<Rigidbody2D> ().MovePosition (closestChairPos);
				Vector3 chPos = closestChairTrans.position;
				//fix z
				chPos.z = transform.position.z;
				transform.position = Vector3.Lerp (transform.position, chPos, 2.5f * Time.deltaTime);
			}else{
				if(cs.occupier==this.gameObject){

					Vector3 chPos = closestChairTrans.position;
					//fix z
					chPos.z = transform.position.z;
					transform.position = Vector3.Lerp (transform.position, chPos, 2.5f * Time.deltaTime);
				}
				else{
					closestChairTrans=null;
				}
			}
		}

		if(closestChairTrans==null) {
			closestChairTrans= getClosestChair();
			if(closestChairTrans==null){
				GameControl gc = GameObject.Find ("GameController").GetComponent<GameControl> ();
				gc.roundEnded = true;
				gc.roundLoser = this.gameObject;

				//go front of user and cry :D
				GetComponent<CircleCollider2D>().isTrigger=true;
				transform.position = Vector3.Lerp (transform.position, GameObject.Find("StandPlace").transform.position, 2.5f * Time.deltaTime);
			}
		}
	}

	void CatchAChair ()
	{
		ChairState cs=null;
		if (closestChairTrans != null) {
			 cs= closestChairTrans.gameObject.GetComponent<ChairState> ();
		}
		if (!onChair) {
			//check this chair captured

			while (cs.isCaptured && cs.occupier!=this.gameObject) {
				transform.position = Vector3.Lerp (transform.position, standPosition, 2.5f * Time.deltaTime);
				//find a new chair
				closestChairTrans = getClosestChair ();

				if (closestChairTrans != null) {
					cs = closestChairTrans.gameObject.GetComponent<ChairState> ();
				} else {
					//if no chair tell game controller i lost :D
					GameControl gc = GameObject.Find ("Game Controller").GetComponent<GameControl> ();
					gc.roundEnded = true;
					gc.roundLoser = this.gameObject;
					break;
				}

			}
		}
		if (!cs.isCaptured) {
			//Debug.Log ("closestChairpos " + closestChairTrans.position);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			//GetComponent<Rigidbody2D> ().MovePosition (closestChairPos);
			Vector3 chPos = closestChairTrans.position;
			//fix z
			chPos.z = transform.position.z;
			transform.position = Vector3.Lerp (transform.position, chPos, 2.5f * Time.deltaTime);
		}

	}
	void getCollCorners(string objectName){

		BoxCollider2D chColl = GameObject.Find(objectName).GetComponent<BoxCollider2D>();
		Vector2 size= chColl.size;
		
		cornersInColl[0] = new Vector3 ((size.x / 2.0f)+chColl.offset.x, (size.y / 2.0f)+chColl.offset.y);

		cornersInColl[1] = cornersInColl[0];
		cornersInColl [1].y *= -1;

		cornersInColl[2] =cornersInColl[1];
		cornersInColl [2].x *= -1;

		cornersInColl[3] = cornersInColl[2];
		cornersInColl [3].y *= -1;

	}

	int getAreaID(Vector3 position){
		//think eight area
		/*    up
		 * 	 ----
		 *	|    |
		 *L	|    | Right
		 * 	 ----
		 * 	 down 
		 * and cross areas
		 * 
		*/	 

		int areaID=0;
		// find which area
		// is on frst row
		if (position.y > cornersInColl [0].y) {
			//find column
			if (position.x < cornersInColl [2].x) {
				areaID=0;
			} else if (position.x > cornersInColl [2].x && position.x < cornersInColl [1].x) {
				areaID=1;
			} else if (position.x > cornersInColl [1].x) {
				areaID=2;
			}
		}
		// is in middle row
		else if (position.y < cornersInColl [0].y && position.y > cornersInColl [1].y) {
			//find column
			if (position.x < cornersInColl [2].x) {
				areaID=3;
			} else if (position.x > cornersInColl [2].x && position.x < cornersInColl [1].x) {
				areaID=4;
			} else if (position.x > cornersInColl [1].x) {
				areaID=5;
			}
		}
		//is in last row
		else if (position.y < cornersInColl [1].y) {
			//find column
			if (position.x < cornersInColl [2].x) {
				areaID=6;
			} else if (position.x > cornersInColl [2].x && position.x < cornersInColl [1].x) {
				areaID=7;
			} else if (position.x > cornersInColl [1].x) {
				areaID=8;
			}
		}
		return areaID;
	}
	Vector3 getAWalkWay(string objectName,Vector3 position){

		//find where i am

		//get corners

		int areaID = getAreaID (position);
		// choose a angle

		Vector3 angle;
		switch (areaID) {
		case 0:
			angle=new Vector3(1,1,0);
			break;
		case 1:
			angle=Vector3.right;
			break;
		case 2:
			angle=new Vector3(1,-1,0);
			break;
		case 3:
			angle=Vector3.up;
			break;
		case 5:
			angle=Vector3.down;
			break;
		case 6:
			angle=new Vector3(-1,1,0);
			break;
		case 7:
			angle=Vector3.left;
			break;
		case 8:
			angle=new Vector3(-1,-1,0);
			break;
		default:
			angle= new Vector3();
			break;
		}

		//add randomize a little

		return angle;
	}

	Transform getClosestChair(){
		//get chairs
		float minDist = float.PositiveInfinity;
		int minIndex = -1;
		Transform closeChair = null;
		GameObject chairs = GameObject.Find ("Chairs");
		for (int i=0; i<chairs.transform.childCount; ++i) {
			BoxCollider2D coll= chairs.transform.GetChild(i).GetComponent<BoxCollider2D>();

			//get distance
			float dist= Vector3.Distance(transform.position,coll.bounds.center);
			if(dist<minDist && coll.gameObject.GetComponent<ChairState>().isCaptured==false){
				minDist=dist;
				minIndex=i;
				closeChair=chairs.transform.GetChild(minIndex).transform;
			}

		}

		//Debug.Log ("close Chair index" + minIndex);
		return closeChair;
	}

}
