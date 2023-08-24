using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Database : ScriptableObject
{
    [SerializeField]
    private List<Item> itemLists = new List<Item>();

    [SerializeField]
    private List<string> medalLists = new List<string>();

    [SerializeField]
    private List<string> serverLists = new List<string>();

    public List<Item> GetItemLists()
    {
        return itemLists;
    }
    public List<string> GetMedalLists()
    {
        return medalLists;
    }
    public List<string> GetServerLists()
    {
        return serverLists;
    }


    public Item GetItemById(int itemId)
    {
        if (itemId >= 0 && itemId < itemLists.Count)
        {
            return itemLists[itemId];
        }
        else
        {
            return null;
        }
    }

    public string GetMedalNameById(int medalId)
    {
        if (medalId >= 0 && medalId < medalLists.Count)
        {
            return medalLists[medalId];
        }
        else
        {
            return "Medal Not Found";
        }
    }

    public string GetServerNameById(int serverId)
    {
        if (serverId >= 0 && serverId < serverLists.Count)
        {
            return serverLists[serverId];
        }
        else
        {
            return "Server Not Found";
        }
    }
}