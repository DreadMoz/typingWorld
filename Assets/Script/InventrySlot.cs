using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventrySlot : MonoBehaviour
{
    private Item item;

    [SerializeField]
    private Image itemImage;
    public Item MyItem { get => item; private set => item = value; }

    public void SetItem(Item item)
    {
        MyItem = item;

        if (item != null)
        {
            itemImage.sprite = item.MyItemImage;
        }
    }
}
