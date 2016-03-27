using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class Vehicle : NetworkBehaviour
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

    [SyncVar] Inputs _inputs;
    [SyncVar] Vector3 _position;
    [SyncVar] Quaternion _rotation;
    [SyncVar] Vector3 _velocity;
    [SyncVar] Vector3 _angularVelocity;


    public Rigidbody rb;
    float jumpCooldownElapsed;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        SendInputsToServer();
        
        UpdatePhysics();

        SetStateForClients();
        UpdateStateFromServer();
    }

    [Server]
    private void SetStateForClients()
    {
        _position = rb.position;
        _rotation = rb.rotation;
        _velocity = rb.velocity;
        _angularVelocity = rb.angularVelocity;
    }

    [Client]
    private void UpdateStateFromServer()
    {
        rb.position = _position;
        rb.rotation = _rotation;
        rb.velocity = _velocity;
        rb.angularVelocity = _angularVelocity;
    }

    private void UpdatePhysics()
    {

        //Update rigidbody centerofmass position
        rb.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);

        //Add drive force   
        if (rb.velocity.magnitude < MaxGroundSpeed)
        {
            if (FWD && RWD)
            {
                Wheel_BL.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_BR.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_FL.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_FR.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
            }
            else if (FWD && !RWD)
            {
                Wheel_FL.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
                Wheel_FR.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
            }
            else if (!FWD && RWD)
            {
                Wheel_BL.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
                Wheel_BR.DriveWheel((_inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
            }
        }

        //Apply steering
        Wheel_FR.transform.localEulerAngles = new Vector3(Wheel_FR.transform.localEulerAngles.x, _inputs.Steering * MaxSteeringAngle * SpeedVsSteeringFactor.Evaluate(rb.velocity.magnitude), Wheel_FR.transform.localEulerAngles.z);
        Wheel_FL.transform.localEulerAngles = new Vector3(Wheel_FL.transform.localEulerAngles.x, _inputs.Steering * MaxSteeringAngle * SpeedVsSteeringFactor.Evaluate(rb.velocity.magnitude), Wheel_FL.transform.localEulerAngles.z);

        //Debug.Log(rb.velocity.magnitude);

        //Apply downforce
        if (UseDownforce)
        {
            rb.AddForce(0, -SpeedVsDownforce.Evaluate(rb.velocity.magnitude), 0);
        }

        //Apply rocket forces       
        if (_inputs.RocketRear > 0)
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
        if ((_inputs.Jump > 0) && (jumpCooldownElapsed >= JumpCooldownTime))
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
            if (_inputs.Handbrake > 0)
            {
                rb.AddRelativeTorque(0, 0, -RollTorque * _inputs.Horizontal, ForceMode.Force);
            }
            else
            {
                rb.AddRelativeTorque(0, YawTorque * _inputs.Horizontal, 0, ForceMode.Force);
            }
            rb.AddRelativeTorque(-PitchTorque * _inputs.Pitch, 0, 0, ForceMode.Force);
        }
    }

    private void SendInputsToServer()
    {
        if (isLocalPlayer)
        {
            //Get input
            CmdSetInputs(new Inputs()
            {
                Throttle = Input.GetAxis("Vertical"),
                Steering = Input.GetAxis("Horizontal"),
                Jump = Input.GetAxis("Jump"),
                RocketRear = Input.GetAxis("RocketRear"),
                Handbrake = Input.GetAxis("Handbrake"),
                Horizontal = Input.GetAxis("Horizontal"),
                Pitch = Input.GetAxis("Pitch")
            });
        }
    }

    [Command]
    private void CmdSetInputs(Inputs inputs)
    {
        _inputs = inputs;
        Debug.Log("Setting inputs!");
    }

    void Update ()
    {

	}

    private struct Inputs
    {
        public float Handbrake;
        public float Horizontal;
        public float Jump;
        public float Pitch;
        public float RocketRear;
        public float Steering;
        public float Throttle;
    }
}
