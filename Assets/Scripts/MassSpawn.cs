using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpawn : MonoBehaviour
{
    [SerializeField] private float min_x = -5f, max_x = 5f;
    [SerializeField] private float min_y = -5f, max_y = 5f;
    [SerializeField] private float min_z = -5f, max_z = 5f;
    [SerializeField] private Phylum phylum;
    public int spawnNum;

    [SerializeField] private float radius = 30f;         // Radius of circle around player
    private Vector3 m_cam;                              // Reference the player's pos

    [SerializeField] private bool IsTesting = false;
    [SerializeField] private GameObject[] testList;

    void Awake()
    {
        m_cam = Camera.main.transform.position;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        if (!IsTesting)
        {
            yield return UIController.Instance.ShowIntroUI();
            //AnimalInfo.LoadAnimalInfo(phylum);
            Spawn();
        }
        else
            TestSpawn();
    }

    bool InsideCircle(Vector3 pos)
    {
        float x = (pos.x - m_cam.x) * (pos.x - m_cam.x);
        float z = (pos.z - m_cam.z) * (pos.z - m_cam.z);
        float d = Mathf.Sqrt(x + z);

        if (d < radius)
        {
            return true;
        }

        return false;
    }

    void Spawn()
    {
        int layerMask = 1 << 8;
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        GameObject[] animals = phylum.GetRandomAnimalList(spawnNum);

        for (int i = 0; i < spawnNum; i++)
        {
            GameObject animal = animals[i];                           		// Random animal to be spawned

            if (animal.GetComponent<Animal>().IsGrounded)
            {
                // Check if animal is moving
                // Different line of codes for each scenario
                if(animal.GetComponent<Animal>().IsMoving)
                {
                    Vector3 ray_pos;
                    var animalPathName = animal.GetComponent<Move>().PathName;
                    Vector3 animalPosition = GameObject.FindGameObjectWithTag(animalPathName).transform.position;
                    Debug.Log(animalPosition);
                    var radius = GameObject.FindGameObjectWithTag(animalPathName).GetComponent<Path>().radius;
                    // Loop while the ray pos is inside the player circle
                    do
                    {
                        // Get the position inside the path circle
                        // Assign x and z as origin of ray cast, 200 default value is for cast height
                        Vector3 pos = RandomCircle(animalPosition, radius);
                        ray_pos = new Vector3(pos.x, 200, pos.z);

                    } while (ray_pos == Vector3.zero);

                    // Activate ray cast to get y value with respect to terrain height
                    Physics.Raycast(ray_pos, down, out hit, Mathf.Infinity, layerMask);
                    // For debugging
                    Debug.DrawLine(ray_pos, hit.point, Color.red, 60.0f);

                    // Generate random rotation on y axis, keep x and z rotation
                    Vector3 rot = new Vector3(animal.transform.eulerAngles.x, Random.Range(0, 360), animal.transform.eulerAngles.z);

                    // Spawn the object in the location of ray cast hit
                    Instantiate(animal, hit.point, Quaternion.Euler(rot));
                }
                else
                {
                    Vector3 ray_pos;
                    do
                    {
                        ray_pos = new Vector3(Random.Range(min_x, max_x), 200, Random.Range(min_z, max_z));     // Generate random ray pos for ray casting
                    } while (InsideCircle(ray_pos));

                    Physics.Raycast(ray_pos, down, out hit, Mathf.Infinity, layerMask);                         // Using ray pos activate ray cast to hit terrain
                    Vector3 rot = new Vector3(animal.transform.eulerAngles.x, Random.Range(0, 360), animal.transform.eulerAngles.z);  // Generate random rotation on y axis

                    Instantiate(animal, hit.point, Quaternion.Euler(rot));

                }

            }
            else
            {
                Vector3 rand_pos;               // Random position for object instantiation
                do
                {
                    rand_pos = new Vector3(Random.Range(min_x, max_x), Random.Range(min_y, max_y), Random.Range(min_z, max_z));
                } while (InsideCircle(rand_pos));

                Vector3 rot = new Vector3(animal.transform.eulerAngles.x, Random.Range(0, 360), Random.Range(0, 360));  // Generate random rotation on y and z axis

                Instantiate(animal, rand_pos, Quaternion.Euler(rot));
            }
        }
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

    void TestSpawn()
    {
        int layerMask = 1 << 8;
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        for (int i = 0; i < spawnNum; i++)
        {
            GameObject testObj = testList[Random.Range(0, testList.Length)];                                // Random animal to be spawned

            Vector3 ray_pos;
            do
            {
                ray_pos = new Vector3(Random.Range(min_x, max_x), 200, Random.Range(min_z, max_z));     // Generate random ray pos for ray casting
            } while (InsideCircle(ray_pos));

            Physics.Raycast(ray_pos, down, out hit, Mathf.Infinity, layerMask);                         // Using ray pos activate ray cast to hit terrain
            Debug.DrawLine(ray_pos, hit.point, Color.red, 60.0f);

            Vector3 rot = testObj.transform.eulerAngles;
            rot.y = Random.Range(0, 360);                                                               // Random y axis rotation

            Instantiate(testObj, hit.point, Quaternion.Euler(rot));
        }
    }
}
