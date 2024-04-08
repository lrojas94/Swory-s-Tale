using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image healthBarForeground;
    [SerializeField]
    private GameObject canvas;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthBarForeground.fillAmount = currentHealth / maxHealth;
        if (currentHealth == 0 && canvas != null)
        {
            canvas.SetActive(false);
        }
    }
}
