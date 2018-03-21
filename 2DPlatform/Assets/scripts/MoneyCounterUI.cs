using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour {

	[SerializeField]
	private Text moneyText;

	// Use this for initialization
	void Awake () {
		moneyText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		moneyText.text = "Money: " +GameMaster.Money.ToString();
	}
}