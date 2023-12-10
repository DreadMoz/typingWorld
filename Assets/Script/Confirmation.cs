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
    private GameObject housePlayer;
    private Animator pAnimator;

    [SerializeField]
    private GameObject kinoko;
    private Animator kAnimator;

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

        pAnimator = housePlayer.GetComponent<Animator>(); // Playerのアニメーターを取得
        kAnimator = kinoko.GetComponent<Animator>(); // kinokoのアニメーターを取得
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
                pAnimator.SetTrigger("yes");
                kAnimator.SetTrigger("buy");
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
                pAnimator.SetTrigger("down");
                kAnimator.SetTrigger("no");
                talk.text = "シーカーがたりないようです。\nタイピングをしてためてきてください。";
                transform.position = new Vector3(transform.position.x, 2000, transform.position.z);
            }
        }
        else
        {
            pAnimator.SetTrigger("down");
            kAnimator.SetTrigger("no");
            talk.text = "もちものがいっぱいのようです。";
            Debug.Log("インベントリに空きが見つかりませんでした。Confirmation.Buy");
            transform.position = new Vector3(transform.position.x, 2000, transform.position.z);
        }
    }

    public void selectNo()
    {
        pAnimator.SetTrigger("no");
        kAnimator.SetTrigger("no");
        transform.position = new Vector3(transform.position.x, 2000, transform.position.z);
        talk.text = "ほかのしょうひんも見ていってくださいね。";
    }

    public void Cancel()
    {
        pAnimator.SetTrigger("cancel");
        kAnimator.SetTrigger("cancel");
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
    public void setIta()
    {
        kAnimator.SetTrigger("ita");
    }
}
