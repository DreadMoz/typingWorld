using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{
    public Transform slotsParent;

    InventrySlot[] slots;

    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private Item[] allItems;

    // Start is called before the first frame update
    void Start()
    {
        setAllItems();
    }
    
    // Update is called once per frame
    public void UpdateUI()
    {
    }

    public void setAllItems()
    {
        try
        {
            slots = slotsParent.GetComponentsInChildren<InventrySlot>();
            int[] saveItem = gm.savedata.Inventory;

            for (int i = 0; i < saveItem.Length; i++)
            {
                if (i < slots.Length)
                {
                    if (saveItem[i] < gm.db.GetItemList().Count)
                    {
                        slots[i].SetItem(gm.db.GetItemList()[saveItem[i]]);
                    }
                    else
                    {
                        Debug.LogError("不正なインデックス: " + saveItem[i]);
                    }
                }
                else
                {
                    Debug.LogError("slotsの長さが不足しています。Index: " + i);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("setAllItemsでエラーが発生しました: " + ex.Message);
        }
    }

    public void turnImage(int no)
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();
        slots[no].TurnItem();
    }

    public void getAllItems()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].MyItem != null)
            {
                gm.savedata.Inventory[i] = slots[i].MyItem.MyItemNo;
            }
            else
            {
                gm.savedata.Inventory[i] = 0;
            }
        }
    }
}
