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
            else if (medalSum[i] > 7)
            {
                medalTop[i] = 3;    // 星2つ
            }
            else if (medalSum[i] > 1)
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
}
