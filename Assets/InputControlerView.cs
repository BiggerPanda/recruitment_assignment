using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputControlerView : MonoBehaviour
{
    [SerializeField] private GameObject keyboardControls1;
    [SerializeField] private GameObject keyboardControls2;
    [SerializeField] private GameObject gamepadControls;

    private InputController inputController;

    private void Start()
    {
        inputController = InputController.Instance;
        PlayerInput.all[0].onControlsChanged += onInputChange;
        changeInputView(PlayerInput.all[0].currentControlScheme);
    }

    private void onInputChange(PlayerInput _playerInput)
    {
        Debug.Log("User changed");
        Debug.Log(_playerInput.currentControlScheme);
        changeInputView(_playerInput.currentControlScheme);
    }


    private void changeInputView(string _controlScheme)
    {
        switch (_controlScheme)
        {
            case "Xbox":
                keyboardControls1.SetActive(false);
                keyboardControls2.SetActive(false);
                gamepadControls.SetActive(true);
                break;
            case "Keyboard":
                keyboardControls1.SetActive(true);
                keyboardControls2.SetActive(true);
                gamepadControls.SetActive(false);
                break;
            default:
                break;
        }
    }
}