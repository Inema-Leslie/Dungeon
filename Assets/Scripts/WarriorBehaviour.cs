using UnityEngine;


[RequireComponent(typeof(Health))]
public class WarriorBehaviour : MonoBehaviour, IEnemyBehaviour, IAttackable
{
    [Header("Detection & Combat")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player; // assign, or auto-found by tag

    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");
    private static readonly int IsDeadParam = Animator.StringToHash("IsDead");

    private Health health;
    private IWarriorState currentState;
    private float attackCooldownTimer = 0f;

    // States (created once, reused)
    public WarriorIdleState IdleState { get; private set; }
    public WarriorChaseState ChaseState { get; private set; }
    public WarriorAttackState AttackState { get; private set; }
    public WarriorDeadState DeadState { get; private set; }

    public float AttackDamage => attackDamage;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public float MoveSpeed => moveSpeed;
    public Animator Animator => animator;
    public Transform Player => player;
    public Health Health => health;

    private void Awake()
    {
        health = GetComponent<Health>();

        IdleState = new WarriorIdleState(this);
        ChaseState = new WarriorChaseState(this);
        AttackState = new WarriorAttackState(this);
        DeadState = new WarriorDeadState(this);
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        health.OnDamaged += HandleDamaged;

        ChangeState(IdleState);
    }

    private void Update()
    {
        if (attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;

        if (health.IsDead && !(currentState is WarriorDeadState))
        {
            ChangeState(DeadState);
            return;
        }

        currentState?.Update();
    }

    public void ChangeState(IWarriorState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector3.Distance(transform.position, player.position);
    }

    public bool CanAttack() => attackCooldownTimer <= 0f;

    public void StartAttackCooldown() => attackCooldownTimer = attackCooldown;

    public void SetAnimSpeed(float speed) => animator?.SetFloat(SpeedParam, speed);
    public void TriggerAttackAnim() => animator?.SetTrigger(AttackTrigger);
    public void TriggerHitAnim() => animator?.SetTrigger(HitTrigger);
    public void SetDeadAnim() => animator?.SetBool(IsDeadParam, true);

    private void HandleDamaged()
    {
        if (!health.IsDead)
            TriggerHitAnim();
    }

    // --- IEnemyBehaviour ---
    public void UpdateBehaviour(Transform self, Transform target)
    {
    
    }

    public void OnPlayerDetected(Transform playerTransform)
    {
        if (currentState == IdleState)
            ChangeState(ChaseState);
    }

    // --- IAttackable ---
    public void Attack(IDamageable target)
    {
        target.TakeDamage(attackDamage);
    }
}