using UnityEngine;
using System.Collections;

public class VehicleWheel : MonoBehaviour
{
    [SerializeField] Vehicle ParentVehicle;
    [SerializeField] float Radius= 1f;
    [SerializeField] float GroundedThreshold = 0.1f;
    public bool Grounded = false;
    [SerializeField] bool DebugRay = false;
    
    [SerializeField] float FrictionLateral_Drift = 0.1f;
    [SerializeField] AnimationCurve LatForceVsLatFricFactor;        //Defines how much the wheel starts to lose friction as more lateral force is applied
   
    RaycastHit hit;
        
    void Start()
    {

    }
    
    void Update()
    {
        //Raycast down to see if wheel is touching something
        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            Grounded = (hit.distance <= (Radius / 2f) + GroundedThreshold) ? true : false;

            //Do lateral friction
            if (Grounded)
            {                
                Vector3 force_lateral = Vector3.Dot(ParentVehicle.rb.GetPointVelocity(transform.position), transform.up) * transform.up;
                float latFrictionFactor = LatForceVsLatFricFactor.Evaluate(force_lateral.magnitude);

                if (Input.GetAxis("Handbrake") > 0)
                {
                    latFrictionFactor = FrictionLateral_Drift;
                }
                ParentVehicle.rb.AddForceAtPosition(-force_lateral * latFrictionFactor * 0.25f, transform.position, ForceMode.VelocityChange);          //Change 0.25f when changing weight transfer

                if (DebugRay)
                {
                    Debug.DrawLine(transform.position, transform.position - force_lateral, Color.blue);
                }
            }

            if (DebugRay)
            {              
                if (Grounded)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                }  
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
            }
        }
    }

    /// <summary>
    /// Used to drive wheel. Positive force is forward rotation, negative is reverse.
    /// </summary>
    /// <param name="force"></param>
    public void DriveWheel(float force)
    {
        if (Grounded)
        {
            ParentVehicle.rb.AddForceAtPosition(transform.forward * force, transform.position, ForceMode.Force);
        }
    }
}
