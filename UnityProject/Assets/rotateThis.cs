using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateThis : MonoBehaviour
{
    public float rotationSpeed = 100;

    public void RotateForward()
    {
        transform.Rotate(Vector3.right * rotationSpeed);
    }
    public void RotateBackward()
    {
        transform.Rotate(Vector3.right * -rotationSpeed);
    }
}
