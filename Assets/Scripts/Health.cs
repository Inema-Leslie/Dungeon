using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isPlayer = false;

    [Header("Regeneration (Gameplay Logic Algorithm)")]
    [SerializeField] private float regenDelay = 4f;
    [SerializeField] private float regenRate = 5f;

    private float currentHealth;
    private bool isDead = false;
    private float timeSinceLastDamage = 0f;
    private float damageMultiplier = 1f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    public event Action OnDamaged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead || !isPlayer) return;

        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= regenDelay && currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + regenRate * Time.deltaTime, maxHealth);
            GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
        }
    }

    public void SetDamageMultiplier(float multiplier) => damageMultiplier = multiplier;

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        timeSinceLastDamage = 0f;

        currentHealth -= amount * damageMultiplier;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log($"[Health:{gameObject.name}] Took {amount} damage. Current: {currentHealth}/{maxHealth}");

        OnDamaged?.Invoke();

        if (isPlayer)
            GameEvents.RaiseHealthChanged(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (isPlayer)
            GameEvents.RaisePlayerDied();
        else
            GameEvents.RaiseEnemyDefeated(gameObject.name);
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (isPlayer)
            GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    public void SetHealth(float amount)
    {
        currentHealth = Mathf.Clamp(amount, 0f, maxHealth);
        isDead = currentHealth <= 0f;
    }
}