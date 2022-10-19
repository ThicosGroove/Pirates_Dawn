using UnityEngine;
using System.Collections;

// Projétil simples. Segue em linha reta usando o eixo y LOCAL.
public class CannonBall : ProjectileBase
{
    [SerializeField] GameObject ExplosionPrefab;
   
    protected override void Behaviour()
    {
        transform.position += travelSpeed * transform.up * Time.fixedDeltaTime;

        // Se o projétil está fora da tela, ele é destruído.
        if (!GameplayManager.IsObjectOnScreen(transform.position))
        {
            Destroy(gameObject);
        }
    }

    protected override void DeathBehaviour()
    {
        var newExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(newExplosion, 0.2f);
        travelSpeed = 0;
        Destroy(this.gameObject, 0.2f);
    }
}
