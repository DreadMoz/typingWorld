using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoubiUI : MonoBehaviour
{
    public Transform slotsParent;

    [SerializeField]
    private GameManager gm;

    InventrySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        setAllSoubi();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
    }

    private void setAllSoubi()
    {
        try
        {
            slots = slotsParent.GetComponentsInChildren<InventrySlot>();
            int[] saveEquip = gm.savedata.getEquipment();

            if (saveEquip.Length < 7 || slots.Length < 7)
            {
                Debug.LogError("配列の長さが不足しています。");
                return;
            }

            for (int i = 0; i < 7; i++)
            {
                if (saveEquip[i] < gm.db.GetItemList().Count)    // あやしいMAX2
                {
                    slots[i].SetItem(gm.db.GetItemList()[saveEquip[i]]);    // あやしいMAX
                }
                else
                {
                    Debug.LogError("不正なインデックス: " + saveEquip[i]);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("setAllEquipmentsでエラーが発生しました: " + ex.Message);
        }
    }

    public void getAllSoubi()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].MyItem != null)
            {
                gm.savedata.setEquipmentIndex(i, slots[i].MyItem.MyItemNo);
            }
            else
            {
                gm.savedata.setEquipmentIndex(i, 0);
            }
        }
    }
}
