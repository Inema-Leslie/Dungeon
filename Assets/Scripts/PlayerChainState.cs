using UnityEngine;

public class PlayerChainState : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject chainProp;
    [SerializeField] private int attemptsRequired = 3;

    private static readonly int StruggleTrigger = Animator.StringToHash("Struggle");
    private static readonly int BreakFreeTrigger = Animator.StringToHash("BreakFree");

    private int attemptCount = 0;
    private bool isChained = true;
    private bool inputHeldLastFrame = false;
    
    void Start()
    {
     if (playerMovement != null)
     playerMovement.SetMovementEnabled (false);   
    }

    // Update is called once per frame
    void Update()
    {
     if (!isChained) return;
     Vector2 moveInput = playerMovement != null ? playerMovement.GetCurrentMoveInput() : Vector2.zero;
     bool inputPressed = moveInput.magnitude > 0.1f;
     
     if (inputPressed && !inputHeldLastFrame)
        {
            attemptCount++;
            if (attemptCount >= attemptsRequired)
            {
                BreakFree();
            }
            else
            {
                if (animator != null) animator.SetTrigger(StruggleTrigger);
            }
        } 
        inputHeldLastFrame = inputPressed;  
    }
    private void BreakFree()
    {
        isChained = false;
        if (animator != null) animator.SetTrigger (BreakFreeTrigger);
        if (chainProp != null) chainProp.SetActive(false);
        if (playerMovement != null)
        playerMovement.SetMovementEnabled(true);

        GameEvents.RaiseChainsBroken();
    }
}
