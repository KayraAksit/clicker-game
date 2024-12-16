using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health of the soldier
    public float currentHealth;   // Current health

    public EnemyManager enemyManager;
    public Slider healthSlider;  // Reference to a UI health bar (optional)

    public float flickDuration = 0.2f;
    public float spawnDelay = 0.5f;
    public int coinReward = 10;
    public int minSpawnPower = 0;

    private Renderer enemyRenderer;  // Reference to the enemy's renderer
    private Color originalColor;

    private bool isDead = false;

    void Start()
    {

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        UpdateHealthUI();

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;

        StartCoroutine(SpawnAnimation());

    }

    // Call this when the player taps the soldier
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0 && isDead==false)
        {
            isDead = true;
            StartCoroutine(DeathAnimation());
        }        
    }

    // Update the health bar UI if present
    void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }
    
    IEnumerator DeathAnimation() // Coroutine to handle the death flicker animation
    {
        float scaleChange = 0.05f; // Scale change amount

        enemyRenderer.material.color = Color.red; // Change the color to red
        transform.localScale = transform.localScale + new Vector3(scaleChange, scaleChange, 0); //Make enemy bigger

        yield return new WaitForSeconds(flickDuration); // Wait for the flicker duration


        enemyRenderer.material.color = originalColor; //Return to original color
        transform.localScale = transform.localScale - new Vector3(scaleChange, scaleChange, 0); //Return to original position

        yield return new WaitForSeconds(flickDuration); // Wait for the flicker duration


        enemyManager.OnEnemyDeath();  // Notify the manager that this enemy died
        Destroy(gameObject); // Destroy the enemy object after the flicker
    }

        
    IEnumerator SpawnAnimation() // Coroutine to handle the spawn scaling animation
    {
        // Start with a very small scale
        transform.localScale = Vector3.zero;

        // Wait for the initial delay
        yield return new WaitForSeconds(spawnDelay);

        // Animate the scale from small to full size
        float timeElapsed = 0f;
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 1);  // Full size (1, 1, 1)

        while (timeElapsed < spawnDelay)
        {
            // Gradually scale up based on the elapsed time
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, timeElapsed / spawnDelay);
            timeElapsed += Time.deltaTime;
            yield return null;  // Wait until the next frame
        }

        // Ensure the final scale is exactly 1,1,1 (full size)
        transform.localScale = targetScale;
    }
}
