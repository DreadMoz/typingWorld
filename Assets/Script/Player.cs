using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public GameObject inventry;
    public GameObject equipment;
    public GameObject ranking;
    public GameObject status;
    public Fade fade;
    private Rigidbody rigidBody;
    private Animator animator;
    private NavMeshAgent agent;
    private float speed = 8f;
    private float inputHorizontal;
    private float inputVertical;
    public bool enableInventry = false;
    public bool enableRanking = false;
    public bool enableStatus = false;
    private int div = 10;
    private int cutCount = 99;

// Start is called before the first frame update
void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        animator = GetComponent<Animator>();
        animator.SetInteger("anim", 1);
        animator.SetTrigger("Hi");
    }

    // Update is called once per frame
    void Update()
    {
        if (!fade.IsFadeInComplete()){
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            return;
        }
        if (Input.GetKeyDown(KeyCode.I) && (cutCount >= div))
        {
            if (!enableStatus)
            {
                enableStatus = true;
                enableInventry = true;
            }
            else
            {
                if (enableInventry)
                {
                    enableStatus = false;
                    enableInventry = false;
                }
                else if (enableRanking)
                {
                    enableStatus = true;
                    enableInventry = true;
                    enableRanking = false;
                }
            }
            cutCount = 0;
        }
        if (Input.GetKeyDown(KeyCode.R) && (cutCount >= div))
        {
            if (!enableStatus)
            {
                enableStatus = true;
                enableRanking = true;
            }
            else
            {
                if (enableRanking)
                {
                    enableStatus = false;
                    enableRanking = false;
                }
                else if (enableInventry)
                {
                    enableStatus = true;
                    enableRanking = true;
                    enableInventry = false;
                }
            }
            cutCount = 0;
        }

        if (cutCount < div)
        {
            if (cutCount < div / 2)
            {
                if (status.activeSelf)
                {
                    status.transform.position += new Vector3(0, 30, 0);
                }
                if ((inventry.activeSelf) && (!enableInventry))
                {
                    inventry.transform.position += new Vector3(110, 0, 0);
                    equipment.transform.position += new Vector3(0, -35, 0);
                }
                if ((ranking.activeSelf) && (!enableRanking))
                {
                    ranking.transform.position += new Vector3(110, 0, 0);
                }
                cutCount++;

                if (cutCount == div / 2)
                {
                    status.SetActive(false);
                    inventry.SetActive(false);
                    equipment.SetActive(false);
                    ranking.SetActive(false);
                }
            }
            else
            {
                if (enableStatus)
                {
                    status.SetActive(true);
                    status.transform.position += new Vector3(0, -30, 0);
                }
                if (enableInventry)
                {
                    inventry.SetActive(true);
                    equipment.SetActive(true);
                    inventry.transform.position += new Vector3(-110, 0, 0);
                    equipment.transform.position += new Vector3(0, 35, 0);
                }
                if (enableRanking)
                {
                    ranking.SetActive(true);
                    ranking.transform.position += new Vector3(-110, 0, 0);
                }
                cutCount++;
            }
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            if (!status.activeSelf)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        animator.SetBool("Run", true);
                        agent.destination = hit.point;
                    }
                }
                if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                {
                    if (Vector3.Distance(transform.position, agent.destination) < 0.2f)
                    {
                        animator.SetBool("Run", false);
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Door")
        {
            animator.SetTrigger("Hi");
        }
        else if (col.gameObject.name != "Terrain")
        {
            animator.SetTrigger("Damage");
        }
    }
}
