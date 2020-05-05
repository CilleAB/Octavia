using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RopeGizmo))]
public class RopeGizmoEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		RopeGizmo ropeGizmo = (RopeGizmo)target;

		if (GUILayout.Button("REGENERATE ROPE"))
		{
			ropeGizmo.thisCatenary.Regenerate();
		}
	}
}
