using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Database db;
    public SaveData savedata;

    static public int sceneNo;      // ワールドシーンの状態番号
    static public int kpm;          // 現在のkpm
    static public int newKpm;       // 直近のタイピング結果のkpm

    [SerializeField]
    private StatusUI statusWindow;

    public GameObject player;        // プレイヤーオブジェクト
    public GameObject cam;           // カメラ
    private Animator animator;       // Playerのアニメーター
    public Fade fade;                // フェード用オブジェクト
    public GameObject inventory;
    public GameObject equipment;
    public GameObject ranking;
    public GameObject status;
    public GameObject typingRoom;
    public GameObject shopRoom;

    public OpenButton inventoryButton;  // インベントリボタン
    public OpenButton rankingButton;    // ランキングボタン

    private bool firstPush = false;      // スタートボタンが2回以上押されないようにするためのフラグ
    private bool goNextScene = false;    // ワールドシーンに遷移するためのフラグ

    [SerializeField]
    private int windowOpenCount = 20;    // ウィンドウが開くフレーム数
    private int count = 0;               // カウンタ
    private int inventoryOpen = 0;
    private int rankingOpen = 0;
    private int cameraMove = 0;          // 0:標準 1:右回転 2:左回転 3:ズームイン

    Vector3 chaseOffset = new Vector3(0f, 8f, -14f);
    Quaternion chaseRotation = Quaternion.Euler(25f, 0f, 0f);
    Vector3 statusOffset = new Vector3(1.4f, 1.3f, -4f);
    Quaternion statusRotation = Quaternion.Euler(5f, 0f, 0f);
    float difx;
    float dify;
    float difz;
    float posx;
    float posy;
    float posz;

    // 目標位置
    Vector3 statusShowPos;
    Vector3 inventoryShowPos;
    Vector3 rankingShowPos;
    Vector3 equipmentShowPos;
    Vector3 statusHidePos;
    Vector3 inventoryHidePos;
    Vector3 rankingHidePos;
    Vector3 equipmentHidePos;

    private void Awake()
    {
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得

        // アニメーションステートが1最初のワールドの場合
        if (sceneNo == 1)
        {
            status.SetActive(false);
            inventory.SetActive(false);
            equipment.SetActive(false);
            ranking.SetActive(false);
            typingRoom.SetActive(false);
            shopRoom.SetActive(false);
        }
        // アニメーションステートが2ショップ後の場合
        else if (sceneNo == 2)
        {
            status.SetActive(false);
            inventory.SetActive(false);
            equipment.SetActive(false);
            ranking.SetActive(false);
            typingRoom.SetActive(true);
            shopRoom.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        difx = (statusRotation.eulerAngles.x - chaseRotation.eulerAngles.x) / windowOpenCount;
        dify = (statusRotation.eulerAngles.y - chaseRotation.eulerAngles.y) / windowOpenCount;
        difz = (statusRotation.eulerAngles.z - chaseRotation.eulerAngles.z) / windowOpenCount;
        posx = (statusOffset.x - chaseOffset.x) / windowOpenCount;
        posy = (statusOffset.y - chaseOffset.y) / windowOpenCount;
        posz = (statusOffset.z - chaseOffset.z) / windowOpenCount;

        // 目標位置
        statusShowPos = new Vector3(1080, 690, 0);
        inventoryShowPos = new Vector3(1080, 340, 0);
        rankingShowPos = new Vector3(1080, 340, 0);
        equipmentShowPos = new Vector3(445, 110, 0);
        statusHidePos = new Vector3(1080, 850, 0);
        inventoryHidePos = new Vector3(1655, 340, 0);
        rankingHidePos = new Vector3(1655, 340, 0);
        equipmentHidePos = new Vector3(445, -110, 0);

        if (statusWindow)
        {
            statusWindow.setStatus();
        }
    }

    public void StartButton()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーションステートが1最初のワールドの場合
        if (sceneNo == 1)
        {
            // インベントリボタンまたはIキーが押され、カウンタが0の場合
            if ((Input.GetKeyDown(KeyCode.I) || inventoryButton.isOpen()) && (count == 0))
            {
                count = windowOpenCount;
                inventoryButton.resetOpen();

                if (inventory.activeSelf)
                {
                    inventoryOpen = -1;
                    rankingOpen = 0;
                    cameraMove = 2;
                }
                else if (ranking.activeSelf)
                {
                    rankingOpen = -1;
                    inventoryOpen = 1;
                    cameraMove = 1;
                }
                else
                {
                    inventoryOpen = 1;
                    rankingOpen = 0;
                    cameraMove = 3;
                }
            }
            // ランキングボタンまたはRキーが押され、カウンタが0の場合
            else if ((Input.GetKeyDown(KeyCode.R) || rankingButton.isOpen()) && (count == 0))
            {
                count = windowOpenCount;
                rankingButton.resetOpen();

                if (ranking.activeSelf)
                {
                    inventoryOpen = 0;
                    rankingOpen = -1;
                    cameraMove = 2;
                }
                else if (inventory.activeSelf)
                {
                    inventoryOpen = -1;
                    rankingOpen = 1;
                    cameraMove = 1;
                }
                else
                {
                    inventoryOpen = 0;
                    rankingOpen = 1;
                    cameraMove = 3;
                }
            }

            if (count > 0)
            {
                // ウィンドウ表示
                if (count > windowOpenCount / 2)
                {
                    if (inventoryOpen == -1)
                    {
                        // オブジェクトの位置を更新する
                        status.transform.position = Vector3.MoveTowards(status.transform.position, statusHidePos, Time.deltaTime * 20000 / windowOpenCount);
                        inventory.transform.position = Vector3.MoveTowards(inventory.transform.position, inventoryHidePos, Time.deltaTime * 70000 / windowOpenCount);
                        equipment.transform.position = Vector3.MoveTowards(equipment.transform.position, equipmentHidePos, Time.deltaTime * 30000 / windowOpenCount);
                    }
                    if (rankingOpen == -1)
                    {
                        status.transform.position = Vector3.MoveTowards(status.transform.position, statusHidePos, Time.deltaTime * 20000 / windowOpenCount);
                        ranking.transform.position = Vector3.MoveTowards(ranking.transform.position, rankingHidePos, Time.deltaTime * 70000 / windowOpenCount);
                    }
                    count--;
                    if (count == windowOpenCount / 2)
                    {
                        status.SetActive(false);
                        inventory.SetActive(false);
                        equipment.SetActive(false);
                        ranking.SetActive(false);
                    }
                }
                // ウィンドウひっこむ
                else
                {
                    if (inventoryOpen == 1)
                    {
                        status.SetActive(true);
                        inventory.SetActive(true);
                        equipment.SetActive(true);

                        // オブジェクトの位置を更新する
                        status.transform.position = Vector3.MoveTowards(status.transform.position, statusShowPos, Time.deltaTime * 20000 / windowOpenCount);
                        inventory.transform.position = Vector3.MoveTowards(inventory.transform.position, inventoryShowPos, Time.deltaTime * 70000 / windowOpenCount);
                        equipment.transform.position = Vector3.MoveTowards(equipment.transform.position, equipmentShowPos, Time.deltaTime * 30000 / windowOpenCount);
                    }
                    if (rankingOpen == 1)
                    {
                        status.SetActive(true);
                        ranking.SetActive(true);
                        status.transform.position = Vector3.MoveTowards(status.transform.position, statusShowPos, Time.deltaTime * 20000 / windowOpenCount);
                        ranking.transform.position = Vector3.MoveTowards(ranking.transform.position, rankingShowPos, Time.deltaTime * 70000 / windowOpenCount);
                    }
                    count--;
                }
                if (cameraMove == 3)
                {
                    cam.transform.rotation = Quaternion.Euler(statusRotation.eulerAngles.x - difx * count, statusRotation.eulerAngles.y - dify * count, statusRotation.eulerAngles.z - difz * count);
                    cam.transform.position = player.transform.position + new Vector3(statusOffset.x - posx * count, statusOffset.y - posy * count, statusOffset.z - posz * count);
                }
                else if (cameraMove == 2)
                {
                    cam.transform.rotation = Quaternion.Euler(chaseRotation.eulerAngles.x + difx * count, chaseRotation.eulerAngles.y + dify * count, chaseRotation.eulerAngles.z + difz * count);
                    cam.transform.position = player.transform.position + new Vector3(chaseOffset.x + posx * count, chaseOffset.y + posy * count, chaseOffset.z + posz * count);
                }
                if ((count == 0) && (cameraMove == 2))
                {
                    cameraMove = 0;
                }
            }
        }
        // アニメーションステートが0タイトルシーンの場合
        if (sceneNo == 0)
        {
            // 1秒ごとにアニメーションを切り替える
            if (Time.time % 60 > 50)
            {
                animator.SetBool("Swim", true);
            }
            else
            {
                animator.SetBool("Swim", false);
            }

            // Sキーが押されたらStartButtonメソッドを呼ぶ
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.StartButton();
            }

            // 画面遷移
            if (!goNextScene && fade.IsFadeOutComplete())
            {
                GameManager.sceneNo = 1;              // ワールドシーンスタート
                SceneManager.LoadScene("WorldScene"); // ワールドシーンに遷移
                goNextScene = true;                   // 2回目以降の遷移を防ぐためのフラグを立てる
            }
        }
    }

    public int RecalculateKpm(int nowKpm)
    {
        int newKpm = -1;

        return newKpm;
    }

    public void setUserName(string msg)
    {
        savedata.setUserName(msg);
    }

    public void setStatus(string msg)
    {
        savedata.setStatus(msg);
    }

    public void setInventory(string msg)
    {
        savedata.setInventory(msg);
    }

    public void setMedals(string msg)
    {
        savedata.setMedals(msg);
    }

    public int getCameraMove()
    {
        return cameraMove;
    }
}
