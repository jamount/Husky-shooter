using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerWeapon : NetworkBehaviour
{
    public float Damage = 10f;

    public float maxRange = 20f;

    public LayerMask weaponHitLayers;
    private Transform _cameraTransform;


    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }
    public void Fire() 
    {
        AnimateWeapon();

        if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, maxRange, weaponHitLayers))
            return;


        if(hit.transform.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(Damage);
        }
    }

    public abstract void AnimateWeapon();
}
