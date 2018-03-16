using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	private static int _remainingLives = 3;
	public static int RemainingLives {
		get { return _remainingLives; }
	}

	// by setting variables in Awake(), you are sure that they will be already set when calling any Start() method
	void Awake(){
		if (gm == null) {
			// set gm to the unique instance of GameMaster
			gm = this;
		}
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 4f;
	public Transform spawnPrefab;
	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameOverUI;

	void Start(){
		if (cameraShake == null)
			Debug.LogError ("no camera shake reference found on GM");
	}

	public IEnumerator RespawnPlayer(){
		GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (spawnDelay);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform particles = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
		Destroy (particles.gameObject, 3f);
	}



	public static void KillPlayer(Player player){
		Destroy (player.gameObject);
		_remainingLives--;
		if (_remainingLives <= 0) {
			gm.EndGame ();
		} else {
			gm.StartCoroutine (gm.RespawnPlayer ());
		}
	}

	public void EndGame(){
		gameOverUI.SetActive (true);
	}

	// local method for killing the enemy
	public void _KillEnemy(Enemy _enemy){
		Transform deathParticles = Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
		Destroy (deathParticles.gameObject, 5f);
		cameraShake.Shake (_enemy.shakeAmount, _enemy.shakeLength);
		if (_enemy)
			Destroy (_enemy.gameObject);
		else
			Debug.LogError ("No enemy reference when _KillEnemy is called!");
	}

	public static void KillEnemy(Enemy enemy){
		gm._KillEnemy (enemy); // kill the enemy with a local method
	}
		
}
