using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public float TimeToLive = 30f;
	public GameObject Explosion;
	private Rigidbody _rigidbody;

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
		Destroy(gameObject);
		var explosion = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
		Destroy(explosion, 5f);
	}
}
