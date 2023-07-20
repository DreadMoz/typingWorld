using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventrySlot : MonoBehaviour
{
    public Image icon;
//    public GameObject removeButton;
    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
//        removeButton.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
//        removeButton.SetActive(false);
    }

}
