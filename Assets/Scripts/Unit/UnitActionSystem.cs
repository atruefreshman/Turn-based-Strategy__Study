using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event Action OnSelectedUnitChangedEvent;
    public event Action OnSelectedActionChangedEvent;
    public event Action<bool> OnUnitTakingActionEvent;

    public Unit selectedUnit { get; private set; }
    private BaseAction selectedAction;
    public BaseAction SelectedAction { get => selectedAction; set { selectedAction = value; OnSelectedActionChangedEvent?.Invoke(); } }

    private bool isBusy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChangedEvent += () 
            => { selectedUnit = null; selectedAction = null; OnSelectedUnitChangedEvent?.Invoke(); OnSelectedActionChangedEvent?.Invoke(); };
    }

    private void Update()
    {
        if (isBusy)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!TurnSystem.Instance.isPlayerTurn)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("Unit")))
                {
                    Unit unit = hitInfo.transform.GetComponent<Unit>();
                    if (unit.IsEnemy)
                        return;
                    if (selectedUnit != unit)
                    {
                        selectedUnit = unit;
                        selectedAction = null;
                        OnSelectedUnitChangedEvent?.Invoke();
                        OnSelectedActionChangedEvent?.Invoke();
                        return;
                    }
                }

                if (selectedAction != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo1, float.MaxValue, 1 << LayerMask.NameToLayer("MousePlane")))
                {
                    //Debug.Log(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetMouseWorldPosition())+""+MouseWorld.Instance.GetMouseWorldPosition()+" "+selectedUnit.MoveAction.IsValidActionGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetMouseWorldPosition())));
                    if (selectedAction.IsValidActionGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetMouseWorldPosition())))
                    {
                        if (!selectedUnit.TryTakeAction(selectedAction))
                            return;

                        isBusy = true;
                        OnUnitTakingActionEvent?.Invoke(true);
                        selectedAction.TakeAction(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetMouseWorldPosition()), () => 
                        { 
                            isBusy = false;
                            OnUnitTakingActionEvent?.Invoke(false);
                            selectedAction = null; 
                            OnSelectedActionChangedEvent?.Invoke();
                        });
                    }
                }
            }
            else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("Unit")))
            {
                Unit unit = hitInfo.transform.GetComponent<Unit>();
                if (unit.IsEnemy)
                    return;
                selectedUnit = unit;
                OnSelectedUnitChangedEvent?.Invoke();
            }

        }
    }

}
