﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class addPhysics : MonoBehaviour
{
    public bool isAtTheTop = false;
    SpringJoint thisJoint;
    Rigidbody thisRB;
    public Vector3 AnchorSetup = new Vector3(0, 0, 0);

    private void OnValidate()
    {
        if (GetComponent <Collider>()== null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
        if (GetComponent<Rigidbody>() == null)
        {
            thisRB = gameObject.AddComponent<Rigidbody>();
            thisRB.angularDrag = .25f;
        }
        if (GetComponent<Rigidbody>() != null)
        {
            thisRB = GetComponent<Rigidbody>();
            thisRB.angularDrag = .25f;
        }
        if (GetComponent<SpringJoint>() == null)
        {
            thisJoint = gameObject.AddComponent<SpringJoint>();
            thisJoint.spring = 30;
            thisJoint.damper = 1;
            thisJoint.enableCollision = true;
        }
        if(GetComponent<SpringJoint>() != null)
        {
            thisJoint = GetComponent<SpringJoint>();
            thisJoint.spring = 30;
            thisJoint.damper = 1;
            //thisJoint.anchor = AnchorSetup;
            thisJoint.enableCollision = true;
        }

        if (!isAtTheTop && transform.parent != null)
        {
            if(transform.parent.GetComponent<Rigidbody>() == null)
            {
                transform.parent.gameObject.AddComponent<Rigidbody>();
            }
            thisJoint.connectedBody = transform.parent.GetComponent<Rigidbody>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + AnchorSetup, .25f);
    }
}
