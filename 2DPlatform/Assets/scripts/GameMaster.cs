using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	private int maxLives = 3;
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
	public string respawnCountdownSoundName = "RespawnCountdown";  // this must match the name of the sound in the aduio manager! (do all from inspector)
	public string spawnSoundName = "Spawn"; 
	public string gameOverSoundName = "GameOver";

	[SerializeField]
	private GameObject gameOverUI;

	[SerializeField]
	private GameObject upgradeMenu;

	[SerializeField]
	private WaveSpawner waveSpawner;

	// delegate: makes us create events and call callbacks
	public delegate void UpgradeMenuCallback(bool active);
	// instance of the delegate. This instance can call the callbacks when the event is cast
	public UpgradeMenuCallback onToggleUpgradeMenu;

	// cache
	private AudioManager audioManager;

	// money
	[SerializeField]
	private int startingMoney;
	public static int Money;

	void Start(){
		if (cameraShake == null)
			Debug.LogError ("no camera shake reference found on GM");
		// set lives
		_remainingLives = maxLives;
		// set money
		Money = startingMoney;

		// caching audio
		audioManager = AudioManager.instance; // sets audio manager to singleton instance of AudioManager
		if (audioManager == null) {
			Debug.LogError ("No audio manager found on scene");
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.U)) {
			ToggleUpgradeMenu ();
		}
	}

	// cast the event of Upgrade menu toggling
	// -> this will call all the callbacks, with argument the state of the menu (active: false or true)
	private void ToggleUpgradeMenu(){
		// toggle upgrade menu on / off
		upgradeMenu.SetActive (!upgradeMenu.activeSelf);

		// toggle wave spawner off / on
		waveSpawner.enabled = !upgradeMenu.activeSelf;

		// call all methods that have subscribed to this delegate
		onToggleUpgradeMenu.Invoke (upgradeMenu.activeSelf);
	}

	public IEnumerator RespawnPlayer(){
		// play countdown sound
		audioManager.PlaySound(respawnCountdownSoundName);
		yield return new WaitForSeconds (spawnDelay);

		// play spawn sound
		audioManager.PlaySound(spawnSoundName);

		// respawn player
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
		audioManager.PlaySound (gameOverSoundName);
		gameOverUI.SetActive (true);
	}

	// local method for killing the enemy
	public void _KillEnemy(Enemy _enemy){
		// death sound
		audioManager.PlaySound(_enemy.soundDeathName);

		// drop money
		Money += _enemy.moneyDrop;
		audioManager.PlaySound ("Money");

		// death particles
		Transform deathParticles = Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);
		Destroy (deathParticles.gameObject, 5f);

		// shake camera
		cameraShake.Shake (_enemy.shakeAmount, _enemy.shakeLength);

		// destroy enemy
		if (_enemy)
			Destroy (_enemy.gameObject);
		else
			Debug.LogError ("No enemy reference when _KillEnemy is called!");
	}

	public static void KillEnemy(Enemy enemy){
		gm._KillEnemy (enemy); // kill the enemy with a local method
	}
		
}
