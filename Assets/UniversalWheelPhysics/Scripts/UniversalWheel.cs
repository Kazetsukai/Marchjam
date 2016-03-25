using UnityEngine;
using System.Collections;

public class UniversalWheel : MonoBehaviour
{

	#region public fields

	public bool drawDebugLines = true;
	public LayerMask collisionMask = 1;

	public Rigidbody rigidBody;

	public Transform visualWheel;
	
	public bool affectOtherRigidbodies = false;
	public bool fluentWheelMovement = true;

	[Range(0,1)]
	public float
		visualWheelMoveSpeed = 0.5f;

	[Range(0.1f, 1000f)]
	public float
		mass = 15f;

	public float radius = 0.35f;

	public float suspensionTravel = 0.2f;

	public float springRate = 4000;

	[Range(0,0.5f)]
	public float
		springPreload = 0;

	public float damping = 3000f;

	public float forwardFrictionCoef = 1f;
	public float forwardSlipCoef = 1;
	public float sidewayFrictionCoef = 1f;
	public float sidewaySlipCoef = 1;

	public float maxSideForce = 0;
	public float maxForwardForce = 0;


	[HideInInspector]
	public float
		axisTorque;
	[HideInInspector]
	public float
		axisBrake;

	[HideInInspector]
	public Vector4
		forwardFrictionVars;
	
	[HideInInspector]
	public Vector4
		sidewaysFrictionVars;
	
	[HideInInspector]
	public float[]
		forwardFrictionCurveArray;
	[HideInInspector]
	public float[]
		sidewayFrictionCurveArray;

	[HideInInspector]
	public AnimationCurve
		forwardAnimCurve;
	[HideInInspector]
	public AnimationCurve
		sidewaysAnimCurve;
    

	#endregion

	#region private fields
	bool contact;
	Vector3 wheelPos;
	float wheelDownTravel;

	float compressionSpeed;
	float onGroundPressureForce;
	float percentOfSpringCompression;
	float lastPercentOfSpringCompression;
	
	float rpm = 0;
	float realRPM = 0;
	float lastRealRPM = 0;
	float RPMacc = 0;

	FrictionCurve sidewayFrictionCurve;
	FrictionCurve forwardFrictionCurve;
	float sidewaySlip;
	float forwardSlip;
	float forwardVelocity;
	float sidewayVelocity;
	
	Vector3 sidewayForce;
	Vector3 forwardForce;

	Vector3 myUp;
	Vector3 myRight;
	Vector3 myForward;

	Vector3 transPos;
	Vector3 totalSlip;
	Vector3 totalForce = Vector3.zero;
	float rotationAngle = 0;
	float lastMass;

	Ray ray;
	RaycastHit hit;

	float deltaTime;
	bool preciseRPMCalculaions = true;
	Vector3 springForce;
	Vector3 dampingForce;
	Vector3 suspensionTotalForce;
	float compressedSpringRate;

	const float Xfor = 1.5f;
	const float Yfor = 1.5f;
	const float Zfor = 1.0f;
	const float Wfor = -1f;
	
	const float Xside = 0.714f;
	const float Yside = 1.4f;
	const float Zside = 1.0f;
	const float Wside = -0.2f;
	#endregion

	#region properties

	public float RPM {
		get {
			return realRPM;
		}
	}

	public Vector3 WheelPos {
		get {
			return wheelPos;
		}
	}

	public float SuspensionTravel {
		get {
			return wheelDownTravel;
		}
	}

	public float SidewaySlip {
		get {
			return sidewaySlip;
		}
	}

	public float ForwardSlip {
		get {
			return forwardSlip;
		}
	}

	public bool HasContact {
		get {
			return contact;
		}
	}

	#endregion
	
	void Start ()
	{

		// create friction curves
		UpdateFrictionCurves (false);

	}
	
	void FixedUpdate ()
	{
		if (rigidBody == null)
			return;

		if (mass < 0.1f) {
			mass = 0.1f;
		}

		transPos = transform.position;

		myUp = transform.up;
		myRight = transform.right;
		myForward = transform.forward;

		CalculateSuspensionForces ();

		if (contact) {
			CalculateSlip ();
			CalculateFrictionForces ();
		}
		ApplyForces ();

	}

	void Update ()
	{
		if (visualWheel != null) {
			UpdateVisuals ();
		}
	}

