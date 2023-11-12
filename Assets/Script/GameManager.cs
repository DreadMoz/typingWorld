using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

enum scene
{
    Title = 0,
    World = 1,
    Typing = 2,
    House = 3
}

public class GameManager : MonoBehaviour
{
    public Database db;
    public SaveData savedata;
    public Connection connection;

    static public int sceneNo;      // ワールドシーンの状態番号
    static public int kpm;          // 現在のkpm
    static public int newKpm;       // 直近のタイピング結果のkpm

    [SerializeField]
    private StatusUI statusWindow;
    [SerializeField]
    private float kpmRatio = 0.8f;

    public GameObject player;        // プレイヤーオブジェクト
    public ChibiCat chibiCat;        // 猫ボディ 
    public GameObject cam;           // カメラ
    private Animator animator;       // Playerのアニメーター
    public GameObject inventory;
    public GameObject equipment;
    public GameObject ranking;
    public GameObject status;
    public GameObject typingRoom;
    public GameObject shopRoom;

    public GameObject inventoryButton;  // インベントリボタン
    public GameObject rankingButton;    // ランキングボタン

    [SerializeField]
    private int windowOpenCount = 20;    // ウィンドウが開くフレーム数
    private int count = 0;               // カウンタ
    private int inventoryOpen = 0;
    private int rankingOpen = 0;
    private int cameraMove = 0;          // 0:標準 1:右回転 2:左回転 3:ズームイン

    Vector3 chaseOffset = new Vector3(0f, 8f, -14f);
    Quaternion chaseRotation = Quaternion.Euler(18.5f, 0f, 0f);
    Vector3 statusOffset = new Vector3(1.4f, 1.3f, -4f);
    Quaternion statusRotation = Quaternion.Euler(5f, 0f, 0f);

    private float difx, dify, difz, posx, posy, posz;

    // 目標位置
    Vector3 statusShowPos;
    Vector3 inventoryShowPos;
    Vector3 rankingShowPos;
    Vector3 equipmentShowPos;
    Vector3 statusHidePos;
    Vector3 inventoryHidePos;
    Vector3 rankingHidePos;
    Vector3 equipmentHidePos;

    private int[] oldInventory;
    private int[] newInventory;
    private int[] oldEquip;
    private int[] newEquip;

