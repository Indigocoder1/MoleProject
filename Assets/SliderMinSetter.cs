using UnityEngine;
using UnityEngine.UI;

public class SliderMinSetter : MonoBehaviour
{
    [SerializeField] private Slider sliderToSet;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateMinValue()
    {
        sliderToSet.minValue = slider.value + 1;
    }
}
