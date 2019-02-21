using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;

public class SpawnManager : Singleton<SpawnManager>
{   
    // New variables
    public Phylum[] phyla;                                              // Array of Phylum to be randomized
    [SerializeField] private int degree = 360;                          // The degree the object is placed in
    [SerializeField] private int spawnNum = 10;                         // How many animals will be spawned
    [SerializeField] private Transform m_Camera;                        // Main Camera used to rotate animal position
    [SerializeField] private Transform parentObject;                    // This is a parent object used to keep scene heirarchy clean
    [SerializeField] private float distanceToCamera = 50;               // Distance of animals to main camera
    [SerializeField] private float spawnLength = 200;                   // Length of where animals can spawn in respect w/ origin
    [SerializeField] private float spawnHeight = 200;                   // Spawn height of animals that are not grounded
    [SerializeField] private ScoreManager.MinigameType gameType;        // The type of game requiring spawn

    private GameObject[] spawnedObjects; 

    // ----------------------------------------------------
    // Functions used for spawning animals in Info mode
    // ----------------------------------------------------

    public void SpawnInfoAnimals()
    {
        

        // Get the phylum of animals to be spawned
        GameObject[] animals = phyla[0].GetRandomUniqueAnimalList(spawnNum);
        // This is enable the gaze script in the main animals of the phylum
        foreach (var a in animals)
        {
            a.GetComponent<Gaze>().enabled = true;
        }
        SpawnAnimals(animals,spawnNum);
        SpawnOtherAnimalsNotBelongingToPhylum();
        // TODO: Find a better algo for objects to spawn
        // outside the circle. Present algo might cause
        // infinite loop and locking code.

        // TODO: Substitute getting random points from a
        // a SQUARE to a CIRCLE for better vr experience?
        // Current set up spawns animals in a square


    }

    // This function will spawn other animals to complete the full environment of the phylum
    private void SpawnOtherAnimalsNotBelongingToPhylum()
    {
        for(int x = 1; x < phyla.Length; x++)
        {
            GameObject[] animals = phyla[x].GetRandomUniqueAnimalList(5);
            foreach(var a in animals)
            {
                a.GetComponent<Gaze>().enabled = false;
            }
            SpawnAnimals(animals,5);
        }
    }

