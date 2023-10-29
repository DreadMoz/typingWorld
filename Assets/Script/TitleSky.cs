using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class TitleSky : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 0.5f;
    private Material skyboxMaterial;

    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject player;        // プレイヤーオブジェクト
    [SerializeField]
    private Fade fade;                // フェード用オブジェクト
    [SerializeField]
    private ChibiCat cat;                // ねこオブジェクト

    [SerializeField]
    private GameObject startButton; // startボタン
    [SerializeField]
    private GameObject standupButton; // standupボタン
    [SerializeField]
    private GameObject nextButton; // nextボタン
    [SerializeField]
    private GameObject prevButton; // prevボタン
    [SerializeField]
    private GameObject confirmButton; // confirmボタン

    private Animator animator;
    private int necoNo = 1;
    private bool firstPush = false;      // スタートボタンが2回以上押されないようにするためのフラグ
    private bool goNextScene = false;    // ワールドシーンに遷移するためのフラグ

    // Start is called before the first frame update
    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_Rotation", 330f);
        animator = player.GetComponent<Animator>(); // Playerのアニメーターを取得

        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(skyboxMaterial.GetFloat("_Rotation") + rotateSpeed * Time.deltaTime, 360f));

        // Sキーが押されたらStartButtonメソッドを呼ぶ
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.StartButton();
        }

        // 画面遷移
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            GameManager.sceneNo = 1;              // ワールドシーンスタート
            SceneManager.LoadScene("WorldScene"); // ワールドシーンに遷移
            goNextScene = true;                   // 2回目以降の遷移を防ぐためのフラグを立てる
        }
    }

    public void StartButton()
    {
        string userName = gm.savedata.getUserName();
        if (userName == "")
        {
            selectNeco();
            return;
        }
        if (!firstPush)
        {
            fade.StartFadeOut();
            firstPush = true;
        }
    }
    private void selectNeco()
    {
        animator.SetBool("Standup", true);
        standupButton.SetActive(true);
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        confirmButton.SetActive(true);
        startButton.SetActive(false);
    }
    public void confirmNeco()
    {
        standupButton.SetActive(false);
        nextButton.SetActive(false);
        prevButton.SetActive(false);
        confirmButton.SetActive(false);
        startButton.SetActive(true);

    }
    public void updownNeco()
    {

        if (animator.GetBool("Standup"))
        {
            animator.SetBool("Standup", false);
        }
        else
        {
            animator.SetBool("Standup", true);
        }
    }
    public void nextNeco()
    {
        necoNo++;
        if (necoNo > 9)
        {
            necoNo = 0;
        }
        cat.setChara(necoNo);
    }
    public void prevNeco()
    {
        necoNo--;
        if (necoNo < 0)
        {
            necoNo = 9;
        }
        cat.setChara(necoNo);
    }
}
