using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance; 

    public const int STRAIGHT_COST = 10;
    public const int DIAGONAL_COST = 14;
    [SerializeField] private Transform AStartDebugPrefab;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        Instance = this;
    }

    public void Setup(int width,int height,float cellSize) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));

#if UNITY_EDITOR
        CreateDebugObjects(AStartDebugPrefab);
#endif

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 cellCenter = gridSystem.GetCellCenter(new GridPosition(x, z));
                if (Physics.Raycast(cellCenter +Vector3.down, Vector3.up, 10, 1 << LayerMask.NameToLayer("Obstacle"))) 
                {
                    GetPathNode(x,z).isWalkable=false;
                }
            }
        }
    }

    public List<GridPosition> GetPath(GridPosition startGridPosition, GridPosition endGridPosition,out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();
        List<GridPosition> path = new List<GridPosition>();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                PathNode pathNode = GetPathNode(new GridPosition(x, z));
                pathNode.Reset();
            }
        }

        PathNode startNode = GetPathNode(startGridPosition);
        PathNode endNode = GetPathNode(endGridPosition);

        path.Add(endNode.gridPosition);

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startGridPosition,endGridPosition);
        openList.Add(startNode);

        while (openList.Count > 0) 
        {
            PathNode currentNode = GetLowestFcostPathNode(openList);

            if (currentNode == endNode) 
            {
                pathLength = currentNode.fCost;

                while (endNode.lastNode != null)
                {
                    endNode = endNode.lastNode;
                    path.Add(endNode.gridPosition);
                }

                path.Reverse();
                return path;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourNode(currentNode.gridPosition)) 
            {
                if (closedList.Contains(neighbourNode))
                    continue;

                if (!neighbourNode.isWalkable) 
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int gCost=currentNode.gCost+CalculateDistance(currentNode.gridPosition,neighbourNode.gridPosition);
                if (gCost< neighbourNode.gCost) 
                {
                    neighbourNode.gCost = gCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode.gridPosition, endGridPosition);
                    neighbourNode.lastNode = currentNode;

                    if(!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }

        pathLength=int.MaxValue;
        return null;
    }

    private int CalculateDistance(GridPosition n1, GridPosition n2)
    {
        int x = Mathf.Abs(n1.x - n2.x);
        int z = Mathf.Abs(n1.z - n2.z);
        int longSide= Mathf.Max(x, z);
        int shortSide = Mathf.Min(x, z);

        return (longSide - shortSide) * STRAIGHT_COST + shortSide * DIAGONAL_COST;
    }

    private List<PathNode> GetNeighbourNode(GridPosition gridPosition) 
    {
        List<PathNode> neighbourNodeList=new List<PathNode>();

        if (gridPosition.x - 1 >= 0) 
        {
            neighbourNodeList.Add(GetPathNode(gridPosition.x-1,gridPosition.z+0));
            if(gridPosition.z-1>=0)
                neighbourNodeList.Add(GetPathNode(gridPosition.x-1,gridPosition.z-1));
            if(gridPosition.z + 1 <gridSystem.height)
                neighbourNodeList.Add(GetPathNode(gridPosition.x-1,gridPosition.z+1));
        }
        if (gridPosition.x + 1 < gridSystem.height) 
        {
            neighbourNodeList.Add(GetPathNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
                neighbourNodeList.Add(GetPathNode(gridPosition.x + 1, gridPosition.z - 1));
            if (gridPosition.z +1< gridSystem.height)
                neighbourNodeList.Add(GetPathNode(gridPosition.x + 1, gridPosition.z + 1));
        }
        if (gridPosition.z - 1 >= 0)
            neighbourNodeList.Add(GetPathNode(gridPosition.x + 0, gridPosition.z - 1));
        if (gridPosition.z +1< gridSystem.height)
            neighbourNodeList.Add(GetPathNode(gridPosition.x + 0, gridPosition.z + 1));

        return neighbourNodeList;
    }

    private PathNode GetLowestFcostPathNode(List<PathNode> pathNodeList) 
    {
        PathNode lowestFcostPathNode =pathNodeList[0];
        foreach (PathNode pathNode in pathNodeList) 
        {
            if(pathNode.fCost<lowestFcostPathNode.fCost)
                lowestFcostPathNode = pathNode; 
        }
        return lowestFcostPathNode;
    }

    public bool HasPath(GridPosition startPosition,GridPosition endPosition,out int pathLength) 
        => GetPath(startPosition,endPosition,out pathLength)!=null;

    public bool IsWalkable(GridPosition gridPosition) 
        => GetPathNode(gridPosition).isWalkable;

    public void SetWalkable(GridPosition gridPosition,bool isWalkable) 
        => GetPathNode(gridPosition).isWalkable = isWalkable;

    public PathNode GetPathNode(GridPosition gridPosition) 
        => gridSystem.GetGridObject(gridPosition);

    public PathNode GetPathNode(int x, int z)
        => gridSystem.GetGridObject(new GridPosition(x,z));

    private void CreateDebugObjects(Transform AStartDebugPrefab) 
    {
        for (int x = 0; x < gridSystem.width; x++)
        {
            for (int z = 0; z < gridSystem.height; z++)
            {
                Instantiate(AStartDebugPrefab, gridSystem.GetCellCenter(new GridPosition(x, z)), Quaternion.identity)
                    .GetComponent<AStarDebugObject>().SetUp(gridSystem.gridObjectArray[x, z]);
            }
        }
    }

}

public class PathNode
{
    public GridPosition gridPosition { get; private set; }

    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

    public PathNode lastNode;

    public bool isWalkable=true;

    public PathNode(GridPosition gridPosition) 
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public void Reset() 
    {
        gCost = int.MaxValue;
        hCost = 0;
        lastNode = null;
    }
}
