using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform actionVirtualCamera;

    private void Start()
    {
        actionVirtualCamera.gameObject.SetActive(false);

        BaseAction.OnAnyActionStartEvent += BaseAction_OnAnyActionStartEvent;
        BaseAction.OnAnyActionEndEvent += BaseAction_OnAnyActionEndEvent;
    }

    private void BaseAction_OnAnyActionEndEvent(BaseAction baseAction)
    {
        if (baseAction is ShootAction)
        {
            actionVirtualCamera.gameObject.SetActive(false);
        }
    }

    private void BaseAction_OnAnyActionStartEvent(BaseAction baseAction)
    {
        if (baseAction is ShootAction) 
        {
            Transform viewTransform = baseAction.unit.transform.Find("ShootViewPoint");
            actionVirtualCamera.SetPositionAndRotation(viewTransform.position,viewTransform.rotation);
            actionVirtualCamera.gameObject.SetActive(true);
        }
    }
}
