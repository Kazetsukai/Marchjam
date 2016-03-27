using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityStandardAssets.Cameras;

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

    [SyncVar(hook ="UpdateStateFromServer")] State _state;

    public Rigidbody rb;
    float jumpCooldownElapsed;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();

        if (isLocalPlayer)
        {
            FindObjectOfType<AutoCam>().SetTarget(gameObject.transform);
        }
	}

    void Update()
    {
        SendInputsToServer();

        var state = GetCurrentState();

        if (isServer) SetStateForClients(state);
    }

    void FixedUpdate()
    {
        
        UpdatePhysics();

    }

    private State GetCurrentState()
    {
        return new State(_state)
        {
            Position = rb.position,
            Rotation = rb.rotation,
            Velocity = rb.velocity,
            AngularVelocity = rb.angularVelocity
        };
    }

    [Server]
    private void SetStateForClients(State state)
    {
        // Set the _state which will go to each client
        _state = state;
    }
    
    private void UpdateStateFromServer(State newState)
    {
            // Store the newly received state.
            _state = newState;
        if (!isServer)
        {
            rb.MovePosition(_state.Position);
            rb.MoveRotation(_state.Rotation);
            rb.velocity = _state.Velocity;
        }
            //rb.angularVelocity = _state.AngularVelocity;
            // Apply it locally but only if it is different enough from prediction.
            //if (rb.position.IsDifferentEnoughTo(_state.Position)) rb.position = _state.Position;
            //if (rb.rotation.IsDifferentEnoughTo(_state.Rotation)) rb.rotation = _state.Rotation;
            //if (rb.velocity.IsDifferentEnoughTo(_state.Velocity)) rb.velocity = _state.Velocity;
            //if (rb.angularVelocity.IsDifferentEnoughTo(_state.AngularVelocity)) rb.angularVelocity = _state.AngularVelocity;
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
                Wheel_BL.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_BR.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_FL.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
                Wheel_FR.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 4f);
            }
            else if (FWD && !RWD)
            {
                Wheel_FL.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
                Wheel_FR.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
            }
            else if (!FWD && RWD)
            {
                Wheel_BL.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
                Wheel_BR.DriveWheel((_state.Inputs.Throttle * DriveForce * Time.deltaTime) / 2f);
            }
        }

        //Apply steering
        Wheel_FR.transform.localEulerAngles = new Vector3(Wheel_FR.transform.localEulerAngles.x, _state.Inputs.Steering * MaxSteeringAngle * SpeedVsSteeringFactor.Evaluate(rb.velocity.magnitude), Wheel_FR.transform.localEulerAngles.z);
        Wheel_FL.transform.localEulerAngles = new Vector3(Wheel_FL.transform.localEulerAngles.x, _state.Inputs.Steering * MaxSteeringAngle * SpeedVsSteeringFactor.Evaluate(rb.velocity.magnitude), Wheel_FL.transform.localEulerAngles.z);

        //Debug.Log(rb.velocity.magnitude);

        //Apply downforce
        if (UseDownforce)
        {
            rb.AddForce(0, -SpeedVsDownforce.Evaluate(rb.velocity.magnitude), 0);
        }

        //Apply rocket forces       
        if (_state.Inputs.RocketRear > 0)
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
        if ((_state.Inputs.Jump > 0) && (jumpCooldownElapsed >= JumpCooldownTime))
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
            if (_state.Inputs.Handbrake > 0)
            {
                rb.AddRelativeTorque(0, 0, -RollTorque * _state.Inputs.Horizontal, ForceMode.Force);
            }
            else
            {
                rb.AddRelativeTorque(0, YawTorque * _state.Inputs.Horizontal, 0, ForceMode.Force);
            }
            rb.AddRelativeTorque(-PitchTorque * _state.Inputs.Pitch, 0, 0, ForceMode.Force);
        }
    }

    private void SendInputsToServer()
    {
        if (isLocalPlayer)
        {
            var input = new Inputs()
            {
                Throttle = Input.GetAxis("Vertical"),
                Steering = Input.GetAxis("Horizontal"),
                Jump = Input.GetAxis("Jump"),
                RocketRear = Input.GetAxis("RocketRear"),
                Handbrake = Input.GetAxis("Handbrake"),
                Horizontal = Input.GetAxis("Horizontal"),
                Pitch = Input.GetAxis("Pitch")
            };
            
            // Send to server
            CmdSetInputs(input);
        }
    }

    [Command]
    private void CmdSetInputs(Inputs inputs)
    {
        _state = new State(_state) { Inputs = inputs };
    }


    struct State
    {
        public State(State oldState)
        {
            TimeStamp = Time.unscaledTime;
            Inputs = oldState.Inputs;
            Position = oldState.Position;
            Rotation = oldState.Rotation;
            Velocity = oldState.Velocity;
            AngularVelocity = oldState.Velocity;
        }

        public float TimeStamp;
        public Inputs Inputs;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
    }

    struct Inputs
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
