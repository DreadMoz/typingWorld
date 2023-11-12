using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class Connection : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TitleSky title;

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void FbAuth();
    [DllImport("__Internal")]
    private static extern void LoadFbData();
    [DllImport("__Internal")]
    private static extern void SaveFbStatus(Dictionary<string, int> value);
    [DllImport("__Internal")]
    private static extern void SaveFbEquipment(string valuePtr);
    [DllImport("__Internal")]
    private static extern void SaveFbInventory(string valuePtr);
    [DllImport("__Internal")]
    private static extern void SaveFbMedals(Dictionary<string, int> value);
    [DllImport("__Internal")]
    private static extern void SaveFbKpm(Dictionary<string, int> value);
#endif

    public void fbAuth()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FbAuth();
#else
        title.finishAuth();
#endif
    }

    public void loadFbData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadFbData();
#else
        getDummyDb();
#endif
    }

    public void saveFbStatus(Dictionary<string, int> value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveFbStatus(value);
#endif
    }

    public void saveFbEquipment(Dictionary<string, int> value)
    {
        var dataString = string.Join(",", value.Select(kv => kv.Key + ":" + kv.Value.ToString()));

        Debug.Log("saveFbEquipment value: " + value);
        Debug.Log("saveFbEquipment dataString: " + dataString);
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveFbEquipment(dataString);
#endif
    }

    public void saveFbInventory(Dictionary<string, int> value)
    {
        var items = value.Select(kv => $"{kv.Key}:{kv.Value}").ToArray();
        var dataString = string.Join(",", items);
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveFbInventory(dataString);
#endif
    }

    public void saveFbMedals(Dictionary<string, int> value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveFbMedals(value);
#endif
    }

    public void saveFbKpm(Dictionary<string, int> value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveFbKpm(value);
#endif
    }

    private void getDummyDb()
    {
        string msg = "dummyneco";
        string msgK = "100, 101, 102, 103, 104, 105, 106, 107, 108, 109";
        // Gold,Server,Rank,Kpm
        string msgS = "30, 1, 150, 0";
        // RightHnad,Glasses(121),Head(151),LeftHand,CatBody(201)あえて0,CatFace(101),NickName(211)
        string msgE = "0, 120, 150, 0, 0, 100, 210";
        string msgI = "1, 2, 3, 4, 0, 0, 5, 0, 121, 0, 0, 6, 0, 151, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
        string msgM = "3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";

        gm.savedata.setUserName(msg);
        gm.setKpm(msgK);
        gm.setStatus(msgS);
        gm.setEquipment(msgE);
        gm.setInventory(msgI);
        gm.setMedals(msgM);

        title.setDummyData();
        title.finishDataLoad();
    }
}