	void CalculateSuspensionForces ()
	{
		// cache deltaTime for late use
		deltaTime = Time.deltaTime;

		// shoot ray down dir
		ray = new Ray (transPos, -myUp);

		if (Physics.Raycast (ray, out hit, radius + suspensionTravel, collisionMask)) {
			contact = true;
		
			wheelPos = transPos + myUp * -(hit.distance - radius);
			wheelDownTravel = hit.distance - radius;

			lastPercentOfSpringCompression = percentOfSpringCompression;
			percentOfSpringCompression = 1 - ((hit.distance - radius) / suspensionTravel);

			compressionSpeed = (percentOfSpringCompression - lastPercentOfSpringCompression) / deltaTime;

			compressedSpringRate = percentOfSpringCompression * springRate + (springPreload * springRate);

			springForce = hit.normal * compressedSpringRate;

			dampingForce = hit.normal * (compressionSpeed * damping / 10);

			suspensionTotalForce = springForce + dampingForce;

			totalForce = suspensionTotalForce;

			onGroundPressureForce = compressedSpringRate + (compressionSpeed * damping / 10);	

			if (affectOtherRigidbodies && hit.transform.GetComponent<Rigidbody> () != null && !hit.transform.GetComponent<Rigidbody> ().isKinematic) {
				hit.transform.GetComponent<Rigidbody> ().AddForceAtPosition (-myUp * onGroundPressureForce, hit.point);
			}
		} else {
			contact = false;
			wheelPos = transPos + myUp * -suspensionTravel;
			wheelDownTravel = suspensionTravel;
			percentOfSpringCompression = 0;
			sidewaySlip = 0;
			forwardSlip = 0;
			totalForce = Vector3.zero;
		}

		rotationAngle += rpm * deltaTime;
		
		lastRealRPM = realRPM;
		realRPM = rpm * 0.1666667f;
		RPMacc = (realRPM - lastRealRPM) / deltaTime;

	}

	void CalculateSlip ()
	{
		//cache velocities for later use
		forwardVelocity = Vector3.Dot (rigidBody.GetPointVelocity (transform.position), myForward);
		sidewayVelocity = Vector3.Dot (rigidBody.GetPointVelocity (transform.position), myRight);
        
		sidewaySlip = sidewayVelocity * sidewaySlipCoef * 2;
		forwardSlip = (forwardVelocity - realRPM * (Mathf.PI / 30) * radius) * forwardSlipCoef * 2; 

	}

	void CalculateFrictionForces ()
	{
		if (onGroundPressureForce <= 0)
			onGroundPressureForce = 0;


		// sideway force
		//if (Mathf.Abs (sidewaySlip) > 0.1f) {
		sidewayForce = myRight * Mathf.Sign (sidewaySlip) * ((sidewayFrictionCurve.GetFriction (sidewaySlip))) * onGroundPressureForce;
		if (maxSideForce > 0) {
			sidewayForce = Vector3.ClampMagnitude (sidewayForce, maxSideForce);
		}
		//} else {// "static" friction force
		//	sidewayForce = myRight * Mathf.Sign (sidewaySlip) * 0.5f * onGroundPressureForce * sidewayFrictionCoef;
		//}

		// forward force
		//if (Mathf.Abs (forwardSlip) > 0.9f) {
		forwardForce = myForward * Mathf.Sign (forwardSlip) * (forwardFrictionCurve.GetFriction (forwardSlip)) * onGroundPressureForce;
		if (maxForwardForce > 0) {
			forwardForce = Vector3.ClampMagnitude (forwardForce, maxForwardForce);
		}
		//} else {
		//	forwardForce = myForward * Mathf.Sign (forwardSlip) * 2.8f * onGroundPressureForce * forwardFrictionCoef;
		//}

		Vector3 forceSum = Vector3.ClampMagnitude ((-forwardForce - sidewayForce), onGroundPressureForce);


		sidewayForce = myRight * Vector3.Dot (forceSum, myRight) * sidewayFrictionCoef;
		forwardForce = myForward * Vector3.Dot (forceSum, myForward) * forwardFrictionCoef;

		//if (Mathf.Abs (forwardSlip) > Mathf.Abs (sidewaySlip)) {
		//	sidewayForce = Vector3.ClampMagnitude (sidewayForce, forwardForce.magnitude * 0.5f);
		//}
		//} else {
		//	forwardForce = Vector3.ClampMagnitude (forwardForce, sidewayForce.magnitude);
		//}

		totalForce += sidewayForce;
		totalForce += forwardForce;

	}

