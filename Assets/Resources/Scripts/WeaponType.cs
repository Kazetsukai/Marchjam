using UnityEngine;

public class WeaponType
{
	public WeaponStyle Style;
	public float Cooldown;
	public float InitialBulletVelocity;
	public GameObject ProjectilePrefab;
}

public enum WeaponStyle
{
	Projectile,
	ProjectileStraight,
	Pulse,
	Hitscan
}