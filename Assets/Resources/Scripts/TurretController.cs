using UnityEngine;
using System.Collections;
using System;

public class TurretController : MonoBehaviour
{
	public enum WeaponType
	{
		Cannon,
		Pulse
	}

	public GameObject projectilePrefab;
	public GameObject pulsePrefab;

	[SerializeField]
	float maxTurretRotation = 180f;
	[SerializeField]
	float maxBarrelLift = 45f;
	[SerializeField]
	float maxBarrelDrop = 15f;
	private Transform _transform;
	private Transform _car;
	private Transform _barrel;
	private Transform _bulletStartPoint;
	[SerializeField]
	float damping = 100f;
	[SerializeField]
	float initialBulletVelocity = 40f;
	[SerializeField]
	float cooldown = 0f;
	public Vector3 cursorPosition;
	public WeaponType CurrentWeaponType;
	private bool SwitchingWeapon;

	// Use this for initialization
	void Start()
	{
		CurrentWeaponType = WeaponType.Cannon;

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
		//weapons switch
		WeaponSwitching();

		var targetPos = GetTargetPosition();

		var lookDir = targetPos - _barrel.position;

		Vector3 firingDirection;
		switch (CurrentWeaponType)
		{
			case WeaponType.Cannon:
				firingDirection = CalculateProjectileFiringSolution(lookDir, initialBulletVelocity);
				break;
			case WeaponType.Pulse:
			default:
				firingDirection = lookDir;
				break;
		}

		RotateBarrelTo(firingDirection);

		HandleFiring();
	}

	private void WeaponSwitching()
	{
		if (Input.GetAxis("WeaponSwitch") > 0)
		{
			if (!SwitchingWeapon)
			{
				CurrentWeaponType = (WeaponType)((int)(CurrentWeaponType + 1) % Enum.GetValues(typeof(WeaponType)).Length);
				SwitchingWeapon = true;
			}
		}
		else
		{
			SwitchingWeapon = false;
		}
	}

	private Vector3 CalculateProjectileFiringSolution(Vector3 lookDir, float initialVelocity)
	{
		var lookY = lookDir.y;
		lookDir.y = 0;
		var lookNorm = lookDir.normalized;

		var g = 9.81f; // gravity
		var v = initialVelocity; // velocity
		var x = lookDir.magnitude; // target x
		var y = lookY; // target y
		var s = (v * v * v * v) - g * (g * (x * x) + 2 * y * (v * v)); //substitution
		var o1 = Mathf.Atan(((v * v) + Mathf.Sqrt(s)) / (g * x)) * Mathf.Rad2Deg; // high launch angle
		var o2 = Mathf.Atan(((v * v) - Mathf.Sqrt(s)) / (g * x)) * Mathf.Rad2Deg; // low launch angle

		Vector3 firingDir = lookDir;

		if (float.IsNaN(o2))
		{
			//print("no firing solution");
		}
		else
		{
			var firingAngle = Quaternion.AngleAxis(o2, Vector3.Cross(lookNorm, Vector3.up).normalized);
			firingDir = firingAngle * lookNorm;
		}

		return firingDir;
	}

	private void RotateBarrelTo(Vector3 firingDir)
	{
		var localFiringDir = _car.InverseTransformVector(firingDir);

		var yawDir = localFiringDir;
		yawDir.y = 0;

		var rotation = Quaternion.LookRotation(yawDir.normalized, Vector3.up);

		if (Quaternion.Angle(rotation, Quaternion.identity) >= maxTurretRotation)
			rotation = Quaternion.RotateTowards(Quaternion.identity, rotation, maxTurretRotation);

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
	}

	private Vector3 GetTargetPosition()
	{
		var mousePos = Input.mousePosition;
		var mouseWorldRay = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit mouseRayHitInfo;
		var hit = Physics.Raycast(mouseWorldRay, out mouseRayHitInfo);

		return hit ? mouseRayHitInfo.point : mouseWorldRay.GetPoint(100000);
	}

	private void HandleFiring()
	{
		var mousedown = Input.GetAxis("Fire1");

		if (mousedown > 0f && cooldown <= 0f)
		{
			GameObject newBullet;
			switch (CurrentWeaponType)
			{
				case WeaponType.Cannon:
					{
						newBullet = GameObject.Instantiate(projectilePrefab);
						cooldown = 1f;
					}
					break;
				case WeaponType.Pulse:
					{
						newBullet = GameObject.Instantiate(pulsePrefab);
						cooldown = 0.1f;
					}
					break;
				default:
					Debug.LogError("No/Invalid Weapon Selected");
					return;
			}
			newBullet.transform.position = _bulletStartPoint.position;
			newBullet.transform.rotation = _bulletStartPoint.rotation;
			newBullet.GetComponent<Rigidbody>().AddForce(_bulletStartPoint.forward * initialBulletVelocity, ForceMode.VelocityChange);

		}

		if (cooldown > 0)
		{
			cooldown -= Time.fixedDeltaTime;
		}
	}

	void OnDrawGizmos()
	{
		//show current aim

		int numSteps = 100;
		float timeDelta = 1.0f / 20f;
		Vector3 gravity = new Vector3(0, -9.8f, 0);

		if (CurrentWeaponType == WeaponType.Cannon)
		{
			Vector3 position = _bulletStartPoint.position;
			Vector3 velocity = _barrel.forward * initialBulletVelocity;
			for (int i = 0; i < numSteps; ++i)
			{
				Debug.DrawLine(position, position + velocity.normalized * 2, Color.red);

				RaycastHit hitInfo;
				if (Physics.Raycast(position, velocity.normalized, out hitInfo, 2))
				{
					cursorPosition = hitInfo.point;
				}

				position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
				velocity += (gravity * timeDelta);
			}
		}
		else
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(_barrel.position, _barrel.forward, out hitInfo, 250))
			{
				Gizmos.DrawLine(_barrel.position, hitInfo.point);
			}
			else
			{
				Gizmos.DrawLine(_barrel.position, _barrel.position + _barrel.forward * 250);
			}
		}
	}
}
