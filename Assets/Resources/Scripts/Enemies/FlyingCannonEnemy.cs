using UnityEngine;
using System.Collections;
using System.Linq;

public class FlyingCannonEnemy : MonoBehaviour {

    private Rigidbody _rigidBody;
    private Transform _cannon;

    [Header("Movement")]
    [SerializeField]
    float MaximumSpeed = 50f;

    float MaxCannonAngle = 30f;
    float CannonAimRate = 0.5f;

    public Vector3 DesiredMovement
    {
        get;
        set;
    }    

	// Use this for initialization
	void Start () {
        _rigidBody = GetComponentInChildren<Rigidbody>();
        _cannon = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "CannonParent");
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        /*DesiredMovement = new Vector3
        (
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            0
        );*/

        float? newAngle = null;
        if (Input.GetAxis("Vertical") > 0)
        {
            newAngle = CannonAimRate;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            newAngle = -CannonAimRate;
        }

        if (newAngle.HasValue)
        {
            float currentAngle = _cannon.rotation.eulerAngles.x;
            if (currentAngle > 180)
            {
                currentAngle = currentAngle - 360;
            }
            if (Mathf.Abs(currentAngle + newAngle.Value) <= MaxCannonAngle)
            {
                _cannon.Rotate(new Vector3(newAngle.Value, 0));
            }
        }

        //_rigidBody.AddForce(DesiredMovement * 10);
    }
}
