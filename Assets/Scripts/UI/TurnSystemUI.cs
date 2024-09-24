using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private Transform enemyTurnUI;

    private void Start()
    {
        nextTurnButton.onClick.AddListener(() => 
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChangedEvent += Instance_OnTurnChangedEvent;


        turnText.SetText($"Turn : {TurnSystem.Instance.turn}");
    }

    private void Instance_OnTurnChangedEvent()
    {
        turnText.SetText($"Turn : {TurnSystem.Instance.turn}");

        if (TurnSystem.Instance.isPlayerTurn)
        {
            enemyTurnUI.gameObject.SetActive(false);
            nextTurnButton.gameObject.SetActive(true);
        }
        else 
        {
            enemyTurnUI.gameObject.SetActive(true);
            nextTurnButton.gameObject.SetActive(false);
        }
    }

}
