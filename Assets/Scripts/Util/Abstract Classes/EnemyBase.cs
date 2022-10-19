using UnityEngine;
using System.Collections;
using System;
using GameEvents;

// Classe abstrata base para todos os inimigos.
public abstract class EnemyBase : MonoBehaviour
{

    [Header("Gameplay Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] [Range(5, 20)] protected float followRadius;
    [SerializeField] protected bool destroyOnCollision;
    public float rotationSpeed = 720f;

    [Header("Manager Info")]
    [SerializeField] protected int damage = 1;
    [SerializeField] int scoreValue = 10;
    [SerializeField] float timeToAutoDescrut = 2f;

    protected int playerLayer;

    private Vector3 moveDirection;
    private bool hasEngaged = false;
    private bool hitGround = false;

    private void OnEnable()
    {
        GameplayEvents.RestartGame += OnRestartGame;
    }

    private void OnDisable()
    {
        GameplayEvents.RestartGame -= OnRestartGame;        
    }

    void OnRestartGame()
    {
        ScoreEvents.OnEmenyDied(this.gameObject);
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");

    }

    private void Update()
    {
        if (!hitGround && GameplayManager.Instance.currentGameState == GameStates.PLAYING) FollowTarget();
        else hasEngaged = false;

        if (GameplayManager.Instance.currentGameState == GameStates.GAMEOVER)
        {
            ScoreEvents.OnEmenyDied(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (GameplayManager.Instance.currentGameState != GameStates.PLAYING) return;

        ShouldAutoDestroy();
        Behaviour();
    }

    protected abstract void Behaviour();

    public void FollowTarget()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, followRadius, playerLayer);
        if (target != null)
        {
            hasEngaged = true;

            Vector3 distanceToTarget = target.transform.position - transform.position;
            distanceToTarget.z = 0;
            Vector3 directionToTarget = distanceToTarget.normalized;

            moveDirection = (moveDirection + directionToTarget).normalized;

            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, -moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            hasEngaged = false;
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void ShouldAutoDestroy()
    {
        if (!hasEngaged)
        {
            timeToAutoDescrut -= Time.fixedDeltaTime;
            if (timeToAutoDescrut <= 0)
            {
                ScoreEvents.OnEmenyDied(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {      
        if (other.gameObject.CompareTag("Ground"))
        {
            hitGround = true;
            hasEngaged = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
