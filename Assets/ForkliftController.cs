using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftController : MonoBehaviour
{
    
    [SerializeField] private ArticulationBody connectedForkBody;
    [SerializeField] private float maxWeight = 3000f;
    [SerializeField] Transform middleOfFork;
    
    private float maxForce = 20f;
    private float applyForce = 0f;
    
    private float massOfItem = 0f;
    private MassController massController;
    private ArticulationDrive yDrive;
    private Transform item;
    private Rigidbody itemRigidbody;

    private void Start()
    {
        massController = connectedForkBody.GetComponent<MassController>();
        
        yDrive = connectedForkBody.yDrive;
        maxForce = maxWeight * -Physics.gravity.y;
        yDrive.forceLimit = maxForce;
        connectedForkBody.yDrive = yDrive;
        applyForce = -maxForce;
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            if(Physics.Raycast(middleOfFork.position,Vector3.up,out RaycastHit hit, 1f))
            { 
                if(item != null && itemRigidbody != null)
                {
                    return;
                }
                
                item = hit.transform;
                itemRigidbody = item.GetComponent<Rigidbody>();
                item.SetParent(middleOfFork);

            }
            
            applyForce = maxForce;
        }

        if (Input.GetButton("Fire1"))
        {
            if (item != null)
            {

                item.SetParent(null);
                item = null;
                itemRigidbody = null;
            }
            
            applyForce = -maxForce;
        }
    }

    void FixedUpdate()
    {
        //Apply the force to the ArticulationBody
        connectedForkBody.AddForce(Vector3.up * applyForce,ForceMode.Force);
    }
}
