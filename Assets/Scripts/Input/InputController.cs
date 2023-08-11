using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    
    private ForkliftInputMap inputMap;
    public bool IsGamepadConnected { get; set; }

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
        return inputMap.ForkliftAction.Up.IsPressed();
    }
    
    public bool MoveForkliftDown()
    {
        return inputMap.ForkliftAction.Down.IsPressed();
    }
}
