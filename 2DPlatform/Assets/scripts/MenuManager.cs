using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	string hoverOverSound = "ButtonHover";

	[SerializeField]
	string pressButtonSound = "ButtonPress";

	AudioManager audioManager;  // audio manager reference to play sounds

	void Start(){
		audioManager = AudioManager.instance;
		if (audioManager == null)
			Debug.LogError ("No audiomanager found");
	}

	public void StartGame(){
		audioManager.PlaySound (pressButtonSound);
		StartCoroutine (ChangeLevel ("MainLevel"));
	}

	public void QuitGame(){
		audioManager.PlaySound (pressButtonSound);
		Application.Quit ();
	}

	IEnumerator ChangeLevel (string levelName){
		float fadeTime = this.GetComponent<Fading> ().BeingFade (1); // fade out
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene (levelName); // loads level scene by name
	}

	public void OnMouseOver(){
		audioManager.PlaySound (hoverOverSound);
	}
}
