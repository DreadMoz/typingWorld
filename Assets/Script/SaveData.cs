using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GssIndex
{
    public const int Status = 4;
    public const int Kpms = 8;
    public const int Medals = 16;
    public const int Equipment = 21;
    public const int Inventory = 28;
}
public class GssSize
{
    public const int Status = 4;
    public const int Kpms = 8;
    public const int Medals = 5;
    public const int Equipment = 7;
    public const int Inventory = 40;
}
// Gold,Server,Rank,Kpm,userName
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

[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData")]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private string userName;

    [SerializeField]
    private int[] status = new int[GssSize.Status];

    [SerializeField]
    private int[] equipment = new int[GssSize.Equipment];

    [SerializeField]
    private int[] inventory = new int[GssSize.Inventory];

    [SerializeField]
    private int[] medals = new int[GssSize.Medals];

    [SerializeField]
    private int[] kpms = new int[GssSize.Kpms];

    private long[] medalCode = new long[GssSize.Medals];

    public void loadAllDataFromGss(IList<object> list)       //スプレッドシートからデータロード時に真っ先に呼ばれる
    {
        int statusIndex = 0; // 配列のインデックスを追跡
        int equipmentIndex = 0;
        int inventoryIndex = 0;
        int medalsIndex = 0;
        int kpmsIndex = 0;


        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = 0;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (i == 3)
            {
                // インデックスが4の場合、ユーザーネームに代入
                userName = list[i].ToString();
            }
            else if (i >= GssIndex.Status && i < GssIndex.Status + GssSize.Status)
            {
                // ステータス配列に代入
                status[statusIndex++] = int.Parse(list[i].ToString());
            }
            else if (i >= GssIndex.Equipment && i < GssIndex.Equipment + GssSize.Equipment)
            {
                // 装備配列に代入
                equipment[equipmentIndex++] = int.Parse(list[i].ToString());
            }
            else if (i >= GssIndex.Inventory && i < GssIndex.Inventory + GssSize.Inventory)
            {
                // 道具箱配列に代入
                inventory[inventoryIndex++] = int.Parse(list[i].ToString());
            }
            else if (i >= GssIndex.Medals && i < GssIndex.Medals + GssSize.Medals)
            {
                // メダル配列に代入
                medalCode[medalsIndex++] = long.Parse(list[i].ToString());
            }
            else if (i >= GssIndex.Kpms && i < GssIndex.Kpms + GssSize.Kpms)
            {
                // Kpm配列に代入
                kpms[kpmsIndex++] = int.Parse(list[i].ToString());
            }
        }
        DecodeFromLongArray();
    }
    private void DecodeFromLongArray()
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

    public int EncodeToLongArray()
    {
        long[] newMedalCode = new long[5]; // エンコードされたデータを格納する配列を初期化

        for (int i = 0; i < medals.Length; i++)
        {
            if (medals[i] < 0)
            {
                medals[i] = 0;
            }

            int index = i / 20; // 現在のmedalが格納されるべきmedalCodeのインデックス
            int position = i % 20; // 現在のmedalのmedalCode内での位置

            // medals[i]を適切な位置にシフトして、medalCode[index]に組み合わせる
            newMedalCode[index] |= ((long)medals[i] << (position * 3));
        }

        for (int j = 0; j < medalCode.Length; j++)
        {
            if (medalCode[j] != newMedalCode[j])
            {
                medalCode[j] = newMedalCode[j];
                return j;
            }
        }
        return -1;
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
        long[] medalData = { medalCode[index] };
        List<object> data = medalData.Cast<object>().ToList();
        GSheet.WriteRow(GssIndex.Medals + index, GssIndex.Medals + index, data);
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
        // 要素1から7までを0から6に移動
        for (int i = 0; i < 7; i++)
        {
            kpms[i] = kpms[i + 1];
        }

        // 7番目の要素に新しい値を代入
        kpms[7] = newKpm;

        // 平均を計算
        double average = 0;
        for (int i = 0; i < kpms.Length; i++)
        {
            average += kpms[i];
        }
        average /= kpms.Length;

        int[] newKpi = new int[1];
        newKpi[0] = (int)Math.Round(average); // 四捨五入してintにキャスト;

        int[] saveKpis = newKpi.Concat(kpms).ToArray();

        saveGssKpis(-1, GssSize.Kpms, saveKpis);    // 先頭の前にKpmを追加しているため-1から
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