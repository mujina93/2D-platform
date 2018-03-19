using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	[SerializeField]
	string mouseHoverSound = "ButtonHover";

	[SerializeField]
	string buttonPressedSound = "ButtonPres";

	AudioManager audioManager;

	void Start(){
		audioManager = AudioManager.instance;
		if (audioManager == null)
			Debug.LogError ("No audio manager found");
	}

	public void Quit(){
		// click sound
		audioManager.PlaySound (buttonPressedSound);

		Debug.Log ("APP quit");
		Application.Quit ();
	}

	public void Retry(){
		// click sound
		audioManager.PlaySound (buttonPressedSound);

		Debug.Log ("APP Retry");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void OnMouseOver(){
		// hover sound
		audioManager.PlaySound (mouseHoverSound);
	}
}
