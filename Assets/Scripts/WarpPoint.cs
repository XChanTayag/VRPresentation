using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class WarpPoint : MonoBehaviour 
{
    [SerializeField] private Transform GUI;                                 // Referenced to teleport with the camera
    [SerializeField] private float playerHeight = 15f;                      // The players height in respect to 0
    [SerializeField] private Transform MainCamera;

    public void Teleport()
    {
        Transform warpObj = this.transform;
        Vector3 position = new Vector3(warpObj.position.x, warpObj.position.y + playerHeight, warpObj.position.z);
        // Teleports the camera and gui to this warp point
        MainCamera.position = position;
        GUI.position = new Vector3(warpObj.position.x, warpObj.position.y, warpObj.position.z);
    }
}
