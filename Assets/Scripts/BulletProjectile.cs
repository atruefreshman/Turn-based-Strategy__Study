using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    [SerializeField] private float speed=200f;
    [SerializeField] private Transform bulletHitFX;
    private Unit targetUnit;
    private Vector3 targetPosition;
    private Vector3 direction;
    private float diatance;
    private Vector3 startPosition;

    public void Setup(Unit targetUnit) 
    {
        trailRenderer=GetComponentInChildren<TrailRenderer>();
        this.targetUnit = targetUnit;
        targetPosition = new Vector3(targetUnit.transform.position.x, transform.position.y, targetUnit.transform.position.z);
        direction = (targetPosition - transform.position).normalized;
        diatance = Vector3.Distance(transform.position, targetPosition);
        startPosition=transform.position;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, startPosition) > diatance)
        {
            transform.position = targetPosition;
            Instantiate(bulletHitFX, targetPosition, Quaternion.identity);
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
