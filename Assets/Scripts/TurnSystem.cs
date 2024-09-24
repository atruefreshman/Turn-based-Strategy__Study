using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event Action OnTurnChangedEvent;

    public int turn { get; private set; } = 1;
    public bool isPlayerTurn { get; private set; } = true;  

    private void Awake()
    {
        Instance = this;
    }

    public void NextTurn() 
    {
        turn++;
        isPlayerTurn=!isPlayerTurn;
        OnTurnChangedEvent?.Invoke();
    }

}
