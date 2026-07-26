using UnityEngine;

public interface IArcherState
{
    void Enter(ArcherBehaviour archer);
    void Tick(ArcherBehaviour archer);
    void Exit(ArcherBehaviour archer);
}

public class ArcherIdleState : IArcherState
{
    public void Enter(ArcherBehaviour archer)
    {
        archer.Animator?.SetFloat("Speed", 0f);
    }

    public void Tick(ArcherBehaviour archer)
    {
        if (archer.Player == null) return;

        float distance = Vector3.Distance(archer.transform.position, archer.Player.position);

        if (distance <= archer.DetectionRange)
        {
            archer.ChangeState(new ArcherShootState());
        }
    }

    public void Exit(ArcherBehaviour archer) { }
}

public class ArcherShootState : IArcherState
{
    private const float RotationOffsetDegrees = 90f;

    public void Enter(ArcherBehaviour archer) { }

    public void Tick(ArcherBehaviour archer)
    {
        if (archer.Player == null) return;

        float distance = Vector3.Distance(archer.transform.position, archer.Player.position);

        if (distance > archer.DetectionRange * 1.5f)
        {
            archer.ChangeState(new ArcherIdleState());
            return;
        }

        Vector3 lookDir = archer.Player.position - archer.transform.position;
        lookDir.y = 0f;

        Debug.Log($"[Archer:{archer.gameObject.name}] Player pos: {archer.Player.position}, Archer pos: {archer.transform.position}, LookDir: {lookDir}");

        if (lookDir.sqrMagnitude > 0.01f)
        {
            Quaternion aimRotation = Quaternion.LookRotation(lookDir);
            archer.transform.rotation = aimRotation * Quaternion.Euler(0f, RotationOffsetDegrees, 0f);
        }

        if (archer.CanShoot())
        {
            archer.Animator?.SetTrigger("Shoot");
            archer.FireArrow();
            archer.StartShootCooldown();
        }
    }

    public void Exit(ArcherBehaviour archer) { }
}