using UnityEngine;

public class HealPotionPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private GameObject potionVisual;

    public string ItemId => "HealPotion";

    private InputSystem_Actions inputActions;
    private bool playerInRange = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Update()
    {
        if (!playerInRange) return;

        if (inputActions.Player.Interact.WasPressedThisFrame())
        {
            OnCollect(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public void OnCollect(GameObject collector)
    {
        Debug.Log("Heal potion collected!");

        PlayerInventory.Instance?.AddCharge(ItemId, 1);

        if (potionVisual != null) potionVisual.SetActive(false);
        gameObject.SetActive(false);
    }
}