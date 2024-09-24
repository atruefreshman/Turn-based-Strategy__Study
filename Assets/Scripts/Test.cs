using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GridPosition startGridPosition;
    private GridPosition endGridPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            startGridPosition=LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetMouseWorldPosition());
        }

        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            endGridPosition= LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetMouseWorldPosition()); 
        }

        if (Input.GetKeyDown(KeyCode.U))
            ShowLine();

    }

    private void ShowLine() 
    {
        Debug.Log(startGridPosition+" "+endGridPosition);

        List<GridPosition> path = PathFinding.Instance.GetPath(startGridPosition,endGridPosition,out int pathLength);

        Debug.Log(path==null);

        for (int i = 0; i < path.Count-1; i++) 
        {
            Debug.DrawLine(LevelGrid.Instance.GetCellCenter(path[i]),LevelGrid.Instance.GetCellCenter(path[i+1]),Color.yellow,1000);
        }
        
    }

}
