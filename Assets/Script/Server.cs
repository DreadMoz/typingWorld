using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/server")]

public class Server : ScriptableObject
{
    [SerializeField]
    private int serverRank;

    [SerializeField]
    private int serverClass;

    [SerializeField]
    private string serverName;

    public int MyItemName { get => serverRank; }
    public int MyItemPrice { get => serverClass; }
    public string MyItemImage { get => serverName; }
}
