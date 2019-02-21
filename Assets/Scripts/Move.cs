using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move : MonoBehaviour
{
    private Transform path;
    public float speed;
    private List<Transform> nodes;
    private int currentNode = 0;
    int rand;
    private Rigidbody rigidBody;

    public bool isMoving = false;
    private bool isGrounded = false;
    private bool decision = false;
    private float distance = 0f;
    private GameObject pathObject;
    private float timeLeft = 0f;
    private string currentState = "";
    [SerializeField] public string PathName;
    [SerializeField] private Animation AnimalAnimationObject;
    [SerializeField] private bool isFlying = true;

    [System.Serializable]
    public class AnimalBehaviour
    {
        [Range(0f, 20f)]
        [SerializeField]
        public int sit;

        [Range(0f, 5f)]
        [SerializeField]
        public int walk;

        public AnimationClip[] animator;
    }
    // Use this for initialization
    //[SerializeField]
    public AnimalBehaviour animalBehaviour;


    void Start()
    {

        //a.AddClip(AnimalBehaviour[0],"new");
        //a.clip = a.GetClip("new");
        //a.playAutomatically = true;
        //Debug.Log(anim.GetComponent<Animation>().Stop());

        //AnimalAnimationObject.Play("Walk");
        //currentState = animalBehaviour.animator[Random.Range(0, 2)].name;
        //AnimalAnimationObject.Play(currentState);


        if (Path.Instance != null)
        {
            // This is to get the array of paths
            var animalPathArray = GameObject.FindGameObjectsWithTag(PathName);
            path = animalPathArray[Random.Range(0,animalPathArray.Length)].transform;
            if (isGrounded)
            {
                distance = 3f;
            }
            else
                distance = 9f;

            rigidBody = GetComponent<Rigidbody>();

            Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();

            nodes = new List<Transform>();

            foreach (var item in pathTransforms)
            {
                if (item != path.transform)
                {
                    nodes.Add(item);
                }
            }
            rand = Random.Range(0, nodes.Count);
        }
        else
        {
            GetComponent<Animal>().AnimalDisableMove();
        }


        isGrounded = GetComponent<Animal>().IsGrounded;
        isMoving = GetComponent<Animal>().IsMoving;
    }

    private void FixedUpdate()
    {

        if (AnimalAnimationObject != null)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                currentState = animalBehaviour.animator[Random.Range(0, animalBehaviour.animator.Length)].name;
                
                AnimalAnimationObject.Play(currentState);

                if (animalBehaviour.animator.Length != 1 || currentState == "Walk")
                {
                    if (currentState == "Walk")
                    {
                        isMoving = true;
                    }
                    else
                    {
                        isMoving = false;
                    }
                }

                timeLeft = Random.Range(0f, animalBehaviour.sit);

            }
        }
        

        if (isMoving)
        {
            CheckWayPointDistance();

            AnimalMove();
        }


    }


    void AnimalMove()
    {
        Vector3 node;
        if (isGrounded)
        {
            node = nodes[rand].position;
        }
        else
        {
            Vector3 tmp = nodes[rand].position;
            tmp.y = this.transform.position.y;
            node = tmp;
        }

        Vector3 pos = Vector3.MoveTowards(transform.position, node, speed * Time.deltaTime);
        GetComponent<Rigidbody>().MovePosition(pos);

        transform.LookAt(node);
    }

    private void CheckWayPointDistance()
    {
        Vector3 node;
        if (isGrounded)
        {
            node = nodes[rand].position;
        }
        else
        {
            Vector3 tmp = nodes[rand].position;
            tmp.y = this.transform.position.y;
            node = tmp;
        }
        if (Vector3.Distance(transform.position, node) < distance)
        {
            if (rand == nodes.Count - 1)
            {
                //currentNode = 0;
                rand = Random.Range(0, nodes.Count);
            }
            else
            {
                //currentNode++;
                rand = Random.Range(0, nodes.Count);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        rand = Random.Range(0, nodes.Count);
    }

}
