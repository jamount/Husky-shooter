using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : NetworkBehaviour
{
    [SerializeField] private float pickUpRange = 4f;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;

    [SerializeField] private LayerMask pickupLayers;

    private Transform _cameraTransform;
    private PlayerWeapon _playerWeapon;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        _cameraTransform = Camera.main.transform;
        if(TryGetComponent(out PlayerWeapon playerWeapon))
        {
            _playerWeapon = playerWeapon;
        }
        else
        {
            Debug.Log("Could not find player weapon component", gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(pickUpKey))
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, pickUpRange, pickupLayers))
            return;


        if (hit.transform.TryGetComponent(out GroundWeapon groundWeapon))
        {
            _playerWeapon.InitializeWeapon(groundWeapon.PickUpWeapon());
        }
    }       
}
