using System;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.SocialPlatforms;
using System.Text;
using System.Net.NetworkInformation;

public class GssIndex
{
    public const int Status = 3;
    public const int Equipment = 7;
    public const int Kpms = 14;
    public const int Medals = 16;
    public const int Inventory = 21;
}
public class GssSize
{
    public const int Status = 3;
    public const int Equipment = 7;
    public const int Kpms = 2;
    public const int Medals = 5;
    public const int Inventory = 4;
}
// Gold,Server,Rank,userName
public class st
{
    public const int Gold = 0;
    public const int Server = 1;
    public const int Rank = 2;
    public const int Kpm = 3;
}
// RightHnad,Glasses(121),Head(151),LeftHand,CatBody(201)あえて0,CatFace(101),NickName(211)
public class eq
{
    public const int RightHnad = 0;
    public const int Glasses = 1;
    public const int Head = 2;
    public const int LeftHand = 3;
    public const int CatBody = 4;
    public const int CatFace = 5;
    public const int NickName = 6;
}
// 拡張機能JSON用トップレベル
public class JsonResponse
{
    public string status;
    public JsonData data;
}

// dataオブジェクト
public class JsonData
{
    public List<List<object>> value;
}

// 拡張機能ランキング
public class ExRank
{
    public int Stage { get; set; }
    public int Ranking { get; set; }
    public string Name { get; set; }
    public int RightHand { get; set; }
    public int Glasses { get; set; }
    public int Head { get; set; }
    public int LeftHand { get; set; }
    public int CatBody { get; set; }
    public int CatFace { get; set; }
    public int NickName { get; set; }
    public int Kpm { get; set; }
}

// スプレッドシートAPIステータス
public class ApiStatus
{
    public string Mail { get; set; }
    public string Ou { get; set; }
    public string LastName { get; set; }
    public int Gold { get; set; }
    public ExRank exRank { get; set; }
    public string rKpm { get; set; }
    public long[] Medal { get; set; }
    public long[] Item { get; set; }

    public ApiStatus()
    {
        Medal = new long[5];
        Item = new long[4];
    }
}

// 拡張機能ステータス
public class ExStatus
{
    public ApiStatus apiStatus { get; set; }
    public int[] Inventory { get; set; }
}


