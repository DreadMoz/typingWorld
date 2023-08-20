using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Database : ScriptableObject
{
    [SerializeField]
    private List<Item> itemLists = new List<Item>();

    public List<Item> GetItemLists()
    {
        return itemLists;
    }
}
