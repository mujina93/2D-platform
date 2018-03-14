using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	// contains and manages GUI for health stats (bar and text)
	[SerializeField]
	private RectTransform healthBar;
	[SerializeField]
	private Text healthText;

	// initialize GUI for health bar
	void Start(){
		if (healthBar == null)
			Debug.LogError ("STATUS INDICATOR: No health bar object reference found");
		if (healthText == null)
			Debug.LogError ("STATUS INDICATOR: No health Text object reference found");
		
	}

	// sets the GUI bar and text to the proper amount, when Health is set
	public void SetHealth(int _cur, int _max){
		// percentage of life
		float _value = (float)_cur/_max;

		// modify X scale based on percentage life
		healthBar.localScale = new Vector3 (_value,healthBar.localScale.y, healthBar.localScale.z);
		healthText.text = _cur + " / " + _max + " HP";
	}
}
