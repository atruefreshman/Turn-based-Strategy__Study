using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButton;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private Transform busyUI;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private void Start()
    {
        CreateUnitActionsButtons();
    }

    private void OnEnable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChangedEvent += Instance_OnSelectedUnitChangedEvent;
        UnitActionSystem.Instance.OnUnitTakingActionEvent += Instance_OnUnitTakingActionEvent;
    }

    

    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChangedEvent -= Instance_OnSelectedUnitChangedEvent;
        UnitActionSystem.Instance.OnUnitTakingActionEvent -= Instance_OnUnitTakingActionEvent;
    }


    private void Instance_OnSelectedUnitChangedEvent()
    {
        ClearUnitActionsButtons();
        CreateUnitActionsButtons();

        UpdateActionText();
    }

    private void Instance_OnUnitTakingActionEvent(bool busy)
    {
        if (busy)
            busyUI.gameObject.SetActive(true);
        else
            busyUI.gameObject.SetActive(false);

        UpdateActionText();
    }

    private void CreateUnitActionsButtons() 
    {
        if (UnitActionSystem.Instance.selectedUnit == null)
            return;

        foreach (BaseAction baseAction in UnitActionSystem.Instance.selectedUnit.baseActionArray) 
        {
            Instantiate(actionButton, actionButtonContainer).GetComponent<ActionButtonUI>().SetUp(baseAction);
        }
    }

    private void ClearUnitActionsButtons() 
    {
        foreach(Transform button in actionButtonContainer)
            Destroy(button.gameObject);
    }

    private void UpdateActionText()
    {
        if (UnitActionSystem.Instance.selectedUnit != null)
            actionPointsText.SetText($"Action Points : {UnitActionSystem.Instance.selectedUnit.actionPoints}");
        else
            actionPointsText.SetText("");
    }
}
