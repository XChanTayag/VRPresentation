using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using System;
using UnityEngine.UI;

[System.Serializable]
public class SlidesModels
{
    public GameObject[] objects;
    public int[] timeShow;
}

public class SlideLoad : MonoBehaviour
{

    [SerializeField] private SelectionSlider m_SelectionSlider;             // Used to activate function
    [SerializeField] private string SceneToLoad = "Phyla";                  // The scene to load default is Phyla
    public Texture2D[] imageNum = new Texture2D[5];
    public AudioSource audiosource;
    public AudioClip[] audioSlides;

    public RawImage slideImage;
    public int count;
    public Text textyes;


    public int numberOfObjects;

    public float timereseted;
    public float timer = Time.time;
    public float timeElapsed = 0;
    public int sec;
    private float temp;


    public SlidesModels[] listShowtime;

    private bool flag = true;

    void Start()
    {
        timer = Time.time;
        flag = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeUpdate();
        SetLocation();
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
        int a;
        int b;
        try
        {
            a = listShowtime.Length;
            b = listShowtime[count].timeShow.Length;
        }
        catch (Exception ex)
        {
            a = listShowtime.Length;
            b = 11111;
        }
        Debug.Log(a + " " + b);
        SlideDetector();
        ResetTime();
        flag = false;
    }
    private void SlideDetector()
    {
        Debug.Log(textyes.text);
        Debug.Log(count.ToString());

        while (true)
        {
            if (slideImage.texture == imageNum[count])
            {
                Debug.Log("found");
                HideModels();
                break;
            }
            count++;
        }

        try
        {
            if (textyes.text == "Next")
            {
                if (count != 29)
                {
                    count++;
                    slideImage.texture = imageNum[count];
                }
            }
            else if (textyes.text == "Back")
            {
                if (count != 0)
                {
                    count--;
                    slideImage.texture = imageNum[count];
                }

            }
            PlayAudio();
        }
        catch (Exception ex)
        {
            
        }
        Debug.Log(count.ToString());
    }

    private void ResetTime()
    {
        timer = Time.time;
        timereseted = timer - timeElapsed;
        Debug.Log(timereseted.ToString());
        timeElapsed = timer;
        sec = 0;
    }
    private void TimeUpdate()
    {
        timer = Time.time;
        float seconds = timer % 60;
        if ((int)temp < (int)seconds)
        {
            sec++;
            temp = seconds;
        }
    }
    private void PlayAudio()
    {
        audiosource.clip = audioSlides[count];
        audiosource.Play();

    }
    private void SetLocation()
    {
        try
        {
            int a = listShowtime.Length;
            int b = listShowtime[count].timeShow.Length;

            for (int i = 0; i < b; i++)
            {
                if (listShowtime[count].timeShow[i] <= sec)
                {
                    float locationX = listShowtime[count].objects[i].transform.position.x;
                    float locationY = listShowtime[count].objects[i].transform.position.y;
                    float locationZ = listShowtime[count].objects[i].transform.position.z;
                    if (locationY < 2)
                    {
                        locationY = listShowtime[count].objects[i].transform.position.y + 0.05f;
                        listShowtime[count].objects[i].transform.position = new Vector3(locationX, locationY, locationZ);

                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    private void HideModels()
    {
        try
        {
            int a = listShowtime.Length;
            int b = listShowtime[count].timeShow.Length;

            for (int i = 0; i < b; i++)
            {
                float locationX = listShowtime[count].objects[i].transform.position.x;
                float locationZ = listShowtime[count].objects[i].transform.position.z;
                listShowtime[count].objects[i].transform.position = new Vector3(locationX, -5f, locationZ);
            }
        }
        catch (Exception ex)
        {

        }
    }
}