using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{
    public Transform slotsParent;

    InventrySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<InventrySlot>();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        Debug.Log("Update UI");
        for(int i=0; i<slots.Length; i++)
        {
            if (i<Inventry.instance.items.Count)
            {
                slots[i].AddItem(Inventry.instance.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
