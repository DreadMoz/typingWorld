using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject typingRoom;
    [SerializeField] private GameObject tiikawa;
    [SerializeField] private GameObject kinoko;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject rankButton;
    [SerializeField] private GameObject itemShop;
    [SerializeField] private GameObject status;
    [SerializeField] private Fade fade;
    [SerializeField] private Fade fadeDoor;
    [SerializeField] private SwitchCam switchCam;
    [SerializeField] private Practice practice;
    [SerializeField] private GameObject housePlayer;
    [SerializeField] private GameObject exitHouse;
    [SerializeField] private GameObject exitShop;
    [SerializeField] private TMP_Text talk;
    [SerializeField] private GameObject inventoryFilter;

    private Animator pAnimator;
    public Camera playerCamera; // レイキャストに使用するカメラ

    private Animator animator;
    private NavMeshAgent agent;
    private float speed = 8f;
    private int typingWindow = 0;
    private int shopWindow = 0;

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

        tiikawa.SetActive(false);
        kinoko.SetActive(false);

        if (GameManager.SceneNo == (int)scene.World)
        {
            exitHouse.SetActive(false);
            exitShop.SetActive(false);
            inventoryFilter.SetActive(false);
            transform.position = new Vector3(286, 1, 96);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        
            animator.SetTrigger("Hi");    // "Hi" トリガーアニメーションを開始
        }
        else if (GameManager.SceneNo == (int)scene.House)
        {
            if (GameManager.TypingDataId < 0)
            {
                talk.text = "あれれ...おかしいなぁ...\nデータが見つからないよぅ > <;";
            }
            // 直近のタイピングデータ前の情報を表示
            practice.calcStars();           // 表示する星を計算
            practice.showRoomMenu();        // ルームメニュー表示
            exitHouse.SetActive(true);
            exitShop.SetActive(false);
            tiikawa.SetActive(true);

            // ここで直近のタイピングデータをセット
            if (GameManager.TypingTab == 2)
            {
                gm.registerRecentTypingResult();
            }

            switchCam.SwitchCamera();           // カメラ切り替え
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
        if (GameManager.SceneNo == (int)scene.House)
        {
            practice.calcStars();       // 表示する星を計算
            practice.showRoomMenu();    // ルームメニュー表示
            practice.showDetail();      // 詳細画面表示
            GameManager.SceneNo = (int)scene.World; // ワールドシーン状態へ
        }
        if (typingWindow == 1)
        {
            if (!fadeDoor.IsFadeOutComplete())
            {
                return;
            }
            typingWindow = 0;
            tiikawa.SetActive(true);
            typingRoom.SetActive(true);
            rankButton.SetActive(false);
            inventoryButton.SetActive(false);
            rankButton.GetComponent<OpenButton>().forceOpen();
            fadeDoor.StartFadeIn();
            exitShop.SetActive(false);
            exitHouse.SetActive(true);

            // カメラ切り替え
            switchCam.SwitchCamera();

            // "Bow" トリガーアニメーションを開始
            pAnimator.SetTrigger("hi");

            practice.calcStars();       // 表示する星を計算
            practice.showRoomMenu();    // ルームメニュー表示

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
            exitShop.SetActive(false);
            exitHouse.SetActive(false);
            tiikawa.SetActive(false);

            transform.position = new Vector3(288, 1, 113);   // タイピングハウス前位置
            transform.rotation = Quaternion.Euler(0, 20, 0); // タイピングハウス前角度

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
            kinoko.SetActive(true);

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
            exitShop.SetActive(false);
            kinoko.SetActive(false);

            transform.position = new Vector3(238.5f, 1, 141.4f);      // ショップ前場所
            transform.rotation = Quaternion.Euler(0, -57, 0);   // ショップ前角度

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
                talk.text = "タイピング練習場へようこそ！";
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
                inventoryFilter.SetActive(true);
                talk.text = "いらっしゃいませ！\nアイテムやさんだよ";
                // "Hi" トリガーアニメーションを開始
                animator.SetTrigger("Hi");
                agent.destination = this.transform.position;

                fadeDoor.StartFadeOut();
                shopWindow = 1;
            }
        }
        else if (col.gameObject.CompareTag("InvisibleFence"))
        {

        }
        else if (col.gameObject.name != "Terrain")
        {
            // "Damage" トリガーアニメーションを開始
            animator.SetTrigger("Damage");
            agent.destination = this.transform.position;
        }
    }

    void OnCollisionStay(Collision col)
    {
        // 接触している間中行いたい処理
        if (col.gameObject.CompareTag("InvisibleFence"))
        {
            animator.SetBool("Walk", true);
            agent.speed = speed/4;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("InvisibleFence"))
        {
            // 通り抜け不可処理解除追加予定
            animator.SetBool("Walk", false);
            agent.speed = speed;
        }
    }

    public void CloseTypingDoor()
    {
        fadeDoor.StartFadeOut();
        typingWindow = -1;
    }
    public void CloseShopDoor()
    {
        inventoryFilter.SetActive(false);
        fadeDoor.StartFadeOut();
        shopWindow = -1;
    }
}
