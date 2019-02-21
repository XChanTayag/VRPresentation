using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : Singleton<Path>
{

    [SerializeField] private float min_x = -5f, max_x = 5f;
    [SerializeField] private float min_z = -5f, max_z = 5f;
    [SerializeField] public float radius = 90;
    [SerializeField] private float nodeLift = 0f;
    //private Transform objTrans;
    public int spawnNum;

    public Color lineColor;
    Vector3 center;


    private List<Transform> nodes;

    private void Start()
    {
        DrawNodes();

        //Debug.Log(objTrans.position);
        //center = objTrans.position;
        //Debug.Log(center.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;


        Transform[] pathTransforms = GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        foreach (var item in pathTransforms)
        {
            if (item != transform)
            {
                nodes.Add(item);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
            {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previousNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }

    }

    private void DrawNodes()
    {
        int layerMask = 1 << 8;
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        Transform objTrans = GetComponent<Transform>();
        Vector3 center = objTrans.position;
        //radius = 90.0f;

        for (int i = 0; i < spawnNum; i++)
        {
            GameObject nodeObject = new GameObject();
            Vector3 ray_pos;
            do
            {
                Vector3 pos = RandomCircle(center, radius);
                ray_pos = new Vector3(pos.x, nodeLift, pos.z);     // Generate random ray pos for ray casting
            } while (ray_pos == Vector3.zero);

            Physics.Raycast(ray_pos, down, out hit, Mathf.Infinity, layerMask);                         // Using ray pos activate ray cast to hit terrain
            Debug.DrawLine(ray_pos, hit.point, Color.red, 60.0f);
            nodeObject.transform.position = ray_pos;
            nodeObject.transform.SetParent(GetComponent<Transform>(), true);
            //Vector3 rot = nodeObject.transform.eulerAngles;
            //rot.y = Random.Range(0, 360);                                                               // Random y axis rotation

            //Instantiate(nodeObject, hit.point, Quaternion.Euler(rot));


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
}
