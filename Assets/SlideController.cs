using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class SlideController : MonoBehaviour {

    [SerializeField] private SelectionSlider m_SelectionSlider;             // Used to activate function
    [SerializeField] private string SceneToLoad = "Phyla";                  // The scene to load default is Phyla

    [SerializeField]
    private Texture[] textureList;
    [SerializeField]
    private RawImage rawImage;
    [SerializeField]
    private bool Next;

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
            var slideIndex = GetSlideIndex() - 1;
            if (slideIndex < 0 || slideIndex > textureList.Length)
                return;
            if (Next)
                slideIndex++;
            else
                slideIndex--;
            rawImage.texture = textureList[slideIndex];
        }
        catch(IndexOutOfRangeException ex)
        {
            Debug.Log(ex.ToString());
        }
        

    }

    private int GetSlideIndex()
    {
        return Convert.ToInt32(rawImage.texture.name.Split('e')[1]);
    }
}
