using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System.Globalization;
using static TypingSoft;
using System.IO;
using UnityEngine.Networking;
using TMPro;

public class TypingSoft : MonoBehaviour
{
    // Assist Keyboard JIS
    private static AssistKeyboardJIS AssistKeyboardObj;

    [SerializeField]
    private GameObject player;        // プレイヤーオブジェクト
    private Animator animator;
    public GameObject targetCam;

    public float totalTime = 60.0f; // タイマーの総時間（秒）
    private float currentTime; // 現在の経過時間
    private bool isTimerRunning = false; // タイマーが実行中かどうかのフラグ

    private static bool spaceStart;
    private static bool spaceEnd;
    // 入力受け付け
    private static bool isInputValid;
    // タイピングの正誤判定器
    private static List<List<string>> typingJudge;
    // index 類
    private static int index;
    private static List<List<int>> indexAdd = new List<List<int>>();
    private static List<List<int>> sentenceIndex = new List<List<int>>();
    private static List<List<int>> sentenceValid = new List<List<int>>();
    public static string CurrentTypingSentence { private set; get; } = "";

    // ミスタイプ記録
    private static bool isRecMistype;
    private static bool isSentenceMistyped;

    //　問題の日本語文
    private string[] qJ;
    //　問題のひらがな文
    private string[] qH;

    //　日本語表示テキスト
    private Text UIJ;
    //　ひらがな表示テキスト
    private Text UIH;
    //　ローマ字表示テキスト
    private TMP_Text UIR;
    //　終了表示テキスト
    private Text END;
    //　カウントダウン表示テキスト
    private Text UICountDown;
    //　残り時間表示テキスト
    private Text UITimer;

    //　日本語問題
    private string nQJ;
    //　ひらがな問題
    private string nQH;
    //　ローマ字問題
    private string nQR;


    //　問題番号
    private int numberOfQuestion;
    //　問題の何文字目か
    private int indexOfString;

    //　入力した文字列テキスト
    private TMP_Text UII;
    //　正解キー数
    private int correctN;
    //　コンボ数
    private int comboN;
    //　失敗数
    private int mistakeN;

    //　正解数表示用テキストUI
    private Text UIcorrect;
    //　正解した文字列を入れておく
    private string correctString;

    //　失敗数表示用テキストUI
    private Text UImistake;

    //　1分間あたりの入力キー数表示用テキストUI
    private Text UIkpm;
    private float kpm;

    //　コンボ表示用テキストUI
    private Text UIcombo;

    // 回答数
    private int answers;
    //　正解率
    private float correctAR;
    //　正解率表示用テキストUI
    private Text UIcorrectAR;
    private bool firstEnd = true;
    // お題が一巡した回数
    private int returnCount;

    private ThemeCollection themeCollection;
    private List<Theme> shuffledThemes = new List<Theme>();
    private int currentThemeIndex = 0;

    [Serializable]
    public class Theme
    {
        public int id;
        public string kanji;
        public string hiragana;
    }

    [Serializable]
    public class ThemeCollection
    {
        public Theme[] themes;
    }


    void Start()
    {
        // スペースでスタート状態にする
        spaceStart = true;
        // スペースでエンド状態を解除する
        spaceEnd = false;
        // 入力禁止状態にする
        isInputValid = false;

        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得
        player.transform.LookAt(targetCam.transform);   // カメラを向く

        animator.SetFloat("walkSpeed", 1.0f);
        animator.SetFloat("moveSpeed", 1.0f);
        animator.SetFloat("runSpeed", 1.0f);

        //　テキストUIを取得
        UIJ = transform.Find("InputPanel/QuestionJ").GetComponent<Text>();
        UIH = transform.Find("InputPanel/QuestionH").GetComponent<Text>();
        UIR = transform.Find("InputPanel/QuestionR").GetComponent<TMP_Text>();
        UII = transform.Find("InputPanel/Input").GetComponent<TMP_Text>();
        END = transform.Find("InputPanel/End").GetComponent<Text>();
        UICountDown = transform.Find("InputPanel/CountDown").GetComponent<Text>();
        UIcorrect = transform.Find("DataPanel/Correct").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/CorrectAR").GetComponent<Text>();
        UIcombo = transform.Find("DataPanel/Combo").GetComponent<Text>();
        UIkpm = transform.Find("DataPanel/Kpm").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UITimer = transform.Find("DataPanel/Timer").GetComponent<Text>();
        AssistKeyboardObj = GameObject.Find("AssistKeyboard").GetComponent<AssistKeyboardJIS>();

        // タイマーを初期化
        currentTime = totalTime;
        UpdateTimerText();

        //　データ初期化処理
        correctN = 0;
        comboN = 0;
        mistakeN = 0;
        answers = 0;
        //        UIcorrectAR.text = correctAR.ToString();

        AssistKeyboardObj.SetNextHighlight(" ");

        if (LoadThemes(GameManager.TypingDataName))
        {
            ShuffleThemes(GameManager.TypingRandom);
        }
    }

