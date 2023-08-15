using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Firebase();

    public Text textInstance;
    public RawImage imageInstance;

    // js������X�V�����������ɌĂяo�����֐�
    public void UpdateText(string newText)
    {
        Debug.Log("UpdateText called with newText: " + newText);
        // �e�L�X�g�R���|�[�l���g�̎擾
        textInstance.text = "zz";
    }


    void Start()
    {
        //js���̊֐����Ăяo���ăf�[�^�̊Ď��J�n
    }

    public void OnButtonX()
    {
        // �e�L�X�g�R���|�[�l���g�̎擾
        textInstance.text = "button";
    }
}