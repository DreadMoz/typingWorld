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
    private int startButtonStatus = 0;   // ログインやらスタートやら


    private string[] scopes = { SheetsService.Scope.Spreadsheets };
    private string spreadsheetId = "1jFRfg-f0uomBdj-suHbB6VvBYmvDEbuVj4ErSCJWuhU";
    private SheetsService service;

    [SerializeField]
    private string code;
    [SerializeField]
    private GoogleAuth googleAuth;


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

        StartButton();
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
        if (startButtonStatus == 0)
        {
            gm.connection.OnRequestData();   // 拡張機能にデータ要求

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

    public void finishDataLoad(string msg)
    {
        Text messageText = message.GetComponentInChildren<Text>();

        // JSONデータをデシリアライズして必要な部分を取得
        var combinedData = JsonConvert.DeserializeObject<ExtensionData>(msg);
        if (combinedData == null)
        {
            messageText.text = "あしあとデータに問題が生じました。";
        }
        else
        {
            if (combinedData.rankingData != null)
            {
                gm.savedata.setRankingFromExtension(JsonConvert.SerializeObject(combinedData.rankingData));
                messageText.text = "ランキングデータをよみこみました。";
            }
            if (combinedData.statusData != null)
            {
                gm.savedata.setStatusFromExtension(JsonConvert.SerializeObject(combinedData.statusData));
                messageText.text = "あしあとデータをよみこみました。";
                gm.isExtension = true;
            }
        }
        showNextStartButton();
    }

    private void showNextStartButton()
    {
        IList<object> rowData;
        startButton.SetActive(false);   // 誤動作防止用、ログイン完了まで一旦消す
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        if (gm.savedata.Email == "")
        {
            messageText.text = "あしあとデータがないので、サーバーから取ってきます。";
            // 拡張機能データが取れない場合、GSSにアクセス。
#if UNITY_WEBGL && !UNITY_EDITOR
// ここでGSSアクセス。
// 仮
rowData = new List<object> { "demonstration@e-net.nara.jp", "/公立学校/低学年/OU市/OU小学校", "0603-24", 999, 7, 87, "moru", 0, 0, 0, 0, 0, 0, 0, 333, "001022333444555666777888", 656279013556373796, 476371964491057444, 0471305275021828764, 511767441717405468, 86064876791434, 0, 0, 0, 0 };
gm.savedata.LoadAllDataFromGss(rowData);
#else
//            rowData = new List<object> { "demonstration@e-net.nara.jp", "/公立学校/低学年/OU市/OU小学校", "0603-24", 999, 7, 87, "moru", 6, 0, 121, 3, 206, 0, 0, 333, "001022333444555666777888", 656279013556373796, 476371964491057444, 0471305275021828764, 511767441717405468, 86064876791434, 0, 0, 0, 0 };
//            gm.savedata.LoadAllDataFromGss(rowData);
            gm.isExtension = true;
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
                messageText.text = firstName.text + "さんはいいネットならのなかまだね。スタートしましょう。";

                cat.setChara(gm.savedata.getEquipment()[(int)eq.CatBody] - 200);
                TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
                buttonText.text = "スタート";
                startButtonStatus = 1;

                startButton.SetActive(true);   // スタートボタンにして表示
            }
        }
        else
        {
            messageText.text = "いいネットなら専用のアプリなんだ。e-net.nara.jpのアカウントでログインしてね。";
        }
    }

    public void handleDataError(string mes)
    {
        showNextStartButton();
    }

    public void OnRequestTimeout()
    {
        showNextStartButton();
    }

    public void handleInitialData(string mes)
    {
        showNextStartButton();
    }

    private async Task SendRequestToGAS(string email, string accessToken)
    {
        string url = $"https://script.google.com/a/macros/e-net.nara.jp/s/AKfycbyeY6PBHokpyUB-Ol86UXN1rFlLe2CVQsk2gNtVnRWIkN7pxkE68QenqxfY6VaRj53C/exec?email={email}&authCode={code}";
        Debug.Log("url: " + url);

        try
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            Debug.Log($"AccessToken: {accessToken}");

            webRequest.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                if (responseText.StartsWith("<!DOCTYPE html>"))
                {
                    // HTMLレスポンスが返された場合の処理
                    Debug.LogError("HTML Error Response Received");
                }
                else
                {
                    // 結果の表示
                    string jsonResponse = webRequest.downloadHandler.text;
                    Debug.Log("Received: " + jsonResponse);

                    // 受け取ったJSON文字列（jsonResponse）からGASResponseオブジェクトをデシリアライズ
                    GASResponse response = JsonUtility.FromJson<GASResponse>(jsonResponse);

                    // contentSheetの文字列から不要なエスケープシーケンスを除去して変換
                    string correctedJson = response.contentSheet.Replace(@"\\", @"\").Replace(@"\n", "\n");
                    string jsonWithoutNewlines = correctedJson.Replace("\n", "");

                    // 修正後のJSON文字列を使用してServiceAccountDataオブジェクトをデシリアライズ
                    ServiceAccountData serviceAccountData = JsonUtility.FromJson<ServiceAccountData>(jsonWithoutNewlines);

                    // デシリアライズされたデータの使用
                    //                Debug.Log("Email: " + response.email);
                    //                Debug.Log("Org Unit Path: " + response.orgUnitPath);
                    //                Debug.Log("Sheet Info: " + response.contentSheet);

                    // 応答に基づいて必要な処理を行う
                    ou.text = response.orgUnitPath;
                    GoogleServiceAccount.SheetInfo = response.contentSheet;
                }
            }
        }
        catch (Exception ex)
        {
            // 例外が発生した場合のエラーログ
            Debug.Log($"An error occurred: {ex.Message}");
        }
    }

    private async Task setDataFromSpreadsheet()
    {
        mailText.text = "moriryo@e-net.nara.jp";    // OAuth認証GASアクセスなしの場合
        // メールアドレスを含む行を取得
        await GSheet.FindRowNumber(spreadsheetId, mailText.text);
        var rowData = await GSheet.GetRow();

        gm.savedata.LoadAllDataFromGss(rowData);
    }

    private void OnImageLoaded(Texture2D texture)
    {
        if (texture != null)
        {
            picture.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("画像のロードに失敗しました。");
        }
    }

    public void forceStart()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
        }
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