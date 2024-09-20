using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWeapon : NetworkBehaviour
{
    [SerializeField]
    private int weaponIndex  = -1;

    public int PickUpWeapon()
    {
        DespawnWeapon();
        return weaponIndex;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnWeapon()
    {
        ServerManager.Despawn(gameObject);
    }
}
