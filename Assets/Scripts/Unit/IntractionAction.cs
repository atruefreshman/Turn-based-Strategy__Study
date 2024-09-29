using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntractionAction : BaseAction
{
    public int MaxInteractDistance { get; } =1;

    private void Update()
    {
        if (!isActive)
            return;

        Callback();
        isActive = false;
        OnAnyActionEnd(this);
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction() { actionValue=0, gridPosition=gridPosition };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionLis = new List<GridPosition>();

        for (int x = -MaxInteractDistance; x <= MaxInteractDistance; x++)
        {
            for (int z = -MaxInteractDistance; z <= MaxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unit.gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) || testGridPosition == unit.gridPosition)
                    continue;

                validActionGridPositionLis.Add(testGridPosition);
            }
        }

        return validActionGridPositionLis;
    }

    public override void TakeAction(GridPosition gridPosition, Action Callback)
    {

        OnAnyActionStart(this);

        this.Callback = Callback;
        isActive = true;
    }
}
