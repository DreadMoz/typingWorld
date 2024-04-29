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
using System.Collections;

[System.Serializable]
public class ResponseData       // GASデータ受信用フォーマット
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
    private Text ouText; // データ表示用
    [SerializeField]
    private Text firstName; // データ表示用
    [SerializeField]
    private Text lastName; // データ表示用
    [SerializeField]
    private Text mailText; // データ表示用
    [SerializeField]
    private Image picture; // データ表示用

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
    [SerializeField]
    private GameObject ashiato;

    private Animator animator;
    private int necoNo = 1;
    private bool firstPush = false;      // スタートボタンが2回以上押されないようにするためのフラグ
    private bool goNextScene = false;    // ワールドシーンに遷移するためのフラグ

    [SerializeField]
    private string code;
    [SerializeField]
    private GoogleAuth googleAuth;

    private bool loginFlg = false;


    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
//        startButton.SetActive(false);   // ログイン完了まで一旦消す
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_Rotation", 330f);
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得

        reLogin.SetActive(false);
        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        userData.SetActive(false);
        message.SetActive(false);
        ashiato.SetActive(false);
        gm.savedata.Equipment[eq.CatBody] = 0;
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
        if (!loginFlg)
        {
            gm.connection.enetLogin();   // OAuth認証要求
        }
        else
        {
            if (!firstPush)
            {
                fade.StartFadeOut();
                firstPush = true;
            }
        }
    }

    public void finishOAuth(string userInfo)
    {
        userData.SetActive(true);
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        string[] parts = userInfo.Split(',');
        string mail = parts[0];
        string name = parts.Length > 1 ? parts[1] : "";
        string imageUrl = parts.Length > 2 ? parts[2] : "";

        mailText.text = mail;
        int spaceIndex = name.IndexOf(' ');
        if (spaceIndex != -1)
        {
            firstName.text = name.Substring(0, spaceIndex);
            lastName.text = name.Substring(spaceIndex + 1);
        }
        StartCoroutine(LoadImage(imageUrl));

        if (mailText.text.Substring(mailText.text.Length - 13) == "e-net.nara.jp")
        {
            messageText.text += firstName.text + "さんはいいネットならのなかまだね。あしあとデータをさがします。";
            gm.connection.loadExtension();
        }
        else
        {
            startButton.SetActive(false);
            messageText.text = "これはいいネットならのアプリなんだ。e-net.nara.jpのアカウントでログインしてね。";
        }
        reLogin.SetActive(true);
    }

    public void finishDataLoad(string msg)
    {
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        // JSONデータをデシリアライズして必要な部分を取得
        var combinedData = JsonConvert.DeserializeObject<ExtensionData>(msg);
        if (combinedData == null)
        {
            messageText.text += "あしあとデータに問題がおこったよ〜〜";
            Debug.Log("あしあとデータに問題がおこったよ");
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
                gm.savedata.Settings[se.Extension] = 1;
                Debug.Log("gm.savedata.settings[se.Extension]: " + gm.savedata.Settings[se.Extension]);
            }
            ashiato.SetActive(true);
            ouText.text = gm.savedata.Ou;
            messageText.text += "あしあとデータをよみこみました。";
            Debug.Log("あしあとデータをよみこみました。");
        }
        checkExtensionData();
    }

    private void checkExtensionData()
    {
        player.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        if (gm.savedata.Settings[se.Extension] == 0)
        {
            messageText.text = "あしあとデータがないので、クラウドから取ってきます。ちょっとまってね。";
            gm.connection.loadGas();    // GSSアクセス。

        }
        if (gm.savedata.Equipment[eq.CatBody] != 0)
        {
            cat.setChara(gm.savedata.Equipment[eq.CatBody] - 200);
            TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = "スタート";
            loginFlg = true;

            startButton.SetActive(true);   // スタートボタンにして表示
        }
        else
        {
            selectNeco();
        }
    }

    public void finishDataLoadGas(string jsonMsg)
    {
        Text messageText = message.GetComponentInChildren<Text>();

        if (string.IsNullOrEmpty(jsonMsg))
        {
            messageText.text += "\nGASデータがありませんでした。";
        }
        else
        {
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(jsonMsg);

            if (responseData.done && !string.IsNullOrEmpty(responseData.response.result))
            {
                string[] dataParts = responseData.response.result.Split(',');
                List<object> dataList = new List<object>(dataParts);
                gm.savedata.LoadAllDataFromGss(dataList);
                messageText.text += "\nGASデータをよみこみました。";
            }
            else
            {
                messageText.text += "\nGASデータに問題が生じました。";
            }
        }
    }

    IEnumerator LoadImage(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // リクエストを送信し、レスポンスを待つ
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // 正常に画像を取得できた場合、TextureをImageに設定する
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                picture.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public void googleLogout()
    {
        gm.savedata.Equipment[eq.CatBody] = 0;
        gm.connection.googleLogout();
    }

    public void finishLogout()
    {
        loginFlg = false;
        ashiato.SetActive(false);
        player.SetActive(false);
        userData.SetActive(false);
        reLogin.SetActive(false);
        startButton.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();
        messageText.text = "ログアウトしました。";

        TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "ログイン";

    }

    public void handleDataError(string mes)
    {
        checkExtensionData();
    }

    public void OnRequestTimeout()
    {
        checkExtensionData();
    }

    public void handleInitialData(string mes)
    {
        checkExtensionData();
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
        gm.savedata.Equipment[eq.CatBody] = 200 + necoNo;

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