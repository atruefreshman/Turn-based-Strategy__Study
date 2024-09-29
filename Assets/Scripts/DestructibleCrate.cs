using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    [SerializeField] private Transform CrateParts;

    private void Start()
    {
        GetComponent<HealthSystem>().OnDeadEvent += DestructibleCrate_OnDeadEvent;
    }

    private void DestructibleCrate_OnDeadEvent()
    {
        PathFinding.Instance.SetWalkable(LevelGrid.Instance.GetGridPosition(transform.position),true);
        Destroy(gameObject);
        Decompose(Instantiate(CrateParts, transform.position, transform.rotation),150f, transform.position, 10f);
    }

    private void Decompose(Transform root,float explosionForce,Vector3 explosionPosition,float explosionRadius) 
    {
        foreach (Transform child in root) 
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) 
            {
                rigidbody.AddExplosionForce(explosionForce,explosionPosition,explosionRadius);
            }

            Decompose(child,explosionForce,explosionPosition,explosionRadius);
        } 
    }
}
