using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public event Action OnAnyUnitMovedGridPosition;

    public static LevelGrid Instance { get; private set; }

    [SerializeField] int height=15;
    [SerializeField] int width=15;
    [SerializeField] float cellSize=2f;
    public Transform debugGridPrefab;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> gridSystem,GridPosition gridPosition) => new GridObject(gridSystem,gridPosition) );

        //CreateDebugObjects(debugGridPrefab);
    }

    private void Start()
    {
        PathFinding.Instance.Setup(width,height,cellSize);
    }

    private void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < gridSystem.width; x++)
        {
            for (int z = 0; z < gridSystem.height; z++)
            {
                Instantiate(debugPrefab, GetCellCenter(new GridPosition(x, z)), Quaternion.identity)
                    .GetComponent<GridGameObject>().SetGridObject(gridSystem.gridObjectArray[x, z]);
            }
        }
    }

    #region unit with grid
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPositin(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnitList(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromPosition, GridPosition toPosition)
    {
        RemoveUnitAtGridPositin(fromPosition, unit);
        AddUnitAtGridPosition(toPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke();
    }
    #endregion

    public int GetHeight() => gridSystem.height;
    public int GetWidth() => gridSystem.width;
    public float GetCellSize() => gridSystem.cellSize;
    public Vector3 GetCellCenter(GridPosition gridPosition) => gridSystem.GetCellCenter(gridPosition);


    public GridPosition GetGridPosition(Vector3 worldPosition)
        => gridSystem.GetGridPosition(worldPosition);

    public bool IsValidGridPosition(GridPosition gridPosition)
        => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOn(GridPosition gridPosition)
        => gridSystem.GetGridObject(gridPosition).HasUnit();

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
       => gridSystem.gridObjectArray[gridPosition.x,gridPosition.z].GetUnitList()[0];

    public GridObject GetGridObject(GridPosition gridPosition)
        => gridSystem.GetGridObject(gridPosition);
}
