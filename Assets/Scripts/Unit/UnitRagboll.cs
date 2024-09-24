using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagboll : MonoBehaviour
{
    [SerializeField] private Transform ragbollRootBone;

    public void Setup(Transform originalRootBone) 
    {
        MatchAllChildTransforms(originalRootBone,ragbollRootBone);

        ApplyExplosionToRagboll(ragbollRootBone,300f,transform.position+new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f)),10f);
    }

    private void MatchAllChildTransforms(Transform originalNode,Transform ragbollNode) 
    {
        foreach (Transform originalBone in originalNode) 
        {
            Transform ragbollBone = ragbollNode.Find(originalBone.name);
            if (ragbollBone != null) 
            {
                ragbollBone.position = originalBone.position;
                ragbollBone.rotation = originalBone.rotation;
            }

            MatchAllChildTransforms(originalBone, ragbollBone);
        }
    }

    private void ApplyExplosionToRagboll(Transform root,float explosionForce,Vector3 explosionPosition,float explosionRadius) 
    {
        foreach (Transform bone in root) 
        {
            if (bone.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) 
            {
                rigidbody.AddExplosionForce(explosionForce,explosionPosition,explosionRadius);
            }

            ApplyExplosionToRagboll(bone, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
