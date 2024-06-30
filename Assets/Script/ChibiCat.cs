using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ChibiCat : MonoBehaviour
{
    private Material[] materials;

    public Material[] cats;
    public Material[] emos;

    [SerializeField] private GameObject spadB;
    [SerializeField] private GameObject panB;
    [SerializeField] private GameObject driedFishB;
    [SerializeField] private GameObject meatB;

    [SerializeField] private GameObject hikingHat;
    [SerializeField] private GameObject hikingHats;
    [SerializeField] private GameObject cowboyHat;
    [SerializeField] private GameObject magicianHat;
    [SerializeField] private GameObject pajamaHat;
    [SerializeField] private GameObject showerHat;
    [SerializeField] private GameObject vikingHelm;
    [SerializeField] private GameObject mexicoHat;
    [SerializeField] private GameObject cakeS;

    [SerializeField] private GameObject grassARed;
    [SerializeField] private GameObject grassABlue;
    [SerializeField] private GameObject grassABlack;

    [SerializeField] private GameObject spadR;
    [SerializeField] private GameObject driedFishR;
    [SerializeField] private GameObject meatR;
    [SerializeField] private GameObject battonWoodR;
    [SerializeField] private GameObject whirligigR;
    [SerializeField] private GameObject panR;
    [SerializeField] private GameObject donutChocoR;
    [SerializeField] private GameObject donutStrawberryR;
    [SerializeField] private GameObject joroR;
    [SerializeField] private GameObject pencilR;
    [SerializeField] private GameObject eraserR;

    [SerializeField] private GameObject spadL;
    [SerializeField] private GameObject driedFishL;
    [SerializeField] private GameObject meatL;
    [SerializeField] private GameObject battonWoodL;
    [SerializeField] private GameObject whirligigL;
    [SerializeField] private GameObject panL;
    [SerializeField] private GameObject donutChocoL;
    [SerializeField] private GameObject donutStrawberryL;
    [SerializeField] private GameObject joroL;
    [SerializeField] private GameObject pencilL;
    [SerializeField] private GameObject eraserL;

    // Start is called before the first frame update
    void Awake()
    {
        materials = GetComponent<Renderer>().materials;
        releaseAllEquip(0xFF);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1)) { setChara(1); }
        // if (Input.GetKeyDown(KeyCode.Alpha2)) { setChara(2); }
        // if (Input.GetKeyDown(KeyCode.Alpha3)) { setChara(3); }
        // if (Input.GetKeyDown(KeyCode.Alpha4)) { setChara(4); }
        // if (Input.GetKeyDown(KeyCode.Alpha5)) { setChara(5); }
        // if (Input.GetKeyDown(KeyCode.Alpha6)) { setChara(6); }
        // if (Input.GetKeyDown(KeyCode.Alpha7)) { setChara(7); }
        // if (Input.GetKeyDown(KeyCode.Alpha8)) { setChara(8); }
        // if (Input.GetKeyDown(KeyCode.Alpha9)) { setChara(9); }
        // if (Input.GetKeyDown(KeyCode.Alpha0)) { setChara(0); }
