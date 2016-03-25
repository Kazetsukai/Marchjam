using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
	private const float maxTurretRotation = 180f;
	private const float maxBarrelLift = 30f;
	private const float maxBarrelDrop = 15f;
	private Transform _transform;
	private Transform _car;
	private Transform _barrel;
	private float damping = 100f;
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

		var liftDir = lookAtLocalTurret - _barrel.localPosition;
		liftDir.x = 0;
		var lift = Quaternion.LookRotation(liftDir, Vector3.up);

		var clampAngle = liftDir.y > 0 ? maxBarrelLift : maxBarrelDrop;
		if (Quaternion.Angle(lift, Quaternion.identity) >= clampAngle)
			lift = Quaternion.RotateTowards(Quaternion.identity, lift, clampAngle);

		_barrel.localRotation = Quaternion.RotateTowards(_barrel.localRotation, lift, Time.deltaTime * damping);
	}
}
