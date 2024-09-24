using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridVisualColor 
{
    White,
    Blue,
    Red,
    RedSoft,
    Yellow
}

[Serializable]
public struct GridVisualColorToMateria 
{
    public GridVisualColor color;
    public Material material;
}

public class GridSyatemVisual : MonoBehaviour
{
    public static GridSyatemVisual Instance { get; private set; }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    [SerializeField] private GridVisualColorToMateria[] colorToMateriaArray;

    private GridSyatemVisualSingle[,] gridSyatemVisualSingleArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InstantiateVisual();

        UpdateGridVisual();

        UnitActionSystem.Instance.OnSelectedActionChangedEvent += UpdateGridVisual;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += UpdateGridVisual;
    }

    /// <summary>
    /// 生成网格图片预制体
    /// </summary>
    private void InstantiateVisual() 
    {
        gridSyatemVisualSingleArray = new GridSyatemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                Transform visualTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetCellCenter(new GridPosition(x,z)), Quaternion.identity);
                gridSyatemVisualSingleArray[x, z] = visualTransform.GetComponent<GridSyatemVisualSingle>();
            }
        }
    }

    public void HideAllGridSyatemVisual() 
    {
        foreach (GridSyatemVisualSingle visual in gridSyatemVisualSingleArray)
            visual.Hide();
    }

    public void ShowGridVisualBy(List<GridPosition> gridPositionList,Material material) 
    {
        foreach (GridPosition gridPosition in gridPositionList) 
        {
            GridSyatemVisualSingle gridSyatemVisualSingle = gridSyatemVisualSingleArray[gridPosition.x, gridPosition.z];
            gridSyatemVisualSingle.Show(material);
        }
    }

    public void UpdateGridVisual() 
    {
        HideAllGridSyatemVisual();

        if (UnitActionSystem.Instance.SelectedAction != null)
        {
            BaseAction selectedAction = UnitActionSystem.Instance.SelectedAction;
            GridVisualColor gridVisualColor=GridVisualColor.White;

            switch (selectedAction) 
            {
                case MoveAction moveAction:
                    gridVisualColor = GridVisualColor.White;
                    break;
                case ShootAction shootAction:
                    gridVisualColor = GridVisualColor.Red;
                    ShowShootRange(shootAction.unit.gridPosition,shootAction.MaxShootDistance,GridVisualColor.RedSoft);
                    break;
                case SpinAction spinAction:
                    gridVisualColor = GridVisualColor.Blue;
                    break;
            }

            ShowGridVisualBy(selectedAction.GetValidActionGridPositionList(),ColorToMateria(gridVisualColor));
        }
    }

    public void ShowShootRange(GridPosition gridPosition,int range, GridVisualColor color) 
    {
        for (int x =-range;x<= range;x++) 
        {
            for(int z =-range;z<= range;z++) 
            {
                GridPosition testPosition=new GridPosition(gridPosition.x+x,gridPosition.z+z);

                if ((Mathf.Abs(x) + Mathf.Abs(z)) > range)
                    continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testPosition)||LevelGrid.Instance.HasAnyUnitOn(testPosition))
                    continue;

                gridSyatemVisualSingleArray[testPosition.x, testPosition.z].Show(ColorToMateria(color));
            }
        }
    }

    public Material ColorToMateria(GridVisualColor color) 
    {
        foreach (GridVisualColorToMateria cTm in colorToMateriaArray) 
        {
            if (color == cTm.color)
                return cTm.material;
        }
        return null;
    }
}
