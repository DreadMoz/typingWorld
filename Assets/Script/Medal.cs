using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/medal")]

public class Medal : ScriptableObject
{
    [SerializeField]
    private int medalRank;

    [SerializeField]
    private int medalClass;

    [SerializeField]
    private Sprite medalImage;

    [SerializeField]
    private string medalComment;

    public int MyItemName { get => medalRank; }
    public int MyItemPrice { get => medalClass; }
    public Sprite MyItemImage { get => medalImage; }
    public string MyItemComment { get => medalComment; }
}
