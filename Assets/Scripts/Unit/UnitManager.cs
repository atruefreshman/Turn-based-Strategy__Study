using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    public List<Unit> unitList { get; private set; }
    public List<Unit> friendlyUnitList { get; private set; }
    public List<Unit> enemyUnitList { get; private set; }

    private void Awake()
    {
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();

        Unit.OnAnyUnitSpawondEvent += Unit_OnAnyUnitSpawondEvent;
        Unit.OnAnyUnitDeadEvent += Unit_OnAnyUnitDeadEvent;
    }

    private void Unit_OnAnyUnitDeadEvent(Unit unit)
    {
        unitList.Remove(unit);
        if (unit.IsEnemy)
            enemyUnitList.Remove(unit);
        else
            friendlyUnitList.Remove(unit);
    }

    private void Unit_OnAnyUnitSpawondEvent(Unit unit)
    {
        unitList.Add(unit);
        if (unit.IsEnemy)
            enemyUnitList.Add(unit);
        else
            friendlyUnitList.Add(unit);
    }
}
