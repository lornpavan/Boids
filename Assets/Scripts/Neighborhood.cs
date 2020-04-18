using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour {
[Header("Set Dynamically")]
public List<Boids> neighbors;
private SphereCollider coll;

// Use this for initialization
void Start () {
	neighbors = new List<Boids>();
	coll = GetComponent<SphereCollider> ();
	coll.radius = Spawner.S.neighborDist / 2;
}


// Update is called once per frame
void FixedUpdate () {
	if (coll.radius != Spawner.S.neighborDist / 2) {
									coll.radius = Spawner.S.neighborDist / 2;
	}
}


/* if a Boid enters my zone add to neighbors list */
void OnTriggerEnter(Collider other){
	Boids b = other.GetComponent<Boids>();
	if (b != null)
									if (neighbors.IndexOf (b) == -1) {
																	neighbors.Add (b);
									}
}


/* if a Boid leave my zone remove it from the neighbors list */
void OnTriggerExit(Collider other){
	Boids b = other.GetComponent<Boids> ();
	if (b != null)
									if (neighbors.IndexOf (b) == -1) {
																	neighbors.Remove(b);
									}
}


public Vector3 avgPos {
	get {
		Vector3 avg = Vector3.zero;
		if (neighbors.Count == 0)
			return avg;
	    for (int i = 0; i < neighbors.Count; i++) {
		    avg += neighbors [i].pos;
		}
		avg /= neighbors.Count;
		return avg;
	}
}

public Vector3 avgVel {
	get {
	    Vector3 avg = Vector3.zero;
		if (neighbors.Count == 0)
			return avg;
		for (int i = 0; i < neighbors.Count; i++) {
			avg += neighbors[i].rigid.velocity;
		}
		avg /= neighbors.Count;

		return avg;
		}
}

public Vector3 avgClosePos {
	get {
		Vector3 avg = Vector3.zero;
		Vector3 delta;
		int nearCount = 0;
		for (int i = 0; i < neighbors.Count; i++) {
			delta = neighbors [i].pos - transform.position;
			if (delta.magnitude <= Spawner.S.collDist) {
				avg += neighbors [i].pos;
				nearCount++;
			}
		}
		// if there are no neighbors too close. return Vector3.zero
	    if (nearCount == 0)
			return avg;
        // otherwise
		avg /= nearCount;
		return avg;
								}
    }
}
