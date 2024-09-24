using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointText;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Unit unit;

    private void Start()
    {
        UpdateActionPointText();

        unit.OnActionPointChangedEvent += UpdateActionPointText;
        unit.healthSystem.OnHealthChangedEvent += UpdateHealthBar;
    }

    private void UpdateActionPointText() 
    {
        actionPointText.SetText(unit.actionPoints.ToString());
    }

    private void UpdateHealthBar(int curHealth,int maxHealth)
    {
        healthBarImage.fillAmount = (float)curHealth / maxHealth;
    }
}
