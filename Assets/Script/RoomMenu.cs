using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomMenu : MonoBehaviour
{
    private TypingDetail typingDetail;

    private int id;

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
    [SerializeField]
    private GameObject MenuParent;
    private Practice practice;

    // Start is called before the first frame update
    void Start()
    {
        resetStars();
        typingDetail = FindObjectOfType<TypingDetail>();
        id = transform.GetSiblingIndex();    // GameObjectの兄弟の中でのインデックスを取得
        practice = MenuParent.GetComponent<Practice>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setStars()
    {
        
        switch (practice.medalTop[id])
        {
            case 0:
                star0.SetActive(false);
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
        star0.SetActive(true);
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
    }

    public void showDetail()
    {
        GameManager.SetTypingDataId(id);
        typingDetail.show();
        typingDetail.transform.position = new Vector3(700, 350, transform.position.z);
    }
}
