using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TownScript;
using System.Resources;
using System.Security.Cryptography;

public class TownUpgradeTemplateScript : MonoBehaviour
{
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI costText;
    private TextMeshProUGUI currentProductionText;
    private TextMeshProUGUI buttonText;
    private Image icon;

    private ResourceProductionEntry resource;
    private TownScript thisTown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {

        titleText = transform.Find("Title_Text").GetComponent<TextMeshProUGUI>();
        costText = transform.Find("Cost_Text").GetComponent<TextMeshProUGUI>();
        currentProductionText = transform.Find("CurrentProduction_Text").GetComponent<TextMeshProUGUI>();
        buttonText = transform.Find("Upgrade_Button/Button_Text").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("UpgradeIcon_Image").GetComponent<Image>();

        // Check for null components and log warnings
        if (titleText == null)
            Debug.LogWarning("Title_Text not found or missing TextMeshProUGUI component on " + gameObject.name);

        if (costText == null)
            Debug.LogWarning("Cost_Text not found or missing TextMeshProUGUI component on " + gameObject.name);

        if (currentProductionText == null)
            Debug.LogWarning("CurrentLevel_Text not found or missing TextMeshProUGUI component on " + gameObject.name);

        if (buttonText == null)
            Debug.LogWarning("Upgrade_Button/Button_Text not found or missing TextMeshProUGUI component on " + gameObject.name);


}

    // Update is called once per frame
    void Update()
    {

    }

    public void setResource(TownScript town, ResourceProductionEntry res, Sprite img)
    {
        this.thisTown = town;
        this.resource = res;
        this.titleText.text = res.ResourceType.ToString() + " LvL: " + this.resource.CurrentLevel;
        this.costText.text = "Upgrade Cost: " + res.InitialCost;
        this.currentProductionText.text = "Current Production " + res.ProductionAmount;
        this.icon.sprite = img;
    }

    public void ResourceUpgrade()
    {
        if (this.resource.CurrentCost < ResourceManager.Instance.getInventory(ResourceManager.ResourceType.Wood) && this.resource.MaxLevel > this.resource.CurrentLevel) // Cost smaller than inventory, can upgrade
        {        
            ResourceManager.Instance.reduceInventory(ResourceManager.ResourceType.Wood, this.resource.CurrentCost); //Reduce the upgrade cost from the Resource inventory

            int old = this.resource.ProductionAmount; // Remember old produciton
            this.resource.ProductionAmount = (this.resource.ProductionAmount / this.resource.CurrentLevel) * this.resource.CurrentLevel+1; // Calculate new production; Init * Level
            ResourceManager.Instance.increaseProduction(this.resource.ResourceType, this.resource.ProductionAmount - old); // Increse production by difference

            this.resource.CurrentLevel++; // Increse the level
            this.resource.CurrentCost = this.resource.InitialCost * (int)Mathf.Pow(this.resource.CurrentLevel, this.thisTown.TownIndex+1); // Calculate new upgrade cost

            //Update the texts
            this.costText.text = "Upgrade Cost: " + this.resource.CurrentCost;
            this.currentProductionText.text = "Current Production " + this.resource.ProductionAmount;
            this.titleText.text = this.resource.ResourceType.ToString() + " LvL: " + this.resource.CurrentLevel;
        }
    }
}
