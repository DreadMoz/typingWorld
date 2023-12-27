using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    private int[] medalSum;             // 指練習、ランダム、応用の合計
    private int[] medalTop;             // トップ画面の星の数
    private int medalTopNum;            // Practiceの項目数

    // Start is called before the first frame update
    void Awake()
    {
        medalTopNum = transform.childCount;
        setStars();
    }

    private void setStars()
    {
        int[] medals = gm.savedata.getMedals();
        for (int i=0; i < medals.Length; i++)
        {
            if (i < medals.Length - 1)    // 次があればステージオープン
            {
                if ((medals[i] > 2) && (medals[i + 1] == 0))    // 星2つ以上で次がクローズだったら
                {
                    gm.savedata.setMedalIndex(i + 1, 1);        // オープン
                    medals[i + 1] = 1;
                    Debug.Log("Oepned detail id:" + (i + 1));
                }
            }
        }
        medalTop = new int[medalTopNum];
        medalSum = new int[medalTopNum];

        for (int i = 0; i < medalTopNum; i++)
        {
            // ３つのステージの星の合計
            medalSum[i] = medals[i * 3] + medals[i * 3 + 1] + medals[i * 3 + 2];

            if (medalSum[i] == 12)
            {
                medalTop[i] = 4;    // 星3つ
            }
            else if (medalSum[i] > 9)
            {
                medalTop[i] = 3;    // 星2つ
                if (i < medalTopNum - 1)    // 次があればステージオープン
                {
                    if (medalTop[i+1] == 0)
                    {
                        medalTop[i + 1] = 1;
                        gm.savedata.setMedalIndex((i + 1) * 3, 1);
                        Debug.Log("Opend stage id:" + (i + 1));
                    }
                }
            }
            else if (medalSum[i] > 2)
            {
                medalTop[i] = 2;    // 星1つ
            }
            else
            {
                medalTop[i] = 1;    // 星0こ
            }
            if (i > 0)
            {
                if (medalTop[i - 1] < 3)    // １つ前が星2つ未満なら
                {
                    medalTop[i] = 0;    // 錠
                }
            }
        }
    }

    public int getMedalTop(int id)
    {
        return medalTop[id];
    }

    // タイピング終了後の詳細画面表示
    public void startOpenDetail()
    {
        int id = GameManager.TypingDataId;
        float answerRate = GameManager.AnswerRate;
        
        if ( id >= 0)
        {
            int roomId = id / 3;
            Transform childTransform = gameObject.transform.GetChild(roomId);
            RoomMenu roommenu = childTransform.GetComponent<RoomMenu>();
            roommenu.showDetail();
            updateRoomMenu();
        }
    }

    private void updateRoomMenu()       // ルームメニューの更新
    {
        for (int no = 0; no < medalTopNum; no++)
        {
            Transform childTransform = gameObject.transform.GetChild(no);
            RoomMenu roommenu = childTransform.GetComponent<RoomMenu>();
            roommenu.showStars();
        }
    }
}
