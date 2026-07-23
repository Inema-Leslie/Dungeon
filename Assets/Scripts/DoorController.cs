using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject doorMesh;
    [SerializeField] private int hitsRequired = 2;

    private InputSystem_Actions inputActions;
    private int hitCount = 0;
    private bool playerInRange = false;
    private bool weaponCollected = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        GameEvents.OnItemCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        GameEvents.OnItemCollected -= HandleItemCollected;
    }

    private void HandleItemCollected(string itemId)
    {
        if (itemId == "Weapon")
            weaponCollected = true;
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (inputActions.Player.Interact.WasPressedThisFrame())
        {
            if (!weaponCollected)
            {
                Debug.Log("Pick up the weapon first.");
                return;
            }

            hitCount++;

            if (hitCount >= hitsRequired)
            {
                OpenDoor();
            }
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

    private void OpenDoor()
    {
        if (doorMesh != null)
            doorMesh.SetActive(false);

        GameEvents.RaiseDoorOpened();
    }
}