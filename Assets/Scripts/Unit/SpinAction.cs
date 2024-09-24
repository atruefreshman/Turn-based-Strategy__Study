using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float rotateAmount;

    private void Update()
    {
        if (isActive) 
        {
            transform.Rotate(Vector3.up, 360 * Time.deltaTime);

            rotateAmount += 360 * Time.deltaTime;
            if (rotateAmount >= 360) 
            {
                isActive = false;
                Callback();

                OnAnyActionEnd(this);
            }
        }
    }

    public override string GetActionName()
        =>"Spin";

    public override void TakeAction(GridPosition gridPosition, Action Callback)
    {
        rotateAmount = 0;
        
        this.Callback = Callback;
        isActive = true;

        OnAnyActionStart(this);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
        => new List<GridPosition>() { unit.gridPosition };

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        => new EnemyAIAction { gridPosition = gridPosition, actionValue = 0 };
}
