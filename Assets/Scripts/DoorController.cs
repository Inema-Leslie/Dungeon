using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject doorMesh;
    [SerializeField] private int hitsRequired = 2;

    private InputSystem_Actions inputActions;
    private int hitCount = 0;
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
            hitCount++;
            Debug.Log("Hit count:" + hitCount);

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
            Debug.Log("Player entered door range");
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