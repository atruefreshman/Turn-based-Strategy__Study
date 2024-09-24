using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event Action OnAnyGrenadeExploedEvent;

    private Vector3 targetPosition;
    private Vector3 direction;
    private float totalDiatance;
    private Vector3 positionXZ;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float damageRadius=4f;
    [SerializeField] private int damage = 30;
    [SerializeField] private Transform geanadeExplodeEffect;
    [SerializeField] private Transform trail;

    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Action Callback;

    private void Update()
    {
        positionXZ += direction * speed * Time.deltaTime;
        transform.position = new Vector3(positionXZ.x,arcYAnimationCurve.Evaluate(1- Vector3.Distance(positionXZ, targetPosition) /totalDiatance)*totalDiatance/5, positionXZ.z);

        if (Vector3.Distance(targetPosition, positionXZ) < .2f) 
        {
            foreach (Collider collider in Physics.OverlapSphere(transform.position, damageRadius)) 
            {
                if (collider.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) 
                {
                    healthSystem.TakeDamage(damage);
                }
            }

            OnAnyGrenadeExploedEvent?.Invoke();

            Instantiate(geanadeExplodeEffect, targetPosition + Vector3.up * 1f, Quaternion.identity);

            trail.parent = null;

            Destroy(gameObject);

            Callback();
        }
    }

    public void Setup(GridPosition targetGridPosition,Action Callback) 
    {
        this.targetPosition = LevelGrid.Instance.GetCellCenter(targetGridPosition);
        direction=(targetPosition - transform.position).normalized;
        positionXZ = transform.position;
        totalDiatance=Vector3.Distance(targetPosition, transform.position);
        this.Callback = Callback;
    }
}
