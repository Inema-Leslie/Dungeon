using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private UnityEngine.UI.Slider healthSlider; 

    private void OnEnable()
    {
        GameEvents.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        
        Health playerHealth = FindPlayerHealth();
        if (playerHealth != null)
        {
            HandleHealthChanged(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
    }

    private Health FindPlayerHealth()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        return playerObj != null ? playerObj.GetComponent<Health>() : null;
    }

    private void HandleHealthChanged(float current, float max)
    {
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }
}