using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Catenary))]
public class CatenayEditor : Editor
{
	GameObject ropeStart;
	GameObject ropeEnd;
	public Color thisRopeGizmos;
	private void OnEnable()
	{
		thisRopeGizmos = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), .5f);
	}
	
	public override void OnInspectorGUI()
	{
		
		Catenary catenary = (Catenary)target;


		DrawDefaultInspector();
		if (catenary.lockEditing == false)
		{

			if (GUI.changed)
			{
				catenary.Regenerate();
			}
			if (GUILayout.Button("Create RopeEnds") && catenary.p1 == null && catenary.p2 == null)
			{
				CreateRopeStart(catenary);
				CreateRopeEnd(catenary);
			}
			if (GUILayout.Button("Destroy Rope Ends"))
			{
				DestroyImmediate(catenary.p1.gameObject);
				catenary.p1 = null;
				DestroyImmediate(catenary.p2.gameObject);
				catenary.p2 = null;
			}
			if (GUILayout.Button("Unassign previous ends"))
			{
				catenary.p1 = null;
				catenary.p2 = null;
			}
		}
	}

	public void CreateRopeStart(Catenary catenary)
	{
		ropeStart = Instantiate(catenary.RopeStart);
		ropeStart.name = catenary.gameObject.name + " Start ";
		catenary.p1 = ropeStart.transform;
		RopeGizmo temp = catenary.p1.GetComponent<RopeGizmo>();
		temp.thisCatenary = catenary;
		temp.gizmoColor = thisRopeGizmos;
		Selection.activeGameObject = ropeStart;
	}

	public void CreateRopeEnd(Catenary catenary)
	{
		ropeEnd = Instantiate(catenary.RopeEnd);
		ropeEnd.name = catenary.gameObject.name +" End ";
		catenary.p2 = ropeEnd.transform;
		RopeGizmo temp = catenary.p2.GetComponent<RopeGizmo>();
		temp.thisCatenary = catenary;
		temp.gizmoColor = thisRopeGizmos;
		Vector3 tempPosition = ropeEnd.transform.position;
		tempPosition.x += 10;
		ropeEnd.transform.position = tempPosition;
		Selection.activeGameObject = ropeEnd;
	}
}
