// WarriorBehaviour.cs
using UnityEngine;


[RequireComponent(typeof(Health))]
public class WarriorBehaviour : MonoBehaviour, IEnemyBehaviour, IAttackable
{
    [Header("Detection & Combat")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 1.2f;      // tuned down from 2f — has to close more distance
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackCooldown = 2.5f;   // tuned up from 1.5f — fewer swings over time

    [Header("Fight Outcome (hit-count based)")]
    [SerializeField] private int hitsToDefeatPlayer = 5;
    [SerializeField] private int hitsToDefeatWarrior = 3;

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

    private int warriorHitsOnPlayer = 0;
    private int playerHitsOnWarrior = 0;

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
        Debug.Log($"[Warrior] State: {currentState?.GetType().Name ?? "None"} -> {newState.GetType().Name}");
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

    /// <summary>Call this whenever the Warrior successfully lands a hit on the Player.</summary>
    public void RegisterHitOnPlayer()
    {
        if (health.IsDead) return; // fight's already over, stop counting

        warriorHitsOnPlayer++;
        Debug.Log($"[Warrior] Hit Player! ({warriorHitsOnPlayer}/{hitsToDefeatPlayer})");

        if (warriorHitsOnPlayer >= hitsToDefeatPlayer)
        {
            Debug.Log("[Warrior] Reached hit threshold — forcing Player defeat.");
            IDamageable playerHealth = player.GetComponent<IDamageable>();
            playerHealth?.Die();
        }
    }

    private void HandleDamaged()
    {
        if (health.IsDead) return;

        TriggerHitAnim();

        playerHitsOnWarrior++;
        Debug.Log($"[Warrior] Took hit from Player! ({playerHitsOnWarrior}/{hitsToDefeatWarrior})");

        if (playerHitsOnWarrior >= hitsToDefeatWarrior)
        {
            Debug.Log("[Warrior] Reached hit threshold — Warrior defeated.");
            health.Die();
            ChangeState(DeadState); // switch immediately, don't wait for next Update()
        }
    }

    /// <summary>Permanently halts the Warrior — used when the Player leaves its area (e.g. enters the Monster's room).</summary>
    public void ForceStop()
    {
        if (!enabled) return; // already stopped

        Debug.Log("[Warrior] ForceStop triggered — Player left Warrior's territory.");
        ChangeState(IdleState);
        SetAnimSpeed(0f);
        enabled = false; // stops Update() entirely — Warrior freezes in place, no more chase/attack logic runs
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