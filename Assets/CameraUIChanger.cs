using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum CameraType
{
    ThirdPerson,
    FirstPerson,
    SideView
}
public class CameraUIChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cinemachineVirtualCameras;

    private CinemachineVirtualCamera activeCinemachineVirtualCamera;
    private TMP_Dropdown dropdown;

    private void Start()
    {
        foreach (CinemachineVirtualCamera _t in cinemachineVirtualCameras)
        {
            _t.VirtualCameraGameObject.SetActive(false);
        }
        
        activeCinemachineVirtualCamera = cinemachineVirtualCameras[0];
        activeCinemachineVirtualCamera.VirtualCameraGameObject.SetActive(true);
        
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { ChangeCamera(dropdown.value); });
    }

    private void ChangeCamera(int _value)
    {
        switch (_value)
        {
            case (int)CameraType.ThirdPerson:
                SwitchCamera((int)CameraType.ThirdPerson);
                break;
            case (int)CameraType.FirstPerson:
                SwitchCamera((int)CameraType.FirstPerson);
                break;
            case (int)CameraType.SideView:
                SwitchCamera((int)CameraType.SideView);
                break;
        }
    }
    
    private void SwitchCamera( int _value)
    {
        activeCinemachineVirtualCamera.VirtualCameraGameObject.SetActive(false);
        activeCinemachineVirtualCamera = cinemachineVirtualCameras[_value];
        activeCinemachineVirtualCamera.VirtualCameraGameObject.SetActive(true);
    }

#if UNITY_EDITOR
    [Button("Find All Cameras")]
    private void FindAllCameras()
    {
        cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
    }
#endif
}
