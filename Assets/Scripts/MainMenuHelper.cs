using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuHelper : MonoBehaviour {

	private Button playButton;
	private Button exitButton;

	private RectTransform playerImage;
	private Image coolImage;

	private Transform leftP;
	private Transform rightP;

	private Transform targetPos;

	private float speed;
	private bool rotateNow;
	// Use this for initialization
	void Start () {

		playButton= GameObject.Find("PlayButton").GetComponent<Button>();
		exitButton= GameObject.Find("ExitButton").GetComponent<Button>();

		playButton.onClick.AddListener (playButtonHandle);
		exitButton.onClick.AddListener (exitButtonHandle);

		playerImage = GameObject.Find ("NormalPlayerImage").GetComponent<RectTransform>();

		leftP = GameObject.Find ("LeftPoint").transform;
		rightP = GameObject.Find ("RightPoint").transform;
		targetPos = leftP;

		speed = 1.5f;
		rotateNow = false;
	}
	
	// Update is called once per frame
	void Update () {

		//run effect
		runAImage (playerImage);

	}

	private void runAImage(RectTransform img){

		if (rotateNow==false && (img.position - leftP.position).magnitude<1) {
			targetPos = rightP;
			rotateNow=true;
		} else if (rotateNow==false && (img.position - rightP.position).magnitude<1) {
			targetPos = leftP;
			rotateNow=true;
		} else {
			img.position = Vector3.Lerp (img.position, targetPos.position,speed*Time.deltaTime);

			//rotate
			if(rotateNow){

				img.transform.Rotate(0,180,0);
			}
			rotateNow=false;
		}
		
	}


	private void playButtonHandle(){
		//load game scene
		Application.LoadLevel ("gameSceneV2");
	}

	private void exitButtonHandle(){
		//exit game
		Debug.Log("Quit Game");
		Application.Quit ();
	}
}
