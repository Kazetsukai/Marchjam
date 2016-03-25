using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
	public GameObject projectilePrefab;

	private const float maxTurretRotation = 180f;
	private const float maxBarrelLift = 30f;
	private const float maxBarrelDrop = 15f;
	private Transform _transform;
	private Transform _car;
	private Transform _barrel;
	private float damping = 100f;
	private float initialBulletForce = 40f;
	private float cooldown = 0f;

	// Use this for initialization
	void Start()
	{
		_transform = GetComponent<Transform>();
		_car = _transform.parent;
		_barrel = _transform.FindChild("barrel");
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		var mousePos = Input.mousePosition;
		var worldMousePos = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hitInfo;
		Physics.Raycast(worldMousePos, out hitInfo);

		var lookAt = hitInfo.point;

		var lookAtLocalCar = _car.InverseTransformPoint(lookAt);

		var lookDir = lookAtLocalCar - transform.localPosition;
		lookDir.y = 0;
		var rotation = Quaternion.LookRotation(lookDir, Vector3.up);
		if (Quaternion.Angle(rotation, Quaternion.identity) >= maxTurretRotation)
			rotation = Quaternion.RotateTowards(Quaternion.identity, rotation, maxTurretRotation);
		transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotation, Time.deltaTime * damping);

		var lookAtLocalTurret = _transform.InverseTransformPoint(lookAt);

		var rawr = lookAtLocalTurret - _barrel.localPosition;

		var g = 9.81f; // gravity
		var v = 40; // velocity
		var x = lookDir.magnitude; // target x
		var y = rawr.y; // target y
		var s = (v * v * v * v) - g * (g * (x * x) + 2 * y * (v * v)); //substitution
		//var o1 = Mathf.Atan(((v * v) + Mathf.Sqrt(s)) / (g * x)) * Mathf.Rad2Deg; // launch angle
		var o2 = Mathf.Atan(((v * v) - Mathf.Sqrt(s)) / (g * x)) * Mathf.Rad2Deg; // launch angle

		if (float.IsNaN(o2))
		{
			print("no firing solution");
		}
		else
		{
			var lift = Quaternion.Euler(-o2, 0, 0);

			_barrel.localRotation = Quaternion.RotateTowards(_barrel.localRotation, lift, Time.deltaTime * damping);
		}

		var mousedown = Input.GetAxis("Fire1");

		if (mousedown > 0f && cooldown <= 0f)
		{
			var newBullet = GameObject.Instantiate(projectilePrefab);
			newBullet.transform.position = _barrel.position;
			newBullet.transform.rotation = _barrel.rotation;
			newBullet.GetComponent<Rigidbody>().AddForce(_barrel.forward * initialBulletForce, ForceMode.Impulse);
			print("bang!");

			cooldown = 1f;
		}

		cooldown -= Time.fixedDeltaTime;
	}
}
