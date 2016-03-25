using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{
    [Header("Ground Movement")]
    [SerializeField] float DriveForce = 100f;
    [SerializeField] float MaxGroundSpeed = 40f;
    [SerializeField] float MaxSteeringAngle = 35f;
    [SerializeField] AnimationCurve SpeedVsSteeringFactor;          //Defines how much steering ability is decreased as speed increases
    [SerializeField] AnimationCurve SpeedVsDownforce;
    [SerializeField] bool UseDownforce = true;
    [SerializeField] bool RWD = true;
    [SerializeField] bool FWD = true;
    [SerializeField] Transform CenterOfMass;

    [Header("Air Movement")]    
    [SerializeField] float JumpForce = 200f;
    [SerializeField] float JumpCooldownTime = 0.5f;
    [SerializeField] float PitchTorque = 10f;
    [SerializeField] float YawTorque = 10f;
    [SerializeField] float RollTorque = 10f;

    [Header("Wheels")]
    [SerializeField] VehicleWheel Wheel_FR;
    [SerializeField] VehicleWheel Wheel_FL;
    [SerializeField] VehicleWheel Wheel_BR;
    [SerializeField] VehicleWheel Wheel_BL;

    [Header("Rockets")]
    [SerializeField] float RearRocketForce = 20f;
    [SerializeField] ParticleSystem RearRocketParticles;

    public Rigidbody rb;
    float jumpCooldownElapsed;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        //Update rigidbody centerofmass position
        rb.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);

        //Get input
        float throttle = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");

        //Add drive force   
        if (rb.velocity.magnitude < MaxGroundSpeed)
        {
            if (FWD && RWD)
            {
                Wheel_BL.DriveWheel((throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_BR.DriveWheel((throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_FL.DriveWheel((throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_FR.DriveWheel((throttle * DriveForce * Time.deltaTime) / 4f);
            }
            else if (FWD && !RWD)
            {
                Wheel_FL.DriveWheel((throttle * DriveForce * Time.deltaTime) / 2f);
                Wheel_FR.DriveWheel((throttle * DriveForce * Time.deltaTime) / 2f);
            }
            else if (!FWD && RWD)
            {
                Wheel_BL.DriveWheel((throttle * DriveForce * Time.deltaTime) / 2f);
                Wheel_BR.DriveWheel((throttle * DriveForce * Time.deltaTime) / 2f);
            }
        }

        //Apply steering
        Wheel_FR.transform.localEulerAngles = new Vector3(Wheel_FR.transform.localEulerAngles.x, steering * MaxSteeringAngle * SpeedVsSteeringFactor.Evaluate(rb.velocity.magnitude), Wheel_FR.transform.localEulerAngles.z);
        Wheel_FL.transform.localEulerAngles = new Vector3(Wheel_FL.transform.localEulerAngles.x, steering * MaxSteeringAngle * SpeedVsSteeringFactor.Evaluate(rb.velocity.magnitude), Wheel_FL.transform.localEulerAngles.z);

        //Debug.Log(rb.velocity.magnitude);

        //Apply downforce
        if (UseDownforce)
        {
            rb.AddForce(0, -SpeedVsDownforce.Evaluate(rb.velocity.magnitude), 0);
        }

        //Apply rocket forces       
        if (Input.GetAxis("RocketRear") > 0)
        {
            rb.AddForce(transform.forward * RearRocketForce, ForceMode.Force);
            RearRocketParticles.enableEmission = true;
        }
        else
        {
            RearRocketParticles.enableEmission = false;
        }

     
        //Count wheels on ground
        int wheelsOnGround = 0;
        if (Wheel_FR.Grounded) { wheelsOnGround++; }
        if (Wheel_FL.Grounded) { wheelsOnGround++; }
        if (Wheel_BR.Grounded) { wheelsOnGround++; }
        if (Wheel_BL.Grounded) { wheelsOnGround++; }

        //Apply jump        
        jumpCooldownElapsed += Time.deltaTime;
        if ((Input.GetAxis("Jump") > 0) && (jumpCooldownElapsed >= JumpCooldownTime))
        {
            // if at least 3/4 wheels are touching ground
            if (wheelsOnGround >= 3)
            {
                //Apply jump force
                rb.AddForce(0, JumpForce, 0, ForceMode.Force);
                jumpCooldownElapsed = 0;                
            }
        }

        //Apply air movement
        if (wheelsOnGround <= 2)
        {
            if (Input.GetAxis("Handbrake") > 0)
            {
                rb.AddRelativeTorque(0, 0, -RollTorque * Input.GetAxis("Horizontal"), ForceMode.Force);
            }
            else
            {
                rb.AddRelativeTorque(0, YawTorque * Input.GetAxis("Horizontal"), 0, ForceMode.Force);
            }             
            rb.AddRelativeTorque(-PitchTorque * Input.GetAxis("Pitch"), 0, 0, ForceMode.Force);
        }
    }

    void Update ()
    {
      


	}
}
