using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;
//using UnityEditor.MemoryProfiler;

public class TitleSky : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 0.5f;
    private Material skyboxMaterial;

    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject player;        // プレイヤーオブジェクト
    [SerializeField]
    private Fade fade;                // フェード用オブジェクト
    [SerializeField]
    private ChibiCat cat;                // ねこオブジェクト

    [SerializeField]
    private Text firstNameText; // ダミーデータ表示用
    [SerializeField]
    private Text lastNameText; // ダミーデータ表示用
    [SerializeField]
    private Text mailText; // ダミーデータ表示用
    [SerializeField]
    private RawImage image; // ダミーデータ表示用

    [SerializeField]
    private GameObject startButton; // startボタン
    [SerializeField]
    private GameObject userData; // ユーザーデータ
    [SerializeField]
    private GameObject message; // メッセージボックス

    [SerializeField]
    private GameObject standupButton; // standupボタン
    [SerializeField]
    private GameObject nextButton; // nextボタン
    [SerializeField]
    private GameObject prevButton; // prevボタン
    [SerializeField]
    private GameObject confirmButton; // confirmボタン

    private Animator animator;
    private int necoNo = 1;
    private bool firstPush = false;      // スタートボタンが2回以上押されないようにするためのフラグ
    private bool goNextScene = false;    // ワールドシーンに遷移するためのフラグ
    private int startButtonStatus = 0;   // ログインやらスタートやら

    // Start is called before the first frame update
    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_Rotation", 330f);
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得

        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        userData.SetActive(false);
        message.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

        // Sキーが押されたらStartButtonメソッドを呼ぶ
        if (Input.GetKeyDown(KeyCode.S))
        {
//            this.StartButton();
        }

        // 画面遷移
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            GameManager.sceneNo = (int)scene.World;      // ワールドシーンスタート
            SceneManager.LoadScene("WorldScene"); // ワールドシーンに遷移
            goNextScene = true;                   // 2回目以降の遷移を防ぐためのフラグを立てる
        }
    }

    public void StartButton()
    {
        if (startButtonStatus == 0)
        {
            startButton.SetActive(false);   // 誤動作防止用、ログイン完了まで一旦消す
            gm.connection.fbAuth();
        }
        else if (startButtonStatus == 1)
        {
            if (!firstPush)
            {
                fade.StartFadeOut();
                firstPush = true;
            }
        }
    }

    public void finishAuth()
    {
        userData.SetActive(true);
        gm.connection.loadFbData();
    }

    public void showDomainError()
    {
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();
        messageText.text = "いいネットなら専用アプリです。e-net.nara.jpのアカウントでログインしてね。";
    }

    public void finishDataLoad()
    {
        Debug.Log("gm.savedata.getEquipment()[(int)eq.CatBody]: " + gm.savedata.getEquipment()[(int)eq.CatBody]);
        if (gm.savedata.getEquipment()[(int)eq.CatBody] == 0)      // catBodyがない状態なら
        {
            selectNeco();
        }
        else
        {
            cat.setChara(gm.savedata.getEquipment()[(int)eq.CatBody] - 200);
            TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = "スタート";
            startButtonStatus = 1;

            startButton.SetActive(true);   // スタートボタンにして表示
        }
    }
    public void setDummyData()
    {
        Thread.Sleep(300);
        firstNameText.text=gm.savedata.getUserName();
        lastNameText.text="0000-00";
        mailText.text="abc-123-xyz@e-net.nara.jp";
        image.texture = Resources.Load<Texture2D>("necoHand");
        userData.SetActive(true);

        Thread.Sleep(300);
    }
    private void selectNeco()
    {
        message.SetActive(true);
        animator.SetBool("Standup", true);
        standupButton.SetActive(true);
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        confirmButton.SetActive(true);
        startButton.SetActive(false);
    }
    public void confirmNeco()
    {
        var fbNecoBody = new Dictionary<string, int>
        {
            { ((int)eq.CatBody).ToString(), 200 + necoNo }
        };

        message.SetActive(false);
        gm.savedata.setEquipmentIndex((int)eq.CatBody, 200 + necoNo);
        gm.connection.saveFbEquipment(fbNecoBody);
        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        startButton.SetActive(true);

        fade.StartFadeOut();
        firstPush = true;
        /*
        TMP_Text startText = startButton.GetComponentInChildren<TMP_Text>();
        startText.text = "スタート";
        startButtonStatus = 1;

        startButton.SetActive(true);   // スタートボタンにして表示
        */
    }
    public void updownNeco()
    {
        TMP_Text standText = standupButton.GetComponentInChildren<TMP_Text>();
        if (animator.GetBool("Standup"))
        {
            standText.text = "↑";
            animator.SetBool("Standup", false);
        }
        else
        {
            standText.text = "↓";
            animator.SetBool("Standup", true);
        }
    }
    public void nextNeco()
    {
        necoNo++;
        if (necoNo > 9)
        {
            necoNo = 0;
        }
        cat.setChara(necoNo);
    }
    public void prevNeco()
    {
        necoNo--;
        if (necoNo < 0)
        {
            necoNo = 9;
        }
        cat.setChara(necoNo);
    }
}
