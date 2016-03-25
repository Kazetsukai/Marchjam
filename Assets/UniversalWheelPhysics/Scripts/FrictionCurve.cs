using UnityEngine;
using System.Collections;

public class FrictionCurve
{

	// pacejka constants
	public float b = 0.714f;
	public float c = 1.40f;
	public float d = 1.00f;
	public float e = -0.20f;

	public float[] curve = new float[0];

	public FrictionCurve (Vector4 frictionVariables)
	{
		this.b = frictionVariables.x;
		this.c = frictionVariables.y;
		this.d = frictionVariables.z;
		this.e = frictionVariables.w;
		CreateFrictionArray ();
	}

	public float GetFriction (float slip)
	{
		float _slip = Mathf.Abs (slip);

		//if (_slip < 1f) {
		//	return staticFrictionCoef;
		//}

		if (_slip > 9) {
			return curve [999];
		} else {
			return curve [(int)(_slip * 100)];
		}
	}
	

	void CreateFrictionArray ()
	{
		curve = new float[1000];
		
		for (int i = 0; i < 1000; i++) {
			curve [i] = SetFriction (i * 0.01f);
		}
	}
	
	float SetFriction (float slip)
	{

		// Magic Pacejka Forumla :D
		float friction = d * Mathf.Sin (c * Mathf.Atan (b * (1 - e) * slip + e * Mathf.Atan (b * slip)));

		return friction;
	}
}
