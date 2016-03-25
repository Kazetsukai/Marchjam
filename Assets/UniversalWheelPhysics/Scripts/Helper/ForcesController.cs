using UnityEngine;
using System.Collections;

public class ForcesController : MonoBehaviour
{
	Rigidbody rb;
	Vector3 acc;
	Vector3 lastV;
	Vector3 currV;

	public UniversalWheel frontLeftWheel;
	public UniversalWheel frontRightWheel;

	public UniversalWheel rearLeftWheel;
	public UniversalWheel rearRightWheel;

	public float steerAngle = 32;
	public float maxTorque = 30000;
	public float brakeForce = 25000;
	public float handBrakeForce = 30000;

	public float force = 10000;

	public float GForce;



	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		lastV = currV;
		currV = rb.velocity;
		acc = (currV - lastV) / Time.deltaTime;
		GForce = acc.magnitude / 9.806f;
	}
	void Update ()
	{

		if (Input.GetKey (KeyCode.O)) {
			rb.velocity = Vector3.zero;
		}
		if (Input.GetKey (KeyCode.H)) {
			rb.AddForce (transform.right * -force);
		}
		if (Input.GetKey (KeyCode.K)) {
			rb.AddForce (transform.right * force);
		}

		if (Input.GetKey (KeyCode.U)) {
			rb.AddForce (transform.forward * force);
		}
		if (Input.GetKey (KeyCode.J)) {
			rb.AddForce (transform.forward * -force);
		}
		if (Input.GetKey (KeyCode.R)) {
			Application.LoadLevel (0);
		}

		if (Input.GetKey (KeyCode.Space)) {
			rearRightWheel.axisBrake = brakeForce;
			rearLeftWheel.axisBrake = brakeForce;
			frontLeftWheel.axisBrake = brakeForce;
			frontRightWheel.axisBrake = brakeForce;
		} else if (Input.GetKey (KeyCode.RightShift)) {
			rearRightWheel.axisBrake = handBrakeForce;
			rearLeftWheel.axisBrake = handBrakeForce;
			frontLeftWheel.axisBrake = 0;
			frontRightWheel.axisBrake = 0;
		} else {
			rearRightWheel.axisBrake = 0;
			rearLeftWheel.axisBrake = 0;
			frontLeftWheel.axisBrake = 0;
			frontRightWheel.axisBrake = 0;
		}

		frontLeftWheel.transform.localEulerAngles = new Vector3 (0, Input.GetAxis ("Horizontal") * steerAngle, 0);
		frontRightWheel.transform.localEulerAngles = new Vector3 (0, Input.GetAxis ("Horizontal") * steerAngle, 0);

		/*
		float _brakingForce = -Mathf.Clamp (Input.GetAxis ("Vertical"), -1, 0) * brakeForce;
		rearRightWheel.axisBrake = _brakingForce;
		rearLeftWheel.axisBrake = _brakingForce;
		frontLeftWheel.axisBrake = _brakingForce;
		frontRightWheel.axisBrake = _brakingForce;
		*/

		rearRightWheel.axisTorque = Input.GetAxis ("Vertical") * maxTorque;
		rearLeftWheel.axisTorque = Input.GetAxis ("Vertical") * maxTorque;

	}
}
