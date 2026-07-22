using UnityEngine;

public class CameraIntroController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private MonoBehaviour cinemachineBrain; 
    [SerializeField] private Transform introCameraPose;

    private void Start()
    {
        if (mainCamera != null && introCameraPose != null)
        {
            mainCamera.transform.position = introCameraPose.position;
            mainCamera.transform.rotation = introCameraPose.rotation;
        }

        if (cinemachineBrain != null)
            cinemachineBrain.enabled = false;

        GameEvents.OnChainsBroken += HandleChainsBroken;
    }

    private void HandleChainsBroken()
    {
        if (cinemachineBrain != null)
            cinemachineBrain.enabled = true;
    }

    private void OnDestroy()
    {
        GameEvents.OnChainsBroken -= HandleChainsBroken;
    }
}