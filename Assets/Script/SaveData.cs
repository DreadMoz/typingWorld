using System;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
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
// RightHnad,Head(151),Glasses(121),LeftHand,CatBody(201)あえて0,CatFace(101),NickName(211)
public class eq
{
    public const int RightHnad = 0;
    public const int Head = 1;
    public const int Glasses = 2;
    public const int LeftHand = 3;
    public const int CatBody = 4;
    public const int CatFace = 5;
    public const int NickName = 6;
}
// Gold,Server,Rank,userName
public class se
{
    public const int Volume = 0;
    public const int CatNum = 1;
    public const int dummy2 = 2;
    public const int dummy3 = 3;
    public const int dummy4 = 4;
    public const int dummy5 = 5;
    public const int dummy6 = 6;
    public const int dummy7 = 7;
    public const int dummy8 = 8;
    public const int dummy9 = 9;
}
[Serializable]
public class ExtensionData
{
    public JsonData rankingData; // JsonDataは先に定義された型を使用
    public SerializableSaveData statusData; // StatusDataTypeはステータスデータの型
}

// dataオブジェクト
public class JsonData
{
    public List<List<object>> value;
}

// 拡張機能ランキング
[Serializable]
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
[Serializable]
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
    // ExRank オブジェクトのインスタンス化
        exRank = new ExRank();
        Medal = new long[5];
        Item = new long[4];
    }
}

// 拡張機能ステータス
[Serializable]
public class ExStatus
{
    public ApiStatus apiStatus { get; set; }
    public int[] Inventory { get; set; }
    public int[] Settings { get; set; }

    public ExStatus()
    {
        apiStatus = new ApiStatus();    // ApiStatus オブジェクトのインスタンス化
        Inventory = new int[40];
        Settings = new int[10];
    }
}

[Serializable]
public class SerializableSaveData
{
    public string Email;
    public string Ou;
    public string LastName;
    public int Gold;
    public int Stage;
    public int Ranking;
    public string Name;
    public int RightHand;
    public int Glasses;
    public int Head;
    public int LeftHand;
    public int CatBody;
    public int CatFace;
    public int NickName;
    public int Kpm;
    public int[] Inventory;
    public long[] Items;
    public long[] Medals;
    public string Kpms;
    public int[] Settings;
    // 必要に応じて他のフィールドも追加

    public string CompileGameDataForExtension(ExStatus exStatus)
    {
        SerializableSaveData data = new SerializableSaveData
        {
            Email = exStatus.apiStatus.Mail,
            Ou = exStatus.apiStatus.Ou,
            LastName = exStatus.apiStatus.LastName,
            Gold = exStatus.apiStatus.Gold,

            Stage = exStatus.apiStatus.exRank.Stage,
            Ranking = exStatus.apiStatus.exRank.Ranking,
            Name = exStatus.apiStatus.exRank.Name,
            RightHand = exStatus.apiStatus.exRank.RightHand,
            Glasses = exStatus.apiStatus.exRank.Glasses,
            Head = exStatus.apiStatus.exRank.Head,
            LeftHand = exStatus.apiStatus.exRank.LeftHand,
            CatBody = exStatus.apiStatus.exRank.CatBody,
            CatFace = exStatus.apiStatus.exRank.CatFace,
            NickName = exStatus.apiStatus.exRank.NickName,
            Kpm = exStatus.apiStatus.exRank.Kpm,

            Inventory = exStatus.Inventory,
            Items = exStatus.apiStatus.Item,
            Medals = exStatus.apiStatus.Medal,
            Kpms = exStatus.apiStatus.rKpm,
            Settings = exStatus.Settings,
        };

        // statusDataプロパティを持つ新しいオブジェクトを作成し、JSONにシリアライズ
        var wrappedData = new { statusData = data };
        return JsonConvert.SerializeObject(wrappedData);
    }
}


