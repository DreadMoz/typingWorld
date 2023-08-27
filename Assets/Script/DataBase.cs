using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Database : ScriptableObject
{
    [SerializeField]
    private List<Item> itemLists = new List<Item>();

    [SerializeField]
    private List<string> serverLists = new List<string>();

    [SerializeField]
    private List<string> medalLists = new List<string>();


    public List<Item> GetItemLists()
    {
        return itemLists;
    }
    public List<string> GetServerLists()
    {
        return serverLists;
    }
    public List<string> GetMedalLists()
    {
        return medalLists;
    }
    public Item GetItem(int no)
    {
        return itemLists[no];
    }
}
