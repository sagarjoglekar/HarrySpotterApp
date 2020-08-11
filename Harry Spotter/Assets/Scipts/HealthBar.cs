using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] public Gradient _gradient;
    [SerializeField] private Image _healthFill;

    public void SetMaxHealth(int Maxhealth)
    {
        _slider.maxValue = Maxhealth;
        _slider.value = Maxhealth;

        _healthFill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth (int health)
    {
        _slider.value = health;
        _healthFill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
