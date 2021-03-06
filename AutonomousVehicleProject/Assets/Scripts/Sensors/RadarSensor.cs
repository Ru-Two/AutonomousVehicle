using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadarSensor : Sensor
{
	public float angleIncAmt = 5;   //This is how many degrees the sensor will adjust every time it sends out a raycast
	public Vector3 currentAngleVector;    //This is the current angle that a raycast is being sent from 
	private float currentAngleDegs = 0;
	private bool isWaiting = false; //True if the program is currently waiting to cast a ray
	private float rayDelay = 0.05f;    //Amount of time between raycasts 
	
	private List<HitObject> hitObjects;
	
	void Start()
	{
		//Start the current angle as the direction that the vehicle is facing
		currentAngleVector = transform.rotation.eulerAngles;
		ResetHitObjects();
	}

	void Update()
	{
		if (!isWaiting)
		{
			StartCoroutine(WaitThenCastRayCoroutine(rayDelay));
		}
	}

	/// <summary>
	/// Wait for the provided delay and then cast a ray at the current angle
	/// </summary>
	/// <param name="delay">The amount of seconds to wait</param>
	/// <returns></returns>
	IEnumerator WaitThenCastRayCoroutine(float delay)
	{
		//Set the bool that represents waiting to true so we don't call this multiple times
		isWaiting = true;
		//Wait for the 'delay' number of seconds
		yield return new WaitForSeconds(delay);
	
		//Increase the float variable that represents the degree to draw the ray at by the angle increment amount
		currentAngleDegs += angleIncAmt;
		
		//Check if the current angle is > 360. If so, we've done one circle around
		if (currentAngleDegs > 360)
		{
			//Print all hit Objects for debugging
			foreach (var hitObject in hitObjects)
			{
				print(hitObject);
			}
			
			ResetHitObjects();
			//Apply modulo 360 to this value so that the value stays in the range [0, 360]
			currentAngleDegs %= 360;
		}
		//Convert from degrees to radians as we need radians to use the built in Mathf functions in Unity
		float currentAngleRads = Mathf.Deg2Rad * currentAngleDegs;
	
		//Update the x and z component of the angle to fire from within the Vector3 variable
		currentAngleVector.x = Mathf.Sin(currentAngleRads);
		currentAngleVector.z = Mathf.Cos(currentAngleRads);
	
		//Cast a ray from the vehicle's position out in the direction of the current angle variable 
		Physics.Raycast(transform.position, transform.TransformDirection(currentAngleVector), out RaycastHit hit);
		//Please note, the ray has much longer range than is shown in the debug window
		Debug.DrawRay(transform.position, transform.TransformDirection(currentAngleVector), Color.red, 0.5f);

		//Print info on the hit object if there was a hit object
		if (hit.collider != null)
		{
			//TODO: Replace the Vector3.zero argument with the position of the vehicle.
			hitObjects.Add(new HitObject(GetInaccurateDistance(hit.distance),currentAngleDegs, this, Vector3.zero));
		}
	
		//Done waiting, can call again
		isWaiting = false;
	}

	public List<HitObject> GetHitObjects(){ return hitObjects; }

	public void ResetHitObjects()
	{
		hitObjects = new List<HitObject>();
	}

}

