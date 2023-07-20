using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiCat : MonoBehaviour
{
    private Material[] materials;

    public Material[] cats;
    public Material[] emos;


    // Start is called before the first frame update
    void Start()
    {
        materials = GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { setChara(1); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { setChara(2); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { setChara(3); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { setChara(4); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { setChara(5); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { setChara(6); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { setChara(7); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { setChara(8); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { setChara(9); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { setChara(0); }
//        if (Input.GetKeyDown(KeyCode.Q)) { setEmo(0); }
//        if (Input.GetKeyDown(KeyCode.W)) { setEmo(3); }
//        if (Input.GetKeyDown(KeyCode.E)) { setEmo(19); }
//        if (Input.GetKeyDown(KeyCode.R)) { setEmo(11); }
    }

    private void setChara(int no)
    {
        Material[] tmp = materials;
        tmp[0] = cats[no];
        GetComponent<Renderer>().materials = tmp;
    }
    private void setEmo(int no)
    {
        Material[] tmp = materials;
        tmp[1] = emos[no];
        GetComponent<Renderer>().materials = tmp;
    }
}
