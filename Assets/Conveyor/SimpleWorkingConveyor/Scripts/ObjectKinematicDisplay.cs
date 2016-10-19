using UnityEngine;
using System.Collections;

public class ObjectKinematicDisplay : MonoBehaviour {

	// This script can be attached to an object that you wish to drop on the conveyor belt.
	// The script will allow you to track the velocity and its angular velocity in the debug console 

	public Vector3 objectVelocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		objectVelocity = GetComponent<Rigidbody> ().velocity;
		Debug.Log ("Object Velocity: " +GetComponent<Rigidbody> ().velocity);
		Debug.Log ("Object Angular Velocity: " +GetComponent<Rigidbody> ().angularVelocity);

	}
}
