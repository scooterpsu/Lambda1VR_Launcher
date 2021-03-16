using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class valuetext : MonoBehaviour
{
    public Slider sliderUI;
    public Text textSliderValue;

    void Start()
    {
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        sliderUI.value = Mathf.Round(sliderUI.value * 100f) / 100f;
        string sliderMessage = "" + sliderUI.value;
        textSliderValue.text = sliderMessage;
    }
}
