using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform bulletProjectile;
    [SerializeField] private Transform shootPoint;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        GetComponent<MoveAction>().OnStartMoveEvent += UnitAnimator_OnStartMoveEvent;
        GetComponent<MoveAction>().OnEndMoveEvent += UnitAnimator_OnEndMoveEvent;
        GetComponent<ShootAction>().OnShootEvent += UnitAnimator_OnShootEvent;
    }

    private void UnitAnimator_OnShootEvent(Unit targetUnit)
    {
        animator.SetTrigger("Shoot");

        Instantiate(bulletProjectile, shootPoint.position, Quaternion.identity).GetComponent<BulletProjectile>().Setup(targetUnit);
    }

    private void UnitAnimator_OnEndMoveEvent()
    {
        animator.SetBool("IsWalking", false);
    }

    private void UnitAnimator_OnStartMoveEvent()
    {
        animator.SetBool("IsWalking", true);
    }
}
