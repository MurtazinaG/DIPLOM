using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook[] virtualCameras;
    private int currentCameraIndex;

    private void Start()
    {
        virtualCameras[currentCameraIndex].Priority = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // При нажатии G меняем цели следования и обзора камеры
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        virtualCameras[currentCameraIndex].Priority = 0;
        currentCameraIndex++;

        if(currentCameraIndex>= virtualCameras.Length)
            currentCameraIndex = 0;
        virtualCameras[currentCameraIndex].Priority = 1;
    }
}