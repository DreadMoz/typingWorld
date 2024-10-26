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
                        if (CheckItemExists(saveItem[i]))   // 同じアイテム番号のアイテムがすでに存在するかチェック
                        {
                            slots[i].SetItem(null);         // 同じアイテム番号が来たら0にする
                            gm.savedata.Inventory[i] = 0;
                        }
                        else
                        {
                            slots[i].SetItem(gm.db.GetItemList()[saveItem[i]]);   // 初めてのアイテムの場合、アイテムを設定
                        }
                    }
                    else
                    {
                        Debug.LogError("不正なアイテム番号: " + saveItem[i]);
                        slots[i].SetItem(null);
                        gm.savedata.Inventory[i] = 0;
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

    // 同じアイテム番号のアイテムが存在するかチェックする関数
    private bool CheckItemExists(int itemNo)
    {
        foreach (InventrySlot slot in slots)        // インベントリは複数あるかチェックする
        {
            if (slot.MyItem != null && slot.MyItem.MyItemNo == itemNo)
            {
                return true;
            }
        }
        foreach (int equip in gm.savedata.Equipment)    // 装備側はシンプルにあるかないか見る
        {
            if (equip == itemNo)
            {
                return true;
            }
        }
        return false;
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
