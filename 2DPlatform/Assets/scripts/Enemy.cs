using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 20;
		private int _currentHealth;
		// through currentHealth you access and change _currentHealth,
		// using custom getters and setters
		public int currentHealth{
			// getter
			get{ return _currentHealth; }
			// setter, which is tweaked in such a way that when we
			// set the value for currentHealth from anywhere, it
			// will never be lesser than 0 or greater than maxHealth
			set{ _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		// damage that the enemy can make to the player
		public int damage = 40;

		// initialize variables
		public void Init(){
			currentHealth = maxHealth;
		}
	}

	public EnemyStats stats = new EnemyStats();

	public Transform deathParticles;

	// amount and length of camera shake when this enemy is destroyed
	public float shakeAmount = 0.1f;
	public float shakeLength = 0.1f;

	// StatusIndicator is the class managing the GUI health bar
	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	// call Init() on EnemyStats object, which
	// initializes the values for its stats
	void Start()
	{
		stats.Init();
		// every time health is set, call statusIndicator.SetHealth
		// to update the GUI
		if(statusIndicator != null)
			statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
		if (deathParticles == null)
			Debug.LogError ("No death particles reference on enemy");
	}

	public void DamageEnemy(int damage){
		stats.currentHealth -= damage;
		if (stats.currentHealth <= 0) {
			GameMaster.KillEnemy (this);
		}
		// every time health is set, call statusIndicator.SetHealth
		// to update the GUI
		if(statusIndicator != null)
			statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
		
	}

	// called everytime this collides with an object
	void OnCollisionEnter2D(Collision2D _collInfo){
		// when colliding, get the Player component from the collision target
		Player _player = _collInfo.collider.GetComponent<Player> ();
		// if there was a player, damage it with your stats.damage
		if (_player != null) {
			_player.DamagePlayer (stats.damage);
			DamageEnemy (999999); // suicide
		}
	}
}
