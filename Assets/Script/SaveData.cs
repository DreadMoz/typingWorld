using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private int[] status = new int[20];

    [SerializeField]
    private int[] inventry = new int[256];

    [SerializeField]
    private int[] medals = new int[100];

    public void setStatus(int[] msg)
    {
        status = msg;
    }

    public void setInventry(int[] msg)
    {
        inventry = msg;
    }

    public void setMedals(int[] msg)
    {
        medals = msg;
    }

    public int[] getInventry()
    {
        return inventry;
    }
}

