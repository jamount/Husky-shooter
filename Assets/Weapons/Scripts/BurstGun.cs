using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGun : Gun
{
    [SerializeField]
    private int burstBulletCount = 3;

    [SerializeField]
    private float burstFireRate = 0.2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Debug.Log("Burst Gun Start");

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        bool shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (shooting && timer >= timeBetweenBullets)
        {
            StartCoroutine(Burst());
        }
    }

    private IEnumerator Burst()
    {
        for (int bullet = 1; bullet <= burstBulletCount; bullet++)
        {
            if (clipAmmo > 0)
            {
                Debug.Log("shooting");
                base.Shoot();

                if(bullet != burstBulletCount)
                {
                    yield return new WaitForSeconds(burstFireRate);
                }
            }
            else if (currentAmmo > 0)
            {
                Debug.Log("reloading");
                base.Reload();
                break;
            }
            else
            {
                Debug.Log("No Ammo");

                break;
            }
        }
    }
}
