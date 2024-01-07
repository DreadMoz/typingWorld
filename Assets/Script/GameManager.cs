using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

enum scene
{
    Title = 0,
    World = 1,
    Typing = 2,
    House = 3
}

public class GameManager : MonoBehaviour
{
    public DataBase db;
    public SaveData savedata;
    public Connection connection;

    static private int sceneNo;             // ワールドシーンの状態番号
    static private int typingTab;           // タイピングステージのタブNo
    static private int kpm;                 // 現在のkpm
    static private int newKpm;              // 直近のタイピング結果のkpm
    static private int numAnswers;          // 回答数
    static private float answerRate;        // 解答率
    static private int typingDataId;        // タイピングデータのJson呼び出しID練習のファイル名は数字
    static private string typingDataName;   // タイピングデータのJson呼び出し用ファイル名
    static private bool typingRandom;       // タイピングをランダムでするかどうか

    static public int SceneNo { get => sceneNo; set => sceneNo = value; }
    static public int TypingTab { get => typingTab; set => typingTab = value; }
    static public int Kpm { get => kpm; set => kpm = value; }
    static public int NewKpm { get => newKpm; set => newKpm = value; }
    static public int NumAnswers { get => numAnswers; set => numAnswers = value; }
    static public float AnswerRate { get => answerRate; set => answerRate = value; }
    static public int TypingDataId { get => typingDataId; set => typingDataId = value; }
    static public string TypingDataName { get => typingDataName; set => typingDataName = value; }
    static public bool TypingRandom { get => typingRandom; set => typingRandom = value; }

    [SerializeField] private StatusUI statusWindow;
    [SerializeField] private float kpmRatio = 0.8f;

    public GameObject player;        // プレイヤーオブジェクト
    public ChibiCat chibiCat;        // 猫ボディ 
    public ChibiCat chibiCat2D;      // 猫ボディ 
    public GameObject cam;           // カメラ
    private Animator animator;       // Playerのアニメーター
    public GameObject inventory;
    public GameObject equip;
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
    Vector2 statusShowPos;
    Vector2 inventoryShowPos;
    Vector2 rankingShowPos;
    Vector2 equipmentShowPos;
    Vector2 statusHidePos;
    Vector2 inventoryHidePos;
    Vector2 rankingHidePos;
    Vector2 equipmentHidePos;

    private int[] oldInventory;
    private int[] newInventory;
    private int[] oldEquip;
    private int[] newEquip;

    [SerializeField] private GameObject hikingHat;
    [SerializeField] private GameObject grassARed;
    [SerializeField] private GameObject battonWoodR;
    [SerializeField] private GameObject spadR;
    [SerializeField] private GameObject whirligigR;
    [SerializeField] private GameObject panR;
    [SerializeField] private GameObject driedFishR;
    [SerializeField] private GameObject meatR;
    [SerializeField] private GameObject battonWoodL;
    [SerializeField] private GameObject spadL;
    [SerializeField] private GameObject whirligigL;
    [SerializeField] private GameObject panL;
    [SerializeField] private GameObject driedFishL;
    [SerializeField] private GameObject meatL;
    [SerializeField] private GameObject spadB;
    [SerializeField] private GameObject panB;
    [SerializeField] private GameObject driedFishB;
    [SerializeField] private GameObject meatB;

