using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    // public Gradient gradient;
    public Image fill;

    // for health bar
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetCurrentHealth(int health, int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void SetHealth(int health) 
    {
        slider.value = health;
    }

    // for shield bar
    public void SetMaxShield(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetCurrentShield(float health, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void SetShield(float health) 
    {
        slider.value = health;
    }
}
