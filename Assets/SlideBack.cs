using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using System;
using UnityEngine.UI;

public class SlideBack : MonoBehaviour {

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
        try
        {
            
            slideImage.texture = imageNum[count];
            count++;
        }
        catch (Exception ex)
        {

        }
    }
}
