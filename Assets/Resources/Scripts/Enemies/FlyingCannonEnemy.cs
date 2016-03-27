using UnityEngine;
using System.Collections;
using System.Linq;

public class FlyingCannonEnemy : EnemyBase
{
    [Header("Movement")]
    [SerializeField] public float MaximumSpeed = 60f;
    [SerializeField] public float StartSpeed = 30f;
    [SerializeField] public float AccelerationRate = 5f;
    [SerializeField] public float BodyTurnRate = 5f;

    [Header("Aiming")]
    [SerializeField] public Transform Cannon;
    [SerializeField] public float MaxCannonAngle = 30f;
    [SerializeField] public float CannonTurnRate = 0.25f;

    [Header("Combat")]
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] float InitialProjectileForce = 30f;
    [SerializeField] float FiringCooldown = 3f;

    public Rigidbody Body
    {
        get;
        private set;
    }

    public float CurrentCooldown
    {
        get;
        private set;
    }

    /// <summary>
    /// Tells the FlyingCannonEnemy to move in the directions of the Vector local to it's current position and rotation
    /// </summary>
    public Vector3 DesiredMovement
    {
        get;
        set;
    }

    /// <summary>
    /// Tells the FlyingCannonEnemy to rotate in the axes specified by the Vector local to it's current position and rotation
    /// </summary>
    public Vector3 DesiredBodyRotation
    {
        get;
        set;
    }

    /// <summary>
    /// Tells the FlyingCannonEnemy to rotate it's cannon in the axes specified by the Vector local to it's current position and rotation
    /// </summary>
    public Vector2 DesiredCannonRotation
    {
        get;
        set;
    }

    /// <summary>
    /// Tells the FlyingCannonEnemy to fire it's cannon
    /// </summary>
    public bool Firing
    {
        get;
        set;
    }

    /// <summary>
    /// Resets all inputs to their default values
    /// </summary>
    public void ResetInputs()
    {
        DesiredMovement = Vector3.zero;
        DesiredBodyRotation = Vector3.zero;
        DesiredCannonRotation = Vector3.zero;
        Firing = false;
    }

	// Use this for initialization
	public new void Start()
	{
		Body = GetComponent<Rigidbody>();
		base.Start();
        CurrentCooldown = 0f;
	}
	
	// Update is called once per frame
	public new void Update()
	{
		base.Update();
	}

    /// <summary>
    /// Rotates the cannon within the constraints that have been set for speed and maximum angle.
    /// Set rotateOnX to true if rotating by the X axis, otherwise will rotate by the Y axis.
    /// </summary>
    /// <param name="rotationChange">Amount of change, 1f = maximum change in positive direction of axis</param>
    /// <param name="rotateOnX">Set rotateOnX to true if rotating by the X axis, otherwise will rotate by the Y axis.</param>
    void RotateCannon(float rotationChange, bool rotateOnX)
    {
        if (rotationChange == 0)
        {
            return;
        }

        //Cap change to turn rate
        if (Mathf.Abs(rotationChange) > CannonTurnRate)
        {
            rotationChange = CannonTurnRate * Mathf.Sign(rotationChange);
        }

        float currentAngle = rotateOnX ? Cannon.localRotation.eulerAngles.x : (Cannon.localRotation.eulerAngles.y);

        //Cap change to max cannon angle
        if (Mathf.Abs(Mathf.DeltaAngle(0, currentAngle + rotationChange)) > MaxCannonAngle)
        {
            rotationChange = Mathf.DeltaAngle(currentAngle, MaxCannonAngle * Mathf.Sign(rotationChange));
        }

        Vector3 rotation = new Vector3(rotateOnX ? rotationChange : 0, rotateOnX ? 0 : rotationChange);
        Cannon.Rotate(rotation ,Space.Self);
    }

    /// <summary>
    /// Returns the maximum force that can be applied for the given input and current velocity for a direction.
    /// </summary>
    /// <param name="rawInput">Amount of input, from 0f to 1f = full throttle</param>
    /// <param name="currentVelocity">Current velocity in the specified direction</param>
    /// <returns></returns>
    float CapSpeedIncrement(float rawInput, float currentVelocity)
    {
        if (rawInput == 0)
        {
            return 0;
        }

        float targetSpeed = Mathf.Max(Mathf.Abs(rawInput * MaximumSpeed), MaximumSpeed);
        float absVelocity = Mathf.Abs(currentVelocity);

        //Snap to starting speed if not already exceeding it
        if (absVelocity < StartSpeed)
        {
            targetSpeed = Mathf.Min(targetSpeed, StartSpeed);
        }
        //Otherwise cap to the current speed + acceleration rate
        else
        {
            targetSpeed = Mathf.Min(targetSpeed, absVelocity + AccelerationRate);
        }

        //Get just the speed offset
        targetSpeed -= absVelocity;
        if (targetSpeed < 0)
        {
            targetSpeed = 0;
        }

        return targetSpeed * Mathf.Sign(rawInput);
    }

    /// <summary>
    /// Returns the maximum torque that can be applied for the given input and current angular velocity for an axis
    /// </summary>
    /// <param name="rawInput">Amount of input, from 0f to 1f = maximum rotation</param>
    /// <param name="currentTorque">Current angular velocity on the specified axis</param>
    /// <returns></returns>
    float CapRotationIncrement(float rawInput, float currentTorque)
    {
        if (rawInput == 0)
        {
            return 0;
        }

        float targetRotation = Mathf.Max(Mathf.Abs(rawInput * BodyTurnRate), BodyTurnRate) - Mathf.Abs(currentTorque);

        return targetRotation * Mathf.Sign(rawInput);
    }

    /// <summary>
    /// Fires a projectile from the cannon if able and activates the cooldown to prevent immediately firing again
    /// </summary>
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
            //Apply movement if input provided
            if (DesiredMovement.magnitude != 0)
            {
                Vector3 localVelocity = Body.transform.InverseTransformDirection(Body.velocity);
                Vector3 actualMovement = new Vector3
                (
                    CapSpeedIncrement(DesiredMovement.x, localVelocity.x),
                    CapSpeedIncrement(DesiredMovement.y, localVelocity.y),
                    CapSpeedIncrement(DesiredMovement.z, localVelocity.z)
                );

				Body.AddRelativeForce(actualMovement, ForceMode.Acceleration);
            }

            //Apply rotation if input provided
            if (DesiredBodyRotation.magnitude != 0)
            {
                Vector3 localAngularVelocity = Body.transform.InverseTransformDirection(Body.angularVelocity);
                Vector3 actualRotation = new Vector3
                (
                    CapRotationIncrement(DesiredBodyRotation.x, localAngularVelocity.x), 
                    CapRotationIncrement(DesiredBodyRotation.y, localAngularVelocity.y), 
                    CapRotationIncrement(DesiredBodyRotation.z, localAngularVelocity.z)
                );

				Body.AddRelativeTorque(actualRotation);
            }

            //Rotate cannon if input provided
            if (DesiredCannonRotation.x != 0)
            {
                RotateCannon(DesiredCannonRotation.x, true);            
            }
            if (DesiredCannonRotation.y != 0)
            {
                RotateCannon(DesiredCannonRotation.y, false);
            }

            //Fire cannon if input provided
            if (Firing)
            {
                FireCannon();
            }
		}
		else
		{
            //Fall to the ground when deaded
			Body.useGravity = true;
		}

		base.FixedUpdate();
    }
}
