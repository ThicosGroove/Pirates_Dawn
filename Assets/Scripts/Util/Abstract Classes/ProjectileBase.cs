using UnityEngine;
using System.Collections;

// Classe abstrata base para todos os projéteis.
public abstract class ProjectileBase : MonoBehaviour
{
    public bool destroyOnCollision = true;
    [SerializeField] float totalLifeTime = 3f;

    [HideInInspector] public float travelSpeed;
    [HideInInspector] public int damage;

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    protected abstract void Behaviour();
    protected abstract void DeathBehaviour();

    private void FixedUpdate()
    {
        Behaviour();
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(totalLifeTime);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        ProcessCollider(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcessCollider(other.gameObject);
    }

    private void ProcessCollider(GameObject other)
    {
        if (transform.CompareTag("Enemy") && other.CompareTag("Enemy")
            || transform.CompareTag("Player") && other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(), other.GetComponent<CapsuleCollider2D>(), true);       
        }
        else
        {
            IDamageable hitPoints = other.gameObject.GetComponent<IDamageable>();
            if (hitPoints != null)
            {
                hitPoints.GetHit(damage);

                if (destroyOnCollision)
                {
                   DeathBehaviour();
                }
            }
        }
    }
}
