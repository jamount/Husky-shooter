using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotgun : APlayerWeapon
{

    public override void AnimateWeapon()
    {
        Debug.Log("Shotgun Fired");
    }
}