    [SerializeField] private GameObject hikingHatHouse;
    [SerializeField] private GameObject grassARedHouse;
    [SerializeField] private GameObject battonWoodRHouse;
    [SerializeField] private GameObject spadRHouse;
    [SerializeField] private GameObject whirligigRHouse;
    [SerializeField] private GameObject panRHouse;
    [SerializeField] private GameObject driedFishRHouse;
    [SerializeField] private GameObject meatRHouse;
    [SerializeField] private GameObject battonWoodLHouse;
    [SerializeField] private GameObject spadLHouse;
    [SerializeField] private GameObject whirligigLHouse;
    [SerializeField] private GameObject panLHouse;
    [SerializeField] private GameObject driedFishLHouse;
    [SerializeField] private GameObject meatLHouse;
    [SerializeField] private GameObject spadBHouse;
    [SerializeField] private GameObject panBHouse;
    [SerializeField] private GameObject driedFishBHouse;
    [SerializeField] private GameObject meatBHouse;

    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "TitleScene")
        {
            GameManager.SceneNo = (int)scene.Title;
        }
        else if (sceneName == "WorldScene")
        {
            if (GameManager.SceneNo != (int)scene.House)
            {
                GameManager.SceneNo = (int)scene.World;
            }
        }
        else if (sceneName == "TypingStage")
        {
            GameManager.SceneNo = (int)scene.Typing;
        }
        
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得

        oldInventory = new int[64];
        newInventory = new int[64];
        oldEquip = new int[10];
        newEquip = new int[10];

        // アニメーションステートが1最初のワールドの場合
        if (SceneNo == (int)scene.World)
        {
            status.SetActive(false);
            inventory.SetActive(false);
            equip.SetActive(false);
            ranking.SetActive(false);
            typingRoom.SetActive(false);
            shopRoom.SetActive(false);
        }
        // アニメーションステートが3タイピング後の場合
        else if (SceneNo == (int)scene.House)
        {
            recalculateKpm();
            inventory.SetActive(false);
            equip.SetActive(false);
            rankingButton.SetActive(false);
            inventoryButton.SetActive(false);
            status.SetActive(true);
            ranking.SetActive(true);
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            status.transform.position = new Vector2(screenWidth * 0.79f, screenHeight * 0.89f);
            ranking.transform.position = new Vector2(screenWidth * 0.79f, screenHeight * 0.44f);
            typingRoom.SetActive(true);
            shopRoom.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (SceneNo != (int)scene.Title)
            {
                if (savedata.getEquipment()[(int)eq.CatBody] != 0)
                {
                    chibiCat.setChara(savedata.getEquipment()[(int)eq.CatBody] - 200);
                    if (chibiCat2D != null)
                    {
                        chibiCat2D.setChara(savedata.getEquipment()[(int)eq.CatBody] - 200);
                    }
                }
            }

            difx = (statusRotation.eulerAngles.x - chaseRotation.eulerAngles.x) / windowOpenCount;
            dify = (statusRotation.eulerAngles.y - chaseRotation.eulerAngles.y) / windowOpenCount;
            difz = (statusRotation.eulerAngles.z - chaseRotation.eulerAngles.z) / windowOpenCount;
            posx = (statusOffset.x - chaseOffset.x) / windowOpenCount;
            posy = (statusOffset.y - chaseOffset.y) / windowOpenCount;
            posz = (statusOffset.z - chaseOffset.z) / windowOpenCount;

            if (statusWindow)
            {
                statusWindow.setStatus();
            }
            changeEquip(0);
            changeEquip(1);
            changeEquip(2);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Startメソッドでエラーが発生しました: " + ex.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーションステートが1最初のワールドの場合
        if (SceneNo == (int)scene.World)
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

            // 画面サイズに基づいてUI要素の位置を計算
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // 目標位置
            statusShowPos = new Vector2(screenWidth * 0.79f, screenHeight * 0.89f);
            inventoryShowPos = new Vector2(screenWidth * 0.79f, screenHeight * 0.44f);
            rankingShowPos = new Vector2(screenWidth * 0.79f, screenHeight * 0.44f);
            equipmentShowPos = new Vector2(screenWidth * 0.33f, screenHeight * 0.13f);
            statusHidePos = new Vector2(screenWidth * 0.79f, screenHeight * 1.15f);
            inventoryHidePos = new Vector2(screenWidth * 1.21f, screenHeight * 0.44f);
            rankingHidePos = new Vector2(screenWidth * 1.21f, screenHeight * 0.44f);
            equipmentHidePos = new Vector2(screenWidth * 0.33f, -screenHeight * 0.12f);
            if (count > 0)
            {
                if (count > windowOpenCount / 2)    // ウィンドウひっこむ
                {
                    if (inventoryOpen == -1)
                    {
                        // オブジェクトの位置を更新する
                        status.transform.position = Vector2.MoveTowards(status.transform.position, statusHidePos, Time.deltaTime * 20000 / windowOpenCount);
                        inventory.transform.position = Vector2.MoveTowards(inventory.transform.position, inventoryHidePos, Time.deltaTime * 70000 / windowOpenCount);
                        equip.transform.position = Vector2.MoveTowards(equip.transform.position, equipmentHidePos, Time.deltaTime * 30000 / windowOpenCount);
                    }
                    if (rankingOpen == -1)
                    {
                        status.transform.position = Vector2.MoveTowards(status.transform.position, statusHidePos, Time.deltaTime * 20000 / windowOpenCount);
                        ranking.transform.position = Vector2.MoveTowards(ranking.transform.position, rankingHidePos, Time.deltaTime * 70000 / windowOpenCount);
                    }
                    count--;
                }
                else if (count == windowOpenCount / 2)
                {
                    status.SetActive(false);
                    inventory.SetActive(false);
                    equip.SetActive(false);
                    ranking.SetActive(false);
                    if (inventoryOpen == -1)
                    {
                        // オブジェクトの位置を確定させる
                        status.transform.position = statusHidePos;
                        inventory.transform.position = inventoryHidePos;
                        equip.transform.position = equipmentHidePos;
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
                        equip.transform.position = equipmentShowPos;
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
                        if (!shopRoom.activeSelf)
                        {
                            equip.SetActive(true);
                        }
                        // オブジェクトの位置を更新する
                        status.transform.position = Vector2.MoveTowards(status.transform.position, statusShowPos, Time.deltaTime * 20000 / windowOpenCount);
                        inventory.transform.position = Vector2.MoveTowards(inventory.transform.position, inventoryShowPos, Time.deltaTime * 70000 / windowOpenCount);
                        equip.transform.position = Vector2.MoveTowards(equip.transform.position, equipmentShowPos, Time.deltaTime * 30000 / windowOpenCount);
                    }
                    if (rankingOpen == 1)
                    {
                        status.SetActive(true);
                        ranking.SetActive(true);
                        status.transform.position = Vector2.MoveTowards(status.transform.position, statusShowPos, Time.deltaTime * 20000 / windowOpenCount);
                        ranking.transform.position = Vector2.MoveTowards(ranking.transform.position, rankingShowPos, Time.deltaTime * 70000 / windowOpenCount);
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
        if (SceneNo == (int)scene.Title)
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
        SoubiUI equipUi = equip.GetComponentInChildren<SoubiUI>();
        inventoryUi.getAllItems();
        equipUi.getAllSoubi();
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
        if (Kpm > NewKpm * kpmRatio)    // 今回の成績が一定の成績以上であれば
        {
            Kpm = savedata.updateKpm(NewKpm);   // kpm更新

            var fbKpms = new Dictionary<string, int>();
            for (int i = 0; i < savedata.getKpms().Length; i++)
            {
                fbKpms[i.ToString()] = savedata.getKpms()[i];
            }
            connection.saveFbKpm(fbKpms);
        }
    }
    public void registerRecentTypingResult()
    {
        int stars = judgeStar(AnswerRate);
        if (savedata.getMedals()[TypingDataId] < stars)
        {
            savedata.setMedalIndex(TypingDataId, stars);
        }
    }

    private int judgeStar(float rate)
    {
        if (rate > 0.95)
        {
            return 4;       // 星3つ
        }
        else if (rate > 0.75)
        {
            return 3;       // 星2つ
        }
        else if (rate > 0.4)
        {
            return 2;       // 星1つ
        }
        else
        {
            return 1;       // 星0こ
        }
    }

    public int getCameraMove()
    {
        return cameraMove;
    }
    public bool getWindowOpen()
    {
        if (inventoryOpen == 1 || rankingOpen == 1)
        {
            return true;
        }
        return false;
    }

    public static void SetTypingDataLevel(int no)
    {
        TypingDataId += no;
        int fileNameId = TypingDataId;
        if (no == 0)
        {
            TypingRandom = false;
        }
        else
        {
            TypingRandom = true;
            if (no == 1)
            {
                fileNameId--;
            }
        }
        TypingDataName = fileNameId.ToString();
    }

    public void changeEquip(int parts)
    {
        switch (parts)
        {
            case 0:     // 両手
                spadB.SetActive(true);          // 全てかばんに付ける
                panB.SetActive(true);
                driedFishB.SetActive(true);
                meatB.SetActive(true);

                battonWoodR.SetActive(false);   // 右手解除
                spadR.SetActive(false);
                whirligigR.SetActive(false);
                panR.SetActive(false);
                driedFishR.SetActive(false);
                meatR.SetActive(false);

                battonWoodL.SetActive(false);   // 左手解除
                spadL.SetActive(false);
                whirligigL.SetActive(false);
                panL.SetActive(false);
                driedFishL.SetActive(false);
                meatL.SetActive(false);

                switch (savedata.getEquipment()[0])     // 右手
                {
                    case 1:
                        spadR.SetActive(true);
                        spadB.SetActive(false);
                        break;
                    case 2:
                        driedFishR.SetActive(true);
                        driedFishB.SetActive(false);
                        break;
                    case 3:
                        meatR.SetActive(true);
                        meatB.SetActive(false);
                        break;
                    case 4:
                        battonWoodR.SetActive(true);
                        break;
                    case 5:
                        whirligigR.SetActive(true);
                        break;
                    case 6:
                        panR.SetActive(true);
                        panB.SetActive(false);
                        break;
                }
                switch (savedata.getEquipment()[3])     // 左手
                {
                    case 1:
                        spadL.SetActive(true);
                        spadB.SetActive(false);
                        break;
                    case 2:
                        driedFishL.SetActive(true);
                        driedFishB.SetActive(false);
                        break;
                    case 3:
                        meatL.SetActive(true);
                        meatB.SetActive(false);
                        break;
                    case 4:
                        battonWoodL.SetActive(true);
                        break;
                    case 5:
                        whirligigL.SetActive(true);
                        break;
                    case 6:
                        panL.SetActive(true);
                        panB.SetActive(false);
                        break;
                }
                break;

            case 1:     // 頭
                hikingHat.SetActive(false);
                switch (savedata.getEquipment()[1])
                {
                    case 151:
                        hikingHat.SetActive(true);
                        break;
                }
                break;

            case 2:     // メガネ
                grassARed.SetActive(false);
                switch (savedata.getEquipment()[2])
                {
                    case 121:
                        grassARed.SetActive(true);
                        break;
                }
                break;
        }
        if (spadBHouse == null)
        {
            return;
        }

        switch (parts)
        {
            case 0:     // 両手
                spadBHouse.SetActive(true);          // 全てかばんに付ける
                panBHouse.SetActive(true);
                driedFishBHouse.SetActive(true);
                meatBHouse.SetActive(true);

                battonWoodRHouse.SetActive(false);   // 右手解除
                spadRHouse.SetActive(false);
                whirligigRHouse.SetActive(false);
                panRHouse.SetActive(false);
                driedFishRHouse.SetActive(false);
                meatRHouse.SetActive(false);

                battonWoodLHouse.SetActive(false);   // 左手解除
                spadLHouse.SetActive(false);
                whirligigLHouse.SetActive(false);
                panLHouse.SetActive(false);
                driedFishLHouse.SetActive(false);
                meatLHouse.SetActive(false);

                switch (savedata.getEquipment()[0])     // 右手
                {
                    case 1:
                        spadRHouse.SetActive(true);
                        spadBHouse.SetActive(false);
                        break;
                    case 2:
                        driedFishRHouse.SetActive(true);
                        driedFishBHouse.SetActive(false);
                        break;
                    case 3:
                        meatRHouse.SetActive(true);
                        meatBHouse.SetActive(false);
                        break;
                    case 4:
                        battonWoodRHouse.SetActive(true);
                        break;
                    case 5:
                        whirligigRHouse.SetActive(true);
                        break;
                    case 6:
                        panRHouse.SetActive(true);
                        panBHouse.SetActive(false);
                        break;
                }
                switch (savedata.getEquipment()[3])     // 左手
                {
                    case 1:
                        spadLHouse.SetActive(true);
                        spadBHouse.SetActive(false);
                        break;
                    case 2:
                        driedFishLHouse.SetActive(true);
                        driedFishBHouse.SetActive(false);
                        break;
                    case 3:
                        meatLHouse.SetActive(true);
                        meatBHouse.SetActive(false);
                        break;
                    case 4:
                        battonWoodLHouse.SetActive(true);
                        break;
                    case 5:
                        whirligigLHouse.SetActive(true);
                        break;
                    case 6:
                        panLHouse.SetActive(true);
                        panBHouse.SetActive(false);
                        break;
                }
                break;

            case 1:     // 頭
                hikingHatHouse.SetActive(false);
                switch (savedata.getEquipment()[1])
                {
                    case 151:
                        hikingHatHouse.SetActive(true);
                        break;
                }
                break;

            case 2:     // メガネ
                grassARedHouse.SetActive(false);
                switch (savedata.getEquipment()[2])
                {
                    case 121:
                        grassARedHouse.SetActive(true);
                        break;
                }
                break;
        }
    }

    //htmlから直でsavedataにアクセスできないため
    public void setUserName(string msg) { savedata.setUserNameFromFireBase(msg);}
    public void setStatus(string msg) { savedata.setStatusFromFireBase(msg);}
    public void setEquipment(string msg) { savedata.setEquipmentFromFireBase(msg);}
    public void setInventory(string msg) { savedata.setInventoryFromFireBase(msg);}
    public void setMedals(string msg) { savedata.setMedalsFromFireBase(msg);}
    public void setKpm(string msg) { savedata.setKpmFromFireBase(msg);}
}
