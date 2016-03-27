using UnityEngine;
using System.Collections;
using System;

public class PulseController : BulletController
{
	public float Damage = 20f;

	public override void DoDamage(Collider other)
	{
		var enemy = other.GetComponentInParent<EnemyBase>();
		if (enemy != null)
		{
			enemy.YaGotShot(Damage);
		}
	}
}
