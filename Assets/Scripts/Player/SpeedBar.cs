using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxSpeedTime(float time)
    {
        slider.maxValue = time;
        slider.value = time;
    }
    
    public void SetSpeedTime(float time)
    {
        slider.value = time;
    }
}
