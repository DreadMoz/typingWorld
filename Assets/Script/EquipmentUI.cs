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

        int[] saveEquip = gm.savedata.getStatus();

        slots[0].SetItem(gm.db.GetItemList()[saveEquip[9]]);
        slots[1].SetItem(gm.db.GetItemList()[saveEquip[6]]);
        slots[2].SetItem(gm.db.GetItemList()[saveEquip[7]]);
        slots[3].SetItem(gm.db.GetItemList()[saveEquip[10]]);
    }
}
