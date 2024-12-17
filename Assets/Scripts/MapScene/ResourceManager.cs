using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public enum ResourceType
    {
        Food,
        Wood,
    }

    public TextMeshProUGUI FoodText;
    public TextMeshProUGUI WoodText;

    private Dictionary<ResourceType, int> resourceInventory = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> resourceProduction = new Dictionary<ResourceType, int>();
    private float productionTimer = 1f;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensures only one instance exists
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject); // Optional: persists across scenes
    }

    void Start()
    {
        // Initialize inventory values to 0
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceInventory[resourceType] = 0; // Start with 0 inventory
        }

        // Initialize all productions to 0
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceProduction[resourceType] = 0;
        }

        // For every town, sum up production values
        TownScript[] towns = FindObjectsByType<TownScript>(FindObjectsSortMode.None);
        foreach (var town in towns)
        {
            foreach (var entry in town.resourceProductionEntries)
            {
                resourceProduction[entry.ResourceType] += entry.ProductionAmount;
            }
        }
    }

    void Update()
    {
        productionTimer -= Time.deltaTime;
        if (productionTimer < 0)
        {
            // Update inventory amounts by production amounts
            foreach (var resource in resourceProduction)
            {
                resourceInventory[resource.Key] += resource.Value; // Add production to inventory
            }

            this.UpdateTexts(); // Update texts after production tick
            productionTimer = 1f;
        }
    }
    private void UpdateTexts()
    {
        FoodText.text = "Food: " + resourceInventory[ResourceType.Food].ToString();
        WoodText.text = "Wood: " + resourceInventory[ResourceType.Wood].ToString();
    }

    public int getInventory(ResourceType resourceType)
    {
        return resourceInventory[resourceType];
    }

    public void reduceInventory(ResourceType resourceType, int amount)
    {
        this.resourceInventory[resourceType] -= amount; // Decrement inventory
        this.UpdateTexts(); // Update texts
    }
    public void increaseInverntory(ResourceType resourceType, int amount)
    {
        this.resourceInventory[resourceType] += amount; // Increment inventory
        this.UpdateTexts(); // Update texts
    }

    public void increaseProduction(ResourceType resourceType, int amount)
    {
        this.resourceProduction[resourceType] += amount;
    }
}
