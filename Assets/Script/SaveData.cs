using System;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using Unity.VisualScripting;
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
// RightHand,Head(151),Glasses(121),LeftHand,CatBody(201)あえて0,CatFace(101),NickName(211)
public class eq
{
    public const int RightHand = 0;
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
    public const int Extension = 0;
    public const int Volume = 1;
    public const int CatNum = 2;
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
    public JsonData rankingData;
    public SerializableExSaveData statusData; // StatusDataTypeはステータスデータの型
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

// 拡張機能ステータス
[Serializable]
public class SerializableExSaveData
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
}


[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData")]
public class SaveData : ScriptableObject
{
    // ExRankのリストを作成
    public List<ExRank> ExRankings = new List<ExRank>();

    [SerializeField]
    public string UserName;

    [SerializeField]
    public string Email;

    [SerializeField]
    public string Ou;

    [SerializeField]
    public string LastName;

    [SerializeField]
    public int[] Status = new int[4];

    [SerializeField]
    public int[] Equipment = new int[7];

    [SerializeField]
    public int[] Inventory = new int[40];

    [SerializeField]
    public bool[] Items = new bool [256];

    [SerializeField]
    public int[] Medals = new int[100];

    [SerializeField]
    public int[] Kpms = new int[8];

    [SerializeField]
    public int[] Settings = new int[10];


    // 拡張機能からランキング一覧を取得する。
    public void setRankingFromExtension(string rankingData)
    {
        Debug.Log("Received Ranking JSON: " + rankingData);

        // JSONデータをデシリアライズ
        var jsonResponse = JsonConvert.DeserializeObject<JsonData>(rankingData);

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
            ExRankings.Add(rank);       // ランキングデータ格納場所
        }
        foreach (var rank in ExRankings)
        {
            Debug.Log($"Ranking: {rank.Ranking}： {rank.Name}： {rank.Kpm}");
        }
    }

    // 初期データ登録。
    public void setNewData(string googleMail, string googleFirstName, string googleLastName, string googleOu)
    {
        Debug.Log("setNewData: " + googleMail + googleFirstName + googleLastName + googleOu);

        // ApiStatus に値を設定
        Email = googleMail;
        Ou = googleOu;
        LastName = googleLastName;
        Status[st.Gold] = 50;

        // ExRank に値を設定
        Status[st.Server] = 0;
        Status[st.Rank] = 0;
        UserName = googleFirstName;
        Equipment[eq.RightHand] = 0;
        Equipment[eq.Glasses] = 0;
        Equipment[eq.Head] = 0;
        Equipment[eq.LeftHand] = 0;
        Equipment[eq.CatFace] = 0;
        Equipment[eq.NickName] = 0;
        Status[st.Kpm] = 0;

        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i] = 0;
        }
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i] = false;
        }
        for (int i = 0; i < Medals.Length; i++)
        {
            Medals[i] = 0;
        }
        Medals[0] = 1;
        for (int i = 0; i < Kpms.Length; i++)
        {
            Kpms[i] = 0;
        }
        for (int i = 0; i < Settings.Length; i++)
        {
            Settings[i] = 0;
        }
        Settings[se.Volume] = 50;
    }

    // 拡張機能からステータスデータを取得する。
    public void setStatusFromExtension(string statusData)
    {
        Debug.Log("Received Status JSON: " + statusData);

        // JSONデータをデシリアライズ
        SerializableExSaveData exData = JsonConvert.DeserializeObject<SerializableExSaveData>(statusData);

        // ApiStatus に値を設定
        Email = exData.Email;
        Ou = exData.Ou;
        LastName = exData.LastName;
        Status[st.Gold] = exData.Gold;

        // ExRank に値を設定
        Status[st.Server] = exData.Stage;
        Status[st.Rank] = exData.Ranking;
        UserName = exData.Name;
        Equipment[eq.RightHand] = exData.RightHand;
        Equipment[eq.Glasses] = exData.Glasses;
        Equipment[eq.Head] = exData.Head;
        Equipment[eq.LeftHand] = exData.LeftHand;
        Equipment[eq.CatBody] = exData.CatBody;
        Equipment[eq.CatFace] = exData.CatFace;
        Equipment[eq.NickName] = exData.NickName;
        Status[st.Kpm] = exData.Kpm;

        // ここは配列40のコピー
        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i] = exData.Inventory[i];
        }

        // ここはlong[4]をbool[100]に変換
        DecodeItemData(exData.Items);

        // ここはlong[5]をint[100]に変換
        DecodeMedalData(exData.Medals);

        // ここは配列8<-文字列
        DecodeKpmData(exData.Kpms);

        // ここは配列10のコピー
        for (int i = 0; i < Settings.Length; i++)
        {
            Settings[i] = exData.Settings[i];
        }
    }

    // 拡張機能なし GSSから最低限のデータ取得
    public void LoadAllDataFromGss(IList<object> list)
    {
        try
        {
            // ApiStatus に値を設定
            Email = list[0].ToString();
            Ou = list[1].ToString();
            LastName = list[2].ToString();
            Status[st.Gold] = Convert.ToInt32(list[3]);

            // ExRank に値を設定
            Status[st.Server] = Convert.ToInt32(list[4]);
            Status[st.Rank] = Convert.ToInt32(list[5]);
            UserName = list[6].ToString();
            Equipment[eq.RightHand] = 0;
            Equipment[eq.Glasses] = 0;
            Equipment[eq.Head] = 0;
            Equipment[eq.LeftHand] = 0;
            Equipment[eq.CatBody] = Convert.ToInt32(list[11]);
            Equipment[eq.CatFace] = 0;
            Equipment[eq.NickName] = 0;
            Status[st.Kpm] = Convert.ToInt32(list[14]);

            // ここは配列8<-文字列
            DecodeKpmData(list[15].ToString());

            long[] gssMedals = new long[5];
            gssMedals[0] = Convert.ToInt64(list[16]);
            gssMedals[1] = Convert.ToInt64(list[17]);
            gssMedals[2] = Convert.ToInt64(list[18]);
            gssMedals[3] = Convert.ToInt64(list[19]);
            gssMedals[4] = Convert.ToInt64(list[20]);

            // ここはlong[5]をint[100]に変換
            DecodeMedalData(gssMedals);

            long[] gssItems = new long[4];
            gssItems[0] = Convert.ToInt64(list[21]);
            gssItems[1] = Convert.ToInt64(list[22]);
            gssItems[2] = Convert.ToInt64(list[23]);
            gssItems[3] = Convert.ToInt64(list[24]);

            // ここはlong[4]をbool[100]に変換
            DecodeItemData(gssItems);

            setInventoryFromItems();
        }
        catch (FormatException ex)
        {
            // エラーメッセージとスタックトレースをログに記録
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            // その他の例外タイプ
            Console.WriteLine($"Unexpected error: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }

    private void setInventoryFromItems()
    {
        int inventoryId = 0;
        Array.Clear(Inventory, 0, Inventory.Length);
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == true)
            {
                Inventory[inventoryId] = i;
                inventoryId++;
            }
        }
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
                Items[i * 64 + bit] = isItemPresent;
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
                Medals[i * 20 + j] = (int)((medalCode[i] >> (j * 3)) & mask);
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
            Kpms[arrayIndex] = int.Parse(part);
            arrayIndex--;
        }
    }

    // 拡張機能に保存するためのデータを現在のゲームデータから作る。
    public string CompileGameDataForExtension(SaveData sd)
    {
        SerializableExSaveData data = new SerializableExSaveData
        {
            Email = sd.Email,
            Ou = sd.Ou,
            LastName = sd.LastName,
            Gold = sd.Status[st.Gold],

            Stage = sd.Status[st.Server],
            Ranking = sd.Status[st.Rank],
            Name = sd.UserName,
            RightHand = sd.Equipment[eq.RightHand],
            Glasses = sd.Equipment[eq.Glasses],
            Head = sd.Equipment[eq.Head],
            LeftHand = sd.Equipment[eq.LeftHand],
            CatBody = sd.Equipment[eq.CatBody],
            CatFace = sd.Equipment[eq.CatFace],
            NickName = sd.Equipment[eq.NickName],
            Kpm = sd.Status[st.Kpm],

            Inventory = sd.Inventory,
            Items = EncodeItemData(sd.Items),
            Medals = EncodeMedalData(sd.Medals),
            Kpms = EncodeKpmData(sd.Kpms),
            Settings = sd.Settings,
        };

        // statusDataプロパティを持つ新しいオブジェクトを作成し、JSONにシリアライズ
        var wrappedData = new { statusData = data };
        Debug.Log("wrappedData(SaveData): " + JsonConvert.SerializeObject(wrappedData));    // ログ出力を追加
        return JsonConvert.SerializeObject(wrappedData);
    }

    // GSSに保存するためのデータを現在のゲームデータから作る。
    public string CompileGameDataForGss(SaveData sd)
    {
        SerializableExSaveData data = new SerializableExSaveData
        {
            Email = sd.Email,
            Ou = sd.Ou,
            LastName = sd.LastName,
            Gold = sd.Status[st.Gold],

            Stage = sd.Status[st.Server],
            Ranking = sd.Status[st.Rank],
            Name = sd.UserName,
            RightHand = sd.Equipment[eq.RightHand],
            Glasses = sd.Equipment[eq.Glasses],
            Head = sd.Equipment[eq.Head],
            LeftHand = sd.Equipment[eq.LeftHand],
            CatBody = sd.Equipment[eq.CatBody],
            CatFace = sd.Equipment[eq.CatFace],
            NickName = sd.Equipment[eq.NickName],
            Kpm = sd.Status[st.Kpm],

            Items = EncodeItemData(sd.Items),
            Medals = EncodeMedalData(sd.Medals),
            Kpms = EncodeKpmData(sd.Kpms),
        };

        // statusDataプロパティを持つ新しいオブジェクトを作成し、JSONにシリアライズ
        Debug.Log("wrappedData(GssSaveData): " + JsonConvert.SerializeObject(data));    // ログ出力を追加
        return JsonConvert.SerializeObject(data);
    }

    public long[] EncodeItemData(bool[] items)
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
        return encodedItems;
    }
    public long[] EncodeMedalData(int[] medals)
    {
        long[] encodedMedals = new long[5];
        for (int i = 0; i < medals.Length; i++)
        {
            int medalIndex = i / 20;
            int bitPosition = (i % 20) * 3;
            encodedMedals[medalIndex] |= ((long)medals[i] << bitPosition);
        }
        return encodedMedals;
    }
    public string EncodeKpmData(int[] kpms)
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
        return sb.ToString();
    }

    public int getBlankInventoryIndex()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public bool existInventory(int id)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool existEquipment(int id)
    {
        for (int i = 0; i < Equipment.Length; i++)
        {
            if (Equipment[i] == id)
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
            Kpms[i] = Kpms[i + 1];
        }

        // 6番目の要素に新しい値を代入
        Kpms[6] = newKpm;

        // 平均を計算
        double average = 0;
        for (int i = 0; i < Kpms.Length; i++)
        {
            average += Kpms[i];
        }
        average /= Kpms.Length;

        int[] newKpi = new int[1];
        newKpi[0] = (int)Math.Round(average); // 四捨五入してintにキャスト;

        //        int[] saveKpis = newKpi.Concat(kpms).ToArray();
        //        saveGssKpis(-1, GssSize.Kpms, saveKpis);    // 先頭の前にKpmを追加しているため-1から
    }
}