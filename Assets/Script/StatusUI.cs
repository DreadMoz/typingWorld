using UnityEngine;
using TMPro;
using System.Collections.Generic;

// Gold,Server,Rank,Kpm,userName
enum st
{
    Gold = 0,
    Server = 1,
    Rank = 2,
    Kpm = 3
}

// RightHnad,Glasses(121),Head(151),LeftHand,CatBody(201)あえて0,CatFace(101),NickName(211)
enum eq
{
    RightHnad = 0,
    Glasses = 1,
    Head = 2,
    LeftHand = 3,
    CatBody = 4,
    CatFace = 5,
    NickName = 6
}

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private TextMeshProUGUI TMPHeadName;
    [SerializeField]
    private TextMeshProUGUI TMPName;
    [SerializeField]
    private TextMeshProUGUI TMPGold;
    [SerializeField]
    private TextMeshProUGUI TMPServer;
    [SerializeField]
    private TextMeshProUGUI TMPWpm;
    [SerializeField]
    private TextMeshProUGUI TMPRank;


    // Start is called before the first frame update
    void Start()
    {
        setStatus();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
    }

    public void setStatus()
    {
        int[] saveStatus = gm.savedata.getStatus();
        int[] saveEquip = gm.savedata.getEquipment();
        string nickname;
        Item item = gm.db.GetItemList()[saveEquip[(int)eq.NickName]];
        if (item != null)
        {
            nickname = item.MyItemName;
        }
        else
        {
            nickname = "さん";
        }
        TMPHeadName.text = gm.savedata.getUserName() + nickname;
        TMPName.text = gm.savedata.getUserName() + nickname;
        TMPGold.text = saveStatus[(int)st.Gold].ToString() + " ｼｰｶｰ";
        TMPServer.text = "サーバー：" + gm.db.GetServerList()[saveStatus[(int)st.Server]];
        TMPWpm.text = "1分間に" + saveStatus[(int)st.Kpm] + "キー";
        TMPRank.text = "(" + saveStatus[(int)st.Rank] + "位 / 200位)";
    }
}
