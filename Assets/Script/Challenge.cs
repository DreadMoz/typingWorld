using System;
using System.IO;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Challenge : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;
    [SerializeField]
    private GameObject odaiPanelPrefab; // チャレンジボタンのプレハブ
    // ここで、ShopItemParentのRectTransformを参照する
    [SerializeField]
    private RectTransform menuParent;
    [SerializeField]
    private string odaiDataPath = "TextChallange"; 

    private bool goNextScene = false;    // 次のシーンに遷移するためのフラグ

    // Start is called before the first frame update
    void Start()
    {
        LoadChallenges();
    }

    void LoadChallenges()
    {
        string path = Path.Combine(Application.dataPath, "Resources/" + odaiDataPath);
        foreach (string file in Directory.GetFiles(path, "*.json"))
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            string jsonText = File.ReadAllText(file);
            ChallengeData data = JsonUtility.FromJson<ChallengeData>(jsonText);
            CreateChallengeButton(fileNameWithoutExtension, data);
        }
    }
    
    void CreateChallengeButton(string fileName, ChallengeData data)
    {
        GameObject odaiPanel = Instantiate(odaiPanelPrefab, menuParent);
        odaiPanel.name = data.title; // ボタンにタイトルを設定
        odaiPanel.GetComponentInChildren<Text>().text = data.title; // テキストコンポーネントにタイトルを設定
        odaiPanel.GetComponent<Button>().onClick.AddListener(() => bootTyping(fileName)); // ボタンにクリックイベントを追加
    }

    void StartChallenge(ChallengeData data)
    {
        // ここにチャレンジを開始するコードを追加
        Debug.Log("Challenge started: " + data.title);
    }
    public void bootTyping(string title)
    {
        GameManager.TypingDataName = odaiDataPath + "/" + title;
        GameManager.Seeker = gm.savedata.Status[st.Gold];

        GameManager.SceneNo = (int)scene.Typing;
        if (!goNextScene)
        {
            SceneManager.LoadScene("typingStage"); // タイピングシーンに遷移
            goNextScene = true;
        }
    }
}

[System.Serializable]
public class ChallengeData
{
    public string title;
    public string[] questions;
}