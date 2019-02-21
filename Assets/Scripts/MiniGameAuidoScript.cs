using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class MiniGameAuidoScript : MonoBehaviour {


    public PlayAudio player;
    private VRInteractiveItem m_InteractiveItem;
    private bool m_GazeOver;                                       // Whether the user is looking at the VRInteractiveItem currently.
    [SerializeField] private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
    [SerializeField] private string m_Tag;
    [SerializeField] private AudioSource[] audio;
    private string audio_tag;
    //[SerializeField] private string pName;
    //[SerializeField] private string pInfo;
    //[SerializeField] private Texture pImg;
    private void Awake()
    {
        m_InteractiveItem = GetComponent<VRInteractiveItem>();
        //m_Renderer = GetComponent<MeshRenderer>();
        m_SelectionRadial = Camera.main.GetComponent<SelectionRadial>();
        //origMaterial = m_Renderer.material;
    }

    private void Start()
    {
        //objects = GameObject.FindGameObjectsWithTag(m_Tag);
        player.playAudio();
    }

    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
    }


    //Handle the Over event
    private void HandleOver()
    {
        //if (!UIController.Instance.isInfo)
        //{
        m_SelectionRadial.Show();
        m_GazeOver = true;

        //foreach (GameObject obj in objects)
        //{
        //var renderer = obj.GetComponent<MeshRenderer>();
        //renderer.material = outlineMaterial;
        //}
        //}
    }


    //Handle the Out event
    private void HandleOut()
    {
        m_SelectionRadial.Hide();
        m_GazeOver = false;

        //foreach (GameObject obj in objects)
        //{
        //    var renderer = obj.GetComponent<MeshRenderer>();
        //    renderer.material = origMaterial;
        //}
    }

    private void HandleSelectionComplete()
    {
        // If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
        if (m_GazeOver)
            StartCoroutine(ActivateButton());

    }
    
    

    

    private IEnumerator ActivateButton()
    {
        for (int i = 0; i < audio.Length; i++)
        {
            if(audio[i].isPlaying)
            {
                if (audio[i].tag == m_Tag)
                {
                    Debug.Log("Success");
                    player.StopAudio();
                    player.playAudio();
                    break;
                }
                else
                {
                    Debug.Log("Failed");
                    player.StopAudio();
                    player.playAudio();
                    break;
                }
            }
        }
            
        //SceneManager.LoadScene(name);
        //StartCoroutine(UIController.Instance.info(pName, pInfo, pImg));
        yield break;
        // If the camera is already fading, ignore.
        // if (m_CameraFade.IsFading)
        //    yield break;

        // If anything is subscribed to the OnButtonSelected event, call it.
        // if (OnButtonSelected != null)
        //     OnButtonSelected(this);

        // // Wait for the camera to fade out.
        // yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));

        // // Load the level.
        // SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
    }
}
