using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(!IsOwner)
        {
            enabled = false;
            return;
        }

    }

    [ServerRpc(RequireOwnership =false)]
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        Debug.Log($"New Player Health: {_currentHealth}");

        if(_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"Player is dead");

    }
}
