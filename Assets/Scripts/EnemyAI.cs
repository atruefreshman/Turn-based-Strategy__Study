using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State 
    {
        Waiting,
        takingTurn,
        Busy
    }

    private State state;
    private float timer = 3f;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChangedEvent += Instance_OnTurnChangedEvent;
        state = State.Waiting;
    }

    private void Update()
    {
        if (TurnSystem.Instance.isPlayerTurn)
            return;

        switch (state) 
        {
            case State.Waiting:
                break;
            case State.takingTurn:
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    if (TryTakeEnemyAIAction(() => { timer = 0.5f; state = State.takingTurn; }))
                        state = State.Busy;
                    else 
                        TurnSystem.Instance.NextTurn();
                }
                break;
            case State.Busy:
                break;
        }

        
    }

    private void Instance_OnTurnChangedEvent()
    {
        if (!TurnSystem.Instance.isPlayerTurn) 
        {
            state= State.takingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeEnemyAIAction(Action Callback) 
    {
        foreach (Unit unit in UnitManager.Instance.enemyUnitList) 
        {
            if(TryTakeEnemyAIAction(unit, Callback))
                return true;    
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit unit,Action Callback)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestAction = null;
        foreach (BaseAction baseAction in unit.baseActionArray) 
        {
            if (unit.actionPoints < baseAction.GetActionPointsCost()) 
                continue;

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestAction = baseAction;
            }
            else 
            {
                EnemyAIAction testEnemyAIAction=baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue) 
                {
                    bestEnemyAIAction=testEnemyAIAction;
                    bestAction=baseAction;  
                }
            }
            baseAction.GetBestEnemyAIAction();
        }

        if (bestEnemyAIAction != null && unit.TryTakeAction(bestAction))
        {
            bestAction.TakeAction(bestEnemyAIAction.gridPosition, Callback);
            return true;    
        }
        else
            return false;
    }
}

public class EnemyAIAction 
{
    public GridPosition gridPosition;
    public int actionValue;
}
