using UnityEngine;

[System.Serializable]
public class Sound {
	public string name;
	public AudioClip clip; // audo clip

	[Range(0f,1f)] // add slider
	public float volume = 0.7f;
	[Range(0.5f,1.5f)] // add slider
	public float pitch = 1f;

	public bool loop = false; //looping

	private AudioSource source;

	[Range(0f,0.5f)]
	public float randomVolume = 0.1f;
	[Range(0f,0.5f)]
	public float randomPitch = 0.1f;

	public void SetSource(AudioSource _source){
		source = _source;
		source.clip = clip;
		source.loop = loop;
	}

	public void Play(){
		source.volume = volume * (1 + Random.Range(-randomVolume/2f, randomVolume / 2f));
		source.pitch = pitch* (1 + Random.Range(-randomPitch/2f, randomPitch / 2f));
		source.Play ();
	}

	public void Stop(){
		source.Stop ();
	}
}

public class AudioManager : MonoBehaviour {

	public static AudioManager instance; // this instance. Singleton

	[SerializeField]
	Sound[] sounds; // list of sound clips to manage

	void Awake(){
		if (instance != null) {
			if (instance != this)
				Destroy (this.gameObject); // if there is already another instance, destroy this one (leave the one that there was already)
		} else {
			instance = this;		  // set instance to this instance
			DontDestroyOnLoad (this); // don't destroy when changing scene
		}
	}

	void Start(){
		for (int i = 0; i < sounds.Length; i++) {
			GameObject _go = new GameObject ("Sound_" + i + "_" + sounds [i].name); // spawns go for all sounds
			_go.transform.SetParent(this.transform); // set parent of sound game object to this, so that sound is centered here
			sounds[i].SetSource(_go.AddComponent<AudioSource>()); // set source for sound
		}

		// play background music
		PlaySound("Music");
	}

	// function that plays a sound from the manager
	public void PlaySound(string _name){
		for (int i = 0; i < sounds.Length; i++) {
			if (sounds [i].name == _name) {
				sounds [i].Play (); // play found sounds
				return;
			} else {
				Debug.LogWarning ("AudioManager: sound not found in list: " + _name);
			}
		}
	}

	// function that stops playing
	public void StopSound(string _name){
		for (int i = 0; i < sounds.Length; i++) {
			if (sounds [i].name == _name) {
				sounds [i].Stop (); // play found sounds
				return;
			} else {
				Debug.LogWarning ("AudioManager: sound not found in list: " + _name);
			}
		}
	}
}
