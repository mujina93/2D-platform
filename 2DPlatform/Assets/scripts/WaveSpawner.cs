using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

	// states enum for the spawner
	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	// wave
	[System.Serializable] // accessible from inspector
	public class Wave{
		public string name; // wave name
		public Transform enemy; // prefab to spawn
		public int count; // number of spawed enemies
		public float rate; // spawn rate
	}

	public Wave[] waves; // array of waves
	public Transform[] spawnPoints; // spawning points
	private int nextWave = 0; // time to next wave
	public int NextWave {
		get { return nextWave + 1 ; } // index starts at 0
	}

	public float timeBetweenWaves = 5f;
	private float waveCountdown; // countdown
	public float WaveCountdown {
		get { return waveCountdown; }
	}

	private float searchCountdown = 1f; // check if enemies are alive each 1 sec

	[SerializeField]
	private SpawnState state = SpawnState.COUNTING; // state for the wave
	// public getter for private state (so that it can be read from other scripts)
	public SpawnState State {
		get { return state; }
	}

	void Start(){
		waveCountdown = timeBetweenWaves; // start from timeBetweenWaves
	}

	// state == COUNTING -> perform countdown, at the end -> start spawning (state == SPAWNING)
	// while state == SPAWNING -> Update() does nothing
	// at the end of the spawning call (IEnumerator), state == WAITING
	// state == WAITING -> Update() checks continuously whether enemies are all dead. When they are, begin new wave (state == COUNTING)
	void Update(){
		if (state == SpawnState.WAITING) {
			// check if enemies are still alive
			if (!EnemyIsAlive ()) {
				// Begin new round if all are dead
				WaveCompleted();
				return;
			} else {
				return;
			}
		}

		// only start the wave when countdown is down to 0
		if (waveCountdown <= 0) {
			if (state != SpawnState.SPAWNING) {
				StartCoroutine (SpawnWave (waves[nextWave])); // spawn next wave
			}
		} else {
			waveCountdown -= Time.deltaTime; // otherwise, tick down the countdown (in real-world time, not in frames)
		}
	}

	void WaveCompleted(){
		Debug.Log ("Wave completed!");
		state = SpawnState.COUNTING;	  // reset state to COUNTING (state that allows to perform the wave countdown and then spawn)
		waveCountdown = timeBetweenWaves; // reset wave countdown
		if (nextWave + 1 > waves.Length - 1) {
			// if you are over the total waves, stop.
			nextWave = 0;
			Debug.Log ("All waves complete. Looping");
		} else {
			// go to the next wave
			nextWave++;
		}
	}

	bool EnemyIsAlive(){
		searchCountdown -= Time.deltaTime; // move countdown
		// check only when countdown gets to 0
		if (searchCountdown <= 0f) {
			searchCountdown = 1f; // reset searchCountdown
			if (GameObject.FindGameObjectWithTag ("Enemy") == null) { // taxing operation!
				return false;
			}
		}
		return true; // yes, enemies are still alive
	}

	IEnumerator SpawnWave(Wave _wave){
		Debug.Log ("Spawning wave: " + _wave.name);
		// lock state to SPAWNING
		state = SpawnState.SPAWNING;

		// spawn _wave.count enemies, 1 each 1/_wave.rate seconds
		for (int i = 0; i < _wave.count; i++) {
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds (1f / _wave.rate);
		}

		// move state to WAITING
		state = SpawnState.WAITING; // wait for player to kill all enemies
		yield break;
	}

	void SpawnEnemy(Transform _enemy){
		if (spawnPoints.Length == 0)
			Debug.LogError ("No spawn points found");
		int at = Random.Range (0, spawnPoints.Length);
		Debug.Log ("Spawing at: " + at);
		Transform _sp = spawnPoints[at]; // select random spawning point
		Instantiate (_enemy, _sp.position, _sp.rotation); // spawn
	}
}
