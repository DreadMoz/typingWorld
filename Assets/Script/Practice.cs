using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    private int[] medalSum;             // 指練習、ランダム、応用の合計
    public int[] medalTop;              // トップ画面の星の数
    private int medalTopNum;            // Practiceの項目数

    // Start is called before the first frame update
    void Start()
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
            medalSum[i] = medals[i * 3] + medals[i * 3 + 1] + medals[i * 3 + 2];

            if (medalSum[i] == 12)
            {
                medalTop[i] = 4;
            }
            else if (medalSum[i] > 5)
            {
                medalTop[i] = 3;
            }
            else if (medalSum[i] > 1)
            {
                medalTop[i] = 2;
            }
            else
            {
                medalTop[i] = 1;
            }
            if (i > 0)
            {
                if (medalTop[i - 1] < 3)
                {
                    medalTop[i] = 0;
                }
            }
        }
        for (int i = 0; i < medalTopNum; i++)
        {
            Transform menu = transform.GetChild(i);
            RoomMenu roomMenu = menu.GetComponent<RoomMenu>();
            roomMenu.setStars();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
