using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    private GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用
    public Player p;
    public GameObject inventry;
    public GameObject ranking;

    Vector3 chaseOffset = new Vector3(0f, 11f, -14f);
    Quaternion chaseRotation = Quaternion.Euler(20f, 0f, 0f);
    Vector3 statusOffset = new Vector3(-1.3f, 1.3f, 4f);
    Quaternion statusRotation = Quaternion.Euler(5f, 180f, 0f);
    bool dir = true;
    float difx;
    float dify;
    float difz;
    float difr;
    float posx;
    float posy;
    float posz;
    int div = 10;
    int camTimer = 99;

    // Use this for initialization
    void Start ()
    {
        this.player = GameObject.Find("Player");
        offset = transform.position - player.transform.position;
        difx = (statusRotation.eulerAngles.x - chaseRotation.eulerAngles.x) / div;
        dify = (statusRotation.eulerAngles.y - chaseRotation.eulerAngles.y) / div;
        difz = (statusRotation.eulerAngles.z - chaseRotation.eulerAngles.z) / div;
        posx = (statusOffset.x - chaseOffset.x) / div;
        posy = (statusOffset.y - chaseOffset.y) / div;
        posz = (statusOffset.z - chaseOffset.z) / div;
        difr = Vector3.Distance(chaseOffset, statusOffset) / div;
    }

    // Update is called once per frame
    void Update ()
    {
        if (camTimer <= div)
        {
            smoothMove(camTimer);
            camTimer++;
        }
        else
        {
            if (!p.enableStatus)
            {
                transform.position = player.transform.position + chaseOffset;
            }
        }
        if ((Input.GetKeyDown(KeyCode.R)) && !inventry.activeSelf)
        {
            camTimer = 0;
            dir = p.enableStatus;
        }
        if ((Input.GetKeyDown(KeyCode.I)) && !ranking.activeSelf)
        {
            camTimer = 0;
            dir = p.enableStatus;
        }
    }
    void smoothMove(int i)
    {
        if (dir)
        {
            transform.rotation = Quaternion.Euler(statusRotation.eulerAngles.x - difx * (div - i), statusRotation.eulerAngles.y - dify * (div - i), statusRotation.eulerAngles.z - difz * (div - i));
            transform.position = player.transform.position + new Vector3(statusOffset.x - posx * (div - i), statusOffset.y - posy * (div - i), statusOffset.z - posz * (div - i));
        }
        else
        {
            transform.rotation = Quaternion.Euler(chaseRotation.eulerAngles.x + difx * (div - i), chaseRotation.eulerAngles.y + dify * (div - i), chaseRotation.eulerAngles.z + difz * (div - i));
            transform.position = player.transform.position + new Vector3(chaseOffset.x + posx * (div - i), chaseOffset.y + posy * (div - i), chaseOffset.z + posz * (div - i));
        }
    }
}
