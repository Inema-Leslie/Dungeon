using UnityEngine;

public class HealAbility : IAbility
{
    public string AbilityName => "Heal";
    public float Cooldown => 10f;

    private readonly float healAmount;
    private float cooldownTimer = 0f;

    public HealAbility(float healAmount = 30f)
    {
        this.healAmount = healAmount;
    }

    public bool CanActivate()
    {
        bool hasCharge = PlayerInventory.Instance != null && PlayerInventory.Instance.HasCharge("HealPotion");
        Debug.Log($"[HealAbility] CanActivate check — cooldownTimer: {cooldownTimer}, hasCharge: {hasCharge}");
        return cooldownTimer <= 0f && hasCharge;
    }

    public void Activate(GameObject user)
    {
        if (!CanActivate())
        {
            Debug.Log("[HealAbility] Activate blocked — CanActivate returned false.");
            return;
        }

        cooldownTimer = Cooldown;

        if (user.TryGetComponent<Health>(out var health))
        {
            Debug.Log($"[HealAbility] Healing for {healAmount}. Current health before: {health.CurrentHealth}");
            health.Heal(healAmount);
        }
        else
        {
            Debug.Log("[HealAbility] No Health component found on user!");
        }

        PlayerInventory.Instance.ConsumeCharge("HealPotion");
    }

    public void Tick(float deltaTime)
    {
        if (cooldownTimer > 0f) cooldownTimer -= deltaTime;
    }
}