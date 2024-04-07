using UnityEngine;
using TMPro;
using System.Collections.Generic;

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
        dispStatus();
    }

    // Update is called once per frame
    public void UpdateUI()
    {
    }

    public void dispStatus()
    {
        string nickname;
        Item item = gm.db.GetItemList()[gm.savedata.equipment[eq.NickName]];
        if (item != null)
        {
            nickname = item.MyItemName;
        }
        else
        {
            nickname = "さん";
        }
        TMPHeadName.text = gm.savedata.userName + nickname;
        TMPName.text = gm.savedata.userName + nickname;
        TMPGold.text = gm.savedata.status[st.Gold].ToString() + " ｼｰｶｰ";
        TMPServer.text = "サーバー：" + gm.db.GetServerList()[gm.savedata.status[st.Server]];
        TMPWpm.text = "1分間に" + gm.savedata.status[st.Kpm] + "キー";
        TMPRank.text = "(" + gm.savedata.status[st.Rank] + "位 / 200位)";
    }
}
