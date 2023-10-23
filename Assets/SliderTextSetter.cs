using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private Slider slider;
    private string originalText;

    private void Start()
    {
        originalText = text.text;
        slider = GetComponent<Slider>();
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = $"{originalText} ({slider.value})";
    }
}
