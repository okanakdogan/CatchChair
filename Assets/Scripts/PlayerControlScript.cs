using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

	//chairs object for acces chairs
	private GameObject chairs;
	private GameControlScript gameControl;
	private Transform closeChair;
	private SpriteRenderer mySprRend;
	public Sprite nerv_face;
	public Sprite rush_face;
	public Sprite cry_face;

	// Use this for initialization
	void Start () {
	
		chairs = GameObject.Find("Chairs");
		gameControl = GameObject.Find ("GameController").GetComponent<GameControlScript> ();
		mySprRend = GetComponent<SpriteRenderer> ();
		//nerv_face = Resources.Load ("Assets/Textures/player-nervous",typeof(Sprite)) as Sprite;
		//rush_face = Resources.Load ("Textures/player-rush",typeof(Sprite)) as Sprite;
		//cry_face = Resources.Load ("Textures/player-cry",typeof(Sprite)) as Sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
		//if state 0 move around
		if (gameControl.gameState == 0) {
			moveAroundChairs ();
		} else if (gameControl.gameState == 1) {
			catchChair ();
		} else if (gameControl.gameState == 2) {
			//stand forward and cry
		} 
	}


	public void moveAroundChairs(){

		float radius = 1.6f;
		float speed = 1f;
		float step = 0.1f;

		//change face
		if (!name.Equals("playerToWin") && mySprRend.sprite != nerv_face) {
			mySprRend.sprite =nerv_face;
		}

		//find close chair
		closeChair = getClosestChair ();

		//get close if you away
		if (Vector3.Distance (closeChair.transform.position, transform.position) > radius) {
			Debug.Log("goClose");
			transform.position= Vector3.Lerp(transform.position,closeChair.transform.position,Time.deltaTime * speed);
		} else {
			Debug.Log("Rotate");
			//find vector toward him
			Vector3 chairToVector = closeChair.position - transform.position;
			chairToVector.Normalize ();

			//rotate vector
			chairToVector = Quaternion.Euler (0, 0, 90) * chairToVector;
			//move

			transform.position = transform.position+chairToVector*step;
		}

	}

	public void catchChair(){
		//rush face load
		if (!name.Equals("playerToWin") && mySprRend.sprite != rush_face) {
			mySprRend.sprite =rush_face;
		}

		// get closest

		if (closeChair != null ) {
			//go chair
			ChairState cs= closeChair.GetComponent<ChairState> ();
			if (!cs.isCaptured) {
				//Debug.Log ("closestChairpos " + closestChairTrans.position);
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				//GetComponent<Rigidbody2D> ().MovePosition (closestChairPos);
				Vector3 chPos = closeChair.transform.position;
				//fix z
				chPos.z = transform.position.z;

				//move to chair
				transform.position = Vector3.Lerp (transform.position, chPos, 2.5f * Time.deltaTime);
			}else{
				if(cs.occupier==this.gameObject){
					
					Vector3 chPos = closeChair.transform.position;
					//fix z
					chPos.z = transform.position.z;
					transform.position = Vector3.Lerp (transform.position, chPos, 2.5f * Time.deltaTime);
				}
				else{
					closeChair=null;
				}
			}
		}
		
		if(closeChair==null) {
			closeChair= getClosestChair();
			if(closeChair==null){


				gameControl.roundLoser = this.gameObject;

				GetComponent<CircleCollider2D>().isTrigger=true;

			}
		}
	}

	Transform getClosestChair(){
		//get chairs
		float minDist = float.PositiveInfinity;

		Transform localCloseChair = null;

		for (int i=0; i<chairs.transform.childCount; ++i) {
			
			//get distance
			float dist = Vector3.Distance (transform.position, chairs.transform.GetChild(i).position);
			if (dist < minDist && chairs.transform.GetChild (i).GetComponent<ChairState>().isCaptured==false) {
				minDist = dist;

				localCloseChair = chairs.transform.GetChild (i).transform;
			}
			
		}
		
		//Debug.Log ("close Chair index" + minIndex);
		return localCloseChair;
	}

	public void moveStandPlace(){
		//if you loser you move there, so cry
		//load cry face
		if (mySprRend.sprite != cry_face) {
			mySprRend.sprite = cry_face;
		}

		transform.position = Vector3.Lerp (transform.position, GameObject.Find("StandPlace").transform.position, 2.5f * Time.deltaTime);
	}
	public void getAwayFromChair(){
		/*transform.position = Vector3.Lerp (transform.position,
		                                   transform.position+ ((closestChairTrans.right) * -80),1f*Time.deltaTime);*/
		if (closeChair != null) {
			Vector3 goVec = closeChair.transform.right * -1.5f;
			goVec.z = transform.position.z;
			transform.position= (closeChair.position + goVec);
		}
	}
}
