using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	public void Quit(){
		Debug.Log ("APP quit");
		Application.Quit ();
	}

	public void Retry(){
		Debug.Log ("APP Retry");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
