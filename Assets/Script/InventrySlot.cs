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

    private GameObject canvas;

    private Transform canvasTransform;

    private Hand hand;

    public Item MyItem { get => item; private set => item = value; }

    private void Start()
    {
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

    public void SetItem(Item item)
    {
        MyItem = item;

        if (item != null)
        {
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = item.MyItemImage;
        }
        else
        {
            itemImage.color = new Color(0, 0, 0, 0);
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
