using UnityEngine;

public class ArcherBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("References")]
    public Transform Player;
    public Animator Animator;
    public Transform BowPoint; // where arrows spawn from

    [Header("Combat Settings")]
    public float DetectionRange = 10f;
    public float ShootCooldown = 2f;

    private IArcherState currentState;
    private float cooldownTimer = 0f;

    private void Awake()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    private void Start()
    {
        ChangeState(new ArcherIdleState());
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        UpdateBehaviour(transform, Player);
    }

    public void ChangeState(IArcherState newState)
    {
        Debug.Log($"[Archer:{gameObject.name}] State: {currentState?.GetType().Name ?? "None"} -> {newState.GetType().Name}");
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public bool CanShoot() => cooldownTimer <= 0f;
    public void StartShootCooldown() => cooldownTimer = ShootCooldown;

    /// <summary>Called via Animation Event on the Shoot clip, at the exact release frame.</summary>
    public void FireArrow()
    {
        if (Player == null || BowPoint == null) return;

        Arrow arrow = ArrowPool.Instance.GetArrow();
        arrow.transform.position = BowPoint.position;
        arrow.transform.rotation = Quaternion.LookRotation((Player.position - BowPoint.position).normalized);

        Debug.Log($"[Archer:{gameObject.name}] Fired arrow at Player.");
    }

    /// <summary>Called once when the Player reaches the Level 4 checkpoint — stops this Archer permanently.</summary>
    public void ForceStop()
    {
        if (!enabled) return;

        Debug.Log($"[Archer:{gameObject.name}] ForceStop — Player reached checkpoint.");
        ChangeState(new ArcherIdleState());
        enabled = false;
    }

    // --- IEnemyBehaviour ---
    public void UpdateBehaviour(Transform self, Transform target)
    {
        if (target != null) Player = target;
        currentState?.Tick(this);
    }

    public void OnPlayerDetected(Transform player)
    {
        Player = player;
        if (currentState is ArcherIdleState)
        {
            ChangeState(new ArcherShootState());
        }
    }
}