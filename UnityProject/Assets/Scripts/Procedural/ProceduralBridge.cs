using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

// Using Sebastian League tool, need to put the credits here

namespace PathCreation.Examples
{
	[ExecuteInEditMode]
	public class ProceduralBridge : PathSceneTool
	{   
		[SerializeField]
		public GameObject[] prefabArray;
		[SerializeField]
		public GameObject planksHolder;
		[SerializeField]
		public GameObject anchorsHolder;		
		public float spacing = 3;
		const float minSpacing = .1f;
		public float anchorSpacing = 1.0f;
		public float anchorOffset = 1.0f;
		[SerializeField]
		public GameObject anchorPrefab;
		private GameObject[] _planksArray; 

		private bool CheckPrefabArray()
		{
			if (prefabArray == null) return false;
			else
			{
				foreach (GameObject prefab in prefabArray)
				{
					if (prefab == null) return false;
				}
			}
			return true;
		}

		void Generate () 
		{
			if (pathCreator != null && CheckPrefabArray() != false && planksHolder != null) 
			{
				DestroyObjects ();

				VertexPath path = pathCreator.path;

				spacing = Mathf.Max(minSpacing, spacing);
				float dst = 0;

				while (dst < path.length) {
					Vector3 point = path.GetPointAtDistance (dst);
					Quaternion rot = path.GetRotationAtDistance (dst);
					Instantiate (prefabArray[Random.Range(0,prefabArray.Length)], point, rot, planksHolder.transform);
					dst += spacing;
				}

				/*if (anchorPrefab != null && anchorsHolder != null)
				{
					AddAnchor(0.0f, anchorSpacing , -anchorOffset);
					// TODO : Looking at the source code, if the distance is 0 or Path.Lenght that will
					// yield the same thing, because of the EndOfPathInstruction.Loop
					// some weird shit, but it is what it is, i could implement an overload to pass a .stop
					// but maybe other day
					AddAnchor(path.length-0.01f, anchorSpacing, anchorOffset);
				}	*/

				SetHingesJoints();
			}

		}


		void SetSpringJoints ()
		{
			int numChildrenPlanks = planksHolder.transform.childCount;
			for (int i = numChildrenPlanks - 1; i >= 0; i--)
			{
				SpringJoint[] springs = planksHolder.transform.GetChild(i).gameObject.GetComponents<SpringJoint>();

				//if (i == 0) Debug.Log("first plank pos:" + planksHolder.transform.GetChild(i).gameObject.transform.position);

				for (int j = 0; j < springs.Length -1; j++)
				{	
					if (i !=  0)
					{
						springs[j].connectedBody = planksHolder.transform.GetChild(i-1).gameObject.GetComponent<Rigidbody>();
					}
					
				}	
			}
		}


		void SetHingesJoints ()
		{
			int numChildrenPlanks = planksHolder.transform.childCount;
			for (int i = numChildrenPlanks - 1; i >= 0; i--)
			{	
				HingeJoint hinge = planksHolder.transform.GetChild(i).gameObject.GetComponent<HingeJoint>();
				if (i == 0) Debug.Log("first plank pos:" + planksHolder.transform.GetChild(i).gameObject.transform.position);
				if (i > 0)
				{
					hinge.connectedBody = planksHolder.transform.GetChild(i-1).gameObject.GetComponent<Rigidbody>();
				}	
			}
			//Add the last hinge
			HingeJoint lastHinge = planksHolder.transform.GetChild(numChildrenPlanks - 1).gameObject.AddComponent<HingeJoint>();
			lastHinge.anchor = new Vector3(0.0f, 0.0f, 0.5f);

		}

		void DestroyObjects () 
		{
			// Planks
			int numChildrenPlanks = planksHolder.transform.childCount;
			for (int i = numChildrenPlanks - 1; i >= 0; i--) 
			{
				DestroyImmediate (planksHolder.transform.GetChild (i).gameObject, false);
			}
			// Anchors
			int numChildrenAnchors = anchorsHolder.transform.childCount;
			for (int i = numChildrenAnchors - 1; i >= 0; i--) 
			{
				DestroyImmediate (anchorsHolder.transform.GetChild (i).gameObject, false);
			}
			
		}

		protected override void PathUpdated () 
		{
			if (pathCreator != null)
			{
				Generate ();
			}
		}

		void AddAnchor(float dst, float spacing ,float offset)
		{
			// Probably I should just check this later in the loop that creates all the anchors
			// not here, redundant
			if (anchorPrefab != null)
			{
				Vector3 point = path.GetPointAtDistance (dst);
				Quaternion rot = path.GetRotationAtDistance (dst);
				// The one to the right
				Vector3 rigthVector = rot*Vector3.right;
				Vector3 forwardVector = rot*Vector3.forward;
				// How apart from the center of the spline
				Vector3 displacedVectorA = (spacing*rigthVector)+point;
				displacedVectorA = displacedVectorA + (offset*forwardVector);
				Vector3 displacedVectorB = (-1.0f*spacing*rigthVector)+point;
				displacedVectorB = displacedVectorB + (offset*forwardVector);
				//TODO : It would be nice to be able to control the rotation of the anchors
				Instantiate(anchorPrefab, displacedVectorA, rot, anchorsHolder.transform);
				Instantiate(anchorPrefab, displacedVectorB, rot, anchorsHolder.transform);
			}
		}

	}		

}
