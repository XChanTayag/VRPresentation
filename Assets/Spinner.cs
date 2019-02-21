using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {
    public float speed = 10f;


    void Update()
    {
        //this.transform.rotation = new Quaternion(0, speed * Time.deltaTime, 0, 0);
        transform.Rotate(Vector3.back, speed * Time.deltaTime);
    }
}
