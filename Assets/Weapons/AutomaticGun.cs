using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGun : Gun
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Debug.Log("SemiAutomatic Start");

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        bool shooting = Input.GetKey(KeyCode.Mouse0);

        if (shooting && timer >= timeBetweenBullets)
        {
            if(clipAmmo > 0)
            {
                Debug.Log("shooting");
                base.Shoot();
            }
            else if (currentAmmo > 0)
            {
                Debug.Log("reloading");
                base.Reload();
            }
            else
            {
                Debug.Log("No Ammo");
                //handle empty ammo
            }
        }
    }
}
