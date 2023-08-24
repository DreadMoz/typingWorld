using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class setStatus : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public TextMeshProUGUI HeadName;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Server;
    public TextMeshProUGUI Wpm;
    public TextMeshProUGUI Seeker;

    public void Start()
    {
    }

    public void setStatusData()
    {
        string name = gameManager.savedata.getUsername() + gameManager.db.GetItemById(gameManager.savedata.getStatus((int)Status.nickname)).MyItemName;

        HeadName.SetText(name);
        Name.SetText(name);
        Server.SetText("サーバー：" + gameManager.db.GetServerNameById(gameManager.savedata.getStatus((int)Status.serverid)));
        Wpm.SetText("1分間に" + gameManager.savedata.getStatus((int)Status.wpm).ToString() + "キー");
        Seeker.SetText(gameManager.savedata.getStatus((int)Status.seeker).ToString() + " ｼｰｶｰ");
        HeadName.SetText(gameManager.savedata.getUsername() + gameManager.db.GetItemById(gameManager.savedata.getStatus((int)Status.nickname)).MyItemName);
    }
}
