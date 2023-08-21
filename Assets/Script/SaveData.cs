using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private int charid;

    [SerializeField]
    private int nickname;

    [SerializeField]
    private int serverid;

    [SerializeField]
    private int wpm;

    [SerializeField]
    private int seeker;


    [SerializeField]
    private bool[] inventry = new bool[256];

    [SerializeField]
    private int[] equipments = new int[5];

    [SerializeField]
    private int[] medals = new int[100];

    public void setStatus(int[] msg)
    {
        charid = msg[0];
        nickname = msg[1];
        serverid = msg[2];
        wpm = msg[3];
        seeker = msg[4];
    }

    public void setInventry(bool[] msg)
    {
        inventry = msg;
    }

    public void setEquipments(int[] msg)
    {
        equipments = msg;
    }

    public void setMedals(int[] msg)
    {
        medals = msg;
    }
}

