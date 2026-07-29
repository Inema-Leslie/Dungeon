// WeaponPickup.cs — full corrected version

using UnityEngine;

public class WeaponPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private GameObject weaponVisual;
    [SerializeField] private GameObject playerHandWeapon;

    public string ItemId => "Weapon";

    private InputSystem_Actions inputActions;
    private bool playerInRange = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start()
    {
        if (PlayerInventory.Instance != null && PlayerInventory.Instance.HasItem(ItemId))
        {
            if (weaponVisual != null) weaponVisual.SetActive(false);
            if (playerHandWeapon != null) playerHandWeapon.SetActive(true);
            gameObject.SetActive(false);

            GameEvents.RaiseItemCollected(ItemId); 
        }
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
        Debug.Log("Weapon collected!");
        if (weaponVisual != null)
            weaponVisual.SetActive(false);

        if (playerHandWeapon != null)
            playerHandWeapon.SetActive(true);

        GameEvents.RaiseItemCollected(ItemId);
        gameObject.SetActive(false);
    }
}