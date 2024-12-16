using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TownScript : MonoBehaviour
{
    [Serializable]
    public class ResourceProductionEntry // Date for each production entry of a town
    {
        public ResourceManager.ResourceType ResourceType;
        public int ProductionAmount;
        public int InitialCost;
        public int CurrentLevel = 0;
        public int CurrentCost;
        public int MaxLevel;
    }

    public int TownIndex;
    public List<ResourceProductionEntry> resourceProductionEntries = new List<ResourceProductionEntry>();

    public GameObject upgradeMenuPanel;  // Reference to Upgrade Menu Panel
    public void ToggleUpgradeMenu() //Open and close upgrade menu panel when clicked on
    {
        if (upgradeMenuPanel != null)
        {
            upgradeMenuPanel.SetActive(!upgradeMenuPanel.activeSelf); // Toggle the panel's active state
            upgradeMenuPanel.GetComponent<TownUpgradeMenuScript>().setTownProductions(this); // Init panel with current town info
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
