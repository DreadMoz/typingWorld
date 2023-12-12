using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject typingRoom;

    [SerializeField]
    private GameObject inventoryButton;

    [SerializeField]
    private GameObject rankButton;

    [SerializeField]
    private GameObject itemShop;

    [SerializeField]
    private GameObject status;

    [SerializeField]
    private Fade fade;

    [SerializeField]
    private Fade fadeDoor;

    [SerializeField]
    private SwitchCam switchCam;

    [SerializeField]
    private GameObject housePlayer;
    private Animator pAnimator;

    public Camera playerCamera; // レイキャストに使用するカメラ

    [SerializeField]
    private GameObject exitHouse;
    [SerializeField]
    private GameObject exitShop;

    private Animator animator;
    private NavMeshAgent agent;
    private float speed = 8f;
    private int typingWindow = 0;
    private int shopWindow = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!gm || !typingRoom || !inventoryButton || !rankButton || !itemShop || !status || !fade || !fadeDoor)
        {
            Debug.LogError("Playerスクリプトで必要なオブジェクトが割り当てられていません。");
            return;
        }
        pAnimator = housePlayer.GetComponent<Animator>(); // Playerのアニメーターを取得
        agent = GetComponent<NavMeshAgent>();  // ナビメッシュエージェントを取得
        agent.speed = speed;

        animator = GetComponent<Animator>();  // Playerのアニメーターを取得
        animator.SetInteger("anim", 1);       // アニメーションステートを1に設定 タイトルのアニメーションを抜ける

        if (GameManager.sceneNo == (int)scene.World)
        {
            exitHouse.SetActive(false);
            exitShop.SetActive(false);
            transform.position = new Vector3(286, 1, 96);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            // "Hi" トリガーアニメーションを開始
            animator.SetTrigger("Hi");
        }
        else if (GameManager.sceneNo == (int)scene.House)
        {
            transform.position = new Vector3(252.2f, 9, -89);
            transform.rotation = Quaternion.Euler(0, -145, 0);
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Hi");

            GameManager.sceneNo = (int)scene.World;
        }
        else if (GameManager.sceneNo == (int)scene.Shop)
        {
            // ここじゃないな
            transform.position = new Vector3(252.2f, 9, -89);
            transform.rotation = Quaternion.Euler(0, -145, 0);
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Hi");
        }
        else if (GameManager.sceneNo == (int)scene.HouseE)
        {
            transform.position = new Vector3(288, 1, 117);
            transform.rotation = Quaternion.Euler(0, 35, 0);
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Bow");
        }
        else if (GameManager.sceneNo == (int)scene.ShopE)
        {
            transform.position = new Vector3(288, 1, 117);
            transform.rotation = Quaternion.Euler(0, 35, 0);
            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Bow");

            GameManager.sceneNo = (int)scene.World;
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
            rankButton.SetActive(false);
            inventoryButton.SetActive(false);
            rankButton.GetComponent<OpenButton>().forceOpen();
            fadeDoor.StartFadeIn();
            exitHouse.SetActive(true);

            // ショップ画面への移動
            transform.position = new Vector3(492, 45, 107.5f);
            transform.rotation = Quaternion.Euler(0, -145, 0);

            // カメラ切り替え
            switchCam.SwitchCamera();

            // "Bow" トリガーアニメーションを開始
            pAnimator.SetTrigger("hi");

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
            rankButton.SetActive(true);
            inventoryButton.SetActive(true);
            rankButton.GetComponent<OpenButton>().OnButton();
            fadeDoor.StartFadeIn();

            // タイピングハウス前への移動
            transform.position = new Vector3(287, 1, 117);
            transform.rotation = Quaternion.Euler(0, 47, 0);

            // カメラ切り替え
            switchCam.SwitchCamera();

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
            rankButton.SetActive(false);
            inventoryButton.SetActive(false);
            inventoryButton.GetComponent<OpenButton>().forceOpen();
            exitShop.SetActive(true);

            // ショップ前への移動
            transform.position = new Vector3(236, 1, 145);
            transform.rotation = Quaternion.Euler(0, -60, 0);

            // カメラ切り替え
            switchCam.SwitchCamera();

            // "Bow" トリガーアニメーションを開始
            pAnimator.SetTrigger("hi");

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
            rankButton.SetActive(true);
            inventoryButton.SetActive(true);
            inventoryButton.GetComponent<OpenButton>().OnButton();
            fadeDoor.StartFadeIn();

            // ショップ前への移動
            transform.position = new Vector3(285, 1, 118);
            transform.rotation = Quaternion.Euler(0, 47, 0);

            // カメラ切り替え
            switchCam.SwitchCamera();

            // "Bow" トリガーアニメーションを開始
            animator.SetTrigger("Bow");
        }

        // ダメージまたは"Hi"アニメーション中またなウィンドウを開いたときはプレイヤーの位置を固定
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hi") || gm.getWindowOpen())
        {
            agent.destination = this.transform.position;
        }
        else
        {
            // UI要素上でマウスカーソルがある場合は操作しない
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
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
                    if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out hit, 100))
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
                        agent.destination = this.transform.position;
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
            if (!gm.getWindowOpen())
            {
                // "Hi" トリガーアニメーションを開始
                animator.SetTrigger("Hi");
                agent.destination = this.transform.position;

                fadeDoor.StartFadeOut();
                typingWindow = 1;
            }
        }
        else if (col.gameObject.name == "ShopDoor")
        {
            if (!gm.getWindowOpen())
            {
                // "Hi" トリガーアニメーションを開始
                animator.SetTrigger("Hi");
                agent.destination = this.transform.position;

                fadeDoor.StartFadeOut();
                shopWindow = 1;
            }
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
