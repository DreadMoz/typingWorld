using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    charid = 0,
    nickname = 1,
    serverid = 2,
    rank = 3,
    wpm = 4,
    seeker = 5,
    head = 6,
    right = 7,
    left = 8,
    glasses = 9,
    face = 10
}

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private string username;

    [SerializeField]
    private int[] status = new int[20];

    [SerializeField]
    private int[] inventry = new int[256];

    [SerializeField]
    private int[] medals = new int[100];

    public void setName(string msg)
    {
        username = msg;
    }
    public void setStatus(int[] msg)
    {
        status = msg;
    }

    public void setInventry(int[] msg)
    {
        inventry = msg;
    }

    public void setItem(int inventryNo, int itemId)
    {
        inventry[inventryNo] = itemId;
    }

    public void setMedals(int[] msg)
    {
        medals = msg;
    }


    public string getUsername()
    {
        return username;
    }

    public int getStatus(int no)
    {
        return status[no];
    }

    public int[] getInventry()
    {
        return inventry;
    }

    public int[] getMedals()
    {
        return medals;
    }
}