    private bool LoadThemes(string fileName)
    {
        string typingDataName = "TextPrompts/" + fileName;
        TextAsset textAsset = Resources.Load<TextAsset>(typingDataName);
        if (textAsset != null)
        {
            themeCollection = JsonUtility.FromJson<ThemeCollection>(textAsset.text);
            return true;
        }
        else
        {
            Debug.Log("Cannot find file!");
            GameManager.TypingDataId = -1;
            GameManager.SceneNo = (int)scene.House;   // ワールドシーンショップ前
            SceneManager.LoadScene("WorldScene"); // ワールドシーンに遷移
            return false;
        }
    }

    private void ShuffleThemes(bool shuffle = true)
    {
        if (themeCollection != null && themeCollection.themes.Length > 0)
        {
            shuffledThemes = new List<Theme>(themeCollection.themes);
            if (shuffle)
            {
                for (int i = 0; i < shuffledThemes.Count; i++)
                {
                    Theme temp = shuffledThemes[i];
                    int randomIndex = UnityEngine.Random.Range(i, shuffledThemes.Count);
                    shuffledThemes[i] = shuffledThemes[randomIndex];
                    shuffledThemes[randomIndex] = temp;
                }
            }
        }
        else
        {
            Debug.LogError("No themes loaded!");
        }
    }

    private IEnumerator ChangeSentence()
    {
        // コンボ数リセット、表示更新
        if (totalTime != currentTime)
        {
            kpm = correctN / (totalTime - currentTime) * 60.0f;
            UIkpm.text = string.Format("{0:0}", kpm);
        }
        if (shuffledThemes.Count > 0)
        {
            Theme currentTheme = shuffledThemes[currentThemeIndex];

            // 選択した問題をテキストUIにセット
            nQJ = currentTheme.kanji;
            nQH = currentTheme.hiragana;

            // 次のお題に移動
            currentThemeIndex++;
            if (currentThemeIndex >= shuffledThemes.Count)
            {
                if (GameManager.TypingRandom)
                {
                    ShuffleThemes(GameManager.TypingRandom);
                }
                currentThemeIndex = 0; // リストの最初に戻る
                returnCount++;
                if (returnCount > 5)
                {
                    currentTime = 0;
                    isTimerRunning = false;
                    isInputValid = false;

                    END.text = "おしまい！";
                    UIJ.text = "";
                    UIH.text = "";
                    UIR.text = "";
                    UII.text = "";

                    // キーカラークリア
                    AssistKeyboardObj.SetAllKeyColorWhite();
                    AssistKeyboardObj.SetNextHighlight(" ");
                    string randEnd = "end" + (new System.Random().Next(1, 6)).ToString();
                    animator.SetTrigger(randEnd);
                    player.transform.LookAt(targetCam.transform);   // カメラを向く

                    spaceEnd = true;
                }
            }
        }
        else
        {
            Debug.LogError("No themes to display!");
        }
        bool isGenerateSuccess;

        // Generate() 関数を呼び出す
        (isGenerateSuccess, nQR, typingJudge) = GenerateSentence.Generate(nQH);

        // 判定器などの初期化
        InitSentenceData();

        UIJ.text = nQJ;
        UIH.text = nQH;
        UIR.text = nQR;
        // 変数等の初期化
        isRecMistype = false;
        isSentenceMistyped = false;
        index = 0;

        var nextHighlight = typingJudge[0][0][0].ToString();

        AssistKeyboardObj.SetNextHighlight(nextHighlight);

        yield return new WaitForSeconds(0.01f);  // なんかとりあえず
    }

    private void UpdateTimerText()
    {
        // 残り時間を表示
        UITimer.text = string.Format("{0:0}", currentTime);
    }

