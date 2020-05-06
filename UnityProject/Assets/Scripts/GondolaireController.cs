using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GondolaireController : MonoBehaviour
{
    Rigidbody rb;
    public rotateThis rotateThisInstance;
    Vector3 upForce = new Vector3(0,9.81f,0);
    float airLeakCoefficient = 0.0001f;
    public float thrustForceVertical = 0.1f;
    public float thrustForceHorizontal = 1f;
    public float torqueStrength = 1f;
    public bool ThrustingUp = false;
    public bool ThrustingDown = false;
    public float velocityRotationSpeed = 1f;
    public Animator LeftOar;
    public Animator RightOar;
    public Animator Handle;
    public Animator Flames;

    float singleStep;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position + Vector3.up*3, transform.position + Vector3.up * 3 + rb.velocity, Color.yellow);
    }

    private void FixedUpdate()
    {
        singleStep = velocityRotationSpeed * Time.deltaTime;
        airCooling();
        upThrust();
        downThrust();
        steering();
        horizontalThrust();
        rb.AddForce(upForce, ForceMode.Acceleration);
    }

    void airCooling()
    {
        if (upForce.y > 0) { upForce.y -= airLeakCoefficient; }
    }

    void downThrust()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ThrustingDown = true;
            upForce.y = 9.81f;
            rb.AddForce(Vector3.down * thrustForceVertical, ForceMode.Force);
        }
        else ThrustingDown = false;
    }

    void upThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Handle.SetBool("Ascending", true);
            Flames.SetBool("ThrustOn", true);
            upForce.y = 9.81f;
            rb.AddForce(Vector3.up * thrustForceVertical, ForceMode.Force);
        }
        else
        {
            Handle.SetBool("Ascending", false);
            Flames.SetBool("ThrustOn", false);
        }
    }

    void horizontalThrust()
    {
        // Go forward
        if (Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce(transform.forward * thrustForceHorizontal, ForceMode.Force);
            rotateThisInstance.RotateForward();
        }
        //Go backward
        if (Input.GetAxis("Vertical") < 0)
        {
            rb.AddForce(transform.forward * -thrustForceHorizontal, ForceMode.Force);
            rotateThisInstance.RotateBackward();
        }
    }

    void steering()
    {
        //Turn Right
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddTorque(Vector3.up * torqueStrength, ForceMode.Force);
            RightOar.SetBool("TurnLeft", false);
            RightOar.SetBool("TurnRight", true);
            LeftOar.SetBool("TurnLeft", false);
            LeftOar.SetBool("TurnRight", true);
        }

        //Turn Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddTorque(Vector3.up * -torqueStrength, ForceMode.Force);
            RightOar.SetBool("TurnLeft", true);
            RightOar.SetBool("TurnRight", false);
            LeftOar.SetBool("TurnLeft", true);
            LeftOar.SetBool("TurnRight", false);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            rb.velocity = Vector3.RotateTowards(rb.velocity, rb.transform.forward, singleStep, 0.0f);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            rb.velocity = Vector3.RotateTowards(rb.velocity, -rb.transform.forward, singleStep, 0.0f);
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            RightOar.SetBool("TurnLeft", false);
            RightOar.SetBool("TurnRight", false);
            LeftOar.SetBool("TurnLeft", false);
            LeftOar.SetBool("TurnRight", false);
        }
    }
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetComponent<Rigidbody>().centerOfMass, 1);
    }
}
