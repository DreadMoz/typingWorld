using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private Animator animator;
    public Fade fade;

    private bool firstPush = false;
    private bool goNextScene = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = player.GetComponent<Animator>();
        animator.SetInteger("anim", 0);
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
        if (Time.time % 60 > 50)
        {
            animator.SetBool("Swim", true);
        }
        else
        {
            animator.SetBool("Swim", false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            this.StartButton();
        }

        if (!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("WorldScene");
            animator.SetInteger("anim", 1);
            goNextScene = true;
        }
    }

}
