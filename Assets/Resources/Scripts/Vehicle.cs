using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityStandardAssets.Cameras;
using System.Collections.Generic;

public class Vehicle : NetworkBehaviour
{
    [Header("Ground Movement")]
    [SerializeField]
    float DriveForce = 100f;
    [SerializeField]
    float MaxGroundSpeed = 40f;
    [SerializeField]
    float MaxSteeringAngle = 35f;
    [SerializeField]
    AnimationCurve SpeedVsSteeringFactor;          //Defines how much steering ability is decreased as speed increases
    [SerializeField]
    AnimationCurve SpeedVsDownforce;
    [SerializeField]
    bool UseDownforce = true;
    [SerializeField]
    bool RWD = true;
    [SerializeField]
    bool FWD = true;
    [SerializeField]
    Transform CenterOfMass;

    [Header("Air Movement")]
    [SerializeField]
    float JumpForce = 200f;
    [SerializeField]
    float JumpCooldownTime = 0.5f;
    [SerializeField]
    float PitchTorque = 10f;
    [SerializeField]
    float YawTorque = 10f;
    [SerializeField]
    float RollTorque = 10f;

    [Header("Wheels")]
    [SerializeField]
    VehicleWheel Wheel_FR;
    [SerializeField]
    VehicleWheel Wheel_FL;
    [SerializeField]
    VehicleWheel Wheel_BR;
    [SerializeField]
    VehicleWheel Wheel_BL;

    [Header("Rockets")]
    [SerializeField]
    float RearRocketForce = 20f;
    [SerializeField]
    ParticleSystem RearRocketParticles;
    [SerializeField]
    Light RocketLight;
    [SerializeField]
    float RocketLightIntensity;
    [SerializeField]
    float RocketLightTurnOnDuration = 0.2f;

    [SyncVar(hook = "UpdateStateFromServer")]
    State _state;

    Queue<State> _predictedStates = new Queue<State>();

    bool rocketActivated;
    float rocketActiveElapsed;
    float rocketLightLerpT;

    public Rigidbody rb;
    float jumpCooldownElapsed;
    private int _nextInputNumber;
    private Vector3 _diffP;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (isLocalPlayer)
        {
            FindObjectOfType<AutoCam>().SetTarget(gameObject.transform);
        }
    }

    void Awake()
    {
        rb.Sleep();
    }

    void Update()
    {

        //UpdateEffects();
    }

    void FixedUpdate()
    {
        SendInputsToServer();

        if (isServer) SetStateForClients(GetCurrentState());

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
        //_state = state;
    }

    private void UpdateStateFromServer(State newState)
    {
        if (isLocalPlayer)
            Debug.Log("Update from server - " + newState.Inputs.InputNumber);

        if (!isServer)
        {
            // Throw out stale states
            while (_predictedStates.Any() && _predictedStates.Peek().Inputs.InputNumber < newState.Inputs.InputNumber)
                _predictedStates.Dequeue();

            // Calculate predicted current state based on authoritative server state and predicted state
            if (_predictedStates.Any())
            {
                var first = _predictedStates.Peek();
                var last = _predictedStates.Last();
                var diffPosition = newState.Position - first.Position;
                var diffRotation = first.Rotation.RotationBetween(newState.Rotation);
                var diffVelocity = newState.Velocity - first.Velocity;
                var diffAngularVelocity = newState.AngularVelocity - first.AngularVelocity;

                //if (diffPosition.magnitude < 2) diffPosition = Vector3.zero;

                //Debug.Log(newState.Position + " " + first.Position + "   -  " + (newState.Position - first.Position));
                //Debug.Log(newState.AngularVelocity + " " + first.AngularVelocity + "   -  " + (diffAngularVelocity));

                rb.position += diffPosition;
                rb.rotation = diffRotation * rb.rotation;
                rb.velocity += diffVelocity;
                rb.angularVelocity += diffAngularVelocity;
            }
        }
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

    void UpdateEffects()
    {
        RearRocketParticles.enableEmission = rocketActivated;

        //Update rocket light (UGLY CODE. will improve later)
        if (RocketLight != null && rocketActivated)
        {
            rocketActiveElapsed = Mathf.Clamp(rocketActiveElapsed + Time.deltaTime, 0, RocketLightTurnOnDuration);
        }
        else
        {
            rocketActiveElapsed = Mathf.Clamp(rocketActiveElapsed - Time.deltaTime, 0, RocketLightTurnOnDuration);
        }

        Debug.Log(rocketActiveElapsed);
        RocketLight.intensity = (rocketActiveElapsed / RocketLightTurnOnDuration) * RocketLightIntensity;
    }

    private void SendInputsToServer()
    {
        if (isLocalPlayer)
        {
            var input = new Inputs()
            {
                InputNumber = _nextInputNumber++,
                Throttle = Input.GetAxis("Vertical"),
                Steering = Input.GetAxis("Horizontal"),
                Jump = Input.GetAxis("Jump"),
                RocketRear = Input.GetAxis("RocketRear"),
                Handbrake = Input.GetAxis("Handbrake"),
                Horizontal = Input.GetAxis("Horizontal"),
                Pitch = Input.GetAxis("Pitch")
            };

            // Locally apply for client prediction
            _state.Inputs = input;
            _state = GetCurrentState();
            _predictedStates.Enqueue(_state);

            // Send to server
            Debug.Log("Sending to server - " + input.InputNumber);
            CmdSetInputs(input);
        }
    }

    [Command]
    private void CmdSetInputs(Inputs inputs)
    {
        _state = new State(GetCurrentState()) { Inputs = inputs };
    }


    struct State
    {
        public State(State oldState)
        {
            Inputs = oldState.Inputs;
            Position = oldState.Position;
            Rotation = oldState.Rotation;
            Velocity = oldState.Velocity;
            AngularVelocity = oldState.AngularVelocity;
        }
        
        public Inputs Inputs;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
    }

    struct Inputs
    {
        public long InputNumber;
        public float Handbrake;
        public float Horizontal;
        public float Jump;
        public float Pitch;
        public float RocketRear;
        public float Steering;
        public float Throttle;
    }
}
