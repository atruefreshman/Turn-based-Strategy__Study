using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event Action OnStartMoveEvent;
    public event Action OnEndMoveEvent;

    private Vector3 targetPosition;
    private List<GridPosition> path;
    [SerializeField] private int maxMoveDiatance = 4;

    protected override void Awake()
    {
        base.Awake();

        targetPosition = transform.position;
        path = new List<GridPosition>();
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * 4 * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * 10f);
        }
        else if (path.Count > 0)
        {
            targetPosition = LevelGrid.Instance.GetCellCenter(path[0]);
            path.RemoveAt(0);   
        }
        else 
        {
            isActive = false;
            Callback();

            OnEndMoveEvent?.Invoke();
            OnAnyActionEnd(this);
        }
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override void TakeAction(GridPosition gridPosition, Action Callback)
    {
        path = PathFinding.Instance.GetPath(unit.gridPosition,gridPosition,out int pathLength);
        targetPosition = LevelGrid.Instance.GetCellCenter(path[0]);
        path.RemoveAt(0);

        this.Callback = Callback;
        isActive = true;

        OnAnyActionStart(this);
        OnStartMoveEvent?.Invoke();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionLis = new List<GridPosition>();
        GridPosition unitGridPosition = unit.gridPosition;

        for (int x = -maxMoveDiatance; x <= maxMoveDiatance; x++)
        {
            for (int z = -maxMoveDiatance; z <= maxMoveDiatance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) || LevelGrid.Instance.HasAnyUnitOn(testGridPosition) || testGridPosition==unitGridPosition)
                    continue;

                if (!PathFinding.Instance.IsWalkable(testGridPosition)||!PathFinding.Instance.HasPath(unit.gridPosition,testGridPosition,out int pathLength)|| pathLength>maxMoveDiatance*PathFinding.STRAIGHT_COST)
                    continue;

                validActionGridPositionLis.Add(testGridPosition);
            }
        }

        return validActionGridPositionLis;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition=unit.shootAction.GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = targetCountAtGridPosition*10 };
    }

}
