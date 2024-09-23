using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを使用するために必要
using TMPro;

public class Setting : MonoBehaviour
{
    private bool isWindowShown = false;

    [SerializeField]
    private GameManager gm;
    public Slider volumeSlider;
    public Slider muteSlider;
    public Slider mailCharSlider;
    public Slider necoNumSlider;
    public Slider capitalSlider;
    public GameObject toGas;
    private bool showFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        toGas.SetActive(false);
        gm.npcManager.UpdateNPCCount(gm.savedata.Settings[se.CatNum]);
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
        if (showFlg)
        {
            showFlg = false;
            gm.npcManager.UpdateNPCCount((int)necoNumSlider.value);

            gm.savedata.Settings[se.Volume] = (int)volumeSlider.value;
            gm.savedata.Settings[se.Mute] = (int)muteSlider.value;
            gm.savedata.Settings[se.CatNum] = (int)necoNumSlider.value;
            gm.savedata.Settings[se.MailChar] = (int)mailCharSlider.value;
            gm.savedata.Settings[se.Capital] = (int)capitalSlider.value;
            gm.setVolume();
            
            // 画面サイズを都度取得しないと途中での最大化などに対応できない
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            Debug.Log("Width:" + screenWidth + "  Height:" + screenHeight);
            transform.position = new Vector2(screenWidth * 0.5f, screenHeight * 2);
            isWindowShown = false; // 非表示に設定
            gm.exportLocal();
        }
    }

    public void show()
    {
        showFlg = true;
        volumeSlider.value = gm.savedata.Settings[se.Volume];
        muteSlider.value = gm.savedata.Settings[se.Mute];
        necoNumSlider.value = gm.savedata.Settings[se.CatNum];
        mailCharSlider.value = gm.savedata.Settings[se.MailChar];
        capitalSlider.value = gm.savedata.Settings[se.Capital];

        // 画面サイズを都度取得しないと途中での最大化などに対応できない
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Debug.Log("Width:" + screenWidth + "  Height:" + screenHeight);
        transform.position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
        isWindowShown = true; // 表示に設定
    }
}
