using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static ResourceManager;
using static TownScript;

public class TownUpgradeMenuScript : MonoBehaviour
{
    private TownScript CurrentTownScript; // Reference to current town script
    private List<GameObject> townResourceMenus;  // List to keep track of instantiated menus for deletion later
    private Dictionary<ResourceType, Sprite> resourceSprites; // Keep sprites here

    public GameObject menuTemplate;
    public Sprite woodSprite;
    public Sprite foodSprite;

    void OnEnable()
    {
        // Initialize the dictionary and add mappings
        resourceSprites = new Dictionary<ResourceType, Sprite>
        {
            { ResourceType.Wood, woodSprite },
            { ResourceType.Food, foodSprite },
        };

        // Initialize the list that will hold the instantiated menu objects
        townResourceMenus = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() { }

    public void setTownProductions(TownScript tScript)
    {
        this.CurrentTownScript = tScript;

        int idx = 0;
        foreach (ResourceProductionEntry res in CurrentTownScript.resourceProductionEntries)
        {
            // Instantiate the prefab at correct position specific position
            GameObject newMenu = Instantiate(menuTemplate, transform.position + new Vector3(0, 800 - (350 * idx), 0), Quaternion.identity);
            newMenu.transform.SetParent(transform);

            // Store the instantiated menu in the list for deletion later
            townResourceMenus.Add(newMenu);

            TownUpgradeTemplateScript script = newMenu.GetComponent<TownUpgradeTemplateScript>(); // Get template's script
            script.setResource(this.CurrentTownScript, res, resourceSprites[res.ResourceType]); // Set current resource to template

            idx++;
        }
    }

    // Called when this GameObject is disabled
    void OnDisable()
    { 
        foreach (GameObject menu in townResourceMenus) // Destroy all the instantiated menus
        {
            Destroy(menu);
        }

        townResourceMenus.Clear(); // Clear the list to prevent any further usage
    }
}
