using Pathfinding;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

	// what to chase?
	public Transform target;

	// how many times per second we will update our path
	public float updateRate = 2f;

	// Cachelog
	private Seeker seeker;
	private Rigidbody2D rb;

	// the calculated path
	public Path path;

	// ai's speed per second
	public float speed = 300f;
	public ForceMode2D fMode;

	[HideInInspector] // won't show up in inspector
	public bool pathIsEnded = false;

	// max distance from AI to waypoint for continuing to the next waypoint
	public float nextWaypointDistance = 3f;

	// the waypoint we are currently moving towards (indexed by an integer)
	private int currentWaypoint = 0;

	// is the enemy chasing the player?
	private bool searchingForPlayer = false;

	void Start(){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

		// if there is no target, and you are not searching for player anymore,
		// start searching for player again
		//Debug.Log("target is: " + target + " at position " + target.localPosition);
		if (target == null) {
			Debug.Log ("target is null");
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			return;
		}

		// start a new path to the target position and return the result to the OnPathComplete method
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		// we want the enemies to update the path when the player moves
		StartCoroutine (UpdatePath ());
	}

	// called when there is no more target to chase:
	// finds the Player GameObject, then if it finds it, stop searching for player, and UpdatePath
	// otherwise, call the SearchForPlayer coroutine again in 0.5 seconds.
	IEnumerator SearchForPlayer(){
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		if (sResult == null) {
			// look for player every 0.5 sec
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchForPlayer ());
		} else {
			// otherwise, start chasing him
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());
			yield return false;
		}
	}

	IEnumerator UpdatePath(){
		// if you lost the target, go back to the 'searching' state until you find it
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			yield return false;
		}

		// start a new path to the target position and return the result to the OnPathComplete method
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1 / updateRate);
		StartCoroutine (UpdatePath ()); // calls itself every 1/updateRate seconds
	}

	public void OnPathComplete(Path p){
		//Debug.Log ("we got a path. Did it have an error?" + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
 

	// gives access to fixedDeltaTime (use FixedUpdate when doing something with physics!)
	void FixedUpdate(){
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			return;
		}

		// TODO always look at player
		if (path == null) {
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count) {
			// when you reach the end of the path, end the search
			if (pathIsEnded)
				return;
			
			//Debug.Log ("end of path reached");
			pathIsEnded = true;
			return;
		}

		// search has not ended yet
		pathIsEnded = false;

		// direction to new waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		// move the enemy by adding a force towards the right direction
		rb.AddForce(dir, fMode);

		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
		if (dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
}
