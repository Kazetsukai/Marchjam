using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UniversalWheel))]
[CanEditMultipleObjects]
public class WheelInspector : Editor
{
	Vector4 curveVars1;
	Vector4 curveVars2;
	UniversalWheel wheel;

	public override void OnInspectorGUI ()
	{

		DrawDefaultInspector ();

		if (wheel == null) {
			wheel = (UniversalWheel)target;
		}


		if (wheel.forwardFrictionCurveArray == null || wheel.sidewayFrictionCurveArray == null) {
			UpdateFrictionCurves (true);
		}
		if (curveVars1 != wheel.forwardFrictionVars || curveVars2 != wheel.sidewaysFrictionVars) {
			UpdateFrictionCurves (false);
		}

		EditorGUILayout.HelpBox ("Friction curve variables", MessageType.Info, true);

		wheel.forwardFrictionVars = EditorGUILayout.Vector4Field ("Forward friction", wheel.forwardFrictionVars);
		EditorGUILayout.CurveField (wheel.forwardAnimCurve, GUILayout.Height (100));


		wheel.sidewaysFrictionVars = EditorGUILayout.Vector4Field ("Sideway friction", wheel.sidewaysFrictionVars);
		EditorGUILayout.CurveField (wheel.sidewaysAnimCurve, GUILayout.Height (100));

		//if (GUILayout.Button ("Update friction curves")) {
		//	wheel.UpdateFrictionCurves (false);
		//}

		if (GUILayout.Button ("Reset curves to default")) {
			wheel.UpdateFrictionCurves (true);
		}
	}

	void UpdateFrictionCurves (bool defaults)
	{
		wheel.UpdateFrictionCurves (defaults);
		curveVars1 = wheel.forwardFrictionVars;
		curveVars2 = wheel.sidewaysFrictionVars;
	}
	

}
