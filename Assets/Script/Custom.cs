using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;
    // ここで、ShopItemParentのRectTransformを参照する
    [SerializeField]
    private RectTransform listParent;

    // Start is called before the first frame update
    void Start()
    {
        float contentHeight;
        // parentObjectは、子オブジェクトの数を数えたいゲームオブジェクトの参照。
        double childLines = Math.Ceiling((double)listParent.transform.childCount / 3);

        // コンテンツエリアの高さをアイテム数に基づいて設定
        contentHeight = (int)childLines * 160 + 20; // アイテムの高さ

        listParent.sizeDelta = new Vector2(listParent.sizeDelta.x, contentHeight);
    }
}
