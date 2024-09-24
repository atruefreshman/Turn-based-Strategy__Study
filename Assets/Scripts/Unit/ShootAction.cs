using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State 
    {
        Aiming,
        Shooting,
        Cooloff
    }

    public event Action<Unit> OnShootEvent;
    public static event Action OnAnyShootEvent;

    [SerializeField] private int maxShootDistance = 4;
    public int MaxShootDistance => maxShootDistance;    
    private Unit targetUnit;

    private State state;
    private float stateTimer;

    private void Update()
    {
        if (!isActive)
            return;

        stateTimer-=Time.deltaTime;

        switch (state) 
        {
            case State.Aiming:
                transform.forward=Vector3.Lerp(transform.forward, (targetUnit.transform.position - transform.position), 10 * Time.deltaTime);
                break;
            case State.Shooting:
                
                break;
            case State.Cooloff:
                
                break;
        }

        if (stateTimer < 0) 
        {
            switch (state) 
            {
                case State.Aiming:
                        state = State.Shooting;
                        stateTimer = 0.6f;
                        OnAnyActionStart(this);
                    break;
                case State.Shooting:
                    OnShootEvent?.Invoke(targetUnit);
                    OnAnyShootEvent?.Invoke();
                    targetUnit.TakeDamage(40);

                    state = State.Cooloff;
                    stateTimer = 1f;
                    break;
                case State.Cooloff:
                    Callback();
                    isActive = false;
                    OnAnyActionEnd(this);
                    break;
            }
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList() 
    {
        GridPosition unitGridPosition = unit.gridPosition;
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public  List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validActionGridPositionLis = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) || !LevelGrid.Instance.HasAnyUnitOn(testGridPosition) || testGridPosition == unitGridPosition)
                    continue;

                if (Mathf.Abs(x) + Mathf.Abs(z) > maxShootDistance)
                    continue;

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy == unit.IsEnemy)
                    continue;

                if (Physics.Raycast(transform.position+Vector3.up*1.7f,LevelGrid.Instance.GetCellCenter(testGridPosition)-transform.position,maxShootDistance*LevelGrid.Instance.GetCellSize(),1<<LayerMask.NameToLayer("Obstacle")))
                    continue;

                validActionGridPositionLis.Add(testGridPosition);
            }
        }

        return validActionGridPositionLis;
    }

    public override void TakeAction(GridPosition gridPosition, Action Callback)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        stateTimer = 0.5f;

        this.Callback = Callback;
        isActive = true;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - unit.healthSystem.healthPercentage)) * 100
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition) 
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
