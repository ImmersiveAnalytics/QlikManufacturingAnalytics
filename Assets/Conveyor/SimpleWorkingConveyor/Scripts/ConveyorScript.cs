using UnityEngine;
using System.Collections;
using System.Collections.Generic; //This is included so lists may be used.

public class ConveyorScript : MonoBehaviour {
	// Updated 23/5/2016 by N00BZILLA STUDIOS

	// This script is attached to the conveyor belt game object (not the entire prefab or the support blocks)
	// The script controls the conveyor belt via its speed and its on/off status.
	// objects with a rigidbody component and a collider component, that collide with the conveyor belt will have their velocities modified by this conveyor script.
	// Note that this script intentionally disables any colliding objects rotation and resets their velocity to zero upon entering collision.
	// This is done so objects colliding with the conveyor belt translate nicely in the driving direction of the conveyor belt and do not rotate/ spin or slip off the belt in an uncontrolled manner.

	public bool conveyorOn; // This turns the conveyor on or off
	public float conveyorDriveSpeed; // This gives the conveyor a target speed to match when it is turned on. Note that a negative speed will drive the conveyor backwards.

	private float conveyorSpeed; // The actual conveyor belt speed (0 when off)
	private float previousConveyorSpeed; // Used to keep track of when the conveyor speed changes for logical reasons.
	private Vector3 conveyorDirectionVector; // A 3D vector that represents the direction of travel along the conveyor body
	private Vector3 conveyorVelocityVector; // A 3D vector that shows the velocity in terms of x, y and z coordinates

	private float conveyorBeltMaterialOffset; //The number that is used to offset the conveyor belt texture (it changes the materialś offset)
	public float conveyorBeltMaterialOffsetConstant; // Used to adjust the rate of change in the conveyor belt texture offset. By adjusting this, the speed of objects on the conveyor can be matched to the speed of the conveyor belt texture to make the belt look more realistic. 
	//This constant multiplied by the conveyorDriveSpeed equals the conveyorBeltMaterialOffset

	private List<Rigidbody> listOfRigidbodiesOnConveyor; //the list of rigidbodies that are currently colliding with the conveyor belt.

	// Use this for initialization
	void Start () {
		listOfRigidbodiesOnConveyor = new List<Rigidbody>();
		previousConveyorSpeed = conveyorSpeed;
		conveyorDirectionVector = transform.rotation * Vector3.down;
	}
	
	// Update is called once per frame
	void Update () {
		if(conveyorOn){
			conveyorSpeed = conveyorDriveSpeed;
		}else{
			conveyorSpeed = 0f;
		}

		//Shift the belt texture along to make it look as if its moving
		conveyorBeltMaterialOffset += conveyorBeltMaterialOffsetConstant * conveyorSpeed * Time.deltaTime;
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2( 0f ,conveyorBeltMaterialOffset));

		//If the conveyor speed changes, adjust the speed of the objects currently on the conveyor.
		if (conveyorSpeed != previousConveyorSpeed) {

			if (listOfRigidbodiesOnConveyor.Count > 0) {
				// Remove the velocity component previously added.
				foreach (Rigidbody rigidbody in listOfRigidbodiesOnConveyor) {
					rigidbody.velocity -= conveyorVelocityVector;
				}
			}
			//Adjust the velocity component
			conveyorVelocityVector = conveyorDirectionVector * conveyorSpeed;
			if (listOfRigidbodiesOnConveyor.Count > 0) {
				//Add the new velocity component 
				foreach (Rigidbody rigidbody in listOfRigidbodiesOnConveyor) {
					rigidbody.velocity += conveyorVelocityVector;
				}
			}
			previousConveyorSpeed = conveyorSpeed;
		}
	}

	void OnCollisionEnter(Collision collision) {
		Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
		//Debug.Log ("Conveyor collided with: " + rigidbody.name);
		listOfRigidbodiesOnConveyor.Add (rigidbody);
		rigidbody.velocity = new Vector3(0f,0f,0f);
		rigidbody.useGravity = false;
		rigidbody.freezeRotation = true;
		rigidbody.velocity += conveyorVelocityVector;
	}

	void OnCollisionExit(Collision collision) {
		Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
		rigidbody.useGravity = true;
		rigidbody.freezeRotation = false;
		listOfRigidbodiesOnConveyor.Remove(rigidbody);
	}

}
