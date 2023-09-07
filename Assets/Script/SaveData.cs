using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private string userName;

    [SerializeField]
    private int[] status = new int[20];

    [SerializeField]
    private int[] inventory = new int[256];

    [SerializeField]
    private int[] medals = new int[100];

    public void setUserName(string msg)
    {
        userName = msg;
    }
    public void setStatus(string msg)
    {
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            status[i] = int.Parse(intStrings[i]);
        }
    }
    public void setInventory(string msg)
    {
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            inventory[i] = int.Parse(intStrings[i]);
        }
    }
    public void setMedals(string msg)
    {
        string[] intStrings = msg.Split(',');
        for (int i = 0; i < intStrings.Length; i++)
        {
            medals[i] = int.Parse(intStrings[i]);
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
    public int[] getInventory()
    {
        return inventory;
    }
    public int[] getMedals()
    {
        return medals;
    }
}