    private void SpawnAnimals(GameObject[] animals,int SpawnNum)
    {
        // Raycast used for determining the y of grounded objects
        int layerMask = 1 << 8;
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        // Loop for the specified spawnNum
        for (int i = 0; i < SpawnNum; i++)
        {
            // Get the animal to be spawned
            GameObject animal = animals[i];

            // Temporary storage for instantiated prefabs
            GameObject tmp;

            // If the animal is grounded, y is ground level
            if (animal.GetComponent<Animal>().IsGrounded)
            {
                if (animal.GetComponent<Animal>().IsMoving)
                {
                    Vector3 ray_pos;
                    // Get the PathName so we can spawn the animals on their respective paths
                    var animalPathName = animal.GetComponent<Move>().PathName;
                    // Get the Paths of the animal
                    // To be used in getting the length or how many paths in there for the animal
                    var animalPathGameObject = GameObject.FindGameObjectsWithTag(animalPathName);
                    
                    // Get the Path position
                    Vector3 pathPosition = GameObject.FindGameObjectsWithTag(animalPathName)[Random.Range(0,animalPathGameObject.Length)].transform.position;
                    Debug.Log(pathPosition);
                    // Get the radius of the path
                    // We are getting the radius of the path so the animals will spawn inside the path radius
                    // radius will be used in the RandomCircle Function
                    var radius = GameObject.FindGameObjectWithTag(animalPathName).GetComponent<Path>().radius;
                    // Loop while the ray pos is inside the player circle
                    do
                    {
                        // Get the position inside the path circle
                        // Assign x and z as origin of ray cast, 200 default value is for cast height
                        Vector3 pos = RandomCircle(pathPosition, radius);
                        ray_pos = new Vector3(pos.x, 200, pos.z);

                    } while (ray_pos == Vector3.zero);

                    // Activate ray cast to get y value with respect to terrain height
                    Physics.Raycast(ray_pos, down, out hit, Mathf.Infinity, layerMask);
                    // For debugging
                    Debug.DrawLine(ray_pos, hit.point, Color.red, 60.0f);

                    // Generate random rotation on y axis, keep x and z rotation
                    Vector3 rot = new Vector3(animal.transform.eulerAngles.x, Random.Range(0, 360), animal.transform.eulerAngles.z);

                    // Spawn the object in the location of ray cast hit
                    tmp = Instantiate(animal, hit.point, Quaternion.Euler(rot));
                }
                else
                {
                    Vector3 ray_pos;

                    // Loop while the ray pos is inside the player circle
                    do
                    {
                        // Divide by half to center spawn
                        float l = spawnLength / 2;
                        // Get x and z value from the within the specified spawn length
                        float x = Random.Range(l * -1, l);
                        float z = Random.Range(l * -1, l);

                        // Assign x and z as origin of ray cast, 200 default value is for cast height
                        ray_pos = new Vector3(x, 200, z);
                    } while (InsideCircle(ray_pos, distanceToCamera));

                    // Activate ray cast to get y value with respect to terrain height
                    Physics.Raycast(ray_pos, down, out hit, Mathf.Infinity, layerMask);

                    // Generate random rotation on y axis, keep x and z rotation
                    Vector3 rot = new Vector3(animal.transform.eulerAngles.x, Random.Range(0, 360), animal.transform.eulerAngles.z);

                    // Spawn the object in the location of ray cast hit
                    tmp = Instantiate(animal, hit.point, Quaternion.Euler(rot));
                }

            }
            // If the animal is not grounded, y is within a range
            else
            {
                Vector3 rand_pos;

                // Loop while the rand pos is inside the player circle
                do
                {
                    // Divide by half to center spawn
                    float l = spawnLength / 2;
                    // Get x and z value from the within the specified spawn length
                    float x = Random.Range(l * -1, l);
                    float z = Random.Range(l * -1, l);
                    // Get y value from specified spawn heigth, default min
                    // of 20 to avoid animals spawning in terrain
                    float y = Random.Range(20, spawnHeight);

                    // Assign all values computed
                    rand_pos = new Vector3(x, y, z);
                } while (InsideCircle(rand_pos, distanceToCamera));

                // Generate random rotation on y and z axis
                Vector3 rot = new Vector3(animal.transform.eulerAngles.x, Random.Range(0, 360), Random.Range(0, 360));

                // Spawn the object using the pos and rot computed
                tmp = Instantiate(animal, rand_pos, Quaternion.Euler(rot));
            }

            // Set the prefabs parent to the parentObject
            tmp.transform.SetParent(parentObject);
        }
    }

