using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForkliftUIController", menuName = "ScriptableObjects/UI/ForkliftUIController")]
public class ForkliftUIController : ScriptableObject
{
    private ForkliftDataModel forkliftDataModel;
    
    private void Awake()
    {
        forkliftDataModel = new ForkliftDataModel();
    }
    
    public float ForkPosition
    {
        get => forkliftDataModel.forkPosition;
        private set => forkliftDataModel.forkPosition = value;
    }
    
    public float HorizontalInput
    {
        get => forkliftDataModel.horizontalInput;
        private set => forkliftDataModel.horizontalInput = value;
    }
    
    public float VerticalInput
    {
        get => forkliftDataModel.verticalInput;
        private set => forkliftDataModel.verticalInput = value;
    }
    
    public float Speed
    {
        get => forkliftDataModel.speed;
        private set => forkliftDataModel.speed = value;
    }
    
    public bool IsObjectOnFork
    {
        get => forkliftDataModel.isObjectOnFork;
        private set => forkliftDataModel.isObjectOnFork = value;
    }

    public void UpdateInput(float _horizontalInput, float _verticalInput)
    {
        HorizontalInput = _horizontalInput;
        VerticalInput = _verticalInput;
    }
    
    public void UpdateSpeed(float _speed)
    {
        Speed = _speed;
    }
    
    public void UpdateForkliftData(float _forkPosition, bool _isObjectOnFork)
    {
        ForkPosition = _forkPosition;
        IsObjectOnFork = _isObjectOnFork;
    }
}


