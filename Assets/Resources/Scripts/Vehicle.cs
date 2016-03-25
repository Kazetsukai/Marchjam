using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float ForwardForce = 100f;
    [SerializeField] float TurnForce = 1000f;                //MUKLTIPLT BNY SPLIT LATER
    [SerializeField] float MaxSteeringAngle = 35f;
    [SerializeField] AnimationCurve CarSpeedVsCheapSteeringForce;

    [Header("Wheels")]
    [SerializeField] GameObject Wheel_FR;
    [SerializeField] GameObject Wheel_FL;
    [SerializeField] GameObject Wheel_BR;
    [SerializeField] GameObject Wheel_BL;
    [SerializeField] AnimationCurve SpeedVsWheelLatForce;

    Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        //Get input
        float throttle = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");

        //Add rear drive force
        rb.AddForceAtPosition(Wheel_BL.transform.forward * (throttle * ForwardForce * Time.deltaTime) / 2f, Wheel_BL.transform.position);
        rb.AddForceAtPosition(Wheel_BR.transform.forward * (throttle * ForwardForce * Time.deltaTime) / 2f, Wheel_BR.transform.position);

        //Rotate wheels
        Wheel_FR.transform.localEulerAngles = new Vector3(Wheel_FR.transform.localEulerAngles.x, steering * MaxSteeringAngle, Wheel_FR.transform.localEulerAngles.z);
        Wheel_FL.transform.localEulerAngles = new Vector3(Wheel_FL.transform.localEulerAngles.x, steering * MaxSteeringAngle, Wheel_FL.transform.localEulerAngles.z);
        
        // Lateral friction
        var FRfarce = Vector3.Dot(rb.GetPointVelocity(Wheel_FR.transform.position), Wheel_FR.transform.up) * Wheel_FR.transform.up;
        var FLfarce = Vector3.Dot(rb.GetPointVelocity(Wheel_FL.transform.position), Wheel_FL.transform.up) * Wheel_FL.transform.up;
        var BRfarce = Vector3.Dot(rb.GetPointVelocity(Wheel_BR.transform.position), Wheel_BR.transform.up) * Wheel_BR.transform.up;
        var BLfarce = Vector3.Dot(rb.GetPointVelocity(Wheel_BL.transform.position), Wheel_BL.transform.up) * Wheel_BL.transform.up;
        
        rb.AddForceAtPosition(-FRfarce * 0.25f, Wheel_FR.transform.position, ForceMode.VelocityChange);
        rb.AddForceAtPosition(-FLfarce * 0.25f, Wheel_FL.transform.position, ForceMode.VelocityChange);
        rb.AddForceAtPosition(-BRfarce * 0.25f, Wheel_BR.transform.position, ForceMode.VelocityChange);
        rb.AddForceAtPosition(-BLfarce * 0.25f, Wheel_BL.transform.position, ForceMode.VelocityChange);                

        Debug.DrawLine(Wheel_FR.transform.position, Wheel_FR.transform.position - FRfarce * 20);
        Debug.DrawLine(Wheel_FL.transform.position, Wheel_FL.transform.position - FLfarce * 20);
        Debug.DrawLine(Wheel_BR.transform.position, Wheel_BR.transform.position - BRfarce * 20);
        Debug.DrawLine(Wheel_BL.transform.position, Wheel_BL.transform.position - BLfarce * 20);
    }
    
    void Update ()
    {
	
	}
}