    // Function that checks if a given pos is within 
    // the area of circle given the radius
    private bool InsideCircle(Vector3 pos, float r)
    {
        Vector3 camPosition = m_Camera.position;                        // Get the Camera position needed for computation

        // Basic formula to know if the given pos is
        // within the main camera's circle in respect
        // with the given radius r
        float x = (pos.x - camPosition.x) * (pos.x - camPosition.x);
        float z = (pos.z - camPosition.z) * (pos.z - camPosition.z);
        float d = Mathf.Sqrt(x + z);

        // If the distance is less than the radius
        if (d < r)
        {
            // The given pos is within the radius
            return true;
        }
        // The given pos is not within the radius
        return false;
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        Vector3 pos;
        float ang = Random.value * 360;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
    
    // ----------------------------------------------------
    // Functions used for spawning animals in Minigame mode
    // ----------------------------------------------------

    public GameObject SpawnGameAnimals()
    {
        // If the game is SEE, there must be more than 1 phlyum
        // The game consist of 1 phylum and 1 extra phlyum
        // You need to spawn n number of animals from original
        // phylum and add 1 animal from extra phlyum
        if (gameType == ScoreManager.MinigameType.SEE && phyla.Length > 1)
        {
            int phylumNum = Random.Range(0, phyla.Length);                                  // Choose a random phylum to instantiate animals from
            Phylum phylum = phyla[phylumNum];                                               // Assign random to var phylum

            // Get random phylum to instantiate extra animal
            int extraPhylumNum;                                                             // Get an random int to assign extra phylum from
            do
            {
                extraPhylumNum = Random.Range(0, phyla.Length);                             // Must not be equal to original phylum
            } while (extraPhylumNum == phylumNum);
            Phylum extraPhylum = phyla[extraPhylumNum];                                     // Using random int assign the extra phlyum

            // Get random animal from the extra phylum
            int extraAnimalNum = Random.Range(0, extraPhylum.animals.Length);               
            GameObject extraAnimal = extraPhylum.animals[extraAnimalNum];

            // Create a list of animals to hold animals from original phylum
            GameObject[] tmp = phylum.GetRandomUniqueAnimalList(spawnNum - 1);
            List<GameObject> animalsList = new List<GameObject>();
            
            // Add all animals in tmp
            for (int i = 0; i < tmp.Length; i++)
                animalsList.Add(tmp[i]);
            // Add the extra animal
            animalsList.Add(extraAnimal);                                                   

            // Convert list back to array
            GameObject[] animals = animalsList.ToArray();

            // Shuffle the animals
            Shuffler.Shuffle(animals);

            return SpawnGameObjectList(animals, extraAnimal);
        }
        // If the game is HEAR, there is only 1 "phylum" required
        // here the phylum is just a temporary storage for ALL animals
        // that produces sound. You simply need to get random animals
        // and a random int to have as the answer (animals[i])
        else
        {
            // TODO: Handle when the spawnNum is 
            // greater than the number of animals w/
            // sound to avoid duplicate animals

            Phylum wSound = phyla[0];                                                        // Get animals with sound
            GameObject[] animals = wSound.GetRandomUniqueAnimalList(spawnNum);               // List of animals to be spawned
            GameObject corrAnimal = animals[Random.Range(0, animals.Length)];                // Use this animal to play sound

            // Shuffle the animals
            Shuffler.Shuffle(animals);

            return SpawnGameObjectList(animals, corrAnimal);
        }
    }

    // Handles object creation and object spacing
    private GameObject SpawnGameObjectList(GameObject[] list, GameObject correct)
    {
        // Distance of objects to each other
        // NOTE: Causes even spaces in circular form
        float space = degree / (list.Length - 1);

        // Find the direction the camera is looking but on a flat plane.
        Vector3 relative = Vector3.ProjectOnPlane(m_Camera.forward, Vector3.up).normalized;
        float start = (Mathf.Atan2(relative.z, relative.x) * Mathf.Rad2Deg) + (space * Mathf.Floor(list.Length / 2));
        if (list.Length % 2 == 0) {
            // If the spawn num is even
            start -= space / 2;
        }

        spawnedObjects = new GameObject[list.Length];          // Put spawned objects here
        GameObject extra = new GameObject();                   // Put correct answer here
        

        for (int i = 0; i < list.Length; i++)
        {
            // Mathematical formula to get even spaces in a circle using Cos and Sin
            float angle = start - (space * i);                                          // Calculate angle to be used in getting x and z
            float x = distanceToCamera * Mathf.Cos(angle * Mathf.Deg2Rad);              // Formula to get x using angle
            float z = distanceToCamera * Mathf.Sin(angle * Mathf.Deg2Rad);              // Formula to get z using angle

            Vector3 pos = new Vector3(x, 0, z);                                         // Create a vector 3 using x and z
            GameObject tmp = Instantiate(list[i], pos, Quaternion.identity);            // Create object i using vector 3 pos

            // Disable movement we don't need it for minigames
            tmp.GetComponent<Animal>().AnimalDisableMove();

            // TODO: Change this
            // Make IsInfo false for the correct GazeFunction
            // tmp.GetComponent<mGaze>().IsInfo = false;
            
            // Rotate the object to look at camera
            tmp.transform.rotation = Quaternion.LookRotation(m_Camera.position - tmp.transform.position);
            tmp.transform.eulerAngles = new Vector3(0, tmp.transform.eulerAngles.y,tmp.transform.eulerAngles.z);
            // Set Parent Object
            tmp.transform.SetParent(parentObject);
            
            // Assign tmp to spawnedObjects
            spawnedObjects[i] = tmp;

            // If this is the correct object, store it in extra
            if (list[i].Equals(correct))
            {
                extra = tmp;
            }
        }

        // Return the correct object
        return extra;
    }

    public void Clear()
    {
        // Destroy existing spawned objects
        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            GameObject.Destroy(spawnedObjects[i]);
        }
    }
}
