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
    [SerializeField] bool FlightModeEngaged = false;
    [SerializeField] GameObject FlightModeIndicator;
    [SerializeField] float AngDrag_FlightMode = 2f;
    [SerializeField] float AngDrag_Grounded = 0.1f;
    [SerializeField] float FMI_GrowRate = 25f;
    [SerializeField] float FMI_ON_Width = 4.27f;            //Its basically just a wing that scales wide and comes out sides of car when flight mode is engaged
    [SerializeField] float FMI_OFF_Width = 0f;
    float FMI_CurrentWidth;

    [Header("Wheels")]
    [SerializeField] VehicleWheel Wheel_FR;
    [SerializeField] VehicleWheel Wheel_FL;
    [SerializeField] VehicleWheel Wheel_BR;
    [SerializeField] VehicleWheel Wheel_BL;

    [Header("Rockets")]
    [SerializeField] float RearRocketForce = 20f;
    [SerializeField] ParticleSystem RearRocketParticles;
    [SerializeField] Light RocketLight;
    [SerializeField] float RocketLightIntensity;
    [SerializeField] float RocketLightTurnOnDuration = 0.2f;

    State _state;
    State _previousState;

    //Rocket engine effects
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

        //Store last input state
        _previousState = _state;
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
        int wheelsOnGround = GetCountOfWheelsOnGround();

        //Apply jump        
        jumpCooldownElapsed += Time.deltaTime;
        if (_state.Inputs.Jump > 0 && _previousState.Inputs.Jump <= 0)
        {
            // if at least 3/4 wheels are touching ground, allow player to press jump button and apply jump force
            if (wheelsOnGround >= 3 && jumpCooldownElapsed >= JumpCooldownTime)
            {                
                rb.AddForce(JumpForce * transform.up, ForceMode.Force);
                jumpCooldownElapsed = 0;
            }

            //If player is not on the ground (i.e, they have jumped once already, or are falling), engage flight mode when they press jump button
            else if (wheelsOnGround == 0)
            {
                FlightModeEngaged = true;                               
            }
        }

        //Check if player has landed since last frame and do necessary things 
        if (wheelsOnGround >= 4)
        {
            //End flight mode when car is fully landed
            FlightModeEngaged = false;
        }

        //Apply air movement if player has pressed space once to jump, and then another to engage flight mode
        if (FlightModeEngaged)
        {
            rb.angularDrag = AngDrag_FlightMode;

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
        else
        {
            rb.angularDrag = AngDrag_Grounded;
        }
    }

    public int GetCountOfWheelsOnGround()
    {
        int wheelsOnGround = 0;
        if (Wheel_FR.Grounded) { wheelsOnGround++; }
        if (Wheel_FL.Grounded) { wheelsOnGround++; }
        if (Wheel_BR.Grounded) { wheelsOnGround++; }
        if (Wheel_BL.Grounded) { wheelsOnGround++; }
        return wheelsOnGround;
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

        //Make wing grow out of car sides when flight mode is engaged
        if (FlightModeEngaged)
        {
            FMI_CurrentWidth = Mathf.Clamp(FMI_CurrentWidth + FMI_GrowRate * Time.deltaTime, FMI_OFF_Width, FMI_ON_Width);            
        }
        else
        {
            FMI_CurrentWidth = Mathf.Clamp(FMI_CurrentWidth - FMI_GrowRate * Time.deltaTime, FMI_OFF_Width, FMI_ON_Width);
        }
        FlightModeIndicator.transform.localScale = new Vector3(FMI_CurrentWidth, FlightModeIndicator.transform.localScale.y, FlightModeIndicator.transform.localScale.z);
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
