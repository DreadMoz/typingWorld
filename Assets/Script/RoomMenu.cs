using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // UI関連のクラスを使用するための名前空間
using TMPro;

public class RoomMenu : MonoBehaviour
{
    private TypingDetail typingDetail;
    private Button thisButton;
    private int id;

    [SerializeField]
    private GameObject memo;
    [SerializeField]
    private GameObject star0;
    [SerializeField]
    private GameObject star1;
    [SerializeField]
    private GameObject star2;
    [SerializeField]
    private GameObject star3;

    [SerializeField]
    private GameManager gm;

    private Practice practice;

    void Awake()
    {
        thisButton = this.GetComponent<Button>();
        resetStars();
        typingDetail = FindObjectOfType<TypingDetail>();
        practice = GetComponentInParent<Practice>();
        id = transform.GetSiblingIndex();    // GameObjectの兄弟の中でのインデックスを取得
    }

    private void Start()
    {
        int starNum = practice.getMedalTop(id);
        setStars(starNum);
    }
    void Update()
    {

    }

    public void setStars(int stars)
    {
        if (thisButton == null)
        {
            thisButton = GetComponent<Button>();
        }
        resetStars();

        switch (stars)
        {
            case 0:
                memo.SetActive(false);
                star0.SetActive(false);
                thisButton.interactable = false;
                break;
            case 1:
                break;
            case 2:
                star1.SetActive(true);
                break;
            case 3:
                star1.SetActive(true);
                star2.SetActive(true);
                break;
            default:
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
                break;
        }
    }

    public void resetStars()
    {
        memo.SetActive(true);
        star0.SetActive(true);
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        thisButton.interactable = true;
    }

    public void showDetail()
    {
        GameManager.SetTypingDataId(id);
        TextMeshProUGUI comment = this.GetComponentInChildren<TextMeshProUGUI>();
        typingDetail.setComment(comment.text);
        typingDetail.show();
    }
}
