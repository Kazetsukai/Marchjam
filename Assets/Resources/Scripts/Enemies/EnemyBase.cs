using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyBase : MonoBehaviour
{
	[Header("Enemy")]
	[SerializeField]
	public float Health = 100f;
	[SerializeField]
	public bool Dead = false;

	// Use this for initialization
	public void Start()
	{

	}

	// Update is called once per frame
	public void Update()
	{

	}

	public void FixedUpdate()
	{
		if (Health <= 0)
		{
			Dead = true;
		}
	}

	public void YaGotShot(float damage)
	{
		print("shot! " + damage);
		Health -= damage;
	}
}
