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

    public async void StartButton()
    {
        try
        {
            if (startButtonStatus == 0)
            {
                startButton.SetActive(false);   // 誤動作防止用、ログイン完了まで一旦消す

                // Google OAuth認証サービスを初期化
                googleAuth = gameObject.AddComponent<GoogleAuth>();

                // 認証してアクセストークンを取得
                var (userInfo, accessToken) = await googleAuth.Authenticate();

                if (userInfo != null)
                {
                    mailText.text = userInfo.Email;
                    firstName.text = userInfo.GivenName;
                    lastName.text = userInfo.FamilyName;
                    StartCoroutine(googleAuth.LoadProfileImage(userInfo.Picture, OnImageLoaded));
                }
                userData.SetActive(true);

                for (int i = 0; i < 3; i++)
                {
                    // ここでいいネットなら判定
                    message.SetActive(true);
                    Text messageText = message.GetComponentInChildren<Text>();
                    if (userInfo.Hd == "e-net.nara.jp")
                    {
                        messageText.text = userInfo.GivenName + "さんはいいネットならのなかまだね。データをロードするね。";
                    }
                    else
                    {
                        messageText.text = "いいネットなら専用のアプリなんだ。e-net.nara.jpのアカウントでログインしてね。";
                    }

                    if (string.IsNullOrEmpty(accessToken))
                    {
                        Debug.LogError("Error: " + "アカウントトークンが得られませんでした。");
                        return;
                    }

                    // アクセストークンを使用してGASにリクエストを送信
                    await SendRequestToGAS(userInfo.Email, accessToken);

                    if (GoogleServiceAccount.SheetInfo == null)
                    {
                        Debug.Log("Error: " + "シートトークンが得られませんでした。" + (i + 1) + "/3");
                    }
                    else
                    {
                        break;
                    }
                }

                await setDataFromSpreadsheet();

                finishDataLoad();
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
        catch (Exception ex)
        {
            message.SetActive(true);
            Text messageText = message.GetComponentInChildren<Text>();
            messageText.text = "ネットワークエラーです。";

            // 例外をキャッチした場合、エラーメッセージをログに記録またはコンソールに出力
            Console.WriteLine($"Authentication failed: {ex.Message}");
            // 必要に応じて、エラー情報を含む例外をスロー
            throw new ApplicationException("Authentication failed.", ex);
        }
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
        // メールアドレスを含む行を取得
        await GSheet.FindRowNumber(spreadsheetId, mailText.text);
        var rowData = await GSheet.GetRow();

        gm.savedata.loadAllDataFromGss(rowData);
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
