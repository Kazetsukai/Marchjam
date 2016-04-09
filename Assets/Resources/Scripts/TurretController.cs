using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class TurretController : MonoBehaviour
{
	public List<WeaponType> AvailableTypes = new List<WeaponType>();
	public int CurrentWeaponIndex;
	private WeaponType CurrentWeapon { get { return AvailableTypes.ElementAtOrDefault(CurrentWeaponIndex); } }
	private bool SwitchingWeapon;
	
	public Rigidbody ParentVehicle;
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
	float cooldown = 0f;
	public Vector3 cursorPosition;

	// Use this for initialization
	void Start()
	{
		AvailableTypes.Add(new WeaponType
		{
			ProjectilePrefab = projectilePrefab,
			InitialBulletVelocity = 40f,
			Style = WeaponStyle.Projectile,
			Cooldown = 1f
		});

		AvailableTypes.Add(new WeaponType
		{
			ProjectilePrefab = projectilePrefab,
			InitialBulletVelocity = 100f,
			Style = WeaponStyle.ProjectileStraight,
			Cooldown = 1f
		});

		AvailableTypes.Add(new WeaponType
		{
			ProjectilePrefab = pulsePrefab,
			InitialBulletVelocity = 100f,
			Style = WeaponStyle.Pulse,
			Cooldown = 0.1f
		});

		CurrentWeaponIndex = 0;

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
		switch (CurrentWeapon.Style)
		{
			case WeaponStyle.Projectile:
				// add parent vehicle velocity?
				firingDirection = CalculateProjectileFiringSolution(lookDir, CurrentWeapon.InitialBulletVelocity);
				break;
			case WeaponStyle.ProjectileStraight:
			case WeaponStyle.Pulse:
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
				CurrentWeaponIndex = (CurrentWeaponIndex + 1) % AvailableTypes.Count;
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
		
		_barrel.localRotation = Quaternion.RotateTowards(_barrel.localRotation, lift, Time.deltaTime * damping);
	}

	private Vector3 GetTargetPosition()
	{
		var mousePos = Input.mousePosition;
		var mouseWorldRay = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit mouseRayHitInfo;
		var hit = Physics.Raycast(mouseWorldRay, out mouseRayHitInfo, 250f, ~(1 << LayerMask.NameToLayer("Bullets")));
		Debug.DrawLine(mouseWorldRay.origin, mouseWorldRay.origin+mouseWorldRay.direction * mouseRayHitInfo.distance);
		return hit ? mouseRayHitInfo.point : mouseWorldRay.GetPoint(100000);
	}

	private void HandleFiring()
	{
		var mousedown = Input.GetAxis("Fire1");

		if (mousedown > 0f && cooldown <= 0f)
		{
			GameObject newBullet = GameObject.Instantiate(CurrentWeapon.ProjectilePrefab);
			newBullet.SetLayerRecursively(LayerMask.NameToLayer("Bullets"));
			newBullet.transform.position = _bulletStartPoint.position;
			newBullet.transform.rotation = _bulletStartPoint.rotation;
			newBullet.GetComponent<Rigidbody>().AddForce((_bulletStartPoint.forward * CurrentWeapon.InitialBulletVelocity) + ParentVehicle.velocity, ForceMode.VelocityChange);
			cooldown = CurrentWeapon.Cooldown;
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
		Gizmos.color = Color.red;

		if (CurrentWeapon.Style == WeaponStyle.Projectile || CurrentWeapon.Style == WeaponStyle.ProjectileStraight)
		{
			Vector3 position = _bulletStartPoint.position;
			Vector3 velocity = _barrel.forward * CurrentWeapon.InitialBulletVelocity + ParentVehicle.velocity;
			for (int i = 0; i < numSteps; ++i)
			{
				var length = timeDelta * CurrentWeapon.InitialBulletVelocity;
				Gizmos.DrawLine(position, position + velocity.normalized * length);

				RaycastHit hitInfo;
				if (Physics.Raycast(position, velocity.normalized, out hitInfo, length))
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
			if (Physics.Raycast(_barrel.position, _barrel.forward, out hitInfo, 250f, ~(1 << LayerMask.NameToLayer("Bullets"))))
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