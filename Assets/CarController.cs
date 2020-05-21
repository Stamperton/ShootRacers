using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{

    //Variables
    #region Serialised

    [SerializeField] private float f_driveSpeed = 20;
    [SerializeField] private float f_turnSpeed = 15;
    [SerializeField] private float f_drag = 0.98f;
    [SerializeField] private float f_maximumLinearVelocity = 25;
    [SerializeField] private float f_maximumAngularVelocity = 3;

    #endregion

    #region Private

    private float f_xIn;
    private float f_zIn;
    private Rigidbody rb;

    #endregion

    //Methods
    #region Unity Standards

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        f_xIn = Input.GetAxisRaw("Horizontal") * f_turnSpeed;
        f_zIn = Input.GetAxisRaw("Vertical") * f_driveSpeed;
    }

    private void FixedUpdate()
    {
        Drive();
        Turn();
        Clamp();
    }

    #endregion

    #region Private Voids

    private void Clamp()
    {
        //Velocity Clamps
        float y = rb.velocity.y;
        rb.velocity = Vector3.Scale(rb.velocity, Vector3.one - Vector3.up);
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, -f_maximumLinearVelocity, f_maximumLinearVelocity);
        //Slow down if you're not driving
        if (f_zIn == 0)
            rb.velocity *= f_drag;

        rb.velocity += Vector3.up * y;

        //Angular Clamps
        rb.angularVelocity = rb.angularVelocity.normalized * Mathf.Clamp(rb.angularVelocity.magnitude, -f_maximumAngularVelocity, f_maximumAngularVelocity);


    }

    private void Drive()
    {
        rb.AddForce(Vector3.ProjectOnPlane(transform.forward, GetFloorPlaneNormal()) * f_zIn);
    }

    private void Turn()
    {
        rb.AddTorque(transform.up * f_xIn);
    }

    #endregion

    #region Public Voids


    #endregion

    #region Private Returns

    private Vector3 GetFloorPlaneNormal()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3))
            return hit.normal;
        else
            return Vector3.up;
    }

    #endregion

    #region Public Returns


    #endregion

}
