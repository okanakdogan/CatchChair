using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

	/*
	 * need to find ho losing
	 * did player lose game
	 * go next stage
	 * or go main menu
	 */
	public bool roundEnded;
	public GameObject roundLoser;

	private int chairDelCount;
	private float[,] collSizes;
	// Use this for initialization
	void Start () {
		chairDelCount=0;
		collSizes = new float[4,2];
		collSizes [0,0] = -0.3f;
		collSizes [0,1] = 7f;
		collSizes [1,0] = 0.6f;
		collSizes [1,1] = 5f;
		collSizes [2,0] = 1.4f;
		collSizes [2,1] = 3f;
		collSizes [3,0] = 2.2f;
		collSizes [3,1] = 2f;



	}
	
	// Update is called once per frame
	void Update () {
		//get music status
		if (roundEnded == true) {

			if (GameObject.Find ("MusicSystem").GetComponent<MusicController> ().isMusicOn == false) {
				//musc stopped find who lost it
				//let them sit
				//yield return new WaitForSeconds(2);

				if (roundLoser!=null && roundLoser.GetComponent<PlayerMove> ().onChair == false) {
					//we found loser tag it
					Debug.Log ("number:" + roundLoser + " lost");

					StartCoroutine (NewMethod());
				}
			}
		}
	}




	IEnumerator NewMethod ()
	{


		if(roundLoser.gameObject.name.Equals("playerToWin")){
			//We lose go Lose UI and Restart
			Debug.Log("Lose Cond.");
			GameObject.Find("Canvas/ScreenPanel/LoseText").SetActive(true);
			yield return new WaitForSeconds (2);
			GameObject.Find("Canvas/ScreenPanel/LoseText").SetActive(false);
			roundLoser=null;
			Application.LoadLevel("2dScene");
		}
		else {

			//Check for last chair ?
			if(chairDelCount>=7){
				//Show win Text

				GameObject.Find("Canvas/ScreenPanel/WinText").SetActive(true);
				yield return new WaitForSeconds (2);
				GameObject.Find("Canvas/ScreenPanel/WinText").SetActive(false);
				Application.LoadLevel("2dScene");
			}else{
				GameObject.Find("Canvas/ScreenPanel/NextRoundText").SetActive(true);



			}
			//
		yield return new WaitForSeconds (2);
		GameObject.Find("Canvas/ScreenPanel/NextRoundText").SetActive(false);
		GameObject.Destroy (roundLoser);
		roundLoser = null;
		}
		GameObject.Find("Canvas/ScreenPanel/MusicButton").GetComponent<Button>().interactable=true;

	}

	public void deleteAChair(){
		//Delete a Chair
		GameObject chairs = GameObject.Find ("Chairs");
		GameObject.Destroy (chairs.transform.GetChild (chairs.transform.childCount - 1).gameObject);
		chairDelCount++;
		//fix chairs collider

		if (chairDelCount % 2 == 0) {
			BoxCollider2D coll=chairs.GetComponent<BoxCollider2D>();

			/*
			foreach(Renderer r in chairs.GetComponentsInChildren<Renderer>()){
				collb.Encapsulate(r.bounds);
			}*/
			coll.offset.Set(coll.offset.x,collSizes[chairDelCount/2,0]);
			coll.size.Set(coll.size.x,collSizes[chairDelCount/2,1]);
		}
	}

}
