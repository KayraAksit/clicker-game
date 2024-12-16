using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> PeasantPrefabs; // List to store peasant prefabs
    public CoinManager coinManager;  // Reference to coin manager
    public DamageManagerScript damageManager; // Reference to damage manager
    //public bool upgradeMenu = false; // Bool to check if upgrade menu is open or not
    public ButtonToggleScript buttonToggleScript;

    private EnemyScript currentEnemyScript; // Script of active enemy
    private float damageTimer = 0f; // Passive damage tick time
    private float difficultyMultiplier = 1.02f;
    private int deadEnemyCount = 0;
    private float passiveDamageInterval = 1.0f;

    private System.Random RNG = new System.Random();


    void Start()
    {
        SpawnEnemyRandom(); // Spawn enemy at start
    }
    private void Update()
    {
        // Calculate when to initiate passive damage
        damageTimer += Time.deltaTime;
        if (damageTimer >= passiveDamageInterval) // If passive damage timer has reached 0
        {
            currentEnemyScript.TakeDamage(damageManager.getTotalPassiveDamage()); // Damage enemy by passive damage amount
            damageTimer = 0f; // Reset timer
            
        }


        HandleTouch();
    }

    public void SpawnEnemyRandom()
    {
        GameObject newEnemy;


        int maxIdx = 0;
        for(;  maxIdx < PeasantPrefabs.Count; maxIdx++) // Find out which enemies can spawn
        {
            int minSP = PeasantPrefabs[maxIdx].GetComponent<EnemyScript>().minSpawnPower; // Get min spawn power of the enemy
            if (deadEnemyCount < minSP)
            {
                break; // When one failse, since later enemies should be stronger, break the loop.
            }

        }

        int idx = Random.Range(0, maxIdx); // Pick randomly from possible enemies to spawn

        newEnemy = Instantiate(PeasantPrefabs[idx], new Vector3(0, -1.5f, 0), Quaternion.identity); // Spawn enemy
        
        currentEnemyScript = newEnemy.GetComponent<EnemyScript>(); // Assign current enemy's script to the manager's variable
        currentEnemyScript.enemyManager = this;  // Assign enemy manger to enemy  

        currentEnemyScript.maxHealth = currentEnemyScript.maxHealth * Mathf.Pow(difficultyMultiplier, deadEnemyCount); // Increse enemy health exponentially with each killed enemy

    }
    void HandleTouch() // Handle tap/click logic
    {
        // Handle touch for mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && buttonToggleScript.isToogled == false)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            if (touchPosition.y > 300) //Touched above the menu buttons
            {
                currentEnemyScript.TakeDamage(damageManager.TapDamage.Damage);  // Every tap deals damage
            }

            /*
            // Handle mouse click for testing on PC
            if (Input.GetMouseButtonDown(0))
            {
                currentEnemyScript.TakeDamage(tapManager.damagePerTap);  // Every click deals damage
            }
            */
        }
    }

    // This function will be called when an enemy dies
    public void OnEnemyDeath()
    {
        deadEnemyCount++; // Inceremet killed enemy count
        coinManager.AddCoins(currentEnemyScript.coinReward);
        // Spawn a new enemy when one dies
        SpawnEnemyRandom();
    }


}
