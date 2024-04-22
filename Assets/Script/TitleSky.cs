using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Networking;
using Google.Apis.Sheets.v4;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
//using UnityEditor.MemoryProfiler;
//using UnityEditor.MemoryProfiler;
[System.Serializable]
public class ResponseData
{
    public bool done;
    public Response response;
}

[System.Serializable]
public class Response
{
    public string type;
    public string result;
}
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
    private ChibiCat cat;             // ねこオブジェクト

    [SerializeField]
    private Text ou; // ダミーデータ表示用
    [SerializeField]
    private Text firstName; // ダミーデータ表示用
    [SerializeField]
    private Text lastName; // ダミーデータ表示用
    [SerializeField]
    private Text mailText; // ダミーデータ表示用
    [SerializeField]
    private Image picture; // ダミーデータ表示用

    [SerializeField]
    private GameObject startButton; // startボタン
    [SerializeField]
    private GameObject userData; // ユーザーデータ
    [SerializeField]
    private GameObject message; // メッセージボックス
    [SerializeField]
    private GameObject reLogin; // ログインしなおす

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

    [SerializeField]
    private string code;
    [SerializeField]
    private GoogleAuth googleAuth;


    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
        startButton.SetActive(false);   // ログイン完了まで一旦消す
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_Rotation", 330f);
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得

        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        userData.SetActive(false);
        message.SetActive(false);

        gm.connection.OnRequestData();   // 拡張機能にデータ要求
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
            GameManager.SceneNo = (int)scene.World;      // ワールドシーンスタート
            SceneManager.LoadScene("WorldScene"); // ワールドシーンに遷移
            goNextScene = true;                   // 2回目以降の遷移を防ぐためのフラグを立てる
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

    public void finishDataLoad(string msg)
    {
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        // JSONデータをデシリアライズして必要な部分を取得
        var combinedData = JsonConvert.DeserializeObject<ExtensionData>(msg);
        if (combinedData == null)
        {
            messageText.text = "あしあとデータに問題がおこったよ〜〜";
        }
        else
        {
            if (combinedData.rankingData != null)
            {
                gm.savedata.setRankingFromExtension(JsonConvert.SerializeObject(combinedData.rankingData));
            }
            if (combinedData.statusData != null)
            {
                gm.savedata.setStatusFromExtension(JsonConvert.SerializeObject(combinedData.statusData));

                gm.savedata.settings[se.Extension] = 1;
            }
            messageText.text = "あしあとデータをよみこみました。";
        }
        accessGss();
    }

    private void accessGss()
    {
        player.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        if (gm.savedata.Email == "")
        {
            messageText.text = "あしあとデータがないので、クラウドから取ってきます。ちょっとまってね。";

#if UNITY_WEBGL && !UNITY_EDITOR
            gm.connection.loadGas();    // ここでGSSアクセス。
#endif
        }
        mailText.text = gm.savedata.Email;

        // あしあとデータまたはサーバーからデータ取得後。ここでいいネットなら判定。全くの新規、外部からのアクセスの可能性もある。
        if (mailText.text.Substring(mailText.text.Length - 13) == "e-net.nara.jp")
        {
            Debug.Log("gm.savedata.equipment[eq.CatBody]: " + gm.savedata.equipment[eq.CatBody]);
            if (gm.savedata.equipment[eq.CatBody] == 0)      // catBodyがない状態なら 新規作成
            {
                selectNeco();
            }
            else
            {
                userData.SetActive(true);       // ユーザーデータウィンドウ表示
                firstName.text = gm.savedata.userName;
                lastName.text = gm.savedata.lastName;
                ou.text = gm.savedata.Ou;
                messageText.text += firstName.text + "さんはいいネットならのなかまだね。スタートしましょう。";

                cat.setChara(gm.savedata.getEquipment()[(int)eq.CatBody] - 200);
                TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
                buttonText.text = "スタート";

                startButton.SetActive(true);   // スタートボタンにして表示
            }
        }
        else
        {
            messageText.text = "いいネットなら専用のアプリなんだ。e-net.nara.jpのアカウントでログインしてね。";
        }
    }

    public void finishDataLoadGas(string jsonMsg)
    {
        Text messageText = message.GetComponentInChildren<Text>();

        if (string.IsNullOrEmpty(jsonMsg))
        {
            messageText.text = "GASデータに問題が生じました。";
        }
        else
        {
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(jsonMsg);

            if (responseData.done && !string.IsNullOrEmpty(responseData.response.result))
            {
                string[] dataParts = responseData.response.result.Split(',');
                List<object> dataList = new List<object>(dataParts);
                gm.savedata.LoadAllDataFromGss(dataList);
                messageText.text = "GASデータをよみこみました。";
            }
            else
            {
                messageText.text = "GASデータに問題が生じました。";
            }
        }
        accessGss();
    }

    public void handleDataError(string mes)
    {
        accessGss();
    }

    public void OnRequestTimeout()
    {
        accessGss();
    }

    public void handleInitialData(string mes)
    {
        accessGss();
    }

    public void setDummyData()
    {
        Thread.Sleep(300);
        firstName.text=gm.savedata.getUserName();
        ou.text="0000-00";
        mailText.text="abc-123-xyz@e-net.nara.jp";
//        image.texture = Resources.Load<Texture2D>("necoHand");
        userData.SetActive(true);

        Thread.Sleep(300);
    }
    private void selectNeco()
    {
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();
        messageText.text = "あたらしくデータをつくるね。いっしょにタイピングをするねこをえらんでね。";
        animator.SetBool("Standup", true);
        standupButton.SetActive(true);
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        confirmButton.SetActive(true);
        startButton.SetActive(false);
    }
    public void confirmNeco()
    {
        message.SetActive(false);
        gm.savedata.setEquipmentIndex(eq.CatBody, 200 + necoNo);
        int[] necoBody = { 200 + necoNo };
        gm.savedata.saveGssItems(eq.CatBody, eq.CatBody, necoBody);

        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        startButton.SetActive(true);

        fade.StartFadeOut();
        firstPush = true;
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