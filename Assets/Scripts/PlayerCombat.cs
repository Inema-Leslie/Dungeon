using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCombat : MonoBehaviour, IAttackable
{
    [SerializeField] private float attackDamage = 25f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private Transform attackOrigin; 
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip hitSound; 

    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    private InputSystem_Actions inputActions;
    private float cooldownTimer = 0f;

    public float AttackDamage => attackDamage;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (inputActions.Player.Attack.WasPressedThisFrame() && cooldownTimer <= 0f)
        {
            PerformAttack();
            cooldownTimer = attackCooldown;
        }
    }

    private void PerformAttack()
{
    if (animator != null)
        animator.SetTrigger(AttackTrigger);

    Vector3 origin = attackOrigin != null ? attackOrigin.position : transform.position + transform.forward;

    Collider[] hits = Physics.OverlapSphere(origin, attackRange);
    foreach (var hit in hits)
    {
        if (hit.gameObject == gameObject) continue;

        IDamageable damageable = hit.GetComponentInParent<IDamageable>(); 
        if (damageable != null)
        {
            Debug.Log($"[Player] Hit {hit.gameObject.name} for {attackDamage} damage.");
            Attack(damageable);
            AudioManager.Instance?.PlayCombatSound(hitSound);
        }
    }
}

    public void Attack(IDamageable target)
    {
        target.TakeDamage(attackDamage);
    }
}