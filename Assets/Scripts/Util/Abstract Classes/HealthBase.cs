using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using GameEvents;

public abstract class HealthBase : MonoBehaviour, IDamageable
{
    [SerializeField] Sprite[] sprite_Ship;

    [SerializeField] Image healthImage;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected int scoreValueFromDeath = 10;
    [SerializeField] private Color maxHealthColor = Color.green;
    [SerializeField] private Color minHealthColor = Color.red;
    
    protected bool isAlive = true;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        spriteRenderer.sprite = sprite_Ship[Const.FULL_HEALTH_SPRITE];

        _currentHealth = _maxHealth;
        healthImage.color = maxHealthColor;
    }

    void Update()
    {
        if (isAlive)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        float fillAmout = _currentHealth / _maxHealth;

        healthImage.fillAmount = fillAmout;
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        audioSource.Play();

        HealthColor();

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    protected void HealthColor()
    {
        if (_currentHealth <= (_maxHealth / 2))
        {
            healthImage.color = minHealthColor;
            spriteRenderer.sprite = sprite_Ship[Const.LOW_HEALTH_SPRITE];
        }
    }

    public void Death()
    {
        if (isAlive)
        {
            if (CompareTag("Enemy"))
            {
                EnemyDeathBehaviour();
            }
            else
            {
                PlayerDeathBehaviour();
            }

            isAlive = false;
        }
    }

    protected virtual void EnemyDeathBehaviour()
    {
        ScoreEvents.OnScoreGained(scoreValueFromDeath);
        ScoreEvents.OnEmenyDied(this.gameObject);

        EnemyBase enemy = GetComponent<EnemyBase>();
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        enemy.moveSpeed = 0;
        enemy.rotationSpeed = 0;

        foreach (Transform child in enemy.GetComponentInChildren<Transform>())
        {
            child.gameObject.SetActive(false);
        }

        spriteRenderer.sprite = sprite_Ship[Const.DEATH_SPRITE];
        Destroy(gameObject, 1f);
    }

    protected virtual void PlayerDeathBehaviour()
    {
        HealthHandler player = GetComponent<HealthHandler>();

        foreach (Transform child in player.GetComponentInChildren<Transform>())
        {
            child.gameObject.SetActive(false);
        }

        spriteRenderer.sprite = sprite_Ship[Const.DEATH_SPRITE];
        Destroy(gameObject, .8f);
        GameplayManager.Instance.UpdateGameState(GameStates.GAMEOVER);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable hitPoints = other.gameObject.GetComponent<IDamageable>();
        if (hitPoints != null)
        {
            if (this.CompareTag("Enemy") && other.gameObject.CompareTag("Player"))
            {
                EnemyDeathBehaviour();
            }
            else if (this.CompareTag("Enemy") && other.gameObject.CompareTag("Enemy"))
            {
                // nao faz nada
            }
            else
            {
                GetHit(1);
            }
        }
    }
}
