using System.Collections;
using UnityEngine;

// Classe abstrata base para todas as armas de fogo.
public abstract class GunBase : MonoBehaviour
{
    public float fireRate;
    [SerializeField] protected int damagePerProjectile = 1;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected GameObject defaultProjectilePrefab;                                                                   

    protected GameObject currentProjectileInstance;
    protected Coroutine firingCoroutine { get; set; }

    private bool canfire = true;

    protected void StartShooting()
    {
        if (canfire)
        {
            firingCoroutine = StartCoroutine(FiringCoroutine());
        }
    }

    protected virtual IEnumerator FiringCoroutine()
    {
        canfire = false;

        Fire();
        StartCoroutine(FireRateHandler());

        yield return null;
    }

    private IEnumerator FireRateHandler()
    {
        float timeToNextFire = 1 / fireRate;

        yield return new WaitForSeconds(timeToNextFire);

        canfire = true;
    }

    private void PreFire()
    {
        Vector3 projectilePosition = transform.position;
        projectilePosition.z = defaultProjectilePrefab.transform.position.z;
        currentProjectileInstance = Instantiate(defaultProjectilePrefab, projectilePosition, transform.rotation);
        currentProjectileInstance.tag = this.tag;
        currentProjectileInstance.GetComponent<ProjectileBase>().travelSpeed = projectileSpeed;
        currentProjectileInstance.GetComponent<ProjectileBase>().damage = damagePerProjectile;
    }

    protected virtual void Fire()
    {
        PreFire();
    }

    protected void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }
}
