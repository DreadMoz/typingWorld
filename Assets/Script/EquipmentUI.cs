using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField]
    private Transform slotsParent;

    [SerializeField]
    private GameManager gm;

    InventrySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        setAllEquipments();
    }
    
    // Update is called once per frame
    public void UpdateUI()
    {
    }

    private void setAllEquipments()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();
        int[] saveEquip = gm.savedata.getEquipment();

        for (int i = 0; i < 4; i++)
        {
            slots[i].SetItem(gm.db.GetItemList()[saveEquip[i]]);
        }
    }
    public void getAllEquipments()
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
