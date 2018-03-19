using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture; // texture that will overlay the screen. Can be a black image or loading graphics
	public float fadeSpeed = 0.8f; 	// fading speed

	private int drawDepth = -1000; // texture's order in the draw hierarchy: a low number means it renders on top
	private float alpha = 1.0f; 	// texture's alpha value between 0 and 1
	private int fadeDir = -1;		// direction to fade: in = -1, out = +1

	void OnGUI(){
		// fade in/out alpha
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		// force (clamp) number between 0 and 1
		alpha = Mathf.Clamp01(alpha);

		// set color of our GUI (texture)
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeingFade(int direction){
		fadeDir = direction;
		return (fadeSpeed);
	}

	// called when level is loaded
	void OnLevelWasLoaded(){
		BeingFade (-1);
	}

}
