using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float DriveForce = 100f;
    [SerializeField] float MaxSteeringAngle = 35f;
    [SerializeField] AnimationCurve SpeedVsDownforce;
    [SerializeField] bool UseDownforce = true;

    [Header("Wheels")]
    [SerializeField] VehicleWheel Wheel_FR;
    [SerializeField] VehicleWheel Wheel_FL;
    [SerializeField] VehicleWheel Wheel_BR;
    [SerializeField] VehicleWheel Wheel_BL;

    public Rigidbody rb;

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
        Wheel_BL.DriveWheel((throttle * DriveForce * Time.deltaTime) / 2f);
        Wheel_BR.DriveWheel((throttle * DriveForce * Time.deltaTime) / 2f);

        //Rotate wheels
        Wheel_FR.transform.localEulerAngles = new Vector3(Wheel_FR.transform.localEulerAngles.x, steering * MaxSteeringAngle, Wheel_FR.transform.localEulerAngles.z);
        Wheel_FL.transform.localEulerAngles = new Vector3(Wheel_FL.transform.localEulerAngles.x, steering * MaxSteeringAngle, Wheel_FL.transform.localEulerAngles.z);

        //Apply downforce
        if (UseDownforce)
        {
            rb.AddForce(0, -SpeedVsDownforce.Evaluate(rb.velocity.magnitude), 0);
        }
    }

    void Update ()
    {
      


	}
}