[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData")]
public class SaveData : ScriptableObject
{
    // ExRankのリストを作成
    public List<ExRank> ExRankings = new List<ExRank>();

    // ApiStatus オブジェクトのインスタンス化
    public ApiStatus apiStatus = new ApiStatus();

    // ExRank オブジェクトのインスタンス化
    public ExRank exRank = new ExRank();



    [SerializeField]
    public string userName;

    [SerializeField]
    public string Email;

    [SerializeField]
    public string Ou;

    [SerializeField]
    public string lastName;

    [SerializeField]
    public int[] status = new int[4];

    [SerializeField]
    public int[] equipment = new int[7];

    [SerializeField]
    public int[] inventory = new int[40];

    [SerializeField]
    public bool[] items = new bool [256];

    [SerializeField]
    public int[] medals = new int[100];

    [SerializeField]
    public int[] kpms = new int[8];


    public IList<object> SaveAllDataForGss()
    {
        IList<object> retData = new List<object> { };

        // ApiStatus のデータを追加
        retData.Add(apiStatus.Mail);
        retData.Add(apiStatus.Ou);
        retData.Add(apiStatus.LastName);
        retData.Add(apiStatus.Gold);

        // ExRank のデータを追加
        retData.Add(exRank.Stage);
        retData.Add(exRank.Ranking);
        retData.Add(exRank.Name);
        retData.Add(exRank.RightHand);
        retData.Add(exRank.Glasses);
        retData.Add(exRank.Head);
        retData.Add(exRank.LeftHand);
        retData.Add(exRank.CatBody);
        retData.Add(exRank.CatFace);
        retData.Add(exRank.NickName);
        retData.Add(exRank.Kpm);

        EncodeFromUnity();

        // 長い数値のデータを追加
        retData.Add(apiStatus.rKpm);
        for (int i = 0; i < apiStatus.Medal.Length; i++)
        {
            retData.Add(apiStatus.Medal[i]);
        }
        for (int i = 0; i < apiStatus.Item.Length; i++)
        {
            retData.Add(apiStatus.Item[i]);
        }

        return retData;
    }

    public void LoadAllDataFromGss(IList<object> list)       //スプレッドシートからデータロード時に真っ先に呼ばれる
    {
        // ApiStatus に値を設定
        apiStatus.Mail = list[0].ToString();
        apiStatus.Ou = list[1].ToString();
        apiStatus.LastName = list[2].ToString();
        apiStatus.Gold = Convert.ToInt32(list[3]);

        // ExRank に値を設定
        exRank.Stage = Convert.ToInt32(list[4]);
        exRank.Ranking = Convert.ToInt32(list[5]);
        exRank.Name = list[6].ToString();
        exRank.RightHand = Convert.ToInt32(list[7]);
        exRank.Glasses = Convert.ToInt32(list[8]);
        exRank.Head = Convert.ToInt32(list[9]);
        exRank.LeftHand = Convert.ToInt32(list[10]);
        exRank.CatBody = Convert.ToInt32(list[11]);
        exRank.CatFace = Convert.ToInt32(list[12]);
        exRank.NickName = Convert.ToInt32(list[13]);
        exRank.Kpm = Convert.ToInt32(list[14]);

        // ApiStatus に ExRank を設定
        apiStatus.exRank = exRank;

        // 長い数値の項目に値を設定
        apiStatus.rKpm = list[15].ToString();
        apiStatus.Medal[0] = Convert.ToInt64(list[16]);
        apiStatus.Medal[1] = Convert.ToInt64(list[17]);
        apiStatus.Medal[2] = Convert.ToInt64(list[18]);
        apiStatus.Medal[3] = Convert.ToInt64(list[19]);
        apiStatus.Medal[4] = Convert.ToInt64(list[20]);
        apiStatus.Item[0] = Convert.ToInt64(list[21]);
        apiStatus.Item[1] = Convert.ToInt64(list[22]);
        apiStatus.Item[2] = Convert.ToInt64(list[23]);
        apiStatus.Item[3] = Convert.ToInt64(list[24]);

        DecodeToUnity();
    }

    public void DecodeToUnity()
    {
        userName = apiStatus.exRank.Name;
        Email = apiStatus.Mail;
        Ou = apiStatus.Ou;
        lastName = apiStatus.LastName;
        status[0] = apiStatus.Gold;
        status[1] = apiStatus.exRank.Stage;
        status[2] = apiStatus.exRank.Ranking;
        status[3] = apiStatus.exRank.Kpm;
        equipment[0] = apiStatus.exRank.RightHand;
        equipment[1] = apiStatus.exRank.Glasses;
        equipment[2] = apiStatus.exRank.Head;
        equipment[3] = apiStatus.exRank.LeftHand;
        equipment[4] = apiStatus.exRank.CatBody;
        equipment[5] = apiStatus.exRank.CatFace;
        equipment[6] = apiStatus.exRank.NickName;
        DecodeItemData(apiStatus.Item);
        DecodeMedalData(apiStatus.Medal);
        DecodeKpmData(apiStatus.rKpm);
    }

    public void DecodeItemData(long[] itemData)
    {
        // 各 long 値をビット単位で調べる
        for (int i = 0; i < itemData.Length; i++)
        {
            long currentItemData = itemData[i];
            for (int bit = 0; bit < 64; bit++)
            {
                // currentItemData から特定のビット位置の値を取得
                bool isItemPresent = (currentItemData & (1L << bit)) != 0;
                // 計算したビット位置に応じた items 配列の位置に値をセット
                items[i * 64 + bit] = isItemPresent;
            }
        }
    }

    public void DecodeMedalData(long[] medalCode)
    {
        int mask = 0b111; // 3ビットを取り出すためのマスク

        for (int i = 0; i < medalCode.Length; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                // encodedValues[i]から3ビットずつ切り出して、配列に格納
                // 最下位ビットから開始するため、シフトするビット数を調整
                medals[i * 20 + j] = (int)((medalCode[i] >> (j * 3)) & mask);
            }
        }
    }

    public void DecodeKpmData(string rkpm)
    {
        int arrayIndex = 7;

        // 文字列の末尾から3文字ずつ取得していく
        for (int i = rkpm.Length; i > 0; i -= 3)
        {
            // 3文字の部分文字列を取得
            string part = rkpm.Substring(Math.Max(i - 3, 0), i - Math.Max(i - 3, 0));
            kpms[arrayIndex] = int.Parse(part);
            arrayIndex--;
        }
    }

    public void EncodeFromUnity()
    {
        // ExRank へのデータの設定
        if (exRank != null)
        {
            exRank.Name = userName;
            exRank.Stage = status[1];
            exRank.Ranking = status[2];
            exRank.Kpm = status[3];
            exRank.RightHand = equipment[0];
            exRank.Glasses = equipment[1];
            exRank.Head = equipment[2];
            exRank.LeftHand = equipment[3];
            exRank.CatBody = equipment[4];
            exRank.CatFace = equipment[5];
            exRank.NickName = equipment[6];
        }

        // ApiStatus へのデータの設定
        if (apiStatus != null)
        {
            apiStatus.Mail = Email;
            apiStatus.Ou = Ou;
            apiStatus.LastName = lastName;
            apiStatus.Gold = status[0];
            apiStatus.exRank = exRank;

            EncodeItemData();
            EncodeMedalData();
            EncodeKpmData();
        }
    }
    public void EncodeItemData()
    {
        long[] encodedItems = new long[4];
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i])
            {
                int itemIndex = i / 64;
                int bitPosition = i % 64;
                encodedItems[itemIndex] |= (1L << bitPosition);
            }
        }
        apiStatus.Item = encodedItems;
    }
    public void EncodeMedalData()
    {
        long[] encodedMedals = new long[5];
        for (int i = 0; i < medals.Length; i++)
        {
            int medalIndex = i / 20;
            int bitPosition = (i % 20) * 3;
            encodedMedals[medalIndex] |= ((long)medals[i] << bitPosition);
        }
        apiStatus.Medal = encodedMedals;
    }
    public void EncodeKpmData()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = kpms.Length - 1; i >= 0; i--)
        {
            // 最初の要素以外は3桁になるように0でパディング
            if (i == kpms.Length - 1 && kpms[i] <= 999)
            {
                sb.Insert(0, kpms[i].ToString());
            }
            else
            {
                sb.Insert(0, kpms[i].ToString("D3"));
            }
        }
        apiStatus.rKpm = sb.ToString();
    }




    public void setStatusIndex(int index, int value)
    {
        status[index] = value;
    }

    public void setEquipmentIndex(int index, int value)
    {
        equipment[index] = value;
    }

    public void setInventoryIndex(int index, int value)
    {
        inventory[index] = value;
    }

    public void saveGssItems(int startId, int endId, int[] saveData)
    {
        List<object> data = saveData.Cast<object>().ToList();
        GSheet.WriteRow(GssIndex.Equipment + startId, GssIndex.Equipment + endId, data);
    }

    public void saveGssKpis(int startId, int endId, int[] saveData)
    {
        List<object> data = saveData.Cast<object>().ToList();
        GSheet.WriteRow(GssIndex.Kpms + startId, GssIndex.Kpms + endId, data);
    }

    public void saveGssMedals(int index)
    {
//        long[] medalData = { medalCode[index] };
  //      List<object> data = medalData.Cast<object>().ToList();
    //    GSheet.WriteRow(GssIndex.Medals + index, GssIndex.Medals + index, data);
    }

    public void setMedalIndex(int index, int value)
    {
        medals[index] = value;
    }

    public int getBlankInventoryIndex()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == 0)
            {
                return i;
            }
        }
        return -1;
    }
    public string getUserName()
    {
        return userName;
    }
    public int[] getStatus()
    {
        return status;
    }
    public int[] getEquipment()
    {
        return equipment;
    }
    public int[] getInventory()
    {
        return inventory;
    }
    public int[] getMedals()
    {
        return medals;
    }
    public int[] getKpms()
    {
        return kpms;
    }

    public bool existInventory(int id)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool existEquipment(int id)
    {
        for (int i = 0; i < equipment.Length; i++)
        {
            if (equipment[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public void updateKpm(int newKpm)
    {
        // 要素1から6までを0から5に移動
        for (int i = 0; i < 6; i++)
        {
            kpms[i] = kpms[i + 1];
        }

        // 6番目の要素に新しい値を代入
        kpms[6] = newKpm;

        // 平均を計算
        double average = 0;
        for (int i = 0; i < kpms.Length; i++)
        {
            average += kpms[i];
        }
        average /= kpms.Length;

        int[] newKpi = new int[1];
        newKpi[0] = (int)Math.Round(average); // 四捨五入してintにキャスト;

//        int[] saveKpis = newKpi.Concat(kpms).ToArray();

//        saveGssKpis(-1, GssSize.Kpms, saveKpis);    // 先頭の前にKpmを追加しているため-1から
    }






    public void setUserNameFromFireBase(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        userName = msg;
    }
    public void setStatusFromFireBase(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setStatus msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            status[i] = int.Parse(intStrings[i]);
        }
    }
    public void setEquipmentFromFireBase(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setEquipment msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            equipment[i] = int.Parse(intStrings[i]);
        }
    }
    public void setInventoryFromFireBase(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setInventory msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            inventory[i] = int.Parse(intStrings[i]);
        }
    }
    public void setMedalsFromFireBase(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setMedals msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            medals[i] = int.Parse(intStrings[i]);
        }
    }
    public void setKpmFromFireBase(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setKpm msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            kpms[i] = int.Parse(intStrings[i]);
        }
    }

    public void setRankingFromExtension(string jsonMsg)
    {
        Debug.Log("Received JSON: " + jsonMsg);

        // JSONデータをデシリアライズ
        var jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(jsonMsg);

        foreach (var item in jsonResponse.data.value)
        {
            var rank = new ExRank
            {
                Stage = Convert.ToInt32(item[0]),
                Ranking = Convert.ToInt32(item[1]),
                Name = (string)item[2],
                RightHand = Convert.ToInt32(item[3]),
                Glasses = Convert.ToInt32(item[4]),
                Head = Convert.ToInt32(item[5]),
                LeftHand = Convert.ToInt32(item[6]),
                CatBody = Convert.ToInt32(item[7]),
                CatFace = Convert.ToInt32(item[8]),
                NickName = Convert.ToInt32(item[9]),
                Kpm = Convert.ToInt32(item[10])
            };
            ExRankings.Add(rank);
        }

        // ここでrankingsリストを使用する
        // 例: Debug.Logでリストの内容を表示
        foreach (var rank in ExRankings)
        {
            Debug.Log($"Ranking: {rank.Ranking}： {rank.Name}： {rank.Kpm}");
        }
    }
}