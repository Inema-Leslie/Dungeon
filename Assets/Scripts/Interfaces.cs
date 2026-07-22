using UnityEngine;

public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    void TakeDamage(float amount);
    void Die();
}

public interface IAttackable
{
    float AttackDamage { get; }
    void Attack(IDamageable target);
}

public interface ICollectable
{
    string ItemId { get; }
    void OnCollect(GameObject collector);
}

public interface IMovable
{
    void Move(Vector2 direction);
    void StopMoving();
}

public interface IEnemyBehaviour
{
    void UpdateBehaviour(Transform self, Transform target);
    void OnPlayerDetected(Transform player);
}