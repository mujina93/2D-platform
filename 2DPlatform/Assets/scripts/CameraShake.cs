using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Camera mainCam;
	float shakeAmount = 0;

	void Awake(){
		if (mainCam == null)
			mainCam = Camera.main;
	}

	public void Shake(float amt, float length){
		shakeAmount = amt;
		// every 0.01 s do a shaking
		// (it's like a coroutine, running separately)
		InvokeRepeating ("DoShake", 0, 0.01f);
		// call StopShake() after length seconds
		Invoke ("StopShake", length);
	}

	void DoShake(){
		if (shakeAmount > 0) {
			Vector3 camPos = mainCam.transform.position;
			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			camPos.x += offsetX;
			camPos.y += offsetY;

			mainCam.transform.position = camPos;
		}
	}

	void StopShake(){
		CancelInvoke ("DoShake");
		// avoid conflicts with other camera scripts. The camera will be under another empty object. The 
		// empty parent will contain the main camera scripts (such as "follow player"), while the inner camera
		// will be the one to be shaken, and then it will be reset to 0 (with respect to the parent object,
		// which will be still following the player).
		mainCam.transform.localPosition = Vector3.zero;
	}
}
