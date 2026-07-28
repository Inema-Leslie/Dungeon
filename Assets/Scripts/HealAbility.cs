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
        return cooldownTimer <= 0f
            && PlayerInventory.Instance != null
            && PlayerInventory.Instance.HasCharge("HealPotion");
    }

    public void Activate(GameObject user)
    {
        if (!CanActivate()) return;

        cooldownTimer = Cooldown;

        if (user.TryGetComponent<Health>(out var health))
        {
            health.Heal(healAmount);
        }

        PlayerInventory.Instance.ConsumeCharge("HealPotion");
    }

    /// <summary>Call every frame from the controller to tick the cooldown down.</summary>
    public void Tick(float deltaTime)
    {
        if (cooldownTimer > 0f) cooldownTimer -= deltaTime;
    }
}