    private void Awake()
    {
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得
        oldInventory = new int[64];
        newInventory = new int[64];
        oldEquip = new int[10];
        newEquip = new int[10];

    }
    // Start is called before the first frame update
    void Start()
    {
        if (sceneNo != (int)scene.Title)
        {
            if (savedata.getEquipment()[(int)eq.CatBody] != 0)
            {
                chibiCat.setChara(savedata.getEquipment()[(int)eq.CatBody] - 200);
            }
        }

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

        // アニメーションステートが1最初のワールドの場合
        if (sceneNo == (int)scene.World)
        {
            status.SetActive(false);
            inventory.SetActive(false);
            equipment.SetActive(false);
            ranking.SetActive(false);
            typingRoom.SetActive(false);
            shopRoom.SetActive(false);
        }
        // アニメーションステートが3タイピング後の場合
        else if (sceneNo == (int)scene.House)
        {
            recalculateKpm();
            inventory.SetActive(false);
            equipment.SetActive(false);
            rankingButton.SetActive(false);
            inventoryButton.SetActive(false);
            status.SetActive(true);
            ranking.SetActive(true);
            status.transform.position = statusShowPos;      // ステータス表示位置
            ranking.transform.position = rankingShowPos;    // ランキング表示位置
            typingRoom.SetActive(true);
            shopRoom.SetActive(false);
        }
        if (statusWindow)
        {
            statusWindow.setStatus();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーションステートが1最初のワールドの場合
        if (sceneNo == (int)scene.World)
        {
            // インベントリボタンまたはIキーが押され、カウンタが0の場合
         //   if ((Input.GetKeyDown(KeyCode.I) || inventoryButton.isOpen()) && (count == 0))
            if (inventoryButton.GetComponent<OpenButton>().isOpen() && (count == 0))
                {
                count = windowOpenCount;
                inventoryButton.GetComponent<OpenButton>().resetOpen();

                if (inventory.activeSelf)   // インベントリ表示中なら
                {
                    inventoryOpen = -1;         // インベントリ引っ込める
                    checkInventory();
                    rankingOpen = 0;            // ランキングなんもなし
                    cameraMove = 2;             // カメラは引き
                }
                else if (ranking.activeSelf)  // ランキング表示中なら
                {
                    rankingOpen = -1;           // ランキング引っ込める
                    inventoryOpen = 1;          // インベントリでてくる
                    keepInventory();
                    cameraMove = 1;             // カメラ動作なし
                }
                else                        // ワールド通常表示中なら
                {
                    inventoryOpen = 1;          // インベントリでてくる
                    keepInventory();
                    rankingOpen = 0;            // ランキングなんもなし
                    cameraMove = 3;             // カメラは寄り
                }
            }
            // ランキングボタンまたはRキーが押され、カウンタが0の場合
            // else if ((Input.GetKeyDown(KeyCode.R) || rankingButton.isOpen()) && (count == 0))
            else if (rankingButton.GetComponent<OpenButton>().isOpen() && (count == 0))
            {
                count = windowOpenCount;
                rankingButton.GetComponent<OpenButton>().resetOpen();

                if (ranking.activeSelf)         // ランキング表示中なら
                {
                    inventoryOpen = 0;              // インベントリなんもなし
                    rankingOpen = -1;               // ランキング引っ込める
                    cameraMove = 2;                 // カメラは引き
                }
                else if (inventory.activeSelf)   // インベントリ表示中なら
                {
                    inventoryOpen = -1;             // インベントリ引っ込める
                    checkInventory();
                    rankingOpen = 1;                // ランキングでてくる
                    cameraMove = 1;                 // カメラ動作なし
                }
                else                            // ワールド通常表示中なら
                {
                    inventoryOpen = 0;              // インベントリなんもなし
                    rankingOpen = 1;                // ランキングでてくる
                    cameraMove = 3;                 // カメラは寄り
                }
            }

            if (count > 0)
            {
                if (count > windowOpenCount / 2)    // ウィンドウひっこむ
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
                }
                else if (count == windowOpenCount / 2)
                {
                    status.SetActive(false);
                    inventory.SetActive(false);
                    equipment.SetActive(false);
                    ranking.SetActive(false);
                    if (inventoryOpen == -1)
                    {
                        // オブジェクトの位置を確定させる
                        status.transform.position = statusHidePos;
                        inventory.transform.position = inventoryHidePos;
                        equipment.transform.position = equipmentHidePos;
                    }
                    if (rankingOpen == -1)
                    {
                        status.transform.position = statusHidePos;
                        ranking.transform.position = rankingHidePos;
                    }
                    count--;
                }
                else if (count == 1)
                {
                    if (inventoryOpen == 1)
                    {
                        // オブジェクトの位置を確定させる
                        status.transform.position = statusShowPos;
                        inventory.transform.position = inventoryShowPos;
                        equipment.transform.position = equipmentShowPos;
                    }
                    if (rankingOpen == 1)
                    {
                        status.transform.position = statusShowPos;
                        ranking.transform.position = rankingShowPos;
                    }
                    count--;
                }
                else   // ウィンドウでてくる
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
                if (cameraMove == 3)            // カメラより
                {
                    cam.transform.rotation = Quaternion.Euler(statusRotation.eulerAngles.x - difx * count, statusRotation.eulerAngles.y - dify * count, statusRotation.eulerAngles.z - difz * count);
                    cam.transform.position = player.transform.position + new Vector3(statusOffset.x - posx * count, statusOffset.y - posy * count, statusOffset.z - posz * count);
                }
                else if (cameraMove == 2)       // カメラひき
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
        if (sceneNo == (int)scene.Title)
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
        }
    }

    private void keepInventory()
    {
        oldInventory = (int[])savedata.getInventory().Clone();    // インベントリを開いた時の並びを保存しておく
        oldEquip = (int[])savedata.getEquipment().Clone();        // インベントリを開いた時の装備を保存しておく
    }

    private void checkInventory()
    {
        InventryUI inventoryUi = inventory.GetComponentInChildren<InventryUI>();
        EquipmentUI equipUi = equipment.GetComponentInChildren<EquipmentUI>();
        inventoryUi.getAllItems();
        equipUi.getAllEquipments();
        newInventory = savedata.getInventory();     // 現在のインベントリの並びを保存
        newEquip = savedata.getEquipment();         // 現在の装備を保存
        var updatesInventory = new Dictionary<string, int>();
        var updatesEquipment = new Dictionary<string, int>();

        if (oldInventory == null || oldEquip == null)    // 何かの手違いで変更前がnullの場合抜ける
        {
            return;
        }

        for (int i = 0; i< oldInventory.Length; i++)
        {
            if (oldInventory[i] != newInventory[i])     // インベントリを開いたときと変化があるかチェック
            {
                updatesInventory.Add(i.ToString(), newInventory[i]);
            }
        }
        if (updatesInventory.Count > 0)
        {
            connection.saveFbInventory(updatesInventory);   // 変化があればFbに保存
        }

        // RightHnad,Glasses(121),Head(151),LeftHand,  CatBody(201),CatFace(101),NickName(211)
        for (int i = 0; i<oldEquip.Length; i++)
        {
            if (oldEquip[i] != newEquip[i])     // インベントリを開いたときと変化があるかチェック
            {
                updatesEquipment.Add(i.ToString(), newEquip[i]);
            }
        }
        if (updatesEquipment.Count > 0)
        {
            connection.saveFbEquipment(updatesEquipment);   // 変化があればFbに保存
        }
        oldInventory = null;        // データクリア
        oldEquip = null;
    }

    public void recalculateKpm()
    {
        if (kpm > newKpm * kpmRatio)    // 今回の成績が一定の成績以上であれば
        {
            kpm = savedata.updateKpm(newKpm);   // kpm更新

            var fbKpms = new Dictionary<string, int>();
            for (int i = 0; i < savedata.getKpms().Length; i++)
            {
                fbKpms[i.ToString()] = savedata.getKpms()[i];
            }
            connection.saveFbKpm(fbKpms);
        }
    }
    public int getCameraMove()
    {
        return cameraMove;
    }

    //htmlから直でsavedataにアクセスできないため
    public void setUserName(string msg) { savedata.setUserName(msg);}
    public void setStatus(string msg) { savedata.setStatus(msg);}
    public void setEquipment(string msg) { savedata.setEquipment(msg);}
    public void setInventory(string msg) { savedata.setInventory(msg);}
    public void setMedals(string msg) { savedata.setMedals(msg);}
    public void setKpm(string msg) { savedata.setKpm(msg);}
}
