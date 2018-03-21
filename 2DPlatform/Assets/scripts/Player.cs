using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))] // to have a component that can be enabled/disabled
public class Player : MonoBehaviour {

	private AudioManager audioManager;

	private PlayerStats stats; // external PlayerStats static instance (controlled by GM)

	public int fallBoundary = -20;

	public string deathSoundName = "DeathVoice";
	public string gruntSoundName = "Grunt";

	[SerializeField]
	private StatusIndicator statusIndicator; // health bar for player

	void Start(){

		// initialize PlayerStats
		stats = PlayerStats.instance;
		stats.InitHealth ();

		// startup health bar GUI
		if (statusIndicator == null) // check if indicator is there
			Debug.LogError ("PLAYER: no statusIndicator reference on player");
		else
			statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth); // update GUI

		// binding the Player's callback OnUpgradeMenuToggle to the onToggleUpgradeMenu event cast by GM
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		// initialize audio reference
		audioManager = AudioManager.instance;
		if (audioManager == null)
			Debug.LogError ("No audio manager found");

		// start regeneration: every 1/regenRate seconds, call RegenHealth which add 1 life
		InvokeRepeating("RegenHealth", 1f/stats.regenRate, 1f/stats.regenRate);
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

	// callback called when upgrade menu is toggled
	void OnUpgradeMenuToggle(bool active){
		// handle what happens when the upgrade menu is toggled:
		// turn the player controller and the weapon on/off when the upgrade menu is off/on
		GetComponent<Platformer2DUserControl>().enabled = !active;
		Weapon _weapon = GetComponentInChildren<Weapon> ();
		if (_weapon != null) {
			_weapon.enabled = !active;
		}
	}

	void RegenHealth(){
		stats.currentHealth += 1;
		// gui update
		statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth);
	}

	void OnDestroy(){
		// UNsubscribe from upgrade menu when dying
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}
}
