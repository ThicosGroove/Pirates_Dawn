using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] Cannon cannon;

    [SerializeField] float cannonFireRate;

    private void Start()
    {
        cannon.fireRate = cannonFireRate;
    }

    public void TryShoot()
    {
        cannon.Shoot();
    } 

    public void StopShooting()
    {
        cannon.StopShoot();
    }
}
