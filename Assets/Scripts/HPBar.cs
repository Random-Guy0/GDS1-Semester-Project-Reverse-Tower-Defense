using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image HP;

    public void SetHP(int currentHP)
    {
        slider.value = currentHP;

        HP.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHP(int maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = maxHP;

        HP.color = gradient.Evaluate(slider.value);
    }
}
