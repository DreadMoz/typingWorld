using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
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

        int[] itemId = gm.savedata.getInventry();
        for (int i = 0; i < itemId.Length; i++)
        {
            if (i < slots.Length)
            {
                slots[i].SetItem(gm.db.GetItemById(itemId[i]));
            }
        }
    }
    
}
