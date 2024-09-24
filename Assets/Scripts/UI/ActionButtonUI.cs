using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;

    public void SetUp(BaseAction baseAction) 
    {
        text.SetText(baseAction.GetActionName());

        button.onClick.AddListener(() => 
        {
            UnitActionSystem.Instance.SelectedAction = baseAction;
        });
    }
}
