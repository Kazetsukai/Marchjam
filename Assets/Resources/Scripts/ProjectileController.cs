using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public float TimeToLive = 30f;
	public GameObject Explosion;

	// Use this for initialization
	void Start()
	{

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
	}

	void OnTriggerEnter()
	{
		Destroy(gameObject);
		var explosion = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
		Destroy(explosion, 5f);
	}
}
