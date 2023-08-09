using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    
    private ForkliftInputMap inputMap;
    
    private void Awake()
    {
        Instance = this;
        inputMap = new ForkliftInputMap();
        inputMap.Enable();

        inputMap.Movement.MovementDirection.performed += _ => GetMoveVector();
        inputMap.ForkliftAction.Up.performed += _ => MoveForkliftUp();
        inputMap.ForkliftAction.Down.performed += _ => MoveForkliftDown();
    }
    
    public Vector2 GetMoveVector()
    {
        return inputMap.Movement.MovementDirection.ReadValue<Vector2>();
    }
    
    public bool MoveForkliftUp()
    {
        return inputMap.ForkliftAction.Up.triggered;
    }
    
    public bool MoveForkliftDown()
    {
        return inputMap.ForkliftAction.Down.triggered;
    }
}
