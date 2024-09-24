using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public abstract class BaseAction : MonoBehaviour
{
    public static event Action<BaseAction> OnAnyActionStartEvent;
    public static event Action<BaseAction> OnAnyActionEndEvent;

    protected Action Callback;
    protected bool isActive;
    public Unit unit { get; private set; }

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action Callback);

    /// <summary>
    /// 得到可寻址网格
    /// </summary>
    /// <returns></returns>
    public abstract List<GridPosition> GetValidActionGridPositionList();

    /// <summary>
    /// 检查网格是否可寻址
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        => GetValidActionGridPositionList().Contains(gridPosition);

    /// <summary>
    /// 返回该动作需要的行动点
    /// </summary>
    /// <returns></returns>
    public virtual int GetActionPointsCost() 
    {
        return 1;
    }

    protected void OnAnyActionStart(BaseAction baseAction) 
    {
        OnAnyActionStartEvent?.Invoke(baseAction);
    }

    protected void OnAnyActionEnd(BaseAction baseAction) 
    {
        OnAnyActionEndEvent?.Invoke(baseAction);
    }

    public EnemyAIAction GetBestEnemyAIAction() 
    {
        List<EnemyAIAction> enemyAIActionList=new List<EnemyAIAction>();
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validGridPositionList) 
        {
            EnemyAIAction enemyAIAction=GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0) 
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => { return b.actionValue - a.actionValue; });
            return enemyAIActionList[0];
        }
        else 
            return null;
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
