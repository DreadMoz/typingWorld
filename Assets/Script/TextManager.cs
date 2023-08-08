using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text textInstance;

    // js側から更新が合った時に呼び出される関数
    public void UpdateText(string newText)
    {
        // テキストコンポーネントの取得
        textInstance.text = newText;
    }


    void Start()
    {
        //js側の関数を呼び出してデータの監視開始
    }

    public void OnButtonX()
    {
        // テキストコンポーネントの取得
        textInstance.text = "button";
    }
}