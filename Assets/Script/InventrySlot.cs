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
        canvasTransform = canvas.transform;

        hand = FindObjectOfType<Hand>();
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

        // 複製を最前面に配置
        draggingObj.transform.SetAsLastSibling();

        // 複製元の色を暗くする
        itemImage.color = Color.gray;

        // Handにアイテムを渡す
        hand.SetGrabbingItem(MyItem);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MyItem == null) return;

        draggingObj.transform.position = hand.transform.position + new Vector3(20, 20, 0);

    }

    public void OnDrop(PointerEventData eventData)
    {
        // Handがアイテムを持っていなかったら早期return
        if (!hand.IsHavingItem()) return;

        // Handからアイテムを受け取る
        Item gotItem = hand.GetGrabbingItem();

        // もともと持っていたアイテムをHandに渡す
        hand.SetGrabbingItem(MyItem);

        SetItem(gotItem);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(draggingObj);

        // OnDropが先に呼ばれる
        // Handからアイテムを受け取る
        Item gotItem = hand.GetGrabbingItem();

        // 複製元の色を明るくする
        itemImage.color = Color.white;

        SetItem(gotItem);
    }
}
