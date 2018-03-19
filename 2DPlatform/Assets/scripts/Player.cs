using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private AudioManager audioManager;

	[System.Serializable]
	public class PlayerStats {
		public int maxHealth = 100;

		private int _currentHealth;
		public int currentHealth {
			get{ return _currentHealth; }
			set{ _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init(){
			currentHealth = maxHealth;
		}
	}

	public PlayerStats stats = new PlayerStats();

	public int fallBoundary = -20;

	public string deathSoundName = "DeathVoice";
	public string gruntSoundName = "Grunt";

	[SerializeField]
	private StatusIndicator statusIndicator; // health bar for player

	void Start(){
		stats.Init (); // initialize health stats for player
		if (statusIndicator == null) // check if indicator is there
			Debug.LogError ("PLAYER: no statusIndicator reference on player");
		else
			statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth); // update GUI

		audioManager = AudioManager.instance;
		if (audioManager == null)
			Debug.LogError ("No audio manager found");
	}

	void Update() {
		if (transform.position.y <= fallBoundary)
			DamagePlayer (999999);
	}

	public void DamagePlayer(int damage){
		// damage
		stats.currentHealth -= damage;

		// check and kill
		if (stats.currentHealth <= 0) {
			// play death sound
			audioManager.PlaySound (deathSoundName);
			// kill
			GameMaster.KillPlayer (this);
		} else {
			// play grunt
			audioManager.PlaySound(gruntSoundName);
		}

		// gui health bar update
		statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth); // update GUI
	}

}
