using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData")]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private string userName;

    [SerializeField]
    private int[] status = new int[10];

    [SerializeField]
    private int[] equipment = new int[10];

    [SerializeField]
    private int[] inventory = new int[64];

    [SerializeField]
    private int[] medals = new int[100];

    [SerializeField]
    private int[] kpms = new int[10];


    public void setUserName(string msg)
    {
        userName = msg;
    }
    public void setStatusIndex(int index, int value)
    {
        status[index] = value;
    }
    public void setStatus(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setStatus msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            status[i] = int.Parse(intStrings[i]);
        }
    }
    public void setEquipment(string msg)       //htmlからデータロード時に真っ先に呼ばれる
    {
        Debug.Log("setEquipment msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            equipment[i] = int.Parse(intStrings[i]);
        }
    }
    public void setEquipmentIndex(int index, int value)       //htmlからデータロード時に真っ先に呼ばれる
    {
        equipment[index] = value;
    }
    public void setInventory(string msg)
    {
        Debug.Log("setInventory msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            inventory[i] = int.Parse(intStrings[i]);
        }
    }
    public void setInventoryIndex(int index, int value)
    {
        inventory[index] = value;
    }
    public void setMedals(string msg)
    {
        Debug.Log("setMedals msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            medals[i] = int.Parse(intStrings[i]);
        }
    }
    public void setKpm(string msg)
    {
        Debug.Log("setKpm msg: " + msg);
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            kpms[i] = int.Parse(intStrings[i]);
        }
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
    public int updateKpm(int newKpm)
    {
        // 要素1から9までを0から8に移動
        for (int i = 0; i < 9; i++)
        {
            kpms[i] = kpms[i + 1];
        }

        // 9番目の要素に新しい値を代入
        kpms[9] = newKpm;

        // 平均を計算
        double average = 0;
        for (int i = 0; i < kpms.Length; i++)
        {
            average += kpms[i];
        }
        average /= kpms.Length;
        return (int)Math.Round(average); // 四捨五入してintにキャスト
    }
}

