﻿using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public float TimeToLive = 30f;
	public GameObject Explosion;
	private Rigidbody _rigidbody;
	private float damageModifier = 25f;
	private float explosionDist = 2f;

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

	void OnTriggerEnter()
	{
		var explosion = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
		var hits = Physics.OverlapSphere(transform.position, explosionDist);
		print("hit " + hits.Length);
		foreach (var col in hits)
		{
			var enemy = col.GetComponentInParent<EnemyBase>();
			if (enemy != null)
			{
				var dist = (transform.position - enemy.transform.position).magnitude;
				var damage = 1/(dist + 0.5f) * damageModifier;
				enemy.YaGotShot(damage);
			}
		}
		Destroy(explosion, 5f);
		Destroy(gameObject);
	}
}
