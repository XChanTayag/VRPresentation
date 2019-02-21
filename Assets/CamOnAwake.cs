using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOnAwake : MonoBehaviour {
    [SerializeField] private Transform camTrans;
	// Use this for initialization
	void Start () {
        camTrans.rotation = new Quaternion(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
