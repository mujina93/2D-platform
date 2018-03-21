using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private Text speedText;

	[SerializeField]
	private float healthMultiplier = 1.3f;
	[SerializeField]
	private float speedMultiplier = 1.3f;

	[SerializeField]
	private int upgradeCost;

	private PlayerStats stats; // reference to static instance for player stats

	void OnEnable(){
		// sets static reference
		stats = PlayerStats.instance;
		// when menu is enabled, update values
		UpdateValues ();
	}

	void UpdateValues(){
		healthText.text = "HEALTH: " + stats.maxHealth.ToString ();
		speedText.text = "SPEED: " + stats.movementSpeed.ToString ();
	}

	// called when pressin upgrade button
	public void UpgradeHealth(){
		stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
		if (GameMaster.Money < upgradeCost) {
			AudioManager.instance.PlaySound ("NoMoney");
			return;
		}
		GameMaster.Money -= upgradeCost;
		AudioManager.instance.PlaySound ("Money");

		UpdateValues ();
	}

	// called when pressin upgrade button
	public void UpgradeSpeed(){
		stats.movementSpeed = Mathf.Round(stats.movementSpeed * speedMultiplier);
		if (GameMaster.Money < upgradeCost) {
			AudioManager.instance.PlaySound ("NoMoney");
			return;
		}
		GameMaster.Money -= upgradeCost;
		AudioManager.instance.PlaySound ("Money");
		UpdateValues ();
	}

}
