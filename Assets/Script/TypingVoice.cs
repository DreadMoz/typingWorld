using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  // UIコンポーネントを扱うために必要

public class TypingVoice : MonoBehaviour
{
    public GameManager gm;
    public Image muteIcon; // インスペクターからアサイン
    public Sprite voiceSprite; // 音声ありの画像
    public Sprite muteSprite; // ミュートの画像
    public Slider slider;
    public AudioSource typingAudio;  // AudioSource コンポーネントへの参照
    [SerializeField] private AudioClip nya;//AudioClip型の変数b1を宣言 使用するAudioClipをアタッチ必要
    [SerializeField] private AudioClip[] dia;//AudioClip型の変数b2を宣言 使用するAudioClipをアタッチ必要 
    [SerializeField] private AudioClip dice;//AudioClip型の変数b3を宣言 使用するAudioClipをアタッチ必要 
    [SerializeField] private AudioClip coin;//AudioClip型の変数b3を宣言 使用するAudioClipをアタッチ必要 
    [SerializeField] private AudioClip coin3;//AudioClip型の変数b3を宣言 使用するAudioClipをアタッチ必要 

    // Start is called before the first frame update

    void Start()
    {
        int mute = gm.savedata.Settings[se.Mute];
        initVolume();
        gm.savedata.Settings[se.Mute] = mute;
        dispMute();
    }

    public void ToggleMute()
    {
        // ミュート状態を切り替える
        gm.savedata.Settings[se.Mute] = 1 - gm.savedata.Settings[se.Mute];
        dispMute();
    }

    private void dispMute()
    {
        // アイコンの更新
        if (gm.savedata.Settings[se.Mute] == 0) // ミュートでなければ
        {
            typingAudio.mute = false;             // ミュート解除
            muteIcon.sprite = voiceSprite;      // 口アイコン設定
            slider.fillRect.GetComponent<Image>().color = new Color(0.502848f, 0.7884344f, 0.9433962f, 1);
        }
        else
        {
            typingAudio.mute = true;             // ミュート
            muteIcon.sprite = muteSprite;       // マスクアイコン設定
            slider.fillRect.GetComponent<Image>().color = new Color(0.7075472f, 0.5416017f, 0.4438857f, 1);
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);    // EventSystemのフォーカスをクリア
    }
    public void updateVolume()      // ボリューム変更時
    {
        gm.savedata.Settings[se.Mute] = 0;      // ミュート解除
        gm.savedata.Settings[se.Volume] = (int)slider.value;    // スライダー値をセーブデータに代入
        typingAudio.volume = slider.value * 0.01f;    // スライダー値をボリュームに
        typingAudio.mute = false;             // ミュート解除
        dispMute();
    }
    public void initVolume()
    {
        slider.value = gm.savedata.Settings[se.Volume];
    }

    public void sayNya()
    {
        typingAudio.PlayOneShot(nya);
    }
    public void sayDia(int no)
    {
        typingAudio.PlayOneShot(dia[no]);
    }
    public void sayDice()
    {
        typingAudio.PlayOneShot(dice);
    }
    public void sayCoin()
    {
        typingAudio.PlayOneShot(coin);
    }
    public void sayCoin3()
    {
        typingAudio.PlayOneShot(coin3);
    }
}