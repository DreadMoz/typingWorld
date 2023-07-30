using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{
    public Transform slotsParent;

    InventrySlot[] slots;

    [SerializeField]
    private Item[] allItems;


    // Start is called before the first frame update
    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventry.instance.items.Count)
            {
                slots[i].SetItem(Inventry.instance.items[i]);
            }
            else
            {
                slots[i].SetItem(null);
            }
        }
    }
    // Update is called once per frame
    public void UpdateUI()
    {
    }
}
