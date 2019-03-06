using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using System;
using UnityEngine.UI;

public class SlideLoad : MonoBehaviour
{

    [SerializeField] private SelectionSlider m_SelectionSlider;             // Used to activate function
    [SerializeField] private string SceneToLoad = "Phyla";                  // The scene to load default is Phyla
    public Texture2D[] imageNum = new Texture2D[5];
    public RawImage slideImage;
    public int count;
    public Text textyes;
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
        Debug.Log(textyes.text);

        while (true)
        {
            if (slideImage.texture == imageNum[count])
            {
                Debug.Log("found");
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
        }
        catch (Exception ex)
        {
            count--;
            slideImage.texture = imageNum[count];
        }
        Debug.Log(count.ToString());
    }
}