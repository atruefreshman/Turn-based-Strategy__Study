using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance; 

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        Instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();   
    }

    private void Start()
    {
        ShootAction.OnAnyShootEvent += ShootAction_OnAnyShootEvent;
        GrenadeProjectile.OnAnyGrenadeExploedEvent += GrenadeProjectile_OnAnyGrenadeExploedEvent;
        SwordAction.OnAnySwordHitEvent += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit()
    {
        Shake(2f);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploedEvent()
    {
        Shake(3f);
    }

    private void ShootAction_OnAnyShootEvent()
    {
        Shake();
    }

    public void Shake(float itensity=1f) 
    {
        impulseSource.GenerateImpulse(itensity);
    }
}
