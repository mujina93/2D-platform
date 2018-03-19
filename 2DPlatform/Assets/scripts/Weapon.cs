using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float fireRate;
	public int damage = 10;
	public LayerMask whatToHit;

	public Transform bulletTrailPrefab;
	public Transform muzzleFlashPrefab;
    public Transform hitPrefab;

	private float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	private float timeToFire = 0; // place in time when we will hace the next shot

	// handle camera shaking
	public float camShakeAmt = 0.05f;
	public float camShakeLength = 0.1f;
	CameraShake camShake;

	public string weaponShootSound = "DefaultShot";

	// caching
	AudioManager audioManager;
		

	Transform firePoint;

	void Awake(){
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("FirePoint not found as child of weapon");
		}
	}

	void Start(){
		camShake = GameMaster.gm.GetComponent<CameraShake> ();
		if (camShake == null) {
			Debug.LogError ("No cameraShake found on GM object");
		}
		audioManager = AudioManager.instance;
		if (audioManager == null)
			Debug.LogError ("No audio manager found");
	}

	// Update is called once per frame
	void Update () {
		// single burst (click once shoot once)
		if (fireRate == 0) {
			if (Input.GetButtonDown("Fire1")){
				Shoot();
			}
		}
		// else, multiple burst (keep clicked keep shooting)
		else {
			// if you are pressing fire AND it's time for the next shot, shoot()
			if (Input.GetButton("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1 / fireRate;
				Shoot ();
			}
		}
	}

	void Shoot(){
		// save mouse coordinates into world coordinates (used for raycast)
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		// store position of firePoint
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		// cast a ray (in direciton of clicked point) and hit everything in the hit mask
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		
		// debug line
		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition)*100, Color.cyan, 1, false);
		// if you hit something, damage
		if (hit.collider != null) {
			// red debug line towards the hit target
			Debug.DrawLine(firePointPosition, hit.point, Color.red, 1, false);
			// try to get the enemy from the hit target
			Enemy enemy = hit.collider.GetComponent<Enemy> ();
			// if you got an enemy, damage it
			if (enemy != null) {
				enemy.DamageEnemy (damage);
			}
		}

        // spawn the bulletTrail and muzzleFlash effects, but not more frequently than effectSpawnRate
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            // spawn the effects, and pass the position of the hit point, and the direction normal to the hit surface
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

	void Effect(Vector3 hitPos, Vector3 hitNormal){
		Transform trail = Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        // if you hit something, spawn a 'hit' effect, with direction normal to the hit surface
        if(hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticles = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticles.gameObject, 1f);
        }

		Transform clone = (Transform) Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation);
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);

		// shake camera
		camShake.Shake(camShakeAmt, camShakeLength);

		// play shoot sound
		audioManager.PlaySound(weaponShootSound);
	}
}
