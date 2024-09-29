using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(UnitAnimator))]
//[RequireComponent(typeof(MoveAction))]
//[RequireComponent(typeof(SpinAction))]
//[RequireComponent(typeof(ShootAction))]
//[RequireComponent(typeof(SwordAction))]
//[RequireComponent(typeof(GrenadeAction))]
//[RequireComponent(typeof(HealthSystem))]
//[RequireComponent(typeof(UnitRagdollSpawner))]
public class Unit : MonoBehaviour
{
    public event Action OnActionPointChangedEvent;
    public static event Action<Unit> OnAnyUnitSpawondEvent;
    public static event Action<Unit> OnAnyUnitDeadEvent;

    private const int MAX_ACTION_POINTS=3;

    [SerializeField] private bool isEnemy;
    public bool IsEnemy => isEnemy;

    public BaseAction[] baseActionArray { get; private set; }
    public MoveAction moveAction { get; private set; }
    public SpinAction spinAction { get; private set; }
    public ShootAction shootAction { get; private set; }
    
    public GridPosition gridPosition { get; private set; }
    public HealthSystem healthSystem { get; private set; }
    public int actionPoints { get; private set; } = MAX_ACTION_POINTS;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDeadEvent += HealthSystem_OnDeadEvent;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        OnAnyUnitSpawondEvent?.Invoke(this);
    }

    private void OnEnable()
    {
        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;
    }

    private void Update()
    {
        GridPosition  newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (gridPosition != newGridPosition) 
        {
            GridPosition oldGridPosition = gridPosition;
            oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, gridPosition);
        }
    }

    private void OnDisable()
    {
        TurnSystem.Instance.OnTurnChangedEvent -= TurnSystem_OnTurnChangedEvent;
    }

    private void TurnSystem_OnTurnChangedEvent()
    {
        if(TurnSystem.Instance.isPlayerTurn&&!isEnemy||!TurnSystem.Instance.isPlayerTurn&&isEnemy)
            actionPoints = MAX_ACTION_POINTS;
    }

    private void HealthSystem_OnDeadEvent()
    {
        LevelGrid.Instance.GetGridObject(gridPosition).RemoveUnitList(this);

        OnAnyUnitDeadEvent?.Invoke(this);

        Destroy(gameObject);
    }

    public T GetAction<T>() where T : BaseAction 
    {
        foreach (BaseAction baseAction in baseActionArray) 
        {
            if(baseAction is T)
                return baseAction as T;
        }
        return null;
    }

    public bool TryTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost()) 
        {
            actionPoints-=baseAction.GetActionPointsCost();
            OnActionPointChangedEvent?.Invoke();
            return true;
        }
        else 
            return false;
    }

    public void TakeDamage(int damageAmount) 
       => healthSystem.TakeDamage(damageAmount);

}
