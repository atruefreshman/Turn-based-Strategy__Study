using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquiptmentType 
{
    Gun,
    Sword,
}

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform bulletProjectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform swordTransform;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        GetComponent<MoveAction>().OnStartMoveEvent += UnitAnimator_OnStartMoveEvent;
        GetComponent<MoveAction>().OnEndMoveEvent += UnitAnimator_OnEndMoveEvent;
        GetComponent<ShootAction>().OnShootEvent += UnitAnimator_OnShootEvent;
        GetComponent<SwordAction>().OnSwordActionStarted += UnitAnimator_OnSwordActionStarted;
        GetComponent<SwordAction>().OnSwordActionFinished += UnitAnimator_OnSwordActionFinished;
    }

    private void UnitAnimator_OnSwordActionFinished()
    {
        Equip(EquiptmentType.Gun);
    }

    private void UnitAnimator_OnSwordActionStarted()
    {
        animator.SetTrigger("SwordSlash");
        Equip(EquiptmentType.Sword);
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

    public void Equip(EquiptmentType equiptmentType) 
    {
        switch (equiptmentType) 
        {
            case EquiptmentType.Gun:
                gunTransform.gameObject.SetActive(true);
                swordTransform.gameObject.SetActive(false);
                break;

            case EquiptmentType.Sword:
                gunTransform.gameObject.SetActive(false);
                swordTransform.gameObject.SetActive(true);
                break;
        }
    }
}
