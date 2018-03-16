using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour {

	[SerializeField]
	private Text livesText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		livesText.text = "Lives: " +GameMaster.RemainingLives.ToString();
	}
}
