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
        string sliderMessage = "" + sliderUI.value;
        textSliderValue.text = sliderMessage;
    }
}
