using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using System;

public class SliderUI : MonoBehaviour
{
    [SerializeField] private SelectionSlider m_SelectionSlider;             // Used to activate function
    [SerializeField] private string SceneToLoad = "Phyla";                  // The scene to load default is Phyla


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
        SceneManager.LoadScene(SceneToLoad);
    }
}
