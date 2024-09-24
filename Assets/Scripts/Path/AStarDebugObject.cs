using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AStarDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private TextMeshPro gridPositionText;

    private PathNode pathNode;

    public void SetUp(PathNode pathNode) 
    {
        this.pathNode = pathNode;
    }

    private void Update()
    {
        if (!pathNode.isWalkable)
        {
            gridPositionText.SetText("");
            gCostText.SetText("");
            hCostText.SetText("");
            fCostText.SetText("");
        }
        else 
        {
            gridPositionText.SetText(pathNode.ToString());
            gCostText.SetText(pathNode.gCost.ToString());
            hCostText.SetText(pathNode.hCost.ToString());
            fCostText.SetText(pathNode.fCost.ToString());
        }
    }
}
