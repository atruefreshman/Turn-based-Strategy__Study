using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    public int MaxThrowDistance { get; } = 5;
    [SerializeField] private Transform grenadePrefab;

    private void Update()
    {
        if (!isActive)
            return;
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionLis = new List<GridPosition>();

        for (int x = -MaxThrowDistance; x <= MaxThrowDistance; x++)
        {
            for (int z = -MaxThrowDistance; z <= MaxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unit.gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) ||  testGridPosition == unit.gridPosition)
                    continue;

                if (Mathf.Abs(x) + Mathf.Abs(z) > MaxThrowDistance)
                    continue;

                validActionGridPositionLis.Add(testGridPosition);
            }
        }

        return validActionGridPositionLis;
    }

    public override void TakeAction(GridPosition gridPosition, Action Callback)
    {
        OnAnyActionStart(this);
        Instantiate(grenadePrefab, transform.position, transform.rotation).GetComponent<GrenadeProjectile>().Setup(gridPosition, () => 
        {
            Callback();
            isActive = false;
            OnAnyActionEnd(this);
        });

        this.Callback = Callback;
        isActive = true;
    }

}
