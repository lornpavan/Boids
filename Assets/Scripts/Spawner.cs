using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

static public Spawner S;
static public List<Boids> boids;

[Header("Set in Inspector: Spawning")]
public GameObject boidsPrefab;
public Transform boidsAnchor;
public int numBoids = 100;
public float spawnRadius = 100f;
public float spawnDelay = 0.1f;

// adjust the flocking behavoir of the Boids
public float velocity = 30f;
public float neighborDist = 30f;
public float collDist = 4f;
public float velMatching = 0.25f;
public float flockCentering = 0.2f;
public float collAvoid = 2f;
public float attractPull = 2f;
public float attractPush = 2f;
public float attractPushDist = 5f;

void Awake () {
	S = this;
	boids = new List<Boids>();
	InstantiateBoids();
}

public void InstantiateBoids(){
	GameObject go = Instantiate (boidsPrefab);
	Boids b = go.GetComponent<Boids> ();
	b.transform.SetParent (boidsAnchor);
	boids.Add (b);
	if (boids.Count < numBoids) {
    	Invoke ("InstantiateBoids", spawnDelay);
	}
}

// Use this for initialization
void Start () {

}

// Update is called once per frame
void Update () {

}
}
