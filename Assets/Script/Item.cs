using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/item")]

public class Item : ScriptableObject
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private short itemPrice;

    [SerializeField]
    private Sprite itemImage;

    [SerializeField]
    private string itemMemo;

    public string MyItemName { get => itemName; }
    public short MyItemPrice { get => itemPrice; }
    public Sprite MyItemImage { get => itemImage; }
    public string MyItemMemo { get => itemMemo; }
}
