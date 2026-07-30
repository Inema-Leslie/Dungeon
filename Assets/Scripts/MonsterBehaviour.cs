using UnityEngine;

[RequireComponent(typeof(Health))]
public class MonsterBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("References")]
    public Transform Player;
    public Animator Animator;
    public Transform AttackPoint;
    public GameObject Child;

    [Header("Combat Settings")]
    public float EngageRange = 4f;
    public float AttackRange = 1.5f;
    public float AttackCooldown = 1.5f;
    public float AttackDamage = 15f;
    public float AttackRadius = 1f;
    public LayerMask PlayerLayer;
    [SerializeField] private AudioClip hitSound; 

    private IMonsterState currentState;
    private Health health;
    private bool deathHandled = false;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    private void OnEnable()
    {
        health.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamaged;
    }

    private void Start()
    {
        ChangeState(new MonsterIdleState());
    }

    private void Update()
    {
        UpdateBehaviour(transform, Player);
    }

    public void ChangeState(IMonsterState newState)
    {
        Debug.Log($"[Monster] State: {currentState?.GetType().Name ?? "None"} -> {newState.GetType().Name}");
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public void PerformAttack()
    {
        Animator?.SetTrigger("Attack");

        Collider[] hits = Physics.OverlapSphere(AttackPoint.position, AttackRadius, PlayerLayer);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(AttackDamage);
                AudioManager.Instance?.PlayCombatSound(hitSound); 
            }
        }
    }

    private void HandleDamaged()
    {
        if (deathHandled) return;

        if (health.CurrentHealth <= 0f)
        {
            deathHandled = true;
            Debug.Log("[Monster] Defeated — child saved.");
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        ChangeState(new MonsterDeadState());
        GameEvents.RaiseChildSaved();
    }

    public void DisableCollisionAndAI()
    {
        enabled = false;
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
    }

    public void UpdateBehaviour(Transform self, Transform target)
    {
        if (target != null) Player = target;
        currentState?.Tick(this);
    }

    public void OnPlayerDetected(Transform player)
    {
        Player = player;
        if (currentState is MonsterIdleState)
        {
            ChangeState(new MonsterAttackState());
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
    }
}