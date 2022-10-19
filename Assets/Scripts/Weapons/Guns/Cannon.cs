using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cannon : GunBase
{
    public void Shoot()
    {
        StartShooting();
    }

    public void StopShoot()
    {
        StopFiring();
    }
}
