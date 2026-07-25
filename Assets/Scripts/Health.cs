using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isPlayer = false; 

    private float currentHealth;
    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

   
    public event Action OnDamaged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
{
    if (isDead) return;

    currentHealth -= amount;
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
}