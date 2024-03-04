using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class SliderSetting : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dispText;

    [SerializeField]
    private Slider slider;

    void Start()
    {
        slider.value = float.Parse(dispText.text);
    }

    public void ChangeValue()
    {
        dispText.text = slider.value.ToString();
    }
}
