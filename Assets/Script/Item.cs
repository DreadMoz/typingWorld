using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/item")]

public class Item : ScriptableObject
{
    [SerializeField]
    private byte itemId;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private short itemPrice;

    [SerializeField]
    private Sprite itemImage;

    public byte MyItemId { get => itemId; }
    public string MyItemName { get => itemName; }
    public short MyItemPrice { get => itemPrice; }
    public Sprite MyItemImage { get => itemImage;}
}
