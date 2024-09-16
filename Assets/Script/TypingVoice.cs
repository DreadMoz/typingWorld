using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  // UIコンポーネントを扱うために必要

public class TypingVoice : MonoBehaviour
{
    public GameManager gm;
    public Image muteIcon; // インスペクターからアサイン
    public Sprite voiceSprite; // 音声ありの画像
    public Sprite muteSprite; // ミュートの画像
    public AudioSource nya;  // AudioSource コンポーネントへの参照
    public Slider slider;

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
            nya.mute = false;             // ミュート解除
            muteIcon.sprite = voiceSprite;      // 口アイコン設定
            slider.fillRect.GetComponent<Image>().color = new Color(0.502848f, 0.7884344f, 0.9433962f, 1);
        }
        else
        {
            nya.mute = true;             // ミュート
            muteIcon.sprite = muteSprite;       // マスクアイコン設定
            slider.fillRect.GetComponent<Image>().color = new Color(0.7075472f, 0.5416017f, 0.4438857f, 1);
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);    // EventSystemのフォーカスをクリア
    }
    public void updateVolume()      // ボリューム変更時
    {
        gm.savedata.Settings[se.Mute] = 0;      // ミュート解除
        gm.savedata.Settings[se.Volume] = (int)slider.value;    // スライダー値をセーブデータに代入
        nya.volume = slider.value * 0.01f;    // スライダー値をボリュームに
        nya.mute = false;             // ミュート解除
        dispMute();
    }
    public void initVolume()
    {
        slider.value = gm.savedata.Settings[se.Volume];
    }
}