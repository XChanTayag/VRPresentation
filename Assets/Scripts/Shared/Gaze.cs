using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;

// This script handles encapsulation of gaze functions
public class Gaze : MonoBehaviour
{
    private SelectionRadial selectionRadial;                                // This controls when the selection is complete
    private GazeManager gazeManager;                                        // Handles actual functions    
    private VRInteractiveItem interactiveItem;                              // Actual class that makes objects interactive
    private bool gazeOver;                                                  // Whether the user is looking at the VRInteractiveItem currently.

    public bool GazeOver { set { gazeOver = value; } }                      // Public property for gazeOver

    private void Awake()
    {
        // Set default references
        gazeManager = GazeManager.Instance;
        interactiveItem = this.GetComponent<VRInteractiveItem>();
        selectionRadial = Camera.main.GetComponent<SelectionRadial>();
    }

    private void OnEnable()
    {
        // Subscribe to public events
        interactiveItem.OnOver += HandleOver;
        interactiveItem.OnOut += HandleOut;
        selectionRadial.OnSelectionComplete += HandleSelectionComplete;
    }


    private void OnDisable()
    {
        // Unsubscribe to public events
        interactiveItem.OnOver -= HandleOver;
        interactiveItem.OnOut -= HandleOut;
        selectionRadial.OnSelectionComplete -= HandleSelectionComplete;
    }

    //Handle the Over event
    private void HandleOver()
    {
        // TODO: Fix this to validate if the game object has a Move script
        // Get Move Script from the gazed game object then set the isMoving to FALSE;
        // if(this.GetComponent<Move>() != null)
        // {
        //     this.GetComponent<Move>().isMoving = false;
        // }
        // Pass function to GazeManager
        gazeManager.HandleOver(this.gameObject);
    }


    //Handle the Out event
    private void HandleOut()
    {
        // TODO: Fix this to validate if the game object has a Move script
        // Get Move Script from the gazed game object then set the isMoving to TRUE;
        // if (this.GetComponent<Move>() != null)
        // {
        //     this.GetComponent<Move>().isMoving = true;
        // }
        // Pass function to GazeManager
        gazeManager.HandleOut(this.gameObject);
    }

    private void HandleSelectionComplete()
    {
        // If the user is looking at the rendering of the scene when 
        // the radial's selection finishes, activate the button.
        if (gazeOver)
            StartCoroutine(GazeManager.Instance.ActivateButton(this.gameObject));
        HandleOut();

    }
}
