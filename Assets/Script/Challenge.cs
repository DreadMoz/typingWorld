using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
    }

    // タイピング終了後の詳細画面表示
    public void startOpenDetail()
    {
        int id = GameManager.TypingDataId;
        float answerRate = GameManager.AnswerRate;
        
        if ( id >= 0)
        {
            int roomId = id / 3;
            Transform childTransform = gameObject.transform.GetChild(roomId);
            RoomMenu roommenu = childTransform.GetComponent<RoomMenu>();
            roommenu.showDetail();
        }
    }
}
