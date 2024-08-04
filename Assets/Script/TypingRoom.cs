using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypingRoom : MonoBehaviour
{
    [SerializeField]
    private TMP_Text talk;

    [SerializeField]
    private GameObject housePlayer;
    private Animator pAnimator;

    [SerializeField]
    private GameObject littleCat;
    private Animator lAnimator;

    [SerializeField]
    private GameObject trainingList;
    [SerializeField]
    private GameObject challengeList;
    [SerializeField]
    private GameObject customList;

    // ここで、ShopItemParentのRectTransformを参照する
    [SerializeField]
    private RectTransform listParent;

    private bool goNextScene = false;    // 次のシーンに遷移するためのフラグ
    

    void Start()
    {
        challengeList.SetActive(false);
        customList.SetActive(false);
        trainingList.SetActive(false);
        switch (GameManager.TypingTab)
        {
            case 0:
                challengeList.SetActive(true);
                break;
            case 1:
                customList.SetActive(true);
                break;
            case 2:
                trainingList.SetActive(true);
                break;
        }
        pAnimator = housePlayer.GetComponent<Animator>(); // Playerのアニメーターを取得
        lAnimator = littleCat.GetComponent<Animator>(); // littleCatのアニメーターを取得
        lAnimator.SetTrigger("jump");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gotoTypingState()
    {
        if (!goNextScene)
        {
            GameManager.SceneNo = (int)scene.Typing;
            SceneManager.LoadScene("typingStage"); // タイピングシーンに遷移
            goNextScene = true;
        }
    }

    public void openChallenge()
    {
        GameManager.TypingTab = 0;
        challengeList.SetActive(true);
        customList.SetActive(false);
        trainingList.SetActive(false);
        pAnimator.SetTrigger("fuda");
        lAnimator.SetTrigger("jump");
        talk.text = "ここでいろんなタイピングにちょうせんしてみてね。";
        ShowMenuList(1);
    }

    public void openCustom()
    {
        GameManager.TypingTab = 1;
        challengeList.SetActive(false);
        customList.SetActive(true);
        trainingList.SetActive(false);
        pAnimator.SetTrigger("fuda");
        lAnimator.SetTrigger("jump");
        talk.text = "みんなが作ってくれたメニューだよ。\nたのしんでいってね。";
        ShowMenuList(2);

    }

    public void openTraining()
    {
        GameManager.TypingTab = 2;
        challengeList.SetActive(false);
        customList.SetActive(false);
        trainingList.SetActive(true);
        pAnimator.SetTrigger("fuda");
        lAnimator.SetTrigger("jump");
        talk.text = "タイピングがうまくなりたい人はここでれんしゅうをしよう。";
        ShowMenuList(3);

    }

    private void ShowMenuList(int menuNo)
    {
        float contentHeight;
        double childLines;
        switch (menuNo)
        {
            case 1:
                // parentObjectは、子オブジェクトの数を数えたいゲームオブジェクトの参照。
                childLines = Math.Ceiling((double)listParent.transform.childCount / 3);
                // コンテンツエリアの高さをアイテム数に基づいて設定
                contentHeight = (int)childLines * 200; // アイテムの高さ
                break;
            case 2:
                // parentObjectは、子オブジェクトの数を数えたいゲームオブジェクトの参照。
                childLines = Math.Ceiling((double)listParent.transform.childCount / 3);
                // コンテンツエリアの高さをアイテム数に基づいて設定
                contentHeight = (int)childLines * 200; // アイテムの高さ
                break;
            case 3:
                // parentObjectは、子オブジェクトの数を数えたいゲームオブジェクトの参照。
                childLines = Math.Ceiling((double)listParent.transform.childCount / 4);
                // コンテンツエリアの高さをアイテム数に基づいて設定
                contentHeight = (int)childLines * 200; // アイテムの高さ
                break;
            default:
                childLines = Math.Ceiling((double)listParent.transform.childCount / 4);
                contentHeight = (int)childLines * 200; // アイテムの高さ
                break;
        }

        listParent.sizeDelta = new Vector2(listParent.sizeDelta.x, contentHeight);
    }
}
