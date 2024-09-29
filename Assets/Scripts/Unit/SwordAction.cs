using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    private enum State 
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    public static event Action OnAnySwordHitEvent;
    public event Action OnSwordActionStarted;
    public event Action OnSwordActionFinished;

    private State state;
    private float stateTimer;

    public int MaxSwordDistance { get; } = 1;

    private Unit targetUnit;

    private void Update()
    {
        if (!isActive)
            return;

        stateTimer-=Time.deltaTime;
        switch (state) 
        {
            case State.SwingingSwordBeforeHit:
                transform.forward = Vector3.Lerp(transform.forward, (targetUnit.transform.position - transform.position), 10 * Time.deltaTime);

                if (stateTimer < 0) 
                {
                    state = State.SwingingSwordAfterHit;
                    stateTimer = 0.5f;
                    targetUnit.TakeDamage(100);
                    OnAnySwordHitEvent?.Invoke();
                }
                break;

            case State.SwingingSwordAfterHit:
                OnSwordActionStarted?.Invoke();

                if (stateTimer < 0) 
                {
                    OnSwordActionFinished?.Invoke();
                    Callback();
                    isActive = false;
                    OnAnyActionEnd(this);
                }
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction() { gridPosition=gridPosition,actionValue=200 };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionLis = new List<GridPosition>();

        for (int x = -MaxSwordDistance; x <= MaxSwordDistance; x++)
        {
            for (int z = -MaxSwordDistance; z <= MaxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unit.gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) || testGridPosition == unit.gridPosition ||!LevelGrid.Instance.HasAnyUnitOn(testGridPosition))
                    continue;

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy == unit.IsEnemy)
                    continue;

                validActionGridPositionLis.Add(testGridPosition);
            }
        }

        return validActionGridPositionLis;
    }

    public override void TakeAction(GridPosition gridPosition, Action Callback)
    {
        state = State.SwingingSwordBeforeHit;
        stateTimer = 0.7f;
        targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(gridPosition)[0];

        OnAnyActionStart(this);

        this.Callback = Callback;
        isActive = true;
    }
}
