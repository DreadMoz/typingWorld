using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventry : MonoBehaviour
{
    public static Inventry instance;

    InventryUI inventryUI;

    [SerializeField]
    GameManager gm;

private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inventryUI = GetComponent<InventryUI>();
    }

    void Update()
    {

    }

    public void Add(int addId, int itemNo)
    {
        gm.savedata.setItem(addId, itemNo);
        inventryUI.setItemData();
    }

    public void Remove(int delNo)
    {
        gm.savedata.setItem(delNo, 0);
        inventryUI.setItemData();
    }
}