[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData")]
public class SaveData : ScriptableObject
{
    // ExRankのリストを作成
    public List<ExRank> ExRankings = new List<ExRank>();

    // Status オブジェクトのインスタンス化
    public ExStatus exStatus = new ExStatus();



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

    [SerializeField]
    public int[] settings = new int[10];


    // 拡張機能からランキング一覧を取得する。
    public void setRankingFromExtension(string jsonMsg)
    {
        Debug.Log("Received Ranking JSON: " + jsonMsg);

        // JSONデータをデシリアライズ
        var jsonResponse = JsonConvert.DeserializeObject<JsonData>(jsonMsg);

        foreach (var item in jsonResponse.value)
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
        foreach (var rank in ExRankings)
        {
            Debug.Log($"Ranking: {rank.Ranking}： {rank.Name}： {rank.Kpm}");
        }
    }

    // 拡張機能から個人データを取得する。
    public void setStatusFromExtension(string jsonMsg)
    {
        Debug.Log("Received Status JSON: " + jsonMsg);

        // JSONデータをデシリアライズ
        SerializableSaveData exData = JsonConvert.DeserializeObject<SerializableSaveData>(jsonMsg);

        // ApiStatus に値を設定
        exStatus.apiStatus.Mail = exData.Email;
        exStatus.apiStatus.Ou = exData.Ou;
        exStatus.apiStatus.LastName = exData.LastName;
        exStatus.apiStatus.Gold = exData.Gold;

        // ExRank に値を設定
        exStatus.apiStatus.exRank.Stage = exData.Stage;
        exStatus.apiStatus.exRank.Ranking = exData.Ranking;
        exStatus.apiStatus.exRank.Name = exData.Name;
        exStatus.apiStatus.exRank.RightHand = exData.RightHand;
        exStatus.apiStatus.exRank.Glasses = exData.Glasses;
        exStatus.apiStatus.exRank.Head = exData.Head;
        exStatus.apiStatus.exRank.LeftHand = exData.LeftHand;
        exStatus.apiStatus.exRank.CatBody = exData.CatBody;
        exStatus.apiStatus.exRank.CatFace = exData.CatFace;
        exStatus.apiStatus.exRank.NickName = exData.NickName;
        exStatus.apiStatus.exRank.Kpm = exData.Kpm;

        // ここは配列40　15〜54
        for (int i = 0; i < exStatus.Inventory.Length; i++)
        {
            exStatus.Inventory[i] = exData.Inventory[i];
        }
        // ここは配列4　55〜58
        for (int i = 0; i < exStatus.apiStatus.Item.Length; i++)
        {
            exStatus.apiStatus.Item[i] = exData.Items[i];
        }
        // ここは配列5　59〜63
        for (int i = 0; i < exStatus.apiStatus.Medal.Length; i++)
        {
            exStatus.apiStatus.Medal[i] = exData.Medals[i];
        }
        exStatus.apiStatus.rKpm = exData.Kpms;
        // ここは配列10　65〜74
        for (int i = 0; i < exStatus.Settings.Length; i++)
        {
            exStatus.Settings[i] = exData.Settings[i];
        }

        DecodeToUnity();
    }

    // 拡張機能なし GSSから最低限のデータ取得
    public void LoadAllDataFromGss(IList<object> list)
    {
        // ApiStatus に値を設定
        exStatus.apiStatus.Mail = list[0].ToString();
        exStatus.apiStatus.Ou = list[1].ToString();
        exStatus.apiStatus.LastName = list[2].ToString();
        exStatus.apiStatus.Gold = Convert.ToInt32(list[3]);

        // ExRank に値を設定
        exStatus.apiStatus.exRank.Stage = Convert.ToInt32(list[4]);
        exStatus.apiStatus.exRank.Ranking = Convert.ToInt32(list[5]);
        exStatus.apiStatus.exRank.Name = list[6].ToString();
        exStatus.apiStatus.exRank.RightHand = Convert.ToInt32(list[7]);
        exStatus.apiStatus.exRank.Glasses = Convert.ToInt32(list[8]);
        exStatus.apiStatus.exRank.Head = Convert.ToInt32(list[9]);
        exStatus.apiStatus.exRank.LeftHand = Convert.ToInt32(list[10]);
        exStatus.apiStatus.exRank.CatBody = Convert.ToInt32(list[11]);
        exStatus.apiStatus.exRank.CatFace = Convert.ToInt32(list[12]);
        exStatus.apiStatus.exRank.NickName = Convert.ToInt32(list[13]);
        exStatus.apiStatus.exRank.Kpm = Convert.ToInt32(list[14]);

        // 長い数値の項目に値を設定
        exStatus.apiStatus.rKpm = list[15].ToString();
        exStatus.apiStatus.Medal[0] = Convert.ToInt64(list[16]);
        exStatus.apiStatus.Medal[1] = Convert.ToInt64(list[17]);
        exStatus.apiStatus.Medal[2] = Convert.ToInt64(list[18]);
        exStatus.apiStatus.Medal[3] = Convert.ToInt64(list[19]);
        exStatus.apiStatus.Medal[4] = Convert.ToInt64(list[20]);
        exStatus.apiStatus.Item[0] = Convert.ToInt64(list[21]);
        exStatus.apiStatus.Item[1] = Convert.ToInt64(list[22]);
        exStatus.apiStatus.Item[2] = Convert.ToInt64(list[23]);
        exStatus.apiStatus.Item[3] = Convert.ToInt64(list[24]);

        DecodeToUnity();
    }

    public string makeExtensionJsonData()
    {
        EncodeFromUnity();

        SerializableSaveData ssd = new SerializableSaveData();
        string jsonData = ssd.CompileGameDataForExtension(exStatus);

        return jsonData;
    }

    // GoogleAPIデータ、拡張機能データをUnityデータに置き換える。
    public void DecodeToUnity()
    {
        DecodeStatusData();
        DecodeEquipmentData();
        DecodeItemData();
        AssignInventory();      // 同じ型の場合はポインタを渡す
        DecodeMedalData();
        DecodeKpmData();
        AssignSettings();       // 同じ型の場合はポインタを渡す
    }

    public void DecodeStatusData()
    {
        userName = exStatus.apiStatus.exRank.Name;
        Email = exStatus.apiStatus.Mail;
        Ou = exStatus.apiStatus.Ou;
        lastName = exStatus.apiStatus.LastName;
        status[0] = exStatus.apiStatus.Gold;
        status[1] = exStatus.apiStatus.exRank.Stage;
        status[2] = exStatus.apiStatus.exRank.Ranking;
        status[3] = exStatus.apiStatus.exRank.Kpm;
    }
    public void DecodeEquipmentData()
    {
        equipment[0] = exStatus.apiStatus.exRank.RightHand;
        equipment[1] = exStatus.apiStatus.exRank.Glasses;
        equipment[2] = exStatus.apiStatus.exRank.Head;
        equipment[3] = exStatus.apiStatus.exRank.LeftHand;
        equipment[4] = exStatus.apiStatus.exRank.CatBody;
        equipment[5] = exStatus.apiStatus.exRank.CatFace;
        equipment[6] = exStatus.apiStatus.exRank.NickName;
    }

    public void DecodeItemData()
    {
        long[] itemData = exStatus.apiStatus.Item;
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

    public void DecodeMedalData()
    {
        long[] medalCode = exStatus.apiStatus.Medal;
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

    public void DecodeKpmData()
    {
        string rkpm = exStatus.apiStatus.rKpm;
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
    public void AssignInventory()
    {
        inventory = exStatus.Inventory;
    }
    public void AssignSettings()
    {
        settings = exStatus.Settings;
    }

    // GSSに保存するためのデータを現在のゲームデータから作る。
    public IList<object> CompileGameDataForGss()
    {
        IList<object> retData = new List<object> { };

        // ApiStatus のデータを追加
        retData.Add(exStatus.apiStatus.Mail);
        retData.Add(exStatus.apiStatus.Ou);
        retData.Add(exStatus.apiStatus.LastName);
        retData.Add(exStatus.apiStatus.Gold);

        // ExRank のデータを追加
        retData.Add(exStatus.apiStatus.exRank.Stage);
        retData.Add(exStatus.apiStatus.exRank.Ranking);
        retData.Add(exStatus.apiStatus.exRank.Name);
        retData.Add(exStatus.apiStatus.exRank.RightHand);
        retData.Add(exStatus.apiStatus.exRank.Glasses);
        retData.Add(exStatus.apiStatus.exRank.Head);
        retData.Add(exStatus.apiStatus.exRank.LeftHand);
        retData.Add(exStatus.apiStatus.exRank.CatBody);
        retData.Add(exStatus.apiStatus.exRank.CatFace);
        retData.Add(exStatus.apiStatus.exRank.NickName);
        retData.Add(exStatus.apiStatus.exRank.Kpm);

        EncodeFromUnity();

        // 長い数値のデータを追加
        retData.Add(exStatus.apiStatus.rKpm);
        for (int i = 0; i < exStatus.apiStatus.Medal.Length; i++)
        {
            retData.Add(exStatus.apiStatus.Medal[i]);
        }
        for (int i = 0; i < exStatus.apiStatus.Item.Length; i++)
        {
            retData.Add(exStatus.apiStatus.Item[i]);
        }

        return retData;
    }

    // UnityデータをGoogleAPIデータ、拡張機能データにまとめる。
    public void EncodeFromUnity()
    {
        EncodeStatusData();
        EncodeEquipmentData();
        EncodeItemData();
        EncodeMedalData();
        EncodeKpmData();
    }

    public void EncodeStatusData()
    {
        if (exStatus.apiStatus.exRank != null)
        {
            exStatus.apiStatus.exRank.Name = userName;
            exStatus.apiStatus.exRank.Stage = status[1];
            exStatus.apiStatus.exRank.Ranking = status[2];
            exStatus.apiStatus.exRank.Kpm = status[3];
        }
        if (exStatus.apiStatus != null)
        {
            exStatus.apiStatus.Mail = Email;
            exStatus.apiStatus.Ou = Ou;
            exStatus.apiStatus.LastName = lastName;
            exStatus.apiStatus.Gold = status[0];
        }
    }
    public void EncodeEquipmentData()
    {
        if (exStatus.apiStatus.exRank != null)
        {
            exStatus.apiStatus.exRank.RightHand = equipment[0];
            exStatus.apiStatus.exRank.Glasses = equipment[1];
            exStatus.apiStatus.exRank.Head = equipment[2];
            exStatus.apiStatus.exRank.LeftHand = equipment[3];
            exStatus.apiStatus.exRank.CatBody = equipment[4];
            exStatus.apiStatus.exRank.CatFace = equipment[5];
            exStatus.apiStatus.exRank.NickName = equipment[6];
        }
    }
    public void EncodeItemData()
    {
        if (exStatus.apiStatus != null)
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
            exStatus.apiStatus.Item = encodedItems;
        }
    }
    public void EncodeMedalData()
    {
        if (exStatus.apiStatus != null)
        {
            long[] encodedMedals = new long[5];
            for (int i = 0; i < medals.Length; i++)
            {
                int medalIndex = i / 20;
                int bitPosition = (i % 20) * 3;
                encodedMedals[medalIndex] |= ((long)medals[i] << bitPosition);
            }
            exStatus.apiStatus.Medal = encodedMedals;
        }
    }
    public void EncodeKpmData()
    {
        if (exStatus.apiStatus != null)
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
            exStatus.apiStatus.rKpm = sb.ToString();
        }
    }

    public void addItem(int index)
    {
        items[index] = true;
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
//        GSheet.WriteRow(GssIndex.Equipment + startId, GssIndex.Equipment + endId, data);
    }

    public void saveGssKpis(int startId, int endId, int[] saveData)
    {
        List<object> data = saveData.Cast<object>().ToList();
//        GSheet.WriteRow(GssIndex.Kpms + startId, GssIndex.Kpms + endId, data);
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

}