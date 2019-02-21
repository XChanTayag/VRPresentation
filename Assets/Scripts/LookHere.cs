using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class LookHere : MonoBehaviour {

    [SerializeField] private SelectionSlider m_SelectionSlider;
    [SerializeField] private float CameraRotation;
    [SerializeField] private Transform Camera;

    float smooth = 5.0f;
    float tiltAngle = 60.0f;

    bool isBarFilled = false;

    private void OnEnable()
    {
        m_SelectionSlider.OnBarFilled += HandleBarFilled;
    }


    private void OnDisable()
    {
        m_SelectionSlider.OnBarFilled -= HandleBarFilled;
    }

    private void HandleBarFilled()
    {
        isBarFilled = true;
        //if (Camera.rotation.y != CameraRotation)
            
        //else
        //    isBarFilled = false;
    }

    private void FixedUpdate()
    {
        if(isBarFilled)
        {
            Quaternion target = Quaternion.Euler(0, CameraRotation, 0);
            Camera.rotation = Quaternion.Slerp(Camera.rotation, target, Time.deltaTime * smooth);
        }

        if (Camera.rotation.y == CameraRotation)
        {
            isBarFilled = false;
        }
    }
}
