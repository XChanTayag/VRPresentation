using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    public bool IsGrounded;                                             // Used in spawn to know if the animal is in the ground or floating
    public bool IsMoving;                                               // Helps identify if the animal should move
    public AudioClip Sound;                                             // The sound produced by this animal
    public string Name;                                                 // This is the animals common name
    public Texture Image;                                               // The animal's picture
    public string Tag;                                                  // Remove this after removing the old gaze script
    [SerializeField] private Animation anim;                            // The reference for the animal's animation

    [Multiline] public string info;                     // This is the animal info used in parsing the info
    private Renderer origRenderer;                                      // Used in resetting the animal's render
    //public string Info                                                  // Prop for info
    //{
    //    get {  return info;  }
    //    set { info = value; }
        
    //}
    public Renderer OrigRenderer                                        // Prop for getting this animals original render
    {
        get
        {
            if (!origRenderer)
                GetRenderer();
            return origRenderer;
        }
    }

    // Used in getting the animal's renderer
    private void GetRenderer()
    {
        Renderer[] r = GetComponentsInChildren<Renderer>();
        origRenderer = r[0];
    }

    // Use this to disable the animals movement animation
    public void AnimalDisableMove()
    {
        IsMoving = false;
        if (GetComponent<Rigidbody>() != null)
        {
            Destroy(GetComponent<Rigidbody>());
        }
        if (anim != null)
        {
            anim.enabled = false;
        }
    }
}
