// GuardianBehaviour.cs

using UnityEngine;

[RequireComponent(typeof(Health))]
public class GuardianBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("References")]
    public Transform Player;
    public Animator Animator;

    [Header("Movement (Blocking Behavior)")]
    public float EngageRange = 5f; 
    public float MoveSpeed = 2f;
    public float StopDistance = 2f;

    [Header("Combat Settings")]
    [SerializeField] private int hitsToDefeat = 10;

    private Health health;
    private IGuardianState currentState;
    private int hitsTaken = 0;
    private bool defeated = false;

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
    Debug.Log($"[Guardian] OnEnable — subscribing to health.OnDamaged. Health InstanceID: {health.GetInstanceID()}, on GameObject: {health.gameObject.name}");
    health.OnDamaged += HandleDamaged;
}

private void OnDisable()
{
    health.OnDamaged -= HandleDamaged;
}

    private void Start()
    {
        ChangeState(new GuardianIdleState()); 
    }

    private void Update()
    {
        currentState?.Tick(this);
    }

    public void ChangeState(IGuardianState newState)
    {
        Debug.Log($"[Guardian] State: {currentState?.GetType().Name ?? "None"} -> {newState.GetType().Name}");
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    private void HandleDamaged()
    {
        if (defeated) return;

        hitsTaken++;
        Debug.Log($"[Guardian] Hit taken! ({hitsTaken}/{hitsToDefeat})");

        if (hitsTaken >= hitsToDefeat)
        {
            defeated = true;
            ChangeState(new GuardianDeadState());
        }
    }

    public void OnDefeated()
    {
        Debug.Log("[Guardian] Defeated — path is clear.");

        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
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
    }
}