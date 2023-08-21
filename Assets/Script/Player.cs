using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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

    public OpenButton inventryButton;
    public OpenButton rankingButton;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();           // �N���b�N�ӏ��ւ̈ړ��p
        agent.speed = speed;

        animator = GetComponent<Animator>();            // Player�̃A�j���[�V����
        animator.SetInteger("anim", 1);                 // �I�[�v�j���O�V�[��0�A���[���h�V�[��1
        animator.SetTrigger("Hi");                      // �A�j���[�V����Hi���s
    }

    // Update is called once per frame
    void Update()
    {
        // �t�F�[�h�C�������łȂ���΃t�F�[�h�C�����s
        if (!fade.IsFadeInComplete()){
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
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
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
                    } else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 315, 0.0f);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    agent.destination = this.transform.position;
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 225.0f, 0.0f);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                    }
                    agent.destination = this.transform.position;
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    agent.destination = this.transform.position;
                    transform.position += transform.forward * speed * Time.deltaTime;
                    animator.SetBool("Run", true);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                    agent.destination = this.transform.position;
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
//                        agent.destination = this.transform.position;
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
            agent.destination = this.transform.position;
        }
        else if (col.gameObject.name != "Terrain")
        {
            animator.SetTrigger("Damage");
            agent.destination = this.transform.position;
        }
    }
}
