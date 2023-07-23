using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public GameManager gameMaster;
    public GameObject status;
    public Fade fade;
    private Animator animator;
    private NavMeshAgent agent;
    private float speed = 8f;
    private float inputHorizontal;
    private float inputVertical;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();           // クリック箇所への移動用
        agent.speed = speed;

        animator = GetComponent<Animator>();            // Playerのアニメーション
        animator.SetInteger("anim", 1);                 // オープニングシーン0、ワールドシーン1
        animator.SetTrigger("Hi");                      // アニメーションHi実行
    }

    // Update is called once per frame
    void Update()
    {
        // フェードイン完了でなければフェードイン実行
        if (!fade.IsFadeInComplete()){
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hi"))
        {
            transform.position = transform.position;
        }
        else
        {
            if (!status.activeSelf)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
                    } else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 315, 0.0f);
                    }
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 225.0f, 0.0f);
                    }
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    agent.destination = this.transform.position;
                    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
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
