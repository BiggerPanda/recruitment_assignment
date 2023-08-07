using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    [SerializeField] private ArticulationBody m_ArticulationBody;
    public float m_Thrust = 20f;
    private float applyForce = 0f;
    
    private float massOfItem = 0f;
    private MassController massController;

    private void Start()
    {
        massController = m_ArticulationBody.GetComponent<MassController>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            //Apply a force to this ArticulationBody in the direction of this GameObject's up-axis
            applyForce = m_Thrust;
        }

        if (Input.GetButton("Fire1"))
        {
            //Apply a force to this ArticulationBody in the direction of this GameObject's down-axis
            applyForce = -m_Thrust;
        }
    }

    void FixedUpdate()
    {
        //Apply the force to the ArticulationBody
        m_ArticulationBody.AddRelativeForce(Vector3.up * applyForce);
    }
}
