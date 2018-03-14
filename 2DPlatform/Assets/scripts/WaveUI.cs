using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

	[SerializeField]
	WaveSpawner spawner;

	[SerializeField]
	Animator waveAnimator;

	[SerializeField]
	Text waveCountdownText;

	[SerializeField]
	Text waveCountText;

	private WaveSpawner.SpawnState previousState;

	// Use this for initialization
	void Start () {
		if (spawner == null) {
			Debug.LogError ("No spawner referenced!");
			this.enabled = false;
		}
		if (waveAnimator == null) {
			Debug.LogError ("No waveAnimator referenced!");
			this.enabled = false;
		}
		if (waveCountdownText == null) {
			Debug.LogError ("No waveCountdownText referenced!");
			this.enabled = false;
		}
		if (waveCountText == null) {
			Debug.LogError ("No waveCountText referenced!");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State) {
			case WaveSpawner.SpawnState.COUNTING:
				UpdateCountingUI ();
				break;
			case WaveSpawner.SpawnState.SPAWNING:
				UpdateSpawningUI ();
				break;
		}
		previousState = spawner.State; // save state in the current frame as the "previousState"
	}

	void UpdateCountingUI(){
		if (previousState != WaveSpawner.SpawnState.COUNTING) {
			// if the spawner just switched state to COUNTING ...
			// switch GUI to Counting animation
			waveAnimator.SetBool("WaveIncoming", false);
			waveAnimator.SetBool("WaveCountdown", true);
		}
		waveCountdownText.text = ((int)spawner.WaveCountdown).ToString(); // update wave countdown
	}

	void UpdateSpawningUI(){
		if (previousState != WaveSpawner.SpawnState.SPAWNING) {
			// if the spawner just switched state to SPAWNING ...
			// switch GUI to Incoming animation
			waveAnimator.SetBool("WaveIncoming", true);
			waveAnimator.SetBool("WaveCountdown", false);
			waveCountText.text = spawner.NextWave.ToString (); // update wave number
		}
	}
}
