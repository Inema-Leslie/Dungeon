using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float healAmount = 30f;

    private InputSystem_Actions inputActions;
    private DashAbility dashAbility;
    private HealAbility healAbility;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        dashAbility = new DashAbility(dashDistance, dashDuration);
        healAbility = new HealAbility(healAmount);
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        dashAbility.Tick(Time.deltaTime);
        healAbility.Tick(Time.deltaTime);

        if (inputActions.Player.Dash.WasPressedThisFrame())
        {
            dashAbility.Activate(gameObject);
        }

        if (inputActions.Player.Heal.WasPressedThisFrame())
        {
            healAbility.Activate(gameObject);
        }
    }
}