using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class TypingSoft : MonoBehaviour
{
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
    private string[] qJ = { "学問", "ダッシュボード", "タイピング", "グッナイ", "ぴゃっだっむっちゃ" };
    //　問題のひらがな文
    private string[] qH = { "がくもん", "だっしゅぼーど", "たいぴんぐ", "ぐっない", "ぴゃっだっむっちゃ" };

    //　日本語表示テキスト
    private Text UIJ;
    //　ひらがな表示テキスト
    private Text UIH;
    //　ローマ字表示テキスト
    private Text UIR;

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
    private Text UII;
    //　正解数
    private int correctN;
    //　正解数表示用テキストUI
    private Text UIcorrectA;
    //　正解した文字列を入れておく
    private string correctString;

    //　失敗数
    private int mistakeN;
    //　失敗数表示用テキストUI
    private Text UImistake;

    //　正解率
    private float correctAR;
    //　正解率表示用テキストUI
    private Text UIcorrectAR;

    void Start()
    {
        //　テキストUIを取得
        UIJ = transform.Find("InputPanel/QuestionJ").GetComponent<Text>();
        UIH = transform.Find("InputPanel/QuestionH").GetComponent<Text>();
        UIR = transform.Find("InputPanel/QuestionR").GetComponent<Text>();
        UII = transform.Find("InputPanel/Input").GetComponent<Text>();
        UIcorrectA = transform.Find("DataPanel/CorrectAnswer").GetComponent<Text>();
        UImistake = transform.Find("DataPanel/Mistake").GetComponent<Text>();
        UIcorrectAR = transform.Find("DataPanel/CorrectAnswerRate").GetComponent<Text>();

        //　データ初期化処理
        correctN = 0;
        UIcorrectA.text = correctN.ToString();
        mistakeN = 0;
        UImistake.text = mistakeN.ToString();
        correctAR = 0;
        UIcorrectAR.text = correctAR.ToString();

        // 次の文章
        StartCoroutine(ChangeSentence());
    }

    private IEnumerator ChangeSentence()
    {
        //　問題数内でランダムに選ぶ
        numberOfQuestion = UnityEngine.Random.Range(0, qJ.Length);

        //　選択した問題をテキストUIにセット
        nQJ = qJ[numberOfQuestion];
        nQH = qH[numberOfQuestion];

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
        // 入力受け付け状態にする
        isInputValid = true;

        yield return new WaitForSeconds(0.1f);  // なんかとりあえず
    }


    void Update()
    {
    }

    private void OnGUI()
    {
        isInputValid = true;        // 仮

        Event e = Event.current;
        var isPushedShiftKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

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
    //　新しい問題を表示する関数
    void OutputQ()
    {
        //　テキストUIを初期化する
        UIJ.text = "";
        UIR.text = "";
        UII.text = "";

        //　正解した文字列を初期化
        correctString = "";
        //　文字の位置を0番目に戻す
        indexOfString = 0;

        //　問題数内でランダムに選ぶ
        numberOfQuestion = UnityEngine.Random.Range(0, qJ.Length);

        //　選択した問題をテキストUIにセット
        nQJ = qJ[numberOfQuestion];
        nQH = qH[numberOfQuestion];
        UIJ.text = nQJ;
        UIH.text = nQH;
        UIR.text = nQR;
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
        isInputValid = false;

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