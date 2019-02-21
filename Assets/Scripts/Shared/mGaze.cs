using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;

// This script handles encapsulation of gaze functions
public class mGaze : MonoBehaviour
{
    private VRInteractiveItem m_InteractiveItem;
    private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.
    private Material origMaterial;
    private GameObject[] objects;
    [SerializeField] private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Texture originalTexture;
    [SerializeField] private Renderer m_Renderer;

    public bool IsInfo;
    private Animal animal;
    private bool isHovering;
    private Vector3 rotOrig;

    private void Awake()
    {
        IsInfo = true;
        m_InteractiveItem = GetComponent<VRInteractiveItem>();
        m_SelectionRadial = Camera.main.GetComponent<SelectionRadial>();
        origMaterial = m_Renderer.material;
    }

    private void Start()
    {
        animal = GetComponent<Animal>();
        objects = GameObject.FindGameObjectsWithTag(animal.Tag);
        rotOrig = this.transform.eulerAngles;
        isHovering = false;
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
        if (!UIController.Instance.Showing)
        {
            m_SelectionRadial.Show();
            m_GazeOver = true;

            if (IsInfo)
            {
                
                foreach (GameObject obj in objects)
                {
                    if (obj.GetComponent<SkinnedMeshRenderer>() == null)
                    {
                        var renderer = obj.GetComponent<MeshRenderer>();
                        renderer.material = outlineMaterial;
                        renderer.material.mainTexture = originalTexture;

                    }
                    else
                    {
                        var renderer = obj.GetComponent<SkinnedMeshRenderer>();
                        renderer.material = outlineMaterial;
                        renderer.material.mainTexture = originalTexture;
                    }

                    if (GetComponent<Move>() != null)
                    {
                        Debug.Log("isMoving False");
                        GetComponent<Move>().isMoving = false;
                    }

                }
            }
            else
            {
                m_Renderer.material = outlineMaterial;
                m_Renderer.material.mainTexture = originalTexture;
                Hover();
            }
        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        m_SelectionRadial.Hide();
        m_GazeOver = false;

        if (IsInfo)
        {
            foreach (GameObject obj in objects)
            {

                if (obj.GetComponent<SkinnedMeshRenderer>() == null)
                {
                    var renderer = obj.GetComponent<MeshRenderer>();
                    renderer.material = origMaterial;
                }
                else
                {
                    var renderer = obj.GetComponent<SkinnedMeshRenderer>();
                    renderer.material = origMaterial;
                }
                if (GetComponent<Move>() != null)
                {
                    GetComponent<Move>().isMoving = true;
                }
            }
        }
        else
        {
            m_Renderer.material = origMaterial;
            m_Renderer.material.mainTexture = originalTexture;
            Hover();
        }
    }

    private void HandleSelectionComplete()
    {
        // If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
        if (m_GazeOver)
            StartCoroutine(ActivateButton());

    }


    private IEnumerator ActivateButton()
    {
        if (IsInfo)
        {
            if (animal.Sound != null)
            {
                SoundManager.Instance.PlayAudio(animal.Sound);
            }

            yield return InfoManager.Instance.ShowInfo(animal.Name, animal.info, animal.Image);
        }
        else
        {
            HandleOut();
            GameManager.Instance.Check(this.gameObject);
        }

        yield break;
    }

    private void Hover()
    {
        // isHovering = !isHovering;           // Set to inverse of self
        // if (isHovering)
        // {
        //     Vector3 pos = this.transform.position;
        //     this.transform.position = new Vector3(pos.x, 2f, pos.z);                // Give hover effect
        // }
        // else
        // {
        //     Vector3 pos = this.transform.position;
        //     this.transform.position = new Vector3(pos.x, 0f, pos.z);                // Restore y pos to 0
        //     this.transform.eulerAngles = rotOrig;                                   // Restore to original rotation
        // }
    }

    void FixedUpdate()
    {
        // if (isHovering && GameManager.Instance.m_GameType == SessionData.GameType.SEE)
        // {
        //     Vector3 rot = this.transform.eulerAngles;
        //     this.transform.eulerAngles = new Vector3(rot.x, rot.y + 5, rot.z);      // Give rotation on hover
        // }
    }

}
