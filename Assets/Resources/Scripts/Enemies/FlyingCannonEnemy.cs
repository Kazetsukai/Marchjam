using UnityEngine;
using System.Collections;
using System.Linq;

public class FlyingCannonEnemy : MonoBehaviour {

    private Rigidbody _rigidBody;
    private Transform _cannon;

    [Header("Movement")]
    [SerializeField] float MaximumSpeed = 60f;
    [SerializeField] float BodyTurnRate = 5f;

    [Header("Aiming")]
    [SerializeField] float MaxCannonAngle = 30f;
    [SerializeField] float CannonTurnRate = 0.25f;

    public Vector3 DesiredMovement
    {
        get;
        set;
    }   
    
    public Vector3 DesiredBodyRotation
    {
        get;
        set;
    }

    public Vector2 DesiredCannonRotation
    {
        get;
        set;
    }

	// Use this for initialization
	void Start () {
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _cannon = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Cannon");
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //If not rotating on X, rotating on Y
    void RotateCannon(float rotationChange, bool rotateOnX)
    {
        if (rotationChange == 0)
        {
            return;
        }

        if (Mathf.Abs(rotationChange) > CannonTurnRate)
        {
            rotationChange = rotationChange > 0 ? CannonTurnRate : -CannonTurnRate;
        }

        if (rotateOnX)
        {
            rotationChange *= -1;
        }

        float currentAngle = rotateOnX ? _cannon.rotation.eulerAngles.x : (_cannon.rotation.eulerAngles.y - 180);

        if (currentAngle > 180)
        {
            currentAngle -= 360;
        }

        if (Mathf.Abs(currentAngle + rotationChange) > MaxCannonAngle)
        {
            rotationChange = currentAngle > 0 ? MaxCannonAngle - currentAngle : -(MaxCannonAngle + currentAngle);
        }

        Vector3 rotation = new Vector3(rotateOnX ? rotationChange : 0, rotateOnX ? 0 : rotationChange);
        _cannon.Rotate(rotation);
    }

    float CapSpeed(float rawInput, float currentVelocity)
    {
        if (rawInput == 0)
        {
            return 0;
        }

        float absoluteSpeed = Mathf.Max(Mathf.Abs((rawInput * MaximumSpeed)), MaximumSpeed) - Mathf.Abs(currentVelocity);

        return rawInput > 0 ? absoluteSpeed : -absoluteSpeed;
    }

    float CapRotation(float rawInput, float currentTorque)
    {
        if (rawInput == 0)
        {
            return 0;
        }

        float absoluteRotation = Mathf.Max(Mathf.Abs(rawInput * BodyTurnRate), BodyTurnRate);

        return rawInput > 0 ? absoluteRotation : -absoluteRotation;
    }

    void FixedUpdate()
    {
        /*
        //Temporarily get movement from user input
        DesiredMovement = new Vector3
        (
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            -Input.GetAxis("Pitch")
        );*/

        if (DesiredMovement.magnitude != 0)
        {
            Vector3 localVelocity = _rigidBody.transform.InverseTransformDirection(_rigidBody.velocity);
            Vector3 actualMovement = new Vector3
            (
                CapSpeed(DesiredMovement.x, localVelocity.x),
                CapSpeed(DesiredMovement.y, localVelocity.y),
                CapSpeed(DesiredMovement.z, localVelocity.z)
            );

            _rigidBody.AddRelativeForce(actualMovement);
        }

        /*
        //Temporarily get rotation from user input
        DesiredBodyRotation = new Vector3
        (
            Input.GetAxis("RotateVertical"),
            Input.GetAxis("RotateHorizontal")
        );*/

        if (DesiredBodyRotation.magnitude != 0)
        {
            Vector3 localAngularVelocity = _rigidBody.transform.InverseTransformDirection(_rigidBody.angularVelocity);
            Vector3 actualRotation = new Vector3
            (
                CapRotation(DesiredBodyRotation.x, localAngularVelocity.x), 
                CapRotation(DesiredBodyRotation.y, localAngularVelocity.y), 
                CapRotation(DesiredBodyRotation.z, localAngularVelocity.z)
            );

            _rigidBody.AddRelativeTorque(actualRotation);
        }

        /*
        //Temporarily get cannon direction from user input
        DesiredCannonRotation = new Vector2
        (
            Input.GetAxis("RotateVertical"),
            Input.GetAxis("RotateHorizontal")
        );*/

        if (DesiredCannonRotation.x != 0)
        {
            RotateCannon(DesiredCannonRotation.x, true);            
        }

        if (DesiredCannonRotation.y != 0)
        {
            RotateCannon(DesiredCannonRotation.y, false);
        }
    }
}
