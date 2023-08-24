using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static Equipment instance;

    EquipmentUI equipmentUI;

    [SerializeField]
    GameManager gm;

private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        equipmentUI = GetComponent<EquipmentUI>();
    }

    void Update()
    {

    }

}
