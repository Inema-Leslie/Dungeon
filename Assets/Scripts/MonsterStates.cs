

public interface IMonsterState
{
    void Enter(MonsterBehaviour monster);
    void Tick(MonsterBehaviour monster);
    void Exit(MonsterBehaviour monster);
}


public class MonsterIdleState : IMonsterState
{
    public void Enter(MonsterBehaviour monster)
    {
        monster.Animator?.SetBool("IsAttacking", false);
    }

    public void Tick(MonsterBehaviour monster)
    {
        if (monster.Player == null) return;

        float distance = UnityEngine.Vector3.Distance(monster.transform.position, monster.Player.position);

        if (distance <= monster.EngageRange)
        {
            monster.ChangeState(new MonsterAttackState());
        }
    }

    public void Exit(MonsterBehaviour monster) { }
}


public class MonsterAttackState : IMonsterState
{
    private float attackTimer;

    public void Enter(MonsterBehaviour monster)
    {
        monster.Animator?.SetBool("IsAttacking", true);
        attackTimer = 0f;
    }

    public void Tick(MonsterBehaviour monster)
    {
        if (monster.Player == null) return;

        float distance = UnityEngine.Vector3.Distance(monster.transform.position, monster.Player.position);

        if (distance > monster.EngageRange * 1.5f)
        {
            monster.ChangeState(new MonsterIdleState());
            return;
        }

        attackTimer += UnityEngine.Time.deltaTime;
        if (attackTimer >= monster.AttackCooldown && distance <= monster.AttackRange)
        {
            monster.PerformAttack();
            attackTimer = 0f;
        }
    }

    public void Exit(MonsterBehaviour monster)
    {
        monster.Animator?.SetBool("IsAttacking", false);
    }
}

// --- Dead: fight is over, monster defeated ---

public class MonsterDeadState : IMonsterState
{
    public void Enter(MonsterBehaviour monster)
    {
        monster.Animator?.SetTrigger("Die");      
        monster.DisableCollisionAndAI();
    }

    public void Tick(MonsterBehaviour monster) { }

    public void Exit(MonsterBehaviour monster) { }
}