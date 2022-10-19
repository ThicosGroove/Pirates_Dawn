using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    [SerializeField] float distToShoot;

    EnemyWeapon myWeapon;
    Collider2D closeToTarget;
    float myMoveSpeed;

    private void Start()
    {
        myWeapon = GetComponent<EnemyWeapon>();
        distToShoot = base.followRadius / 1.8f;
        myMoveSpeed = base.moveSpeed;
    }

    protected override void Behaviour()
    {
        closeToTarget = Physics2D.OverlapCircle(transform.position, distToShoot, base.playerLayer);

        if (closeToTarget != null)
        {
            base.moveSpeed = 0;
            myWeapon.TryShoot();
        }
        else
        {
            base.moveSpeed = myMoveSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        if (closeToTarget == null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, followRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distToShoot);
        }
    }
}