	void ApplyForces ()
	{
		if (contact) {

			rigidBody.AddForceAtPosition (totalForce, wheelPos);

			float value = Mathf.Sign (forwardSlip) * forwardForce.magnitude * 50 / mass * deltaTime;

			// rpm needed to have 0 slip, this if formula to calculate RPM from linear velocity in m/s, /0.16(6) is needed to convert angular velocity to RPM
			float perfectRPM = (forwardVelocity * 60 / (radius * 2 * Mathf.PI)) / 0.1666667f;
			//radius * realRPM * 0.10472f (forwardSpeed / 0.16666f);

			// difference between rpm and no-slip rpm, max value that can be added to rpm at once
			float rpmDiff = rpm - perfectRPM;

			if (value < 0) {
				value = Mathf.Clamp (value, -rpmDiff, 0);
			} else {
				value = Mathf.Clamp (value, 0, -rpmDiff);
			} 

			rpm += value;

		} else {
			if (RPMacc != 0)
				rigidBody.AddTorque (-myRight * (RPMacc * mass * radius * 2 * deltaTime));
		}

		//Apply motor torque
		rpm += axisTorque / radius / mass * deltaTime;
		
		//Apply brake torque
		rpm -= Mathf.Sign (rpm) * Mathf.Min (Mathf.Abs (rpm), axisBrake / radius / mass * deltaTime);

	}

	void UpdateVisuals ()
	{
		if (fluentWheelMovement) {

			float updateCoef = Mathf.Lerp (8f, 40f, visualWheelMoveSpeed);
			visualWheel.localPosition = Vector3.Lerp (visualWheel.localPosition, new Vector3 (visualWheel.localPosition.x, -wheelDownTravel, visualWheel.localPosition.z), updateCoef * Time.deltaTime);

		} else {
			visualWheel.localPosition = Vector3.zero;
			visualWheel.Translate (-myUp * (wheelDownTravel), Space.World);
		}

		visualWheel.localEulerAngles = new Vector3 (rotationAngle, 0, 0);
	}

	public bool GetGroundHit (out RaycastHit _hit)
	{
		_hit = hit;
		if (contact) {
			return true;
		} else {
			return false;
		}
	}

	public void UpdateFrictionCurves (bool defaultValues)
	{

		if (defaultValues) {
			forwardFrictionVars = new Vector4 (Xfor, Yfor, Zfor, Wfor);
			sidewaysFrictionVars = new Vector4 (Xside, Yside, Zside, Wside);
		}

		forwardFrictionCurve = new FrictionCurve (forwardFrictionVars);
		sidewayFrictionCurve = new FrictionCurve (sidewaysFrictionVars);

		forwardFrictionCurveArray = forwardFrictionCurve.curve;
		sidewayFrictionCurveArray = sidewayFrictionCurve.curve; 
        
		forwardAnimCurve = new AnimationCurve ();
		for (int i = 0; i < 999; i++) {
			forwardAnimCurve.AddKey (i, forwardFrictionCurveArray [i]);
		}
		
		sidewaysAnimCurve = new AnimationCurve ();
		for (int i = 0; i < 999; i++) {
			sidewaysAnimCurve.AddKey (i, sidewayFrictionCurveArray [i]);
		}
	}

	void OnDrawGizmos ()
	{
		if (!drawDebugLines)
			return;

		if (percentOfSpringCompression == 0) {
			Gizmos.color = Color.green;
		} else if (percentOfSpringCompression < 1) {
			Gizmos.color = Color.blue;
		} else {
			Gizmos.color = Color.red;
		}
		Gizmos.DrawLine (transform.position, transform.position - transform.up * ((1 - percentOfSpringCompression) * suspensionTravel));

		Vector3 wheelCenter;
		Vector3 suspensionMove;

		if (Application.isPlaying) {
			wheelCenter = wheelPos;
			suspensionMove = -Vector3.up * ((1 - percentOfSpringCompression) * suspensionTravel);
		} else {
			wheelCenter = transform.position;
			suspensionMove = Vector3.zero;
		}

		Vector3 pos;
		Vector3 pos2 = transform.TransformPoint (suspensionMove + radius * new Vector3 (0, Mathf.Sin (0), Mathf.Cos (0)));
		for (int i = 1; i <= 20; ++i) {
			pos = transform.TransformPoint (suspensionMove + radius * new Vector3 (0, Mathf.Sin (i / 20.0f * Mathf.PI * 2.0f), Mathf.Cos (i / 20.0f * Mathf.PI * 2.0f)));
			Gizmos.DrawLine (pos2, pos);
			pos2 = pos;
		}

	}
    
}
