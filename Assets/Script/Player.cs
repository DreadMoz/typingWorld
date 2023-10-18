using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameManager gameMaster;

    [SerializeField]
    private GameObject typingRoom;

    [SerializeField]
    private GameObject itemShop;

    [SerializeField]
    private GameObject status;

    [SerializeField]
    private Fade fade;

    [SerializeField]
    private Fade fadeDoor;

    private Animator animator;
    private NavMeshAgent agent;
    private float speed = 8f;
    private int typingWindow = 0;
    private int shopWindow = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  // ナビメッシュエージェントを取得
        agent.speed = speed;

        animator = GetComponent<Animator>();  // Playerのアニメーターを取得
        animator.SetInteger("anim", 1);       // アニメーションステートを1に設定 タイトルのアニメーションを抜ける

        if (GameManager.sceneNo == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            // "Hi" トリガーアニメーションを開始
            animator.SetTrigger("Hi");
        }
        else
        {
            transform.position = new Vector3(288, 1, 117);
            transform.rotation = Quaternion.Euler(0, 35, 0);
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Bow");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // フェードイン中は操作しない
        if (!fade.IsFadeInComplete())
        {
            // プレイヤーの向きを固定
            transform.rotation = transform.rotation;
            return;
        }

        if (typingWindow == 1)
        {
            if (!fadeDoor.IsFadeOutComplete())
            {
                return;
            }
            typingWindow = 0;
            typingRoom.SetActive(true);
            fadeDoor.StartFadeIn();
        }
        else if (typingWindow == -1)
        {
            if (!fadeDoor.IsFadeOutComplete())
            {
                return;
            }
            typingWindow = 0;
            typingRoom.SetActive(false);
            fadeDoor.StartFadeIn();
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Bow");
        }
        if (shopWindow == 1)
        {
            if (!fadeDoor.IsFadeOutComplete())
            {
                return;
            }
            shopWindow = 0;
            itemShop.SetActive(true);
            fadeDoor.StartFadeIn();
        }
        else if (shopWindow == -1)
        {
            if (!fadeDoor.IsFadeOutComplete())
            {
                return;
            }
            shopWindow = 0;
            itemShop.SetActive(false);
            fadeDoor.StartFadeIn();
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Bow");
        }

        // UI要素上でマウスカーソルがある場合は操作しない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // ダメージまたは"Hi"アニメーション中はプレイヤーの位置を固定
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
        // 衝突したオブジェクトに応じてアニメーションと目的地を設定
        if (col.gameObject.name == "TypingDoor")
        {
            // "Hi" トリガーアニメーションを開始
            animator.SetTrigger("Hi");
            agent.destination = this.transform.position;

            fadeDoor.StartFadeOut();
            typingWindow = 1;
        }
        else if (col.gameObject.name == "ShopDoor")
        {
            // "Hi" トリガーアニメーションを開始
            animator.SetTrigger("Hi");
            agent.destination = this.transform.position;

            fadeDoor.StartFadeOut();
            shopWindow = 1;
        }
        else if (col.gameObject.name != "Terrain")
        {
            // "Damage" トリガーアニメーションを開始
            animator.SetTrigger("Damage");
            agent.destination = this.transform.position;
        }
    }
    public void CloseTypingDoor()
    {
        fadeDoor.StartFadeOut();
        typingWindow = -1;
    }
    public void CloseShopDoor()
    {
        fadeDoor.StartFadeOut();
        shopWindow = -1;
    }
}
