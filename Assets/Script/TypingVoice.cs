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

    void Start()
    {
        dispMute();
    }

    public void ToggleMute()
    {
        // ミュート状態を切り替える
        gm.savedata.Settings[se.Mute] = 1 - gm.savedata.Settings[se.Mute];
        dispMute();
        updateVolume();
    }

    private void dispMute()
    {
        // アイコンの更新
        if (gm.savedata.Settings[se.Mute] == 0)
        {
            muteIcon.sprite = voiceSprite;
        }
        else
        {
            muteIcon.sprite = muteSprite;
        }
    }
    public void updateVolume()
    {
        if (gm.savedata.Settings[se.Mute] == 1)
        {
            nya.volume = 0;
        }
        else
        {
            nya.volume = gm.savedata.Settings[se.Volume];
        }
    }
}