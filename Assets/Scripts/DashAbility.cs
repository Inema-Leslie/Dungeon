using UnityEngine;

public class DashAbility : IAbility
{
    public string AbilityName => "Dash";
    public float Cooldown => 2f;

    private readonly float dashDistance;
    private readonly float dashDuration;
    private float cooldownTimer = 0f;

    public DashAbility(float dashDistance = 5f, float dashDuration = 0.2f)
    {
        this.dashDistance = dashDistance;
        this.dashDuration = dashDuration;
    }

    public bool CanActivate() => cooldownTimer <= 0f;

    public void Activate(GameObject user)
    {
        if (!CanActivate()) return;

        cooldownTimer = Cooldown;

        if (user.TryGetComponent<PlayerMovement>(out var movement))
        {
            movement.StartDash(dashDistance, dashDuration);
        }
    }

    public void Tick(float deltaTime)
    {
        if (cooldownTimer > 0f) cooldownTimer -= deltaTime;
    }
}