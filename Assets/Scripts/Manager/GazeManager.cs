using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
public class GazeManager : Singleton<GazeManager>
{
    // Enum to identify the different modes for gaze
    public enum GazeMode
    {
        INFO,
        GAME,
        MENU
    };


    [SerializeField] private GazeMode mode;                                 // Helps identify how to handle gaze function
    [SerializeField] private SelectionRadial selectionRadial;               // This controls when the selection is complete
    [SerializeField] private Material outlineMaterial;                      // Used in INFO to help outline same animals
    private bool highlight = false;                                         // Used in checking if highlight is active
    private bool hovering = false;                                          // Used in checking if hover is active
    public GazeMode Mode { get { return mode; } }                           // Prop for public access of gaze mode


    // Calls function handler depending on the Gaze mode
    public void HandleOver(GameObject obj)
    {
        // Make sure no UI is currrently showing
        if (!UIController.Instance.Showing)
        {
            // Show selection radial and activate gazeOver
            selectionRadial.Show();
            obj.GetComponent<Gaze>().GazeOver = true;

            switch (mode)
            {
                case GazeMode.INFO:
                    // Highlight all animals
                    // Highlight(obj);

                    // TODO: Fix this to validate if the game object has a Move script
                    // Get Move Script from the gazed game object then set the isMoving to FALSE;
                    if (obj.GetComponent<Move>() != null)
                    {
                        obj.GetComponent<Move>().isMoving = false;
                    }
                    break;
                case GazeMode.GAME:
                    break;
                case GazeMode.MENU:
                    break;
                default:
                    Debug.LogError("Invalid gaze mode!");
                    break;
            }
        }
    }

    public void HandleOut(GameObject obj)
    {
        // Make sure no UI is currrently showing
        if (!UIController.Instance.Showing)
        {
            // Hide selection radial and deactivate gazeOver
            selectionRadial.Hide();

            obj.GetComponent<Gaze>().GazeOver = false;

            switch (mode)
            {
                case GazeMode.INFO:
                    // Dehighlight all animals
                    // Highlight(obj);
                    // Get Move Script from the gazed game object then set the isMoving to TRUE;
                    if (obj.GetComponent<Move>() != null)
                    {
                        obj.GetComponent<Move>().isMoving = true;
                    }
                    break;
                case GazeMode.GAME:
                    break;
                case GazeMode.MENU:
                    break;
                default:
                    Debug.LogError("Invalid gaze mode!");
                    break;
            }
        }
    }

    public IEnumerator ActivateButton(GameObject obj)
    {
        if (obj.GetComponent<WarpPoint>() != null)
        {
            // If the object is a warppoint, teleport
            obj.GetComponent<WarpPoint>().Teleport();
            obj.GetComponent<Gaze>().GazeOver = false;
        }
        else
        {
            switch (mode)
            {
                case GazeMode.INFO:
                    {

                        if (!UIController.Instance.Showing)
                        {
                            // Get Move Script from the gazed game object then set the isMoving to TRUE;
                            if (obj.GetComponent<Move>() != null)
                            {
                                obj.GetComponent<Move>().isMoving = true;
                            }
                            Animal animal = obj.GetComponent<Animal>();
                            obj.GetComponent<Gaze>().GazeOver = false;
                            // Show animal info
                            yield return ActivateInfo(animal);
                        }
                        break;
                    }
                case GazeMode.GAME:
                    {
                        // Check for user's answer
                        GameManager.Instance.Check(obj);
                        
                        break;
                    }
                case GazeMode.MENU:
                    {
                        break;
                    }
                default:
                    Debug.LogError("Invalid gaze mode!");
                    break;
            }
        }
        yield break;
    }

    private IEnumerator ActivateInfo(Animal animal)
    {
        // Play animal sound
        if (animal.Sound != null)
        {
            SoundManager.Instance.PlayAudio(animal.Sound);
        }

        // Show info for this animal
        yield return InfoManager.Instance.ShowInfo(animal.Name, animal.info, animal.Image);
    }

    // Handles outline of game objects
    // TODO: Implement caching for better performance
    // private void Highlight(GameObject obj)
    // {
    //     // Reference animal script from game obj
    //     Animal animal = obj.GetComponent<Animal>();
    //     // Get game animals with the same tag
    //     GameObject[] animals = GameObject.FindGameObjectsWithTag(animal.tag);

    //     if (!highlight)
    //     {
    //         // Outline all animal with the same tag as object
    //         foreach (GameObject o in animals)
    //         {
    //             Animal oAnimal = o.GetComponent<Animal>();

    //             if (o.GetComponent<SkinnedMeshRenderer>() == null)
    //             {
    //                 var renderer = o.GetComponent<MeshRenderer>();
    //                 renderer.material = outlineMaterial;
    //                 renderer.material.mainTexture = oAnimal.OriginalTexture;

    //             }
    //             else
    //             {
    //                 var renderer = o.GetComponent<SkinnedMeshRenderer>();
    //                 renderer.material = outlineMaterial;
    //                 renderer.material.mainTexture = oAnimal.OriginalTexture;
    //             }

    //             // User is looking at this animal, disable movement
    //             if (o.GetComponent<Move>() != null)
    //             {
    //                 o.GetComponent<Move>().isMoving = false;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         // Set to original material
    //         foreach (GameObject o in animals)
    //         {
    //             Animal oAnimal = o.GetComponent<Animal>();

    //             if (o.GetComponent<SkinnedMeshRenderer>() == null)
    //             {
    //                 var renderer = o.GetComponent<MeshRenderer>();
    //                 renderer.material = oAnimal.OriginalMaterial;
    //             }
    //             else
    //             {
    //                 var renderer = o.GetComponent<SkinnedMeshRenderer>();
    //                 renderer.material = oAnimal.OriginalMaterial;
    //             }

    //             // User no longer looking, enable movement
    //             if (o.GetComponent<Move>() != null)
    //             {
    //                 o.GetComponent<Move>().isMoving = true;
    //             }
    //         }
    //     }

    //     // Set to inverse
    //     highlight = !highlight;
    // }
}
