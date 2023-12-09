using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Confirmation : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private GameObject status;

    [SerializeField]
    private TMP_Text talk;

    [SerializeField]
    private GameObject shopList;

    private ShopList shopListReset;
    private InventryUI inventoryui;
    private StatusUI statusui;

    private int itemId;
    private int itemPrice;

    void Awake()
    {
        transform.position = new Vector3(800, 2000, transform.position.z);
        inventoryui = inventory.GetComponentInChildren<InventryUI>();
        statusui = status.GetComponentInChildren<StatusUI>();
        shopListReset = shopList.GetComponentInChildren<ShopList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy()
    {
        itemPrice = int.Parse(transform.Find("Price").GetComponent<TextMeshProUGUI>().text);
        int blankIndex = gm.savedata.getBlankInventoryIndex();
        if (blankIndex >= 0)
        {
            int saifu = gm.savedata.getStatus()[0];
            if (saifu >= itemPrice)
            {
                gm.savedata.setInventoryIndex(blankIndex, itemId);
                gm.savedata.setStatusIndex(0, saifu - itemPrice);
                talk.text = "まいどありがとうございます！";
                transform.position = new Vector3(transform.position.x, 2000, transform.position.z);

                inventoryui.setAllItems();
                statusui.setStatus();
                shopListReset.ShowItemList(0);

                inventoryui.turnImage(blankIndex);
            }
            else
            {
                talk.text = "シーカーがたりないようです。\nタイピングをしてためてきてください。";
                transform.position = new Vector3(transform.position.x, 2000, transform.position.z);
            }
        }
        else
        {
            talk.text = "もちものがいっぱいのようです。";
            Debug.Log("インベントリに空きが見つかりませんでした。Confirmation.Buy");
            transform.position = new Vector3(transform.position.x, 2000, transform.position.z);
        }
    }

    public void Cancel()
    {
        transform.position = new Vector3(transform.position.x, 2000, transform.position.z);
        talk.text = "ほかのしょうひんも見ていってくださいね。";
    }

    public void setItemPrice()
    {
    }

    public void setItemId(int id)
    {
        itemId = id;
    }
}
