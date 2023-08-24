using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    public Transform slotsParent;

    InventrySlot[] slots;

    [SerializeField]
    private Item[] allItems;


    void Start()
    {
        setItemData();
    }

    public void setItemData()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();

        int head = gm.savedata.getStatus((int)Status.head);
        int right = gm.savedata.getStatus((int)Status.right);
        int left = gm.savedata.getStatus((int)Status.left);
        int glasses = gm.savedata.getStatus((int)Status.glasses);
        int face = gm.savedata.getStatus((int)Status.face);
        int body = gm.savedata.getStatus((int)Status.charid);

        slots[0].SetItem(gm.db.GetItemById(right));
        slots[1].SetItem(gm.db.GetItemById(head));
        slots[2].SetItem(gm.db.GetItemById(glasses));
        slots[3].SetItem(gm.db.GetItemById(left));
    }
    
}