//        if (Input.GetKeyDown(KeyCode.Q)) { setEmo(0); }
//        if (Input.GetKeyDown(KeyCode.W)) { setEmo(3); }
//        if (Input.GetKeyDown(KeyCode.E)) { setEmo(19); }
//        if (Input.GetKeyDown(KeyCode.R)) { setEmo(11); }
    }

    public void setName(string name)
    {
        var textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = name; // 名前をテキストにセット
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on " + gameObject.name);
        }
    }

    public void setChara(int no)
    {
        if (no == 0)
        {
            no = 1;
        }
        else
        {
            no -= 200;
        }
        Material[] tmp = materials;
        if (tmp != null)
        {
            tmp[0] = cats[no];
            GetComponent<Renderer>().materials = tmp;
        }
    }
    public void setEmo(int no)
    {
        Material[] tmp = materials;
        tmp[1] = emos[no];
        GetComponent<Renderer>().materials = tmp;
    }
    public void changeEquipHands(int itemIdRight, int itemIdLeft, int bagItem)
    {
        releaseHands(bagItem);
        switch (itemIdRight)     // 右手
        {
            case 1:
                spadR.SetActive(true);
                spadB.SetActive(false);
                break;
            case 2:
                driedFishR.SetActive(true);
                driedFishB.SetActive(false);
                break;
            case 3:
                meatR.SetActive(true);
                meatB.SetActive(false);
                break;
            case 4:
                battonWoodR.SetActive(true);
                break;
            case 5:
                whirligigR.SetActive(true);
                break;
            case 6:
                panR.SetActive(true);
                panB.SetActive(false);
                break;
            case 7:
                donutChocoR.SetActive(true);
                break;
            case 8:
                donutStrawberryR.SetActive(true);
                break;
            case 9:
                joroR.SetActive(true);
                break;
            case 10:
                pencilR.SetActive(true);
                break;
            case 11:
                eraserR.SetActive(true);
                break;
        }
        switch (itemIdLeft)     // 左手
        {
            case 1:
                spadL.SetActive(true);
                spadB.SetActive(false);
                break;
            case 2:
                driedFishL.SetActive(true);
                driedFishB.SetActive(false);
                break;
            case 3:
                meatL.SetActive(true);
                meatB.SetActive(false);
                break;
            case 4:
                battonWoodL.SetActive(true);
                break;
            case 5:
                whirligigL.SetActive(true);
                break;
            case 6:
                panL.SetActive(true);
                panB.SetActive(false);
                break;
            case 7:
                donutChocoL.SetActive(true);
                break;
            case 8:
                donutStrawberryL.SetActive(true);
                break;
            case 9:
                joroL.SetActive(true);
                break;
            case 10:
                pencilL.SetActive(true);
                break;
            case 11:
                eraserL.SetActive(true);
                break;
        }
    }

    public void changeEquipGlasses(int itemIdGrass)
    {
        releaseGlasses();
        switch (itemIdGrass)
        {
            case 121:
                grassARed.SetActive(true);
                break;
            case 122:
                grassABlue.SetActive(true);
                break;
            case 123:
                grassABlack.SetActive(true);
                break;
        }
    }
    public void changeEquipHead(int itemIdHead)
    {
        releaseHead();
        switch (itemIdHead)
        {
            case 151:
                hikingHat.SetActive(true);
                break;
            case 152:
                hikingHats.SetActive(true);
                break;
            case 153:
                cowboyHat.SetActive(true);
                break;
            case 154:
                magicianHat.SetActive(true);
                break;
            case 155:
                pajamaHat.SetActive(true);
                break;
            case 156:
                showerHat.SetActive(true);
                break;
            case 157:
                vikingHelm.SetActive(true);
                break;
            case 158:
                mexicoHat.SetActive(true);
                break;
            case 159:
                cakeS.SetActive(true);
                break;
        }
    }

    public void releaseAllEquip(int bagItem = 0)
    {
        releaseHands(bagItem);
        releaseHead();
        releaseGlasses();
    }

    private void releaseHands(int bagItem)
    {
        panB.SetActive((bagItem & 0x01) == 0x01);          // インベントリにあればかばんに付ける
        spadB.SetActive((bagItem & 0x02) == 0x02);
        driedFishB.SetActive((bagItem & 0x04) == 0x04);
        meatB.SetActive((bagItem & 0x08) == 0x08);

        battonWoodR.SetActive(false);   // 右手解除
        spadR.SetActive(false);
        whirligigR.SetActive(false);
        panR.SetActive(false);
        driedFishR.SetActive(false);
        meatR.SetActive(false);
        donutChocoR.SetActive(false);
        donutStrawberryR.SetActive(false);
        joroR.SetActive(false);
        pencilR.SetActive(false);
        eraserR.SetActive(false);

        battonWoodL.SetActive(false);   // 左手解除
        spadL.SetActive(false);
        whirligigL.SetActive(false);
        panL.SetActive(false);
        driedFishL.SetActive(false);
        meatL.SetActive(false);
        donutChocoL.SetActive(false);
        donutStrawberryL.SetActive(false);
        joroL.SetActive(false);
        pencilL.SetActive(false);
        eraserL.SetActive(false);
    }

    private void releaseGlasses()
    {
        grassARed.SetActive(false);
        grassABlue.SetActive(false);
        grassABlack.SetActive(false);
    }

    private void releaseHead()
    {
        hikingHat.SetActive(false);
        hikingHats.SetActive(false);
        cowboyHat.SetActive(false);
        magicianHat.SetActive(false);
        pajamaHat.SetActive(false);
        showerHat.SetActive(false);
        vikingHelm.SetActive(false);
        mexicoHat.SetActive(false);
        cakeS.SetActive(false);
    }
}
