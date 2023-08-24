using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public SaveData savedata;
    public Database db;

    public GameObject player;                           // �v���C���[
    public GameObject cam;                              // �J����
    private Animator animator;                          // Player�A�j���[�V����
    public Fade fade;                                   // ��ʃt�F�[�h����
    public GameObject inventry;
    public GameObject equipment;
    public GameObject ranking;
    public GameObject status;
    public setStatus stat;
    public TextMeshProUGUI headName;

    public OpenButton inventryButton;                   // �C���x���g���E�B���h�E�{�^��
    public OpenButton rankingButton;                    // �����L���O�E�B���h�E�{�^��

    private bool firstPush = false;                     // �X�^�[�g�{�^��2�x�����h�~�t���O
    private bool goNextScene = false;                   // ���[���h�V�[��2�x�����h�~�t���O
    private int windowOpenCount = 2;                   // �E�B���h�E�J�t���[���J�E���g
    private int count = 0;                              // �t���[���J�E���g
    private int inventryOpen = 0;
    private int rankingOpen = 0;
    private int cameraMove = 0;                         // 0:�ǔ� 1:�ړ��Ȃ� 2:�ǔ��ʒu 3:�X�e�[�^�X

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

    Canvas canvasName;

    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponent<Animator>();     // Player�A�j���[�V����
        animator.SetInteger("anim", 0);                 // �I�[�v�j���O�V�[�� 0

        difx = (statusRotation.eulerAngles.x - chaseRotation.eulerAngles.x) / windowOpenCount;
        dify = (statusRotation.eulerAngles.y - chaseRotation.eulerAngles.y) / windowOpenCount;
        difz = (statusRotation.eulerAngles.z - chaseRotation.eulerAngles.z) / windowOpenCount;
        posx = (statusOffset.x - chaseOffset.x) / windowOpenCount;
        posy = (statusOffset.y - chaseOffset.y) / windowOpenCount;
        posz = (statusOffset.z - chaseOffset.z) / windowOpenCount;
        difr = Vector3.Distance(chaseOffset, statusOffset) / windowOpenCount;

        canvasName = player.GetComponentInChildren<ChibiCat>().GetComponentInChildren<Canvas>();
        string name = savedata.getUsername() + db.GetItemById(savedata.getStatus((int)Status.nickname)).MyItemName;
        headName.SetText(name);
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
        // ���[���h�V�[�� 1
        if (animator.GetInteger("anim") == 1)
        {
            // �C���x���g���E�B���h�E�I�[�v������
            if ((Input.GetKeyDown(KeyCode.I) || inventryButton.isOpen()) && (count == 0))
            {
                stat.setStatusData();
                count = windowOpenCount;
                inventryButton.resetOpen();

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
            // �����L���O�E�B���h�E�I�[�v������
            else if ((Input.GetKeyDown(KeyCode.R) || rankingButton.isOpen()) && (count == 0))
            {
                stat.setStatusData();
                count = windowOpenCount;
                rankingButton.resetOpen();

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
                // �E�B���h�E���鏈��
                if (count > windowOpenCount / 2)
                {
                    if (inventryOpen == -1)
                    {
                        status.transform.position += new Vector3(0, 300/windowOpenCount, 0);
                        inventry.transform.position += new Vector3(1100/windowOpenCount, 0, 0);
                        equipment.transform.position += new Vector3(0, -350/windowOpenCount, 0);
                    }
                    if (rankingOpen == -1)
                    {
                        status.transform.position += new Vector3(0, 300/windowOpenCount, 0);
                        ranking.transform.position += new Vector3(1100/windowOpenCount, 0, 0);
                    }
                    count--;
                    if (count == windowOpenCount / 2)
                    {
                        status.SetActive(false);
                        inventry.SetActive(false);
                        equipment.SetActive(false);
                        ranking.SetActive(false);

                        canvasName.gameObject.SetActive(true);
                    }
                }
                // �E�B���h�E�J������
                else
                {
                    if (inventryOpen == 1)
                    {
                        status.SetActive(true);
                        inventry.SetActive(true);
                        equipment.SetActive(true);
                        canvasName.gameObject.SetActive(false);

                        status.transform.position += new Vector3(0, -300/windowOpenCount, 0);
                        inventry.transform.position += new Vector3(-1100/windowOpenCount, 0, 0);
                        equipment.transform.position += new Vector3(0, 350/windowOpenCount, 0);
                    }
                    if (rankingOpen == 1)
                    {
                        status.SetActive(true);
                        ranking.SetActive(true);
                        canvasName.gameObject.SetActive(false);

                        status.transform.position += new Vector3(0, -300/windowOpenCount, 0);
                        ranking.transform.position += new Vector3(-1100/windowOpenCount, 0, 0);
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
        // �I�[�v�j���O�V�[�� 0
        else if (animator.GetInteger("anim") == 0)
        {
            // 1����10�b�΂�������
            if (Time.time % 60 > 50)
            {
                animator.SetBool("Swim", true);
            }
            else
            {
                animator.SetBool("Swim", false);
            }

            // S�L�[�ŃX�^�[�g
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.StartButton();
            }

            // �t�F�[�h�A�E�g�����������烏�[���h�V�[���ֈڍs
            if (!goNextScene && fade.IsFadeOutComplete())
            {
                SceneManager.LoadScene("WorldScene");       // �V�[���ڍs
                animator.SetInteger("anim", 1);             // ���[���h�V�[�� 1
                goNextScene = true;                         // 2��ڎ��{�h�~
            }
        }
    }

    public void setName(string msg)
    {
        savedata.setName(msg);
    }

    public void setStatus(int[] msg)
    {
        savedata.setStatus(msg);
    }

    public void setInventry(int[] msg)
    {
        savedata.setInventry(msg);
    }

    public void setMedals(int[] msg)
    {
        savedata.setMedals(msg);
    }


    public int getCameraMove()
    {
        return cameraMove;
    }
}
