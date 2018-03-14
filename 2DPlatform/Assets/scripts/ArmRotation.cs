using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

	public float rotationOffset = 0f;

	// Update is called once per frame
	void Update () {
		// difference between mouse and character
		Vector3 Difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		Difference.Normalize ();

		float RotZ = Mathf.Atan2 (Difference.y, Difference.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler (0f, 0f, RotZ + rotationOffset);
	}
}
