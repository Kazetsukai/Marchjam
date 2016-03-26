using UnityEngine;
using System.Collections;
using System.Linq;

public class FlyingCannonEnemy : EnemyBase {

    public Rigidbody _rigidBody
    {
        get;
        private set;
    }

    

    [Header("Movement")]
    [SerializeField] public float MaximumSpeed = 60f;
    [SerializeField] public float BodyTurnRate = 5f;

    [Header("Aiming")]
    [SerializeField] public Transform Cannon;
    [SerializeField] public float MaxCannonAngle = 30f;
    [SerializeField] public float CannonTurnRate = 0.25f;

    [Header("Combat")]
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] float InitialProjectileForce = 30f;
    [SerializeField] float FiringCooldown = 3f;

    public float CurrentCooldown
    {
        get;
        private set;
    }

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

    public bool Firing
    {
        get;
        set;
    }

    public void ResetInputs()
    {
        DesiredMovement = new Vector3(0,0,0);
        DesiredBodyRotation = new Vector3(0,0,0);
        DesiredCannonRotation = new Vector2(0,0);
        Firing = false;
    }

	// Use this for initialization
	public new void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();
        Cannon = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Cannon");
		base.Start();
        CurrentCooldown = 0f;
	}
	
	// Update is called once per frame
	public new void Update()
	{
		base.Update();
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

        float currentAngle = rotateOnX ? Cannon.localRotation.eulerAngles.x : (Cannon.localRotation.eulerAngles.y - 180);

        if (currentAngle > 180)
        {
            currentAngle -= 360;
        }

        if (Mathf.Abs(currentAngle + rotationChange) > MaxCannonAngle)
        {
            rotationChange = 0;// currentAngle > 0 ? MaxCannonAngle - currentAngle : -(MaxCannonAngle + currentAngle);
        }

        Vector3 rotation = new Vector3(rotateOnX ? rotationChange : 0, rotateOnX ? 0 : rotationChange);
        Cannon.Rotate(rotation ,Space.Self);
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

    void FireCannon()
    {
        if (CurrentCooldown > 0)
        {
            return;
        }

        CurrentCooldown = FiringCooldown;
        GameObject projectile = Instantiate(ProjectilePrefab);
        projectile.transform.position = Cannon.position + Cannon.forward * 1.7f;
        projectile.transform.rotation = Cannon.rotation;
        projectile.GetComponent<Rigidbody>().AddForce(Cannon.forward * InitialProjectileForce, ForceMode.Impulse);

        //Temporary HAXX so I don't have to calculate parabolas and stuff
        projectile.GetComponent<Rigidbody>().useGravity = false;
        
    }

	public new void FixedUpdate()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown -= Time.fixedDeltaTime;
        }

		if (!Dead)
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

        /*
        //Temporarily get firing from user input
        Firing = Input.GetAxis("Fire1") > 0;
        */

        if (Firing)
        {
            FireCannon();
        }
		}
		else
		{
			_rigidBody.useGravity = true;
		}

		base.FixedUpdate();
    }
}
