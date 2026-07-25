
using UnityEngine;


public interface IWarriorState
{
    void Enter();
    void Update();
    void Exit();
}

public class WarriorIdleState : IWarriorState
{
    private readonly WarriorBehaviour warrior;

    public WarriorIdleState(WarriorBehaviour warrior)
    {
        this.warrior = warrior;
    }

    public void Enter()
    {
        warrior.SetAnimSpeed(0f);
    }

    public void Update()
    {
        if (warrior.DistanceToPlayer() <= warrior.DetectionRange)
        {
            warrior.ChangeState(warrior.ChaseState);
        }
    }

    public void Exit() { }
}


public class WarriorChaseState : IWarriorState
{
    private readonly WarriorBehaviour warrior;

    public WarriorChaseState(WarriorBehaviour warrior)
    {
        this.warrior = warrior;
    }

    public void Enter()
    {
        warrior.SetAnimSpeed(warrior.MoveSpeed);
    }

    public void Update()
    {
        float distance = warrior.DistanceToPlayer();

        if (distance <= warrior.AttackRange)
        {
            warrior.ChangeState(warrior.AttackState);
            return;
        }

        if (distance > warrior.DetectionRange * 1.5f)
        {
            warrior.ChangeState(warrior.IdleState);
            return;
        }

        MoveTowardPlayer();
    }

    private void MoveTowardPlayer()
    {
        Transform self = warrior.transform;
        Vector3 direction = (warrior.Player.position - self.position);
        direction.y = 0f;
        direction.Normalize();

        self.position += direction * warrior.MoveSpeed * Time.deltaTime;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            self.rotation = Quaternion.Slerp(self.rotation, targetRotation, 8f * Time.deltaTime);
        }
    }

    public void Exit() { }
}


public class WarriorAttackState : IWarriorState
{
    private readonly WarriorBehaviour warrior;

    public WarriorAttackState(WarriorBehaviour warrior)
    {
        this.warrior = warrior;
    }

    public void Enter()
    {
        warrior.SetAnimSpeed(0f);
    }

    public void Update()
    {
        float distance = warrior.DistanceToPlayer();

        if (distance > warrior.AttackRange)
        {
            warrior.ChangeState(warrior.ChaseState);
            return;
        }

        if (warrior.CanAttack())
        {
            warrior.TriggerAttackAnim();
            warrior.StartAttackCooldown();

            IDamageable playerHealth = warrior.Player.GetComponent<IDamageable>();
            if (playerHealth != null)
            {
                warrior.Attack(playerHealth);
                warrior.RegisterHitOnPlayer();
            }
        }
    }

    public void Exit() { }
}


public class WarriorDeadState : IWarriorState
{
    private readonly WarriorBehaviour warrior;

    public WarriorDeadState(WarriorBehaviour warrior)
    {
        this.warrior = warrior;
    }

    public void Enter()
    {
        warrior.SetDeadAnim();
        warrior.SetAnimSpeed(0f);

        Collider col = warrior.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void Update() { }
    public void Exit() { }
}