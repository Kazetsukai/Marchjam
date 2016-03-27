using UnityEngine;
using System.Collections;

public class PulseController : MonoBehaviour
{
	public float TimeToLive = 30f;
	private Rigidbody _rigidbody;
	private float damageModifier = 25f;

	// Use this for initialization
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		TimeToLive -= Time.fixedDeltaTime;
		if (TimeToLive < 0)
		{
			Destroy(gameObject);
		}

		var vel = _rigidbody.velocity;

		transform.LookAt(transform.position + vel);
	}

	void OnTriggerEnter(Collider other)
	{
		var enemy = other.GetComponentInParent<EnemyBase>();
		if (enemy != null)
		{
			enemy.YaGotShot(20);
		}
		Destroy(gameObject);
	}
}
