using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this script for testing new script functions
public class ScriptTester : MonoBehaviour
{
	private Transform m_Camera;

	[SerializeField] private int m_DistanceFromCamera;
	[SerializeField] private Transform testObj;

    // Use this for initialization
    void Start()
    {
		m_Camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
		Test();
    }

    void Test()
    {
        // Find the direction the camera is looking but on a flat plane.
        Vector3 targetDirection = Vector3.ProjectOnPlane(m_Camera.forward, Vector3.up).normalized;
		Debug.Log("targetDir: " + targetDirection.ToString());
		
        // Calculate a target position from the camera in the direction at the same distance from the camera as it was at Start.
        Vector3 targetPosition = m_Camera.position + targetDirection * m_DistanceFromCamera;
    }
}
