using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class CarController : MonoBehaviour
{
    [BoxGroup("Specs(in meters)")] [SerializeField]
    private float wheelBase = 1.7f;

    [BoxGroup("Specs(in meters)")] [SerializeField]
    private float rearTread = 0.97f;

    [BoxGroup("Specs(in meters)")] [SerializeField]
    private float frontTread = 1f;

    [BoxGroup("Specs(in meters)")] [SerializeField]
    private float turnRadius = 2.5f;

    [SerializeField] private float maxForwardForce = 500f;
    [SerializeField] private bool isFrontWheelDrive = true;
    [FormerlySerializedAs("forntWheels")] [SerializeField] private Wheel[] frontWheels;
    [SerializeField] private Wheel[] rearWheels;
    [SerializeField] private GameObject steeringWheelGameObject;

    [SerializeField] private float inputSteer = 0f;
    [SerializeField] private float maxAngleOfSteeringWheel = 540f;
    [SerializeField] private float steerTimeOfSteeringWheel = 8f;

    
    private float ackermannAngleLeft = 0f;
    private float ackermannAngleRight = 0f;
    private float steerWheelAngle = 0f;

    // Update is called once per frame
    void Update()
    {
        inputSteer = InputController.Instance.GetMoveVector().x; 

        // Calculate Ackermann angles
        if (inputSteer > 0)
        {
            CalculateAckermannAngles(inputSteer, turnRadius);
        }
        else if (inputSteer < 0)
        {
            CalculateAckermannAngles(inputSteer, turnRadius);
        }
        else
        {
            ackermannAngleLeft = 0f;
            ackermannAngleRight = 0f;
        }

        // Apply Ackermann angles
        applyAckermannAngles();
    }

    private void FixedUpdate()
    {
        applyForwardForce();
    }

    private void CalculateAckermannAngles(float _inputSteer, float _turnRadius)
    {
        if (_inputSteer < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg *
                                 Mathf.Atan(wheelBase / 
                                            (_turnRadius - (isFrontWheelDrive ? rearTread : frontTread / 2))) *
                                 inputSteer;
            ackermannAngleRight = Mathf.Rad2Deg *
                                  Mathf.Atan(wheelBase /
                                             (_turnRadius + (isFrontWheelDrive ? rearTread : frontTread / 2))) *
                                  inputSteer;
        }
        else if (_inputSteer > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg *
                                 Mathf.Atan(wheelBase / 
                                            (_turnRadius + (isFrontWheelDrive ? rearTread : frontTread / 2))) *
                                 inputSteer;
            ackermannAngleRight = Mathf.Rad2Deg *
                                  Mathf.Atan(wheelBase /
                                             (_turnRadius - (isFrontWheelDrive ? rearTread : frontTread / 2))) *
                                  inputSteer;
        }
    }

    private void applyAckermannAngles()
    {
        // Apply Ackermann angles
        if (isFrontWheelDrive)
        {
            foreach (Wheel _wheel in frontWheels)
            {
                _wheel.ApplyAckermannAngle(ackermannAngleLeft, ackermannAngleRight);
            }
        }
        else
        {
            foreach (Wheel _wheel in rearWheels)
            {
                _wheel.ApplyAckermannAngle(ackermannAngleLeft, ackermannAngleRight);
            }
        }
        
        // Apply steering wheel rotation
        steerWheelAngle = Mathf.Lerp(steerWheelAngle, maxAngleOfSteeringWheel * -inputSteer, Time.deltaTime * steerTimeOfSteeringWheel);
        steeringWheelGameObject.transform.localRotation = Quaternion.Euler(0f, 0f, steerWheelAngle);
    }
    
    private void applyForwardForce()
    {
        if (isFrontWheelDrive)
        {
            foreach (Wheel _wheel in frontWheels)
            {
                _wheel.ApplyForce(maxForwardForce);
            }
        }
        else
        {
            foreach (Wheel _wheel in rearWheels)
            {
                _wheel.ApplyForce(maxForwardForce);
            }
        }
    }
}