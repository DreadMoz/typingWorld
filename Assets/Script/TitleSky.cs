using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Collections;

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

    private int loginFlg = 0;


    // Start is called before the first frame update
    void Start()
    {
        TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "ログイン";
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

//        startButton.SetActive(false);   // ログイン完了まで一旦消す
//        StartButton();
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
//        gm.savedata.testEncodeMedals();   // Medalデバッグ
        if (loginFlg == 0)
        {
            startButton.SetActive(false);   // ログイン完了まで一旦消す
            gm.connection.enetLogin();    // OAuthログイン。
        }
        else if (loginFlg == 1)
        {
            if (!firstPush)
            {
                fade.StartFadeOut();
                firstPush = true;
            }
        }
        else if (loginFlg == 2)
        {
            selectNeco();
        }

    }

    public void finishOAuth(string userInfo)
    {
        userData.SetActive(true);
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        string[] parts = userInfo.Split(',');
        string mail = parts[0];
        string first_name = parts.Length > 1 ? parts[1] : "";
        string last_name = parts.Length > 1 ? parts[2] : "";
        string imageUrl = parts.Length > 2 ? parts[3] : "";

        mailText.text = mail;
        firstName.text = first_name;
        lastName.text = last_name;
        StartCoroutine(LoadImage(imageUrl));

        if ((mailText.text.Substring(mailText.text.Length - 13) == "e-net.nara.jp") || gm.gmailToggle.isOn)
        {
            messageText.text = firstName.text + "さんはいいネットならのなかまだね。あしあとデータをけんさく中・・・";
            gm.connection.loadLocal();      // あしあとデータサーチ
        }
        else
        {
            messageText.text = "これはいいネットならのアプリなんだ。e-net.nara.jpのアカウントでログインしてね。";
        }
        reLogin.SetActive(true);        // ログアウトボタン表示
    }
    
    public void testFinishDataLoad()
    {
        string statusJson = @"{
            ""statusData"": {
                ""Email"": ""xxxxx@gmail.com"",
                ""Ou"": """",
                ""LastName"": ""Mori"",
                ""Gold"": 100000,
                ""Stage"": 1,
                ""Ranking"": 20,
                ""Name"": ""Ryosuke"",
                ""RightHand"": 1,
                ""Glasses"": 0,
                ""Head"": 0,
                ""LeftHand"": 0,
                ""CatBody"": 0,
                ""CatFace"": 0,
                ""NickName"": 0,
                ""Kpm"": 444,
                ""Inventory"": [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                ""Items"": [""2"", ""0"", ""0"", ""0""],
                ""Medals"": [""18446744044258085156"", ""0"", ""0"", ""0"", ""0""],
                ""Kpms"": ""0000000000000000000000"",
                ""Settings"": [1, 50, 0, 0, 0, 0, 0, 0, 0, 0]
            }
        }";
        finishDataLoadExtStatus(statusJson);

        string rankingJson = @"{
                'value': [
                    [500,2,'seiji',2,0,0,0,202,0,215,324],[500,3,'yu',3,0,0,1,203,0,219,323],[500,4,'hana',0,0,0,2,204,0,228,323],[500,5,'yui',0,0,0,3,205,0,217,322],[500,6,'rin',0,151,121,4,206,0,214,322],[500,7,'mei',4,0,0,5,208,0,213,321],[500,8,'mio',5,0,0,1,209,0,211,321],[500,9,'saki',0,151,0,1,201,0,229,320],[500,10,'aoi',1,0,0,2,207,0,224,320],[500,11,'yuna',1,0,0,0,201,0,224,319],[500,12,'maika',0,0,0,0,202,0,227,319],[500,13,'kokona',0,0,0,3,203,0,211,318],[500,14,'miku',0,0,121,0,204,0,218,318],[500,15,'nana',0,0,0,0,205,0,228,317],[500,16,'rika',6,0,0,0,206,0,214,317],[500,17,'yuka',1,0,0,0,208,0,227,316],[500,18,'haruka',2,0,0,0,209,0,220,316],[500,19,'emi',3,0,0,1,201,0,224,315],[500,20,'risa',0,0,0,2,207,0,221,315],[500,21,'yuri',0,0,0,3,201,0,213,314],[500,22,'sakura',0,0,0,4,202,0,217,314],[500,23,'rei',4,0,121,5,203,0,211,313],[500,24,'noa',5,0,0,1,204,0,213,313],[500,25,'mai',0,0,0,1,205,0,227,312],[500,26,'rio',1,0,0,2,206,0,213,312],[500,27,'meika',1,0,0,0,208,0,226,311],[500,28,'erika',6,0,0,0,209,0,226,311],[500,29,'airi',1,0,0,3,201,0,217,310],[500,30,'marin',2,0,0,0,207,0,218,310],[500,31,'aya',3,0,0,0,201,0,213,309],[500,32,'mina',0,0,121,0,202,0,212,309],[500,33,'yuko',0,0,121,0,203,0,214,308],[500,34,'kaede',0,0,0,0,204,0,228,308],[500,35,'ayumu',4,0,0,1,205,0,225,307],[500,36,'taiga',5,0,0,2,206,0,221,307],[500,37,'shota',0,0,0,3,208,0,221,306],[500,38,'eito',1,0,0,4,209,0,220,306],[500,39,'reo',1,151,121,5,201,0,222,305],[500,40,'kensei',6,0,0,1,207,0,223,305],[500,41,'shin',1,0,0,1,201,0,223,304],[500,42,'manato',2,151,0,2,202,0,212,304],[500,43,'ryoga',3,0,0,0,203,0,220,303],[500,44,'kanata',0,0,0,0,204,0,229,303],[500,45,'tsubasa',0,0,0,3,205,0,229,302],[500,46,'itsuki',0,0,0,0,206,0,215,302],[500,47,'asahi',4,0,121,0,208,0,213,301],[500,48,'mahiro',5,0,0,0,209,0,224,301],[500,49,'haru',0,0,0,0,201,0,223,300],[500,50,'ikki',1,0,0,0,207,0,218,300],[500,51,'sho',1,0,0,1,201,0,216,299],[500,52,'yuki',6,0,0,2,202,0,226,299],[500,53,'kyou',1,0,0,3,203,0,219,298],[500,54,'ayaka',2,0,0,4,204,0,214,298],[500,55,'sena',3,0,0,5,205,0,228,297],[500,56,'himari',0,0,121,1,206,0,223,297],[500,57,'yume',0,0,0,1,208,0,212,296],[500,58,'aina',0,0,0,2,209,0,214,296],[500,59,'kanon',4,0,0,0,201,0,222,295],[500,60,'ryosuke',5,0,0,0,207,0,222,295],[500,61,'saya',0,0,0,3,201,0,228,294],[500,62,'kaho',1,0,0,0,202,0,221,294],[500,63,'fumi',1,0,0,0,203,0,221,293],[500,64,'sara',6,0,0,0,204,0,223,293],[500,65,'momoka',1,0,121,0,205,0,228,292],[500,66,'sumire',2,0,121,0,206,0,224,292],[500,67,'akari',3,0,0,1,208,0,215,291],[500,68,'hinako',0,0,0,2,209,0,224,291],[500,69,'yuina',0,0,0,3,201,0,211,290],[500,70,'riona',0,0,0,4,207,0,228,290],[500,71,'manami',4,0,0,5,201,0,224,289],[500,72,'sayaka',5,151,121,1,202,0,225,289],[500,73,'nao',0,0,0,1,203,0,226,288],[500,74,'yusuke',1,0,0,2,204,0,221,288],[500,75,'tatsuya',1,151,0,0,205,0,229,287],[500,76,'kazuma',6,0,0,0,206,0,214,287],[500,77,'masato',1,0,0,3,208,0,225,286],[500,78,'shun',2,0,0,0,209,0,222,286],[500,79,'kyohei',3,0,0,0,201,0,214,285],[500,80,'takuya',0,0,121,0,207,0,214,285],[500,81,'naoki',0,0,0,0,201,0,216,284],[500,82,'kenta',0,0,0,0,202,0,224,284],[500,83,'jun',4,0,0,1,203,0,229,283],[500,84,'misaki',5,0,0,2,204,0,215,283],[500,85,'riko',0,0,0,3,205,0,225,282],[500,86,'chinatsu',1,0,0,4,206,0,230,282],[500,87,'kumi',6,0,121,3,207,0,221,281],[500,88,'miyu',1,0,0,5,208,0,216,281],[500,89,'ryou',6,0,0,1,209,0,226,280],[500,90,'naoko',1,0,121,1,201,0,230,280],[500,91,'keiko',2,0,0,2,207,0,223,279],[500,92,'chie',3,0,0,0,201,0,216,279],[500,93,'akiko',0,0,0,0,202,0,222,278],[500,94,'asuka',0,0,0,3,203,0,214,278],[500,95,'kaito',0,0,0,0,204,0,213,277],[500,96,'natsuki',4,0,0,0,205,0,217,277],[500,97,'ryohei',5,0,0,0,206,0,215,276],[500,98,'satoshi',0,0,0,0,208,0,229,276],[500,99,'takahiro',1,0,121,0,209,0,230,275],[500,100,'yasuharu',1,0,121,1,201,0,223,275],[500,101,'yoshiki',6,0,0,2,207,0,217,274],[500,102,'yota',1,0,0,3,201,0,214,274],[500,103,'daigo',2,0,0,4,202,0,211,273],[500,104,'ema',3,0,0,5,203,0,213,273],[500,105,'himawari',0,0,0,1,204,0,218,272],[500,106,'ichika',0,151,121,1,205,0,224,272],[500,107,'juri',0,0,0,2,206,0,226,271],[500,108,'kairi',4,0,0,0,208,0,227,271],[500,109,'runa',5,151,0,0,209,0,221,270],[500,110,'mao',0,0,0,3,201,0,218,270],[500,111,'nagisa',1,0,0,0,207,0,224,269],[500,112,'otoha',1,0,0,0,201,0,214,269],[500,113,'hina',6,0,0,0,202,0,227,268],[500,114,'rena',1,0,121,0,203,0,213,268],[500,115,'suzu',2,0,0,0,204,0,218,267],[500,116,'saiga',3,0,0,1,205,0,219,267],[500,117,'umi',0,0,0,2,206,0,219,266],[500,118,'nami',0,0,0,3,208,0,217,266],[500,119,'wakana',0,0,0,4,209,0,215,265],[500,120,'yuto',4,0,0,5,201,0,222,265],[500,121,'haruto',5,0,0,1,207,0,218,264],[500,122,'yuto',0,0,0,1,201,0,213,264],[500,123,'sota',1,0,121,2,202,0,230,263],[500,124,'yuki',1,0,0,0,203,0,220,263],[500,125,'hayato',6,0,0,0,204,0,216,262],[500,126,'haruki',1,0,0,3,205,0,220,262],[500,127,'ryusei',2,0,0,0,206,0,227,261],[500,128,'kaito',3,0,0,0,208,0,219,261],[500,129,'kota',0,0,0,0,209,0,222,260],[500,130,'yuma',0,0,0,0,201,0,223,260],[500,131,'soma',0,0,0,0,207,0,211,259],[500,132,'riku',4,0,121,1,201,0,218,259],[500,133,'sora',5,0,0,2,202,0,217,258],[500,134,'ryota',0,0,0,3,203,0,229,258],[500,135,'daiki',1,0,0,4,204,0,212,257],[500,136,'minato',1,0,0,5,205,0,218,257],[500,137,'ren',6,0,0,1,206,0,224,256],[500,138,'hinata',1,0,0,1,208,0,226,256],[500,139,'kazuki',2,0,0,2,209,0,216,255],[500,140,'takumi',3,0,0,0,201,0,217,255],[500,141,'hiroto',0,0,121,0,207,0,230,254],[500,142,'ryuto',0,0,0,3,201,0,228,254],[500,143,'yuma',0,0,0,0,202,0,211,253],[500,144,'sosuke',4,0,0,0,203,0,224,253],[500,145,'ryu',5,0,121,0,204,0,220,252],[500,146,'keita',0,0,121,0,205,0,230,252],[500,147,'koki',1,0,0,0,206,0,230,251],[500,148,'toma',1,0,0,0,208,0,212,251],[500,149,'seiji',1,0,0,0,209,0,226,250],[500,150,'yu',1,0,0,0,201,0,211,250]
                ]
            }";
        gm.finishDataLoadExtRanking(rankingJson);
    }

    public void finishDataLoadExtStatus(string statusDataJson)
    {
        message.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        try
        {
            if (statusDataJson != null)
            {
                gm.savedata.setStatusFromLocal(statusDataJson);
                ouText.text = gm.savedata.Ou;
                Debug.Log("ステータスデータをロードしました。");
                ashiato.SetActive(true);
                checkLocalData();
            }
            else
            {
                Debug.Log("ステータスデータがnull");
            }
        }
        catch (Exception ex)
        {
            messageText.text = "データを読み込む時にエラーが発生しました: ";
            Debug.LogError("データの読み込み中に例外発生: " + ex);
        }
    }


    private void checkLocalData()
    {
        Text messageText = message.GetComponentInChildren<Text>();

        if (gm.savedata.Equipment[eq.CatBody] == 0)        // ねこボディなし
        {
            Debug.Log("ネコボディなしGASアクセスへ");
            messageText.text = "あしあとデータがないので、クラウドのデータをさがしに行っています・・・";
            gm.savedata.Settings[se.CatNum] = 0;        // NPC表示なし
            gm.connection.loadGas();    // GSSアクセス。
        }
        else
        {
            Debug.Log("拡張機能正常データあり");
            messageText.text = "あしあとデータがみつかったよ。スタートしましょう。";
            showStart();
        }
    }

    public void finishDataLoadGas(string jsonMsg)
    {
        reLogin.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();

        if (string.IsNullOrEmpty(jsonMsg))
        {
            messageText.text = "クラウドデータがありませんでした。あたらしくつくりましょう。";
            showStart();
        }
        else
        {
<<<<<<< HEAD
            string[] dataParts = jsonMsg.Split(',');

            if (dataParts.Length < 25)
            {
                messageText.text += "\nクラウドデータに問題が生じました。";
                showStart();
                return;
            }

            List<object> dataList = new List<object> {
                dataParts[0], // Email
                dataParts[1], // Ou
                dataParts[2], // LastName
                Convert.ToInt32(dataParts[3]), // Gold
                Convert.ToInt32(dataParts[4]), // Stage
                Convert.ToInt32(dataParts[5]), // Ranking
                dataParts[6], // Name
                Convert.ToInt32(dataParts[7]), // RightHand
                Convert.ToInt32(dataParts[8]), // Glasses
                Convert.ToInt32(dataParts[9]), // Head
                Convert.ToInt32(dataParts[10]), // LeftHand
                Convert.ToInt32(dataParts[11]), // CatBody
                Convert.ToInt32(dataParts[12]), // CatFace
                Convert.ToInt32(dataParts[13]), // NickName
                Convert.ToInt32(dataParts[14]), // Kpm
                dataParts[15] // Kpms
            };

            dataList.AddRange(dataParts[16].Split('|')); // Medals
            dataList.AddRange(dataParts[21].Split('|')); // Items

            gm.savedata.LoadAllDataFromGss(dataList);
            Debug.Log("dataList: " + dataList);
            messageText.text = "クラウドデータを読み込みました。";
=======
            SerializableStatusData userData = JsonUtility.FromJson<SerializableStatusData>(jsonMsg);

            if (userData != null)
            {
                // データをリストに変換
                List<object> dataList = new List<object> {
                    userData.Email,
                    userData.Ou,
                    userData.LastName,
                    userData.Gold,
                    userData.Stage,
                    userData.Ranking,
                    userData.Name,
                    userData.RightHand,
                    userData.Glasses,
                    userData.Head,
                    userData.LeftHand,
                    userData.CatBody,
                    userData.CatFace,
                    userData.NickName,
                    userData.Kpm,
                    userData.Kpms
                };

                dataList.AddRange(userData.Medals);
                dataList.AddRange(userData.Items);

                gm.savedata.LoadAllDataFromGss(dataList);
                Debug.Log("dataList: " + dataList);
                messageText.text = "クラウドデータを読み込みました。";
            }
            else
            {
                messageText.text += "\nクラウドデータに問題が生じました。";
            }
>>>>>>> bd86c7417859cb2653b12a06247f8fef8aca314c
            showStart();
        }
    }

    private void showStart()
    {
        if (gm.savedata.Equipment[eq.CatBody] != 0)
        {
            cat.setChara(gm.savedata.Equipment[eq.CatBody] - 200);
            cat.changeEquipHands(gm.savedata.Equipment[eq.RightHand], gm.savedata.Equipment[eq.LeftHand], gm.checkBagItem());
            cat.changeEquipHead(gm.savedata.Equipment[eq.Head]);
            cat.changeEquipGlasses(gm.savedata.Equipment[eq.Glasses]);
            TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = "スタート";
            loginFlg = 1;
        }
        else
        {
            TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = "つくる";
            loginFlg = 2;
        }
        startButton.SetActive(true);
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
        confirmButton.SetActive(false);
        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);

        gm.savedata.Equipment[eq.CatBody] = 0;
        gm.connection.googleLogout();
    }

    public void finishLogout()
    {
        loginFlg = 0;
        ashiato.SetActive(false);
        userData.SetActive(false);
        reLogin.SetActive(false);
        startButton.SetActive(true);
        Text messageText = message.GetComponentInChildren<Text>();
        messageText.text = "ログアウトしました。";

        TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "ログイン";

    }

    public void handleDataError()
    {
        Debug.Log("handleDataError");
        checkLocalData();
    }

    public void OnRequestTimeout()
    {
        Debug.Log("OnRequestTimeout");
        checkLocalData();
    }

    public void handleInitialData()
    {
        Debug.Log("handleInitialData");
        checkLocalData();
    }

    private void selectNeco()
    {
        Text messageText = message.GetComponentInChildren<Text>();
        messageText.text = "あたらしくデータをつくるね。いっしょにタイピングをするねこをえらんでね。";
        animator.SetBool("Standup", true);
        message.SetActive(true);
        standupButton.SetActive(true);
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        confirmButton.SetActive(true);
        startButton.SetActive(false);
    }
    public void confirmNeco()
    {
        gm.savedata.Equipment[eq.CatBody] = 200 + necoNo;

        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        startButton.SetActive(true);

        gm.savedata.setNewData(mailText.text, firstName.text, lastName.text, ouText.text);
        gm.exportLocal();  // 拡張機能に保存

        Text messageText = message.GetComponentInChildren<Text>();
        messageText.text = "あたらしいデータをつくりました。スタートしましょう。";
        TMP_Text buttonText = startButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = "スタート";
        loginFlg = 1;

        startButton.SetActive(true);   // スタートボタンにして表示
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