using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [BoxGroup("Suspension")] [SerializeField]
    private float restDistance = 0.5f;

    [BoxGroup("Suspension")] [SerializeField]
    private float springTravel = 0.2f;

    [BoxGroup("Suspension")] [SerializeField]
    private float springStiffness = 120000f;

    [BoxGroup("Suspension")] [SerializeField]
    private float springDamping = 10000f;

    [BoxGroup("Wheel")] [SerializeField] private float wheelRadius = 1f;
    [BoxGroup("Wheel")] [SerializeField] private bool isLeftWheel = true;
    [BoxGroup("Wheel")] [SerializeField] private float steerTime = 10f;

    private float steerAngle = 0f;
    private float wheelAngle = 0f;

    //Cached values
    private float minLength = 0f;
    private float maxLength = 0f;
    private float lastLength = 0f;
    private float sprigVelocity = 0f;
    private float springLength = 0f;
    private float springForce = 0f;
    private float damperForce = 0f;
    private float fowardForce = 0f;
    private float sidewaysForce = 0f;

    private ArticulationBody mainBody;

    private Vector3 springForceVector = Vector3.zero;
    private Vector3 wheelVelocity = Vector3.zero;

    private void Start()
    {
        mainBody = transform.root.root.GetComponent<ArticulationBody>();

        minLength = restDistance - springTravel;
        maxLength = restDistance + springTravel;
    }

    private void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, Time.deltaTime * steerTime);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x,
                                                 transform.localRotation.y + wheelAngle,
                                                   transform.localRotation.z);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, -transform.up * (maxLength + wheelRadius), Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            sprigVelocity = (lastLength - springLength) / Time.fixedDeltaTime;

            springForce = (restDistance - springLength) * springStiffness;
            damperForce = sprigVelocity * springDamping;

            springForceVector = transform.up * (springForce + damperForce);

            wheelVelocity = transform.InverseTransformDirection(mainBody.GetPointVelocity(hit.point));
            
            
            mainBody.AddForceAtPosition(springForceVector + (fowardForce * transform.forward) + (sidewaysForce*-transform.right), hit.point);
        }
    }

    public void ApplyAckermannAngle(float _ackermannAngleLeft, float _ackermannAngleRight)
    {
        if (isLeftWheel)
        {
            steerAngle = _ackermannAngleLeft;
        }
        else
        {
            steerAngle = _ackermannAngleRight;
        }
    }
    
    public void ApplyForce(float _force)
    {
        fowardForce = Input.GetAxis("Vertical") * _force ;
        sidewaysForce = wheelVelocity.x * _force;
    }
}