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

        float distance = UnityEngine.Vector3.Distance(archer.transform.position, archer.Player.position);

        if (distance <= archer.DetectionRange)
        {
            archer.ChangeState(new ArcherShootState());
        }
    }

    public void Exit(ArcherBehaviour archer) { }
}

public class ArcherShootState : IArcherState
{
    public void Enter(ArcherBehaviour archer) { }

    public void Tick(ArcherBehaviour archer)
    {
        if (archer.Player == null) return;

        float distance = UnityEngine.Vector3.Distance(archer.transform.position, archer.Player.position);

        if (distance > archer.DetectionRange * 1.5f)
        {
            archer.ChangeState(new ArcherIdleState());
            return;
        }

        
        UnityEngine.Vector3 lookDir = archer.Player.position - archer.transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.01f)
        {
            archer.transform.rotation = UnityEngine.Quaternion.LookRotation(lookDir);
        }

        if (archer.CanShoot())
        {
            archer.Animator?.SetTrigger("Shoot"); 
            archer.StartShootCooldown();
        }
    }

    public void Exit(ArcherBehaviour archer) { }
}