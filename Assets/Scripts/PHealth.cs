using UnityEngine;
using UnityEngine.UI;

public class PHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    [Header("UI References")]
    [SerializeField] private Image healthBarFill; // drag your green fill Image here
    [SerializeField] private GameObject gameOverCanvas; // optional

    void Start()
    {
        currentHealth = maxHealth;

        if (gameOverCanvas)
            gameOverCanvas.SetActive(false);

        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        //Time.timeScale = 0f;
        Destroy(gameObject);
        if (gameOverCanvas)
            gameOverCanvas.SetActive(true);
    }
}