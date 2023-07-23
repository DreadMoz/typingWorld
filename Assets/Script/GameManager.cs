using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;                           // プレイヤー
    public GameObject cam;                              // カメラ
    private Animator animator;                          // Playerアニメーション
    public Fade fade;                                   // 画面フェード処理
    public GameObject inventry;
    public GameObject equipment;
    public GameObject ranking;
    public GameObject status;

    public OpenButton inventryButton;                   // インベントリウィンドウボタン
    public OpenButton rankingButton;                    // ランキングウィンドウボタン

    private bool firstPush = false;                     // スタートボタン2度押し防止フラグ
    private bool goNextScene = false;                   // ワールドシーン2度押し防止フラグ
    private int windowOpenCount = 10;                   // ウィンドウ開閉フレームカウント
    private int count = 0;                              // フレームカウント
    private int inventryOpen = 0;
    private int rankingOpen = 0;
    private int cameraMove = 0;                         // 0:追尾 1:移動なし 2:追尾位置 3:ステータス

    Vector3 chaseOffset = new Vector3(0f, 8f, -14f);
    Quaternion chaseRotation = Quaternion.Euler(25f, 0f, 0f);
    Vector3 statusOffset = new Vector3(-1.3f, 1.3f, 4f);
    Quaternion statusRotation = Quaternion.Euler(5f, 180f, 0f);
    float difx;
    float dify;
    float difz;
    float difr;
    float posx;
    float posy;
    float posz;

    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponent<Animator>();     // Playerアニメーション
        animator.SetInteger("anim", 0);                 // オープニングシーン 0

        difx = (statusRotation.eulerAngles.x - chaseRotation.eulerAngles.x) / windowOpenCount;
        dify = (statusRotation.eulerAngles.y - chaseRotation.eulerAngles.y) / windowOpenCount;
        difz = (statusRotation.eulerAngles.z - chaseRotation.eulerAngles.z) / windowOpenCount;
        posx = (statusOffset.x - chaseOffset.x) / windowOpenCount;
        posy = (statusOffset.y - chaseOffset.y) / windowOpenCount;
        posz = (statusOffset.z - chaseOffset.z) / windowOpenCount;
        difr = Vector3.Distance(chaseOffset, statusOffset) / windowOpenCount;
    }

    public void StartButton()
    {
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // ワールドシーン 1
        if (animator.GetInteger("anim") == 1)
        {
            // インベントリウィンドウオープン操作
            if ((Input.GetKeyDown(KeyCode.I) || inventryButton.isOpen()) && (count == 0))
            {
                count = windowOpenCount;

                if (inventry.activeSelf)
                {
                    inventryOpen = -1;
                    rankingOpen = 0;
                    cameraMove = 2;
                }
                else if(ranking.activeSelf)
                {
                    rankingOpen = -1;
                    inventryOpen = 1;
                    cameraMove = 1;
                }
                else
                {
                    inventryOpen = 1;
                    rankingOpen = 0;
                    cameraMove = 3;
                }
            }
            // ランキングウィンドウオープン操作
            else if ((Input.GetKeyDown(KeyCode.R) || rankingButton.isOpen()) && (count == 0))
            {
                count = windowOpenCount;

                if (ranking.activeSelf)
                {
                    inventryOpen = 0;
                    rankingOpen = -1;
                    cameraMove = 2;
                }
                else if (inventry.activeSelf)
                {
                    inventryOpen = -1;
                    rankingOpen = 1;
                    cameraMove = 1;
                }
                else
                {
                    inventryOpen = 0;
                    rankingOpen = 1;
                    cameraMove = 3;
                }
            }

            if (count > 0)
            {
                // ウィンドウ閉じる処理
                if (count > windowOpenCount / 2)
                {
                    if (inventryOpen == -1)
                    {
                        status.transform.position += new Vector3(0, 30, 0);
                        inventry.transform.position += new Vector3(110, 0, 0);
                        equipment.transform.position += new Vector3(0, -35, 0);
                    }
                    if (rankingOpen == -1)
                    {
                        status.transform.position += new Vector3(0, 30, 0);
                        ranking.transform.position += new Vector3(110, 0, 0);
                    }
                    count--;
                    if (count == windowOpenCount / 2)
                    {
                        status.SetActive(false);
                        inventry.SetActive(false);
                        equipment.SetActive(false);
                        ranking.SetActive(false);
                    }
                }
                // ウィンドウ開く処理
                else
                {
                    if (inventryOpen == 1)
                    {
                        status.SetActive(true);
                        inventry.SetActive(true);
                        equipment.SetActive(true);
                        status.transform.position += new Vector3(0, -30, 0);
                        inventry.transform.position += new Vector3(-110, 0, 0);
                        equipment.transform.position += new Vector3(0, 35, 0);
                    }
                    if (rankingOpen == 1)
                    {
                        status.SetActive(true);
                        ranking.SetActive(true);
                        status.transform.position += new Vector3(0, -30, 0);
                        ranking.transform.position += new Vector3(-110, 0, 0);
                    }
                    count--;
                }
                if (cameraMove == 3)
                {
                    cam.transform.rotation = Quaternion.Euler(statusRotation.eulerAngles.x - difx * count, statusRotation.eulerAngles.y - dify * count, statusRotation.eulerAngles.z - difz * count);
                    cam.transform.position = player.transform.position + new Vector3(statusOffset.x - posx * count, statusOffset.y - posy * count, statusOffset.z - posz * count);
                }
                else if (cameraMove == 2)
                {
                    cam.transform.rotation = Quaternion.Euler(chaseRotation.eulerAngles.x + difx * count, chaseRotation.eulerAngles.y + dify * count, chaseRotation.eulerAngles.z + difz * count);
                    cam.transform.position = player.transform.position + new Vector3(chaseOffset.x + posx * count, chaseOffset.y + posy * count, chaseOffset.z + posz * count);
                }
                if ((count == 0) && (cameraMove == 2))
                {
                    cameraMove = 0;
                }
            }
        }
        // オープニングシーン 0
        else if (animator.GetInteger("anim") == 0)
        {
            // 1分に10秒ばたつかせる
            if (Time.time % 60 > 50)
            {
                animator.SetBool("Swim", true);
            }
            else
            {
                animator.SetBool("Swim", false);
            }

            // Sキーでスタート
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.StartButton();
            }

            // フェードアウトが完了したらワールドシーンへ移行
            if (!goNextScene && fade.IsFadeOutComplete())
            {
                SceneManager.LoadScene("WorldScene");       // シーン移行
                animator.SetInteger("anim", 1);             // ワールドシーン 1
                goNextScene = true;                         // 2回目実施防止
            }
        }
    }
    public int getCameraMove()
    {
        return cameraMove;
    }
}
