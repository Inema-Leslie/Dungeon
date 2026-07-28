using UnityEngine;

public class ArcherBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("References")]
    public Transform Player;
    public Animator Animator;
    public Transform BowPoint;

    [Header("Combat Settings")]
    public float DetectionRange = 10f;
    public float ShootCooldown = 2f;
    [SerializeField] private AudioClip shootSound; // NEW

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

    public void FireArrow()
    {
        if (Player == null || BowPoint == null) return;

        Vector3 targetPoint = Player.position + Vector3.up * 1f;
        Vector3 aimDir = (targetPoint - BowPoint.position).normalized;
        Debug.Log($"[Archer:{gameObject.name}] BowPoint pos: {BowPoint.position}, AimDir: {aimDir}");

        Arrow arrow = ArrowPool.Instance.GetArrow();
        arrow.transform.position = BowPoint.position;
        arrow.transform.rotation = Quaternion.LookRotation(aimDir);

        AudioManager.Instance?.PlayCombatSound(shootSound); // NEW

        Debug.Log($"[Archer:{gameObject.name}] Fired arrow at Player.");
    }

    public void ForceStop()
    {
        if (!enabled) return;

        Debug.Log($"[Archer:{gameObject.name}] ForceStop — Player reached checkpoint.");
        ChangeState(new ArcherIdleState());
        enabled = false;
    }

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