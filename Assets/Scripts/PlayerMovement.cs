using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IMovable
{
    [Header("Movement")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private static readonly int SpeedParam = Animator.StringToHash("Speed");

    private CharacterController controller;
    private IMovementStrategy walkStrategy;
    private IMovementStrategy runStrategy;
    private IMovementStrategy duckStrategy;
    private IMovementStrategy currentStrategy;

    private Vector3 currentVelocity;
    private float verticalVelocity;
    private bool movementEnabled = true;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        walkStrategy = new WalkStrategy();
        runStrategy = new RunStrategy();
        duckStrategy = new DuckStrategy();
        currentStrategy = walkStrategy;

        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        if (!movementEnabled)
        {
            ApplyGravity();
            if (animator != null) animator.SetFloat(SpeedParam, 0f);
            return;
        }

        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        if (inputActions.Player.Crouch.IsPressed())
            currentStrategy = duckStrategy;
        else if (inputActions.Player.Sprint.IsPressed())
            currentStrategy = runStrategy;
        else
            currentStrategy = walkStrategy;

        MoveAndRotate(input);
        ApplyGravity();

        if (animator != null)
        {
            float horizontalSpeed = new Vector3(currentVelocity.x, 0, currentVelocity.z).magnitude;
            animator.SetFloat(SpeedParam, horizontalSpeed);
        }
    }

    private void MoveAndRotate(Vector2 input)
    {
        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;

        Vector3 moveDir = Vector3.zero;
        if (inputDir.sqrMagnitude > 0.01f)
        {
            float camYaw = cameraTransform != null ? cameraTransform.eulerAngles.y : 0f;
            Quaternion camRotation = Quaternion.Euler(0, camYaw, 0);
            moveDir = camRotation * inputDir;

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 targetVelocity = moveDir * currentStrategy.TargetSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

        Vector3 motion = currentVelocity;
        motion.y = verticalVelocity;
        controller.Move(motion * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;
    }

    public void Move(Vector2 direction) { }
    public void StopMoving() => currentVelocity = Vector3.zero;

    public void SetMovementEnabled(bool enabled) => movementEnabled = enabled;

    /// <summary>Exposes current move input so PlayerChainState can count attempts.</summary>
    public Vector2 GetCurrentMoveInput() => inputActions.Player.Move.ReadValue<Vector2>();
}