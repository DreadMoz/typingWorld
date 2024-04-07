using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Setting : MonoBehaviour
{
    private bool isWindowShown = false;

    [SerializeField]
    private GameManager gm;
    public TMP_Text necoNum;

    // Start is called before the first frame update
    void Start()
    {
        //hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchWindow()
    {
        if (isWindowShown)
        {
            hide();
        }
        else
        {
            show();
        }
    }

    public void hide()
    {
        gm.npcManager.UpdateNPCCount(int.Parse(necoNum.text));
        // 画面サイズを都度取得しないと途中での最大化などに対応できない
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log("Width:" + screenWidth + "  Height:" + screenHeight);
        transform.position = new Vector2(screenWidth * 0.5f, screenHeight * 2);
        isWindowShown = false; // 非表示に設定
    }

    public void show()
    {
        // 画面サイズを都度取得しないと途中での最大化などに対応できない
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log("Width:" + screenWidth + "  Height:" + screenHeight);
        transform.position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
        isWindowShown = true; // 表示に設定
    }
}
