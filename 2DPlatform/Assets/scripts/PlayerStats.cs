using UnityEngine;

public class PlayerStats : MonoBehaviour{

	public static PlayerStats instance;

	public int maxHealth = 100;

	private int _currentHealth;
	public int currentHealth {
		get{ return _currentHealth; }
		set{ _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
	}

	public float regenRate = 2f;

	public float movementSpeed = 10f;

	public void InitHealth(){
		currentHealth = maxHealth;
	}

	void Awake(){
		if (instance == null) {
			instance = this;
		}
		InitHealth ();
	}
}