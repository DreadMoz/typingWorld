using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetailMenu : MonoBehaviour
{
    private int id;
    private int level;

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

    // Start is called before the first frame update
    void Start()
    {
        resetStars();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setStars()
    {
        id = GameManager.GetTypingDataId();
        level = transform.GetSiblingIndex();    // GameObjectの兄弟の中でのインデックスを取得
        int medal = gm.savedata.getMedals()[id + level];

        switch (medal)
        {
            case 0:
                memo.SetActive(false);
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

    public void chooseTypingLevel()
    {
        resetStars();
        GameManager.SetTypingDataLevel(level);

        GameManager.SceneNo = (int)scene.Typing;
        SceneManager.LoadScene("typingStage"); // タイピングシーンに遷移

    }
}
