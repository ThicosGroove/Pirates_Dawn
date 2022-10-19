using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMineBall : ProjectileBase
{
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] float explosionRadius = 0.2f;
    [SerializeField] float explosionTime;
    CircleCollider2D coll;

    private void Start()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    protected override void Behaviour()
    {
        // Não tem movimento no fixedUpdate
    }


    protected override void DeathBehaviour()
    {
        coll.radius = explosionRadius;
        var newExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(newExplosion, explosionTime);
        Destroy(this.gameObject, explosionTime);
    }

}
