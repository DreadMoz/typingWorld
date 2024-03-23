using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro; // TextMeshProの名前空間を使用

public class Ranking : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject rankBoardPrefab; // RankBoardのプレファブ

    [SerializeField]
    private Transform rankBoardParent;  // RankBoardをインスタンス化する親オブジェクト

    // Start is called before the first frame update
    void Start()
    {
        DisplayRankings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // JavaScriptからデータを受け取るメソッド
    public void ReceiveDataFromJS(string data)
    {
        Debug.Log("Received data from JS: " + data);
        // 受け取ったデータを処理
    }

    // ランキングデータを受け取って表示するメソッド
    public void DisplayRankings()
    {
        // 既存のランキングをクリアする
        foreach (Transform child in rankBoardParent)
        {
            Destroy(child.gameObject);
        }
        if (gm.savedata.ExRankings == null)
        {
            return;
        }

        // 新しいランキングデータをUIに表示する
        foreach (ExRank rank in gm.savedata.ExRankings)
        {
            string nickname;
            Item item = gm.db.GetItemList()[rank.NickName];
            if (item != null)
            {
                nickname = item.MyItemName;
            }
            else
            {
                nickname = "さん";
            }

            // RankBoardのプレファブをインスタンス化
            GameObject newRankBoard = Instantiate(rankBoardPrefab, rankBoardParent);

            // RankBoardのUIコンポーネントにデータを設定
            newRankBoard.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = rank.Ranking.ToString();
            newRankBoard.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = rank.Name + nickname;
            newRankBoard.transform.Find("Kpm").GetComponent<TextMeshProUGUI>().text = rank.Kpm.ToString();
        }
    }
}
