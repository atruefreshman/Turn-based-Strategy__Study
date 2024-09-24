using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<T>
{
    public int width { get; private set; }
    public int height { get; private set; }
    public float cellSize { get; private set; }

    public T[,] gridObjectArray { get; private set; }

    public GridSystem(int width, int height, float cellSize,Func<GridSystem<T>,GridPosition,T> CreateGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new T[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridObjectArray[x, z] = CreateGridObject(this, new GridPosition(x, z));
            }
        }
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
        => new GridPosition(Mathf.FloorToInt(worldPosition.x / cellSize), Mathf.FloorToInt(worldPosition.z / cellSize));

    public Vector3 GetCellCenter(GridPosition gridPosition)
        => new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize + new Vector3(0.5f * cellSize, 0, 0.5f * cellSize);

    public T GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
        => gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < width && gridPosition.z < height;

}

/// <summary>
/// 网格坐标的封装结构体
/// </summary>
public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return "x: " + x + ", " + "z: " + z;
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
}

/// <summary>
/// 单个网格信息的封装类
/// </summary>
public class GridObject
{
    public GridSystem<GridObject> gridSystem;
    public GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
        => unitList.Add(unit);

    public List<Unit> GetUnitList()
        => unitList;

    public void RemoveUnitList(Unit unit)
        => unitList.Remove(unit);

    public bool HasUnit()
        => unitList.Count > 0;

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit.ToString() + "\n";
        }
        return gridPosition.ToString() + "\n" + unitString;
    }
}
