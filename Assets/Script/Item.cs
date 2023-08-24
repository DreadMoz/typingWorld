using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/item")]

public class Item : ScriptableObject
{
    public enum itemtype
    {
        hand,
        head,
        face,
        glasses,
        nickname
    }

    [SerializeField]
    private itemtype itemType;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private short itemPrice;

    [SerializeField]
    private Sprite itemImage;

    [SerializeField]
    private string itemComment;

    public itemtype MyItemType { get => itemType; }
    public string MyItemName { get => itemName; }
    public short MyItemPrice { get => itemPrice; }
    public Sprite MyItemImage { get => itemImage; }
    public string MyItemComment { get => itemComment; }
}