    /// <summary>
    /// カウントダウン演出
    /// </summary>
    private IEnumerator CountDown()
    {
        animator.SetTrigger("kamae");
        // キーカラークリア
        AssistKeyboardObj.SetAllKeyColorWhite();
        player.transform.rotation *= Quaternion.Euler(0, -60, 0);
        var count = 1;
        while (count > 0)
        {
            UICountDown.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        UICountDown.text = "";

        // 次の文章
        StartCoroutine(ChangeSentence());
        isTimerRunning = true;
        isInputValid = true;

        animator.SetBool("move", true);
    }

    void Update()
    {
        // タイマーが実行中の場合、時間を減少させる
        if (isTimerRunning)
        {
            // 進んでいく フレームに依存しないTime.deltaTime
            player.transform.position += new Vector3(1f * Time.deltaTime, 0, 0);
            player.transform.LookAt(targetCam.transform);   // カメラを向く
            player.transform.rotation *= Quaternion.Euler(0, -60, 0);

            currentTime -= Time.deltaTime;
            // タイマーが0以下になったら停止
            if (currentTime < 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                isInputValid = false;

                END.text = "おしまい！";
                UIJ.text = "";
                UIH.text = "";
                UIR.text = "";
                UII.text = "";

                // キーカラークリア
                AssistKeyboardObj.SetAllKeyColorWhite();
                AssistKeyboardObj.SetNextHighlight(" ");
                string randEnd = "end" + (new System.Random().Next(1, 6)).ToString();
                animator.SetTrigger(randEnd);
                player.transform.LookAt(targetCam.transform);   // カメラを向く

                spaceEnd = true;
            }
            UpdateTimerText();
        }

        if (spaceStart)
        {
            // アニメーションを切り替える
            if (Time.time % 20 > 19.9)
            {
                animator.SetTrigger("reset");
                System.Threading.Thread.Sleep(100);     // 連続実行防止
            }
        }
        if (currentTime == 0)
        {
            // アニメーションを切り替える
            if (Time.time % 12 > 11.9)
            {
                if (firstEnd)
                {
                    END.text = "";
                    UIH.text = "スペースキーでしゅうりょう";
                    firstEnd = false;
                    System.Threading.Thread.Sleep(100);     // 連続実行防止
                }
                else
                {
                    animator.SetTrigger("make");
                    System.Threading.Thread.Sleep(100);     // 連続実行防止
                }
            }
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        var isPushedShiftKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (spaceStart)
        {
            var inputStr = ConvertKeyCodeToStr(e.keyCode, isPushedShiftKey);
            if (inputStr.Equals(" "))
            {
                // スペースでスタート状態を解除する
                spaceStart = false;
                UIH.text = "";
                StartCoroutine(CountDown());
                return;
            }
        }
        else if (spaceEnd)
        {
            var inputStr = ConvertKeyCodeToStr(e.keyCode, isPushedShiftKey);
            if (inputStr.Equals(" "))
            {
                GameManager.NewKpm = (int)kpm;
                GameManager.NumAnswers = answers;
                GameManager.AnswerRate = correctAR;
                GameManager.SceneNo = (int)scene.House;   // ワールドシーンショップ前
                SceneManager.LoadScene("WorldScene"); // ワールドシーンに遷移
            }
        }
        if (isInputValid && e.type == EventType.KeyDown && e.keyCode != KeyCode.None
        && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            var inputStr = ConvertKeyCodeToStr(e.keyCode, isPushedShiftKey);
            double currentTime = Time.realtimeSinceStartup;
            // タイピングで使用する文字以外は受け付けない
            // Esc など画面遷移などで使うキーと競合を避ける
            if (!inputStr.Equals(""))
            {
                // 正誤チェック
                StartCoroutine(TypingCheck(inputStr));
            }
        }
    }
    /// <summary>
    /// キーコードから string
    /// <param name="key">keycode</param>
    /// <param name="isShiftkeyPushed">シフトキーが押されたかどうか</param>
    /// </summary>
    private string ConvertKeyCodeToStr(KeyCode key, bool isShiftkeyPushed)
    {
        switch (key)
        {
            // かな入力用に便宜的にタブ文字を Shift+0 に割り当てている
            case KeyCode.Alpha0:
                return isShiftkeyPushed ? "\t" : "0";
            case KeyCode.Alpha1:
                return isShiftkeyPushed ? "!" : "1";
            case KeyCode.Alpha2:
                return isShiftkeyPushed ? "\"" : "2";
            case KeyCode.Alpha3:
                return isShiftkeyPushed ? "#" : "3";
            case KeyCode.Alpha4:
                return isShiftkeyPushed ? "$" : "4";
            case KeyCode.Alpha5:
                return isShiftkeyPushed ? "%" : "5";
            case KeyCode.Alpha6:
                return isShiftkeyPushed ? "&" : "6";
            case KeyCode.Alpha7:
                return isShiftkeyPushed ? "\'" : "7";
            case KeyCode.Alpha8:
                return isShiftkeyPushed ? "(" : "8";
            case KeyCode.Alpha9:
                return isShiftkeyPushed ? ")" : "9";
            case KeyCode.A:
                return isShiftkeyPushed ? "A" : "a";
            case KeyCode.B:
                return isShiftkeyPushed ? "B" : "b";
            case KeyCode.C:
                return isShiftkeyPushed ? "C" : "c";
            case KeyCode.D:
                return isShiftkeyPushed ? "D" : "d";
            case KeyCode.E:
                return isShiftkeyPushed ? "E" : "e";
            case KeyCode.F:
                return isShiftkeyPushed ? "F" : "f";
            case KeyCode.G:
                return isShiftkeyPushed ? "G" : "g";
            case KeyCode.H:
                return isShiftkeyPushed ? "H" : "h";
            case KeyCode.I:
                return isShiftkeyPushed ? "I" : "i";
            case KeyCode.J:
                return isShiftkeyPushed ? "J" : "j";
            case KeyCode.K:
                return isShiftkeyPushed ? "K" : "k";
            case KeyCode.L:
                return isShiftkeyPushed ? "L" : "l";
            case KeyCode.M:
                return isShiftkeyPushed ? "M" : "m";
            case KeyCode.N:
                return isShiftkeyPushed ? "N" : "n";
            case KeyCode.O:
                return isShiftkeyPushed ? "O" : "o";
            case KeyCode.P:
                return isShiftkeyPushed ? "P" : "p";
            case KeyCode.Q:
                return isShiftkeyPushed ? "Q" : "q";
            case KeyCode.R:
                return isShiftkeyPushed ? "R" : "r";
            case KeyCode.S:
                return isShiftkeyPushed ? "S" : "s";
            case KeyCode.T:
                return isShiftkeyPushed ? "T" : "t";
            case KeyCode.U:
                return isShiftkeyPushed ? "U" : "u";
            case KeyCode.V:
                return isShiftkeyPushed ? "V" : "v";
            case KeyCode.W:
                return isShiftkeyPushed ? "W" : "w";
            case KeyCode.X:
                return isShiftkeyPushed ? "X" : "x";
            case KeyCode.Y:
                return isShiftkeyPushed ? "Y" : "y";
            case KeyCode.Z:
                return isShiftkeyPushed ? "Z" : "z";
            case KeyCode.Minus:
                return isShiftkeyPushed ? "=" : "-";
            case KeyCode.Caret:
                return isShiftkeyPushed ? "~" : "^";
            case KeyCode.At:
                return isShiftkeyPushed ? "`" : "@";
            case KeyCode.LeftBracket:
                return isShiftkeyPushed ? "{" : "[";
            case KeyCode.RightBracket:
                return isShiftkeyPushed ? "}" : "]";
            case KeyCode.Semicolon:
                return isShiftkeyPushed ? "+" : ";";
            case KeyCode.Colon:
                return isShiftkeyPushed ? "*" : ":";
            case KeyCode.Comma:
                return isShiftkeyPushed ? "<" : ",";
            case KeyCode.Period:
                return isShiftkeyPushed ? ">" : ".";
            case KeyCode.Slash:
                return isShiftkeyPushed ? "?" : "/";
            case KeyCode.Underscore:
                return "_";
            case KeyCode.Space:
                return " ";
            case KeyCode.Backslash:
                return isShiftkeyPushed ? "|" : "Yen";
            default:
                return "";
        }
    }

    /// <summary>
    /// タイピングの正誤判定部分
    /// </summary>
    private IEnumerator TypingCheck(string nextString)
    {
        // まだ可能性のあるセンテンス全てに対してミスタイプかチェックする
        bool isMistype = JudgeTyping(nextString);
        if (!isMistype)
        {
            Correct(nextString);
        }
        else
        {
            Mistype();
        }
        correctAR = (float)correctN / ((float)correctN + (float)mistakeN);
        UIcorrectAR.text = string.Format("{0:0.0} %", correctAR*100);

        if (comboN == 0)
        {
            animator.SetBool("run", false);
            animator.SetBool("move", false);
            animator.SetBool("walk", true);

            float run = animator.GetFloat("runSpeed");
            if (run > 2)
            {
                animator.SetTrigger("die2");
            }
            else if (run > 1)
            {
                animator.SetTrigger("die1");
            }
            else
            {
                animator.SetTrigger("damage");
            }
        }
        else if (comboN > 20)    // コンボ依存のアニメーション(run)
        {
            animator.SetBool("run", true);
            animator.SetBool("move", false);
            animator.SetBool("walk", false);
            if (comboN > 30)
            {
                animator.SetFloat("runSpeed", 1 + (float)((comboN - 30) / 35));
            }
        }
        else if (comboN > 8)    // コンボ依存のアニメーション（move)
        {
            animator.SetBool("run", false);
            animator.SetBool("move", true);
            animator.SetBool("walk", false);
            animator.SetFloat("walkSpeed", 1.0f);

            if (comboN > 10)
            {
                animator.SetFloat("moveSpeed", 1 + (float)((comboN - 10) / 6));
            }
            animator.ResetTrigger("die1");
            animator.ResetTrigger("die2");
        }
        else if (comboN > 5)    // トリガー解除
        {
            animator.ResetTrigger("damage");
            if (comboN > 3)
            {
                animator.SetFloat("walkSpeed", 1 + (float)((comboN - 3) / 3));
            }
            else
            {
                animator.SetFloat("walkSpeed", 1.0f);
            }
            animator.SetFloat("runSpeed", 1.0f);
            animator.SetFloat("moveSpeed", 1.0f);
        }

        yield return null;
    }

    /// <summary>
    /// ミスタイプ判定と次打つべき文字のインデックス更新
    /// </summary>
    /// <param name="currentStr">打った文字</param>
    /// <returns>ミスタイプなら true</returns>
    private bool JudgeTyping(string currentStr)
    {
        bool isMistype = true;
        for (int i = 0; i < typingJudge[index].Count; ++i)
        {
            // すでに打った文字から判定候補でないとわかるときはパス
            if (sentenceValid[index][i] == 0) { continue; }
            int j = sentenceIndex[index][i];
            string judgeString = typingJudge[index][i][j].ToString();
            if (currentStr.Equals(judgeString))
            {
                isMistype = false;
                indexAdd[index][i] = 1;
            }
            else { indexAdd[index][i] = 0; }
        }
        return isMistype;
    }

    /// <summary>
    /// タイピング正解時の処理
    /// <param name="typeChar">打った文字</param>
    /// </summary>
    private void Correct(string typeChar)
    {
        correctN++;
        comboN++;
        UIcorrect.text = string.Format("{0:0}", correctN);
        UIcombo.text = string.Format("{0:0}", comboN);

        // 可能な入力パターンのチェック
        bool isIndexCountUp = IsJudgeIndexCountUp(typeChar);
        // ローマ字入力表示を更新
        UpdateSentence(typeChar);
        if (isIndexCountUp) { index++; }

        // 文章入力完了処理
        if (index >= typingJudge.Count) { CompleteTask(); }
    }

    /// <summary>
    /// 有効パターンをチェックし、インデックスを増やすかどうか判定する
    /// index はオートマトン上での index
    /// <param names="typeChar">打った文字</param>
    /// <returns>インデックス増やすなら true、さもなくば false</returns>
    /// </summary>
    private bool IsJudgeIndexCountUp(string typeChar)
    {
        bool ret = false;
        // 可能な入力パターンを残す
        for (int i = 0; i < typingJudge[index].Count; ++i)
        {
            // typeChar と一致しないものを無効化処理
            if (!typeChar.Equals(typingJudge[index][i][sentenceIndex[index][i]].ToString()))
            {
                sentenceValid[index][i] = 0;
            }
            // 次のキーへ
            sentenceIndex[index][i] += indexAdd[index][i];
            // 次の文字へ
            if (sentenceIndex[index][i] >= typingJudge[index][i].Length) { ret = true; }
        }
        return ret;
    }
    /// <summary>
    /// 1文打ち終わった後の処理
    /// </summary>
    private void CompleteTask()
    {
        // タイプした文字を緑色に
        UII.text = $"<color=#20A01D>{UII.text}</color>";
        answers++;
        // 次の文章
        StartCoroutine(ChangeSentence());
    }

    /// <summary>
    /// 画面上に表示する打つ文字の表示を更新する
    /// <param name="typeChar">打った文字</param>
    /// </summary>
    private void UpdateSentence(string typeChar)
    {
        // 打った文字を消去するオプションの場合
        // 複数入力パターンが考えられるときは最初にマッチしたものを表示しなおす
        var nextTypingSentence = "";
        for (int i = 0; i < typingJudge.Count; ++i)
        {
            if (i < index) { continue; }
            for (int j = 0; j < typingJudge[i].Count; ++j)
            {
                if (index == i && sentenceValid[index][j] == 0) { continue; }
                else if (index == i && sentenceValid[index][j] == 1)
                {
                    for (int k = 0; k < typingJudge[index][j].Length; ++k)
                    {
                        if (k >= sentenceIndex[index][j])
                        {
                            nextTypingSentence += typingJudge[index][j][k].ToString();
                        }
                    }
                    break;
                }
                else if (index != i && sentenceValid[i][j] == 1)
                {
                    nextTypingSentence += typingJudge[i][j];
                    break;
                }
            }
        }
        correctString += typeChar;
        // Space は打ったか打ってないかわかりにくいので表示上はアンダーバーに変更
        var UIStr = "";
        if (ConfigScript.IsBeginnerMode || ConfigScript.IsShowTypeSentence)
        {
            UIStr = nextTypingSentence;
        }
        else
        {
            UIStr = correctString + (isSentenceMistyped ? ("<color=#ff0000ff>" + nextTypingSentence + "</color>") : "");
        }
        SetUITypeText(UIStr);
        CurrentTypingSentence = nextTypingSentence;


        if (CurrentTypingSentence == "" || !isInputValid)
        {
            AssistKeyboardObj.SetAllKeyColorWhite();
        }
        else if (isInputValid)
        {
            var nextHighlight = CurrentTypingSentence[0].ToString();
//            handAnimation(nextHighlight);
            AssistKeyboardObj.SetNextHighlight(nextHighlight);
        }
    }

    /// <summary>
    /// タイピング文の半角スペースをアンダーバーに置換して表示
    /// 打ったか打ってないかわかりにくいため、アンダーバーを表示することで改善
    /// </summary>
    private void SetUITypeText(string sentence)
    {
        UII.text = sentence.Replace(' ', '_');
    }

    /// <summary>
    /// タイピング正誤判定まわりの初期化
    /// </summary>
    private void InitSentenceData()
    {
        var sLength = typingJudge.Count;
        sentenceIndex.Clear();
        sentenceValid.Clear();
        indexAdd.Clear();
        sentenceIndex = new List<List<int>>();
        sentenceValid = new List<List<int>>();
        indexAdd = new List<List<int>>();
        for (int i = 0; i < sLength; ++i)
        {
            var typeNum = typingJudge[i].Count;
            sentenceIndex.Add(new List<int>());
            sentenceValid.Add(new List<int>());
            indexAdd.Add(new List<int>());
            for (int j = 0; j < typeNum; ++j)
            {
                sentenceIndex[i].Add(0);
                sentenceValid[i].Add(1);
                indexAdd[i].Add(0);
            }
        }
    }

    /// <summary>
    /// ミスタイプ時の処理
    /// </summary>
    private void Mistype()
    {
        // コンボ数リセット、表示更新
        comboN = 0;
        UIcombo.text = string.Format("{0:0}", comboN);
        mistakeN++;
        UImistake.text = string.Format("{0:0}", mistakeN);

        isSentenceMistyped = true;
        // 打つべき文字を赤く表示
        if (!isRecMistype)
        {
            string UIStr = "";
            if (ConfigScript.IsBeginnerMode || ConfigScript.IsShowTypeSentence)
            {
                UIStr = "<color=#ff0000ff>" + CurrentTypingSentence.ToString() + "</color>";
            }
            else
            {
                UIStr = correctString + "<color=#ff0000ff>" + CurrentTypingSentence.ToString() + "</color>";
            }
            SetUITypeText(UIStr);
        }
        // color タグを多重で入れないようにする
        isRecMistype = true;
    }
}