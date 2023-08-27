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

    private void setAllItems()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();

        int[] saveItem = gm.savedata.getInventry();

        for (int i = 0; i < saveItem.Length; i++)
        {
            if (i < slots.Length)
            {
                slots[i].SetItem(gm.db.GetItem(saveItem[i]));
            }
        }
    }
}
