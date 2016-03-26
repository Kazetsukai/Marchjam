using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
	public GameObject projectilePrefab;

	private const float maxTurretRotation = 180f;
	private const float maxBarrelLift = 45f;
	private const float maxBarrelDrop = 15f;
	private Transform _transform;
	private Transform _car;
	private Transform _barrel;
	private Transform _bulletStartPoint;
	private float damping = 100f;
	private float initialBulletForce = 40f;
	private float cooldown = 0f;

	// Use this for initialization
	void Start()
	{
		_transform = GetComponent<Transform>();
		_car = _transform.parent;
		_barrel = _transform.FindChild("barrel");
		_bulletStartPoint = _barrel.FindChild("bullet_start");
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
		var hit = Physics.Raycast(worldMousePos, out hitInfo);

		var targetPos = hit ? hitInfo.point : worldMousePos.GetPoint(100000);

		var lookDir = targetPos - _barrel.position;

		Debug.DrawLine(_barrel.position, _barrel.position + lookDir, Color.red);

		var lookY = lookDir.y;
		lookDir.y = 0;
		var lookNorm = lookDir.normalized;

		var g = 9.81f; // gravity
		var v = 40; // velocity
		var x = lookDir.magnitude; // target x
		var y = lookY; // target y
		var s = (v * v * v * v) - g * (g * (x * x) + 2 * y * (v * v)); //substitution
		var o1 = Mathf.Atan(((v * v) + Mathf.Sqrt(s)) / (g * x)) * Mathf.Rad2Deg; // high launch angle
		var o2 = Mathf.Atan(((v * v) - Mathf.Sqrt(s)) / (g * x)) * Mathf.Rad2Deg; // low launch angle

		Debug.DrawRay(_barrel.position, Quaternion.AngleAxis(o2, Vector3.Cross(lookNorm, Vector3.up).normalized) * lookNorm * 5, Color.green);
		Debug.DrawRay(_barrel.position, Quaternion.AngleAxis(o1, Vector3.Cross(lookNorm, Vector3.up).normalized) * lookNorm * 5, Color.cyan);
		Debug.DrawLine(_barrel.position, _barrel.position + lookDir, Color.yellow);
		Debug.DrawLine(_barrel.position + lookDir, _barrel.position + lookDir + new Vector3(0, lookY, 0), Color.magenta);


		Vector3 firingDir = targetPos;

		if (float.IsNaN(o2))
		{
			//print("no firing solution");
		}
		else
		{
			var firingAngle = Quaternion.AngleAxis(o2, Vector3.Cross(lookNorm, Vector3.up).normalized);
			firingDir = firingAngle * lookNorm;
		}

		var localFiringDir = _car.InverseTransformVector(firingDir);

		var yawDir = localFiringDir;
		yawDir.y = 0;

		var rotation = Quaternion.LookRotation(yawDir.normalized, Vector3.up);

		//if (Quaternion.Angle(rotation, Quaternion.identity) >= maxTurretRotation)
		//	rotation = Quaternion.RotateTowards(Quaternion.identity, rotation, maxTurretRotation);

		transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotation, Time.deltaTime * damping);

		var lookAtLocalTurret = _transform.InverseTransformVector(firingDir);

		var liftDir = lookAtLocalTurret;
		liftDir.x = 0;
		var lift = Quaternion.LookRotation(liftDir.normalized, Vector3.up);

		var clampAngle = liftDir.y > 0 ? maxBarrelLift : maxBarrelDrop;
		if (Quaternion.Angle(lift, Quaternion.identity) >= clampAngle)
			lift = Quaternion.RotateTowards(Quaternion.identity, lift, clampAngle);

		//print(Quaternion.Angle(lift, Quaternion.identity));
		_barrel.localRotation = Quaternion.RotateTowards(_barrel.localRotation, lift, Time.deltaTime * damping);
		
		var mousedown = Input.GetAxis("Fire1");

		if (mousedown > 0f && cooldown <= 0f)
		{
			var newBullet = GameObject.Instantiate(projectilePrefab);
			newBullet.transform.position = _bulletStartPoint.position;
			newBullet.transform.rotation = _bulletStartPoint.rotation;
			newBullet.GetComponent<Rigidbody>().AddForce(_bulletStartPoint.forward * initialBulletForce, ForceMode.Impulse);
			print("bang!");

			cooldown = 1f;
		}

		cooldown -= Time.fixedDeltaTime;
	}
}
