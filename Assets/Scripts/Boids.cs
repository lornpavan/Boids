using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour {

[Header("Set Dynamically")]
public Rigidbody rigid;

/* keep track of my neighbors */
private Neighborhood neighborhood;

void Awake(){
	neighborhood = GetComponent<Neighborhood> ();
	rigid = GetComponent<Rigidbody> ();
    // random initial position
	pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

	// random initial velocity
	Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
	rigid.velocity = vel;

	LookAhead ();
    // Random Color Boid
	Color randColor = Color.black;
	while (randColor.r + randColor.g + randColor.b < 1.0f) {
									randColor = new Color (Random.value, Random.value, Random.value);
	}
	Renderer[] rends = gameObject.GetComponentsInChildren<Renderer> ();
	foreach (Renderer r in rends) {
									r.material.color = randColor;
	}
	TrailRenderer tRend = GetComponent<TrailRenderer> ();
	tRend.material.SetColor ("_TintColor", randColor);
}

void LookAhead(){
	// orients the Boid
	transform.LookAt (pos + rigid.velocity);
}

void FixedUpdate(){
	Vector3 vel = rigid.velocity;
	Spawner spn = Spawner.S;
	// Collison Dectections
	Vector3 velAvoid = Vector3.zero;
	Vector3 tooClosePos = neighborhood.avgClosePos;
	// too close to another boid
	if (tooClosePos != Vector3.zero) {
									velAvoid = pos - tooClosePos;
									velAvoid.Normalize ();
									velAvoid *= spn.velocity;
	}

	// velocity matching
	Vector3 velAlign = neighborhood.avgVel;
	if (velAlign != Vector3.zero) {
									velAlign.Normalize ();
									velAlign *= spn.velocity;
	}

	// flocking behavior to center
	Vector3 velCenter = neighborhood.avgPos;
	if (velCenter != Vector3.zero) {
									velCenter -= transform.position;
									velCenter.Normalize ();
									velCenter *= spn.velocity;
	}

	// Attractor
	Vector3 delta = Attractor.POS - pos;

	// Is the Boid already moving towards the Attractor?
	bool attracted = (delta.magnitude > spn.attractPushDist);
	Vector3 velAttract = delta.normalized * spn.velocity;

	float fdt = Time.fixedDeltaTime;
	if (velAvoid != Vector3.zero) {
									vel = Vector3.Lerp (vel, velAvoid, spn.collAvoid * fdt);
	} else {
									if (velAlign != Vector3.zero) {
																	vel = Vector3.Lerp (vel, velAlign, spn.velMatching * fdt);
									}
									if (velCenter != Vector3.zero) {
																	vel = Vector3.Lerp (vel, velAlign, spn.flockCentering * fdt);
									}
									if (velAttract != Vector3.zero) {
																	if (attracted) {
																									vel = Vector3.Lerp (vel, velAttract, spn.attractPull * fdt);
																	} else {
																									vel = Vector3.Lerp (vel, -velAttract, spn.attractPush * fdt);
																	}
									}
	}

	if (attracted) {
									vel = Vector3.Lerp (vel, velAttract, spn.attractPull * fdt);
	} else {
									vel = Vector3.Lerp (vel, -velAttract, spn.attractPush * fdt);
	}
	vel = vel.normalized * spn.velocity;
	rigid.velocity = vel;
	LookAhead ();
}

// Use this for initialization
void Start () {

}

// Update is called once per frame
void Update () {

}

public Vector3 pos {
	get { return transform.position; }
	set { transform.position = value; }
}
}
