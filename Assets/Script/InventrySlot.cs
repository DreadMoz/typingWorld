using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventrySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    private Item item;

    [SerializeField]
    private Image itemImage;

    private GameObject draggingObj;

    [SerializeField]
    private GameObject itemImageObj;

    [SerializeField]
    private Sprite imageTe;
    [SerializeField]
    private Sprite imageAtama;
    [SerializeField]
    private Sprite imageMe;

    private int slotNo;

    private bool soubiSlot;

    private GameObject canvas;

    private Transform canvasTransform;

    private Hand hand;

    public Item MyItem { get => item; private set => item = value; }

    private bool isAnimating = false; // アニメーション中かどうか
    private float animationDuration = 1.5f; // アニメーションにかかる時間（秒）
    private float animationTime = 0f; // アニメーション開始からの経過時間

    void Start()
    {
        // GameObjectの兄弟の中でのインデックスを取得してslotNoに割り当てる
        slotNo = transform.GetSiblingIndex();

        soubiSlot = false;
        // 親が'SoubiParent'タグを持っているかどうかをチェック
        soubiSlot = transform.parent.CompareTag("SoubiParent");

        canvas = GameObject.Find("Canvas");
        if (!canvas)
        {
            Debug.LogError("InventrySlot: Canvasが見つかりません。");
            return;
        }
        canvasTransform = canvas.transform;

        hand = FindObjectOfType<Hand>();
        if (!hand)
        {
            Debug.LogError("InventrySlot: Handコンポーネントが見つかりません。");
            return;
        }
    }

    void Update()
    {
        if (isAnimating)
        {
            animationTime += Time.deltaTime;

            // 回転角度とスケールを計算
            float rotationAngle = (360f / animationDuration) * Time.deltaTime;
            float scale = 1f;
            if (animationTime <= animationDuration / 2f)
            {
                // 拡大
                scale = Mathf.Lerp(0.01f, 1f, animationTime / (animationDuration / 2f));
            }

            // 回転とスケールの適用
            itemImage.transform.Rotate(0, 0, rotationAngle);
            itemImage.transform.localScale = new Vector3(scale, scale, 1f);

            if (animationTime >= animationDuration)
            {
                // アニメーションを終了
                isAnimating = false;
                animationTime = 0f;
                // 画像をリセット
                itemImage.transform.localScale = Vector3.one;
                itemImage.transform.rotation = Quaternion.identity;
            }
        }
    }

    public void TurnItem()
    {
        if (!isAnimating)
        {
            // アニメーションを開始
            isAnimating = true;
            animationTime = 0f;
        }
    }

    public void SetItem(Item item, int no = 0)
    {
        MyItem = item;

        if (item != null)
        {
            // 画像の透明度を設定
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = item.MyItemImage;
        }
        else
        {
            if (!soubiSlot)
            {
                itemImage.color = new Color(0, 0, 0, 0);
                return;
            }
            // slotNoの値に応じて異なる画像を設定
            switch (slotNo)
            {
                case 0:
                    itemImage.sprite = imageTe;
                    itemImage.color = new Color(1, 1, 1, 0.9f);
                    break;
                case 1:
                    itemImage.sprite = imageAtama;
                    itemImage.color = new Color(1, 1, 1, 0.9f);
                    break;
                case 2:
                    itemImage.sprite = imageMe;
                    itemImage.color = new Color(1, 1, 1, 0.9f);
                    break;
                case 3:
                    itemImage.sprite = imageTe;
                    itemImage.color = new Color(1, 1, 1, 0.9f);
                    break;
                default:
                    // slotNoがその他の場合は、画像を透明にする
                    itemImage.color = new Color(0, 0, 0, 0);
                    break;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (MyItem == null) return;

        // アイテムのイメージを複製
        draggingObj = Instantiate(itemImageObj, canvasTransform);

        // 複製したオブジェクトを最前面に配置
        draggingObj.transform.SetAsLastSibling();

        // 複製元のイメージの色をグレーにする
        itemImage.color = Color.gray;

        // Handにアイテムを設定
        hand.SetGrabbingItem(MyItem);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MyItem == null) return;

        draggingObj.transform.position = hand.transform.position + new Vector3(10, 10, 0);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (soubiSlot)
        {
            if (slotNo == 0 | slotNo == 3)
            {
                if (hand.GetGrabbingItemType() != ItemType.Weapon)
                {
                    return;
                }
            }
            if (slotNo == 1)
            {
                if (hand.GetGrabbingItemType() != ItemType.Hat)
                {
                    return;
                }
            }
            if (slotNo == 2)
            {
                if (hand.GetGrabbingItemType() != ItemType.Glasses)
                {
                    return;
                }
            }
        }
        // Handにアイテムがなければ何もせずにreturn
        if (!hand.IsHavingItem()) return;

        // Handからアイテムを取得
        Item gotItem = hand.GetGrabbingItem();

        // 交換先としてアイテムをHandに設定
        hand.SetGrabbingItem(MyItem);

        SetItem(gotItem);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(draggingObj);

        // OnDropで行われた
        // Handからアイテムを取得
        Item gotItem = hand.GetGrabbingItem();

        // 複製元のイメージの色を元に戻す
        itemImage.color = Color.white;

        SetItem(gotItem);
    }
}
