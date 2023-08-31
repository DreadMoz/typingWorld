using UnityEngine;
using TMPro;

enum status
{
    Gold = 0,
    Server = 1,
    Rank = 2,
    Wpm = 3,
    CatBody = 4,
    CatFace = 5,
    Glasses = 6,
    Head = 7,
    NickName = 8,
    RightHnad =9,
    LeftHand = 10
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

        TMPHeadName.text = gm.savedata.getUserName() + gm.db.GetItemList()[saveStatus[(int)status.NickName]].MyItemName;
        TMPName.text = gm.savedata.getUserName() + gm.db.GetItemList()[saveStatus[(int)status.NickName]].MyItemName;
        TMPGold.text = saveStatus[(int)status.Gold].ToString() + "ｼｰｶｰ";
        TMPServer.text = "サーバー：" + gm.db.GetServerList()[saveStatus[(int)status.Server]];
        TMPWpm.text = "1分間に" + saveStatus[(int)status.Wpm] + "キー";
    }
}
