using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDeadEvent += HealthSystem_OnDeadEvent;
    }

    private void HealthSystem_OnDeadEvent()
    {
        Instantiate(ragdollPrefab, transform.position, transform.rotation).GetComponent<UnitRagboll>().Setup(originalRootBone);
    }
}
