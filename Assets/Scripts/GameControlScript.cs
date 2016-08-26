using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControlScript : MonoBehaviour {


	public int gameState;
	private MusicControllerScript mc;
	private ChairsController cs;
	public GameObject roundLoser;
	private bool chairDeleted;
	public float playerStep ;
	private int score;
	private Text ScoreText;

	private GameObject winText;
	private GameObject loseText;
	private GameObject nextRoundText;

	// Use this for initialization
	void Start () {
		gameState = 0;
		mc = GameObject.Find("MusicController").GetComponent<MusicControllerScript>();
		cs = GameObject.Find ("Chairs").GetComponent<ChairsController> ();
		roundLoser = null;
		chairDeleted = false;
		winText = GameObject.Find("Canvas/WinText");
		loseText = GameObject.Find ("Canvas/LoseText");
		nextRoundText = GameObject.Find ("Canvas/NextRoundText");
		winText.SetActive (false);
		loseText.SetActive (false);
		nextRoundText.SetActive (false);
		playerStep = 0.05f;
		score = 0;
		ScoreText = GameObject.Find ("Canvas/ScoreText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		//game loop with states

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel("MainMenu");
		}

		//state 0
		if (gameState == 0) {
			//start music
			mc.startMusic();
			//reset chair delete flag
			chairDeleted=false;
			//rotate players around chairs (at player script)
			//listen for input
			if (Input.GetMouseButtonDown (0)) {
				gameState++;
				//state change
			}

			//take screen touch
		}
		//state 1
		else if (gameState == 1) {
			//stop music
			mc.stopMusic ();
			//make them catch chair( at Player script)

			//wait for roundloser and everybody sit
			if (roundLoser != null && cs.checkChairsFull ())
				gameState++;
		}
		//state 2
		else if (gameState == 2) {
			//show loser
			if (roundLoser != null)
				roundLoser.GetComponent<PlayerControlScript> ().moveStandPlace ();
			if (checkOnStandPlace ()) {
				//if player - game over screen
				if( roundLoser.name.Equals("playerToWin")){
					//game over screen
					loseText.SetActive(true);
					if (Input.GetMouseButtonDown (0)) {
						//restart scene
						Application.LoadLevel("gameSceneV2");
					}
				}
				//else - next round
				else{
					//check win
					//player wins
					if( cs.getChairCount()==1){
						//show win screen
						winText.SetActive(true);
						GameObject.Find("playerToWin").transform.position=new Vector3(0,1.5f,0);
						//wait input
						if (Input.GetMouseButtonDown (0)) {
							//restart scene
							Application.LoadLevel("gameSceneV2");
						}
					}else{
					//next round gui
						nextRoundText.SetActive(true);

					//wait for input to start music
						if (Input.GetMouseButtonDown (0)) {
							// calculate score
							if(cs.getPointChairIndex()!=-1){
								GameObject pointSitter =cs.transform.GetChild(cs.getPointChairIndex()).GetComponent<ChairState>().occupier;
								if(cs.isPointChairOn() && pointSitter.name.Equals("playerToWin")){
									score+=20;
								}
								else{
									score+=10;
								}
							}
							else{
								score+=10;
							}
							ScoreText.text =score.ToString();
							//getaway
							getAwayAllPlayers ();
							nextRoundText.SetActive(false);
							gameState=3;
						}
					}
				}
			}
		} else if (gameState == 3) {


			if(roundLoser!=null){
				//delete loser
				Destroy (roundLoser);
				roundLoser = null;	
			}

			//delete one chair
			if(chairDeleted==false){
				cs.deleteAChair ();
				chairDeleted=true;
				//clear chair states
				cs.clearChairStatus ();

				//inc. round speed
				playerStep = 0.05f + (8-cs.chairCount)*(0.1f/7f);
			}

			gameState=0;
		}
	}

	//checks is the loser next to stand place
	private bool checkOnStandPlace(){
	
		return (Vector3.Distance (roundLoser.transform.position, GameObject.Find ("StandPlace").transform.position) < 0.5f);

	}	
	// moves all players which sat end of round
	private void getAwayAllPlayers(){
		Transform players = GameObject.Find ("Players").transform;

		for(int i=0; i< players.childCount;++i){
			players.GetChild(i).GetComponent<PlayerControlScript>().getAwayFromChair();
		}
	}
}
