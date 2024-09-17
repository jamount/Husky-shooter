using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour 
{
    [SerializeField]
    private int damagePerShot = 20;
    [SerializeField]
    protected float timeBetweenBullets = 0.2f;
    [SerializeField]
    private float weaponRange = 100.0f;

    [SerializeField]
    private int MaxAmmo = 240;
    [SerializeField]
    private int StartingAmmo = 120;
    [SerializeField]
    private int ClipSize = 30;


    [SerializeField]
    private Camera raycastCamera;

    protected float timer;
    [SerializeField]
    protected int currentAmmo;
    [SerializeField]
    protected int clipAmmo;



    //TODO: Juice Up the gun

    //[SerializeField]
    //private ParticleSystem gunParticles;
    //[SerializeField]
    //private LineRenderer gunLine;
    //[SerializeField]
    //private Animator animator;

    protected virtual void Start()
    {
        timer = 0.0f;
        clipAmmo = ClipSize;
        currentAmmo = StartingAmmo - ClipSize;

        Debug.Log("Gun Start");
    }

    protected void Shoot() 
    {
        timer = 0.0f;
        RaycastHit shootHit;
        Ray shootRay = raycastCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        if (Physics.Raycast(shootRay, out shootHit, weaponRange, LayerMask.GetMask("Shootable")))
        {
            string hitTag = shootHit.transform.gameObject.tag;
            switch (hitTag)
            {
                case "Player":
                    //shootHit.collider.GetComponent<>() //Get Damagable Component and apply damage
                    break;
                default:
                    //add gunshot decal to hit object 
                    break;
            }
        }

        clipAmmo -= 1;
    }
    protected void Reload()
    {
        clipAmmo = ClipSize - (currentAmmo % ClipSize);
        currentAmmo -= clipAmmo;
    }

    protected int AddAmmo(int newAmmo)
    {
        currentAmmo += newAmmo;
        if(currentAmmo > MaxAmmo)
        {
            int remainder = currentAmmo - MaxAmmo;
            currentAmmo = MaxAmmo;
            return remainder;
        }
        return 0;
    }
}
