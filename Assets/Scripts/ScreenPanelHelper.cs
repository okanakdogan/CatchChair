using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenPanelHelper : MonoBehaviour {

	private Button musicButton;
	private Button homeButton;
	// Use this for initialization
	void Start () {
		musicButton = GameObject.Find ("MusicButton").GetComponent<Button> ();
		musicButton.onClick.AddListener (musicButtonHandle);

		homeButton = GameObject.Find ("HomeButton").GetComponent<Button> ();
		homeButton.onClick.AddListener (homeButtonHandle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void musicButtonHandle(){
		Debug.Log ("Musicbutton pressd");
		musicButton.interactable=false;
		MusicController mc = GameObject.Find("MusicSystem").GetComponent<MusicController>();
		if (mc.isMusicOn) {

			mc.stopMusic ();

		} else {
			//delete a chair too
			StartCoroutine(MusicButtonWaitter());
			GameObject.Find("GameController").GetComponent<GameControl>().deleteAChair();
			mc.startMusic();
		}
	}
	IEnumerator MusicButtonWaitter ()
	{	
		musicButton.interactable = false;
		yield return new WaitForSeconds (2.5f);
		musicButton.interactable = true;
	}
	private void homeButtonHandle(){

		Application.LoadLevel ("MainMenu");
	}
}
