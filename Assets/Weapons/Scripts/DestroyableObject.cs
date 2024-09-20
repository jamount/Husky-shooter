using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour , IDamageable
{
    [SerializeField]
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Taken Damage: " + damage + ". New Health: " + health);
        if(health < 0)
        {
            Debug.Log("I am dead");
            gameObject.SetActive(false);
        }
    }
}
