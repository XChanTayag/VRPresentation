using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using System;
using UnityEngine.UI;

//[System.Serializable]
//public class SlidesModels
//{
//    public GameObject[] objects;
//}

public class ShowObjects : MonoBehaviour
{

    [SerializeField] private SelectionSlider m_SelectionSlider;             // Used to activate function
    [SerializeField] private string SceneToLoad = "Phyla";                  // The scene to load default is Phyla

    public float timer = Time.time;
    public float timeElapsed = 0;
    //public SlidesModels[] showObjects;
    private bool flag = true;

    void Start()
    {
        flag = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (flag == false)
        {
            SetLocation();
        }
    }
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

        ResetTime();
        flag = false;
        
        
    }
    private void ResetTime()
    {
        timer = Time.time;
        float timereseted = timer - timeElapsed;
        Debug.Log(timereseted.ToString());
        timeElapsed = timer;
    }
    private void SetLocation()
    {
        //float locationX = showObjects.transform.position.x;
        //float locationY = showObjects.transform.position.y + 0.05f;
        //float locationZ = showObjects.transform.position.z;
        //showObjects.transform.position = new Vector3(locationX, locationY, locationZ);
        //if (locationY > 2)
        //{
        //    flag = true;
        //}
    }
}