using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "Database")]
public class Database : ScriptableObject
{
    [SerializeField]
    private List<Item> itemList = new List<Item>();

    [SerializeField]
    private List<string> serverList = new List<string>();

    [SerializeField]
    private List<string> medalList = new List<string>();


    public List<Item> GetItemList()
    {
        return itemList;
    }
    public List<string> GetServerList()
    {
        return serverList;
    }
    public List<string> GetMedalList()
    {
        return medalList;
    }
}
