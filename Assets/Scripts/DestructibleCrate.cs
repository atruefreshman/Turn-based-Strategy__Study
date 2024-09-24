using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    private void Start()
    {
        GetComponent<HealthSystem>().OnDeadEvent += DestructibleCrate_OnDeadEvent;
    }

    private void DestructibleCrate_OnDeadEvent()
    {
        PathFinding.Instance.SetWalkable(LevelGrid.Instance.GetGridPosition(transform.position),true);
        Destroy(gameObject);
    }
}
