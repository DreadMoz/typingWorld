using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField]
    private Transform slotsParent;

    private GameManager gm;     // WebGLのえらーにより動的取得に変更

    InventrySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        // GameManagerのインスタンスを動的に検索して取得
        gm = FindObjectOfType<GameManager>();
        if (gm == null)
        {
            Debug.LogError("GameManagerが見つかりませんでした。");
            return;
        }
        setAllEquipments();
    }
    
    // Update is called once per frame
    public void UpdateUI()
    {
    }

    private void setAllEquipments()
    {
        try
        {
            slots = slotsParent.GetComponentsInChildren<InventrySlot>();
            int[] saveEquip = gm.savedata.getEquipment();

            if (saveEquip.Length < 4 || slots.Length < 4)
            {
                Debug.LogError("配列の長さが不足しています。");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (saveEquip[i] < gm.db.GetItemList().Count)
                {
                    slots[i].SetItem(gm.db.GetItemList()[saveEquip[i]]);
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
