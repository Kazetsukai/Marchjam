using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityStandardAssets.Cameras;
using System.Collections.Generic;

public class Vehicle : MonoBehaviour
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

    State _state;
    

    bool rocketActivated;
    float rocketActiveElapsed;
    float rocketLightLerpT;

    public Rigidbody rb;
    float jumpCooldownElapsed;
    private bool _freshState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        UpdateEffects();
    }

    void FixedUpdate()
    {
        if (_freshState)
        {
            _freshState = false;
            rb.position = _state.Position;
            rb.velocity = _state.Velocity;
            rb.rotation = _state.Rotation;
            rb.angularVelocity = _state.AngularVelocity;
        }

        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        //Update rigidbody centerofmass position
        rb.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);

        var handbraking = _state.Inputs.Handbrake > 0;
        Wheel_BL.Handbraking = handbraking;
        Wheel_BR.Handbraking = handbraking;
        Wheel_FL.Handbraking = handbraking;
        Wheel_FR.Handbraking = handbraking;

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
            Debug.Log("boost");
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
                rb.AddForce(JumpForce * transform.up, ForceMode.Force);
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
        rocketActivated = _state.Inputs.RocketRear > 0 ? true : false;
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

        if (RocketLight != null)
        {
            RocketLight.intensity = (rocketActiveElapsed / RocketLightTurnOnDuration) * RocketLightIntensity;
        }
    }


    public State GetCurrentState()
    {
        return new State(_state)
        {
            Position = rb.position,
            Rotation = rb.rotation,
            Velocity = rb.velocity,
            AngularVelocity = rb.angularVelocity
        };
    }

    public void SetState(State state, bool fresh = true)
    {
        _state = state;
        _freshState = fresh;
    }

    public Inputs GetPlayerInputs()
    {
        return new Inputs()
        {
            Throttle = Input.GetAxis("Vertical"),
            Steering = Input.GetAxis("Horizontal"),
            Jump = Input.GetAxis("Jump"),
            RocketRear = Input.GetAxis("RocketRear"),
            Handbrake = Input.GetAxis("Handbrake"),
            Horizontal = Input.GetAxis("Horizontal"),
            Pitch = Input.GetAxis("Pitch")
        };
    }

    public struct State
    {
        public State(State oldState)
        {
            Inputs = oldState.Inputs;
            ServerTime = oldState.ServerTime;
            Position = oldState.Position;
            Rotation = oldState.Rotation;
            Velocity = oldState.Velocity;
            AngularVelocity = oldState.AngularVelocity;
        }

        public Inputs Inputs;
        public float ServerTime;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
    }

    public struct Inputs
    {
        public long InputNumber;
        public float ServerTime;
        public float Handbrake;
        public float Horizontal;
        public float Jump;
        public float Pitch;
        public float RocketRear;
        public float Steering;
        public float Throttle;
    }